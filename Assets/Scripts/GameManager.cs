using System.Collections.Generic;
using TwitchChatter;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager singleton { get { return m_Singleton; } }

	public GameState currentGameState { get { return m_CurrentGameState; } }
	public List<GameState> possibleGameStates { get { return m_PossibleGameStates; } }
	public BoardManager boardManager { get { return m_BoardManager; } }
	public UserManager userManager { get { return m_UserManager; } }
	public bool allowMessages;

	[SerializeField] GameState m_CurrentGameState;
	[SerializeField] List<GameState> m_PossibleGameStates;
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