using System.Collections;
using System.Collections.Generic;
using TwitchChatter;
using UnityEngine;

public class User : MonoBehaviour
{
	public UserData userData;
	public int points;
	public int heat;
	public bool overheated;
	public float repeatPeriod = 1f;

	const float m_SecondsBetweenShots = 0.2f;

	string m_UserDataFilePath;
	BoardManager m_BoardManager;
	Leaderboard m_Leaderboard;
	Turret m_Turret;
	Queue<GameObject> m_Shots = new Queue<GameObject>();
	bool m_Shooting;
	bool m_AllowShooting;
	float m_NextRepeatTime;
	TwitchChatClient m_TwitchChatClient;

	void Start()
	{
		m_TwitchChatClient = TwitchChatClient.singleton;
		m_NextRepeatTime = Time.time + repeatPeriod;
	}

	void Update()
	{
		if( Time.time > m_NextRepeatTime )
		{
			m_NextRepeatTime += repeatPeriod;
			if( heat > 0 )
			{
				heat--;
				if( overheated && heat < m_BoardManager.cool )
				{
					overheated = false;
					heat = 0;
					m_TwitchChatClient.SendWhisper( userData.userName, "Cooldown complete!" );
				}
			}
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

		if( !overheated )
		{
			if( message.emoteData != null )
			{
				ShootEmotes( message.emoteData );
			}
		}
		else
		{
			m_TwitchChatClient.SendWhisper( userData.userName, "Wait until cooldown complete to shoot again!" );
		}
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
			if( heat >= m_BoardManager.overheat )
			{
				overheated = true;
				m_TwitchChatClient.SendWhisper( userData.userName, "Overheated!" );
				m_Shots.Clear();
				break;
			}
			m_Shooting = true;
			heat++;
			GameObject shot = m_Shots.Dequeue();
			m_Turret.Shoot( shot );
			GameManager gameManager = GameManager.singleton;
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
		points = 0;
		heat = 0;
	}
}