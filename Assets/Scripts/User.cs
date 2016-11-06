using System.Collections;
using System.Collections.Generic;
using TwitchChatter;
using UnityEngine;

public class User : MonoBehaviour
{
	public UserData userData;

	const float m_SecondsBetweenShots = 0.2f;
	const int m_MaxLevel = 5;
	const int m_LevelCost = 1000;
	const string m_Help = "!help";
	const string m_CheckStats = "!mystats";
	const string m_PurchaseUpgrade = "!upgrade";
	const string m_SetPowerLevel = "!setpower";

	int[] m_ShotCosts = { 1, 5, 10, 15, 20 };
	string m_UserDataFilePath;
	TwitchChatClient m_TwitchChatClient;
	GameManager m_GameManager;
	BoardManager m_BoardManager;
	Leaderboard m_Leaderboard;
	Turret m_Turret;
	Queue<GameObject> m_Shots = new Queue<GameObject>();
	bool m_Shooting;
	bool m_AllowShooting;
	int m_SelectedPower = 1;

	void Start()
	{
		m_TwitchChatClient = TwitchChatClient.singleton;
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
		if( userData.level < 1 )
		{
			userData.level = 1;
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
		userData.emotes += popReward;
		userData.score += popReward;
		PointsLabel.InstantiatePointsLabelGameObject( popReward.ToString(), peg.transform.position );
	}

	void CheckForCommand( string text )
	{
		switch( text )
		{
			case m_Help:
				Help();
			break;
			case m_CheckStats:
				CheckStats();
			break;
			case m_PurchaseUpgrade:
				PurchaseUpgrade();
			break;
			default:
				if( text.Contains( m_SetPowerLevel ) )
				{
					int powerLevel;
					string stringAfterCommand = text.After( m_SetPowerLevel );
					if( int.TryParse( stringAfterCommand, out powerLevel ) )
					{
						SetPowerLevel( powerLevel );
					}
				}
			break;
		}
	}

	void Help()
	{
		m_TwitchChatClient.SendWhisper( userData.userName, "Use all these commands and stuff!" );
	}

	void CheckStats()
	{
		m_TwitchChatClient.SendWhisper( userData.userName,
			"Max Unlocked Power: " + userData.level + ", " +
			"Currently Selected Power: " + m_SelectedPower + ", " +
			"Emotes: " + userData.emotes + ", " +
			"Score: " + userData.score );
	}

	void PurchaseUpgrade()
	{
		int levelEmoteCost = userData.level * m_LevelCost;
		if( userData.level >= m_MaxLevel )
		{
			m_TwitchChatClient.SendWhisper( userData.userName,
				"You have already unlocked the maximum power level of " + m_MaxLevel + "!" );
		}
		else if( userData.emotes >= levelEmoteCost )
		{
			userData.emotes -= levelEmoteCost;
			userData.level++;
			m_TwitchChatClient.SendWhisper( userData.userName,
				"Purchased unlock of max power level " + userData.level +
				". Enter command `!setpower " + userData.level + "` to use it!" );
		}
		else
		{
			m_TwitchChatClient.SendWhisper( userData.userName,
				"You don't have enough emotes to unlock the next level! You have " +
				userData.emotes + " but you need " + levelEmoteCost + "." );
		}
	}

	void SetPowerLevel( int powerLevel )
	{
		if( powerLevel > 0 && powerLevel <= userData.level )
		{
			m_SelectedPower = powerLevel;
			m_TwitchChatClient.SendWhisper( userData.userName, "Power level set to " + powerLevel + "." );
		}
		else if( powerLevel > userData.level && powerLevel <= m_MaxLevel )
		{
			m_TwitchChatClient.SendWhisper( userData.userName, "You haven't reached that power level yet." );
		}
		else if( powerLevel > m_MaxLevel )
		{
			m_TwitchChatClient.SendWhisper( userData.userName, "That power level is too high!" );
		}
		else if( powerLevel < 1 )
		{
			m_TwitchChatClient.SendWhisper( userData.userName, "That power level is too low!" );
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
			GameObject emoteGameObject = Emote.InstantiateEmoteGameObject( emote.id, this, m_SelectedPower );
			m_Shots.Enqueue( emoteGameObject );
		}
	}

	IEnumerator ShootCoroutine()
	{
		while( m_Shots.Count > 0 )
		{
			m_Shooting = true;
			GameObject shot = m_Shots.Dequeue();
			PurchaseShot( m_ShotCosts[ shot.gameObject.GetComponent<Emote>().power - 1 ] );
			m_Turret.Shoot( shot );
			GameManager gameManager = GameManager.singleton;
			gameManager.leaderboard.UpdateScore( userData );
			yield return new WaitForSeconds( m_SecondsBetweenShots );
		}
		m_Shooting = false;
		yield return null;
	}

	void PurchaseShot( int shotCost )
	{
		if( userData.emotes >= shotCost )
		{
			userData.emotes -= shotCost;
		}
		else
		{
			m_SelectedPower = 1;
			if( userData.emotes > 0 )
			{
				userData.emotes--;
			}
		}
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