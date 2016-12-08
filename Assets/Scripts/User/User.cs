using System.Collections;
using System.Collections.Generic;
using TwitchChatter;
using UnityEngine;

public class User : MonoBehaviour
{
	public UserData model;

	const int m_MaxAmmo = 10;
	const float m_SecondsBetweenReload = 2f;
	const float m_SecondsBetweenShots = 0.2f;
	const string m_ScoreCommand = "!score";

	bool m_Shooting;
	float m_NextReloadTime;
	string m_UserDataFilePath;
	Queue<GameObject> m_Shots = new Queue<GameObject>();
	TwitchChatClient m_TwitchChatClient;
	GameManager m_GameManager;
	BoardManager m_BoardManager;
	Leaderboard m_Leaderboard;
	Turret m_Turret;
	UserActivityTableCell m_UserActivityTableCell;

	void Start()
	{
		m_TwitchChatClient = TwitchChatClient.singleton;
		model.ammo = m_MaxAmmo;
		m_NextReloadTime = Time.time + m_SecondsBetweenReload;
	}

	void Update()
	{
		if( ShouldReload() )
		{
			Reload();
		}
	}

	void OnDisable()
	{
		JsonScriptableObject.SaveToFile<UserData>( model, m_UserDataFilePath );
	}

	void OnDestroy()
	{
		if( m_BoardManager != null )
		{
			m_BoardManager.boardReset.RemoveListener( OnBoardFrozen );
			m_BoardManager.boardReset.RemoveListener( OnBoardReset );
		}
	}

	public static User CreateInstance( string userName, string userDataFilePath, Transform parent )
	{
		User user;
		UserData userData;
		bool fileFound;
		userData = JsonScriptableObject.LoadFromFile<UserData>( userDataFilePath, out fileFound );
		if( !fileFound )
		{
			userData.userName = userName;
		}
		var userGameObject = new GameObject( userName );
		userGameObject.transform.parent = parent;
		user = userGameObject.AddComponent<User>();
		user.model = userData;
		user.m_UserDataFilePath = userDataFilePath;
		user.m_UserActivityTableCell = UserActivityTableCell.InstantiateUserActivityTableCellGameObject( user.model );
		user.m_GameManager = GameManager.singleton;
		user.m_BoardManager = GameManager.singleton.boardManager;
		user.m_Turret = GameManager.singleton.boardManager.turret;
		user.m_BoardManager.boardFrozen.AddListener( user.OnBoardFrozen );
		user.m_BoardManager.boardReset.AddListener( user.OnBoardReset );
		user.m_Leaderboard = GameManager.singleton.leaderboard;
		user.m_Leaderboard.UpdateScore( user.model );
		return user;
	}

	public void HandleMessage( TwitchChatMessage message )
	{
		UpdateUserData( message );

		CheckForCommand( message.chatMessagePlainText );

		TwitchChatMessage.EmoteData[] emoteData = message.emoteData;
		if( emoteData != null && emoteData.Length > 0 )
		{
			ShootEmotes( emoteData );
			m_UserActivityTableCell.Activity();
		}
	}

	public void ScorePop( Peg peg )
	{
		int popReward = m_GameManager.popReward;
		model.score += popReward;
		model.ammo = m_MaxAmmo;
	}

	void CheckForCommand( string text )
	{
		switch( text )
		{
			case m_ScoreCommand:
				Score();
			break;
		}
	}

	void Score()
	{
		m_TwitchChatClient.SendWhisper( model.userName,
			"Your score: " + model.score );
	}

	void UpdateUserData( TwitchChatMessage message )
	{
		model.userName = message.userName;
		model.userNameColor = message.userNameColor;
		model.isSubscriber = message.isSubscriber;
		model.isTurbo = message.isTurbo;
		model.isMod = message.isMod;
	}

	bool ShouldReload()
	{
		return model.ammo < m_MaxAmmo && Time.time > m_NextReloadTime;
	}

	void Reload()
	{
		m_NextReloadTime += m_SecondsBetweenReload;
		model.ammo++;
		PointsLabel.InstantiatePointsLabelGameObject(
			"<color=" + model.userNameColor + ">Ammo: " + model.ammo + "</color>",
			m_Turret.barrel.position );
	}

	void ShootEmotes( TwitchChatMessage.EmoteData[] emoteData )
	{
		EnqueueEmoteShots( emoteData );
		if( m_BoardManager.allowUserShooting )
		{
			if( !m_Shooting )
			{
				StartCoroutine( ShootCoroutine() );
			}
		}
	}

	void EnqueueEmoteShots( TwitchChatMessage.EmoteData[] emoteData )
	{
		foreach( var emote in emoteData )
		{
			GameObject emoteGameObject = Emote.InstantiateEmoteGameObject( emote.id, this );
			emoteGameObject.SetActive( false );
			m_Shots.Enqueue( emoteGameObject );
		}
	}

	IEnumerator ShootCoroutine()
	{
		while( m_Shots.Count > 0 )
		{
			m_Shooting = true;
			GameObject shot = m_Shots.Dequeue();
			if( model.ammo > 0 )
			{
				m_Turret.Shoot( shot );
				GameManager gameManager = GameManager.singleton;
				gameManager.leaderboard.UpdateScore( model );
				model.ammo--;
				PointsLabel.InstantiatePointsLabelGameObject(
					"<color=" + model.userNameColor + ">Ammo: " + model.ammo + "</color>",
					m_Turret.barrel.position );
			}
			else
			{
				PointsLabel.InstantiatePointsLabelGameObject(
					"<color=" + model.userNameColor + ">click</color>",
					m_Turret.barrel.position );
			}
			yield return new WaitForSeconds( m_SecondsBetweenShots );
		}
		m_Shooting = false;
		yield return null;
	}

	void OnBoardFrozen()
	{
		if( m_Shots.Count > 0 )
		{
			StartCoroutine( ShootCoroutine() );
		}
	}

	void OnBoardReset()
	{
		m_Shots = new Queue<GameObject>();
	}
}