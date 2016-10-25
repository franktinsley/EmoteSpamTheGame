using System.Collections;
using System.Collections.Generic;
using TwitchChatter;
using UnityEngine;

public class User : MonoBehaviour
{
	public UserData userData;

	const float m_SecondsBetweenShots = 0.2f;
	const int m_popScore = 10;

	string m_UserDataFilePath;
	Turret m_Cannon;
	Queue<GameObject> m_Shots = new Queue<GameObject>();
	bool m_Shooting;

	void OnDisable()
	{
		JsonScriptableObject.SaveToFile<UserData>( userData, m_UserDataFilePath );
	}

	void OnDestroy()
	{
		if( GameManager.singleton.boardManager != null )
		{
			GameManager.singleton.boardManager.boardReset.RemoveListener( OnBoardReset );
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
		user.m_Cannon = GameManager.singleton.boardManager.turret;
		GameManager.singleton.boardManager.boardReset.AddListener( user.OnBoardReset );
		Leaderboard.singleton.UpdateScore( user.userData );
		return user;
	}

	public void HandleMessage( TwitchChatMessage message )
	{
		UpdateUserData( message );

		if( message.emoteData != null )
		{
			ShootEmotes( message.emoteData );
		}
	}

	public void ScorePop()
	{
		userData.score += m_popScore;
		Leaderboard.singleton.UpdateScore( userData );
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
		if( !m_Shooting )
		{
			StartCoroutine( ShootCoroutine() );
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
			m_Cannon.Shoot( m_Shots.Dequeue() );
			if( userData.score > 0 )
			{
				userData.score--;
				Leaderboard.singleton.UpdateScore( userData );
			}
			yield return new WaitForSeconds( m_SecondsBetweenShots );
		}
		m_Shooting = false;
		yield return null;
	}

	void OnBoardReset()
	{
		m_Shots = new Queue<GameObject>();
	}
}