using System.Collections;
using System.Collections.Generic;
using TwitchChatter;
using UnityEngine;

public class User : MonoBehaviour
{
	public UserData userData;

	const float m_SecondsBetweenShots = 0.2f;

	string m_UserDataFilePath;
	BoardManager m_BoardManager;
	Leaderboard m_Leaderboard;
	Turret m_Turret;
	Queue<GameObject> m_Shots = new Queue<GameObject>();
	bool m_Shooting;
	bool m_AllowShooting;

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

		if( message.emoteData != null )
		{
			Debug.Log( "Shoot " + message.emoteData.Length + " emotes." );
			ShootEmotes( message.emoteData );
		}
	}

	public void ScorePoints( int points )
	{
		userData.score += points;
		m_Leaderboard.UpdateScore( userData );
	}

	void UpdateUserData( TwitchChatMessage message )
	{
		userData.userName = message.userName;
		userData.userNameColor = message.userNameColor;
		userData.isSubscriber = message.isSubscriber;
		userData.isTurbo = message.isTurbo;
		userData.isMod = message.isMod;
	}

	void ShootEmotes( TwitchChatMessage.EmoteData[] emoteData )
	{
		EnqueueEmoteShots( emoteData );
		if( m_AllowShooting )
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
			m_BoardManager.IncreaseJackpot();
			m_Turret.Shoot( m_Shots.Dequeue() );
			GameManager gameManager = GameManager.singleton;
			userData.score -= gameManager.boardManager.shotCost;
			gameManager.leaderboard.UpdateScore( userData );
			yield return new WaitForSeconds( m_SecondsBetweenShots );
		}
		m_Shooting = false;
		yield return null;
	}

	void OnBoardFrozen()
	{
		m_AllowShooting = true;
		if( m_Shots.Count > 0 )
		{
			StartCoroutine( ShootCoroutine() );
		}
	}

	void OnBoardReset()
	{
		m_AllowShooting = false;
		m_Shots = new Queue<GameObject>();
	}
}