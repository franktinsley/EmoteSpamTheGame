using System.Collections;
using System.Collections.Generic;
using TwitchChatter;
using UnityEngine;

public class User : MonoBehaviour
{
	public UserData userData;
	public int ammo;

	const int m_MaxAmmo = 10;
	const float m_SecondsBetweenReload = 1f;
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

	void Start()
	{
		m_TwitchChatClient = TwitchChatClient.singleton;
		ammo = m_MaxAmmo;
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
		JsonScriptableObject.SaveToFile<UserData>( userData, m_UserDataFilePath );
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
		user.userData = userData;
		user.m_UserDataFilePath = userDataFilePath;
		user.m_GameManager = GameManager.singleton;
		user.m_BoardManager = GameManager.singleton.boardManager;
		user.m_Turret = GameManager.singleton.boardManager.turret;
		user.m_BoardManager.boardFrozen.AddListener( user.OnBoardFrozen );
		user.m_BoardManager.boardReset.AddListener( user.OnBoardReset );
		user.m_Leaderboard = GameManager.singleton.leaderboard;
		user.m_Leaderboard.UpdateScore( user.userData );
		return user;
	}

	public void HandleMessage( TwitchChatMessage message )
	{
		UpdateUserData( message );

		CheckForCommand( message.chatMessagePlainText );

		TwitchChatMessage.EmoteData[] emoteData = message.emoteData;
		if( emoteData != null )
		{
			ShootEmotes( emoteData );
		}
	}

	public void ScorePop( Peg peg )
	{
		int popReward = m_GameManager.popReward;
		userData.score += popReward;
		ammo = m_MaxAmmo;
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
		m_TwitchChatClient.SendWhisper( userData.userName, "Your score: " + userData.score );
	}

	void UpdateUserData( TwitchChatMessage message )
	{
		userData.userName = message.userName;
		userData.userNameColor = message.userNameColor;
		userData.isSubscriber = message.isSubscriber;
		userData.isTurbo = message.isTurbo;
		userData.isMod = message.isMod;
	}

	bool ShouldReload()
	{
		return ammo < m_MaxAmmo && Time.time > m_NextReloadTime;
	}

	void Reload()
	{
		m_NextReloadTime += m_SecondsBetweenReload;
		ammo++;
		PointsLabel.InstantiatePointsLabelGameObject(
			"<color=" + userData.userNameColor + ">Ammo: " + ammo + "</color>",
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
			m_Shots.Enqueue( emoteGameObject );
		}
	}

	IEnumerator ShootCoroutine()
	{
		while( m_Shots.Count > 0 )
		{
			m_Shooting = true;
			if( ammo > 0 )
			{
				GameObject shot = m_Shots.Dequeue();
				m_Turret.Shoot( shot );
				GameManager gameManager = GameManager.singleton;
				gameManager.leaderboard.UpdateScore( userData );
				ammo--;
				PointsLabel.InstantiatePointsLabelGameObject(
					"<color=" + userData.userNameColor + ">Ammo: " + ammo + "</color>",
					m_Turret.barrel.position );
			}
			else
			{
				PointsLabel.InstantiatePointsLabelGameObject(
					"<color=" + userData.userNameColor + ">click</color>",
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