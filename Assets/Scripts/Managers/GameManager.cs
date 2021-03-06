﻿using TwitchChatter;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager singleton { get { return m_Singleton; } }

	static GameManager m_Singleton;

	public UserManager userManager;
	public BoardManager boardManager;
	public Leaderboard leaderboard;
	public UserActivityTable userActivityTable;
	public int popReward = 10;
	public int finalPopReward = 100;

	void Awake()
	{
		if( m_Singleton == null )
		{
			m_Singleton = this;
		}
		else
		{
			Destroy( gameObject );
		}
	}

	void Start()
	{
		SetUnityPreferences();
		SubscribeToEvents();
		StartGame();
		TwitchChatClient.singleton.EnableWhispers();
	}

	void OnDestroy()
	{
		UnsubscribeToEvents();
	}

	void SetUnityPreferences()
	{
		Application.runInBackground = true;
	}

	void SubscribeToEvents()
	{
		if( TwitchChatClient.singleton != null )
		{
			TwitchChatClient.singleton.AddChatListener( OnChatMessage );
		}
	}

	void UnsubscribeToEvents()
	{
		if( TwitchChatClient.singleton != null )
		{
			TwitchChatClient.singleton.RemoveChatListener( OnChatMessage );
		}
	}

	void OnChatMessage( ref TwitchChatMessage message )
	{
		userManager.HandleChatMessage( message );
	}

	void StartGame()
	{
		boardManager.CreateBoard();
	}
}