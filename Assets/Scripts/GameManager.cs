﻿using TwitchChatter;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager singleton { get { return m_Singleton; } }

	public BoardManager boardManager { get { return m_BoardManager; } }
	public UserManager userManager { get { return m_UserManager; } }
	public bool allowMessages;

	[SerializeField] UserManager m_UserManager;
	[SerializeField] BoardManager m_BoardManager;

	static GameManager m_Singleton;

	void Awake()
	{
		m_Singleton = this;
	}

	void Start()
	{
		SetUnityPreferences();
		SubscribeToEvents();
		StartGame();
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
		if( allowMessages )
		{
			m_UserManager.HandleChatMessage( message );
		}
	}

	void StartGame()
	{
		m_BoardManager.CreateBoard();
	}
}