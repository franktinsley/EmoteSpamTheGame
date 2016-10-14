using System.Collections.Generic;
using TwitchChatter;
using UnityEngine;

public class Launcher2DTest : MonoBehaviour
{
	[SerializeField] Launcher2D m_Launcher;
	[SerializeField] List<string> m_LaunchTestCommands;
	[SerializeField] List<string> m_PushLeftTestCommands;
	[SerializeField] List<string> m_PushRightCommands;
	[SerializeField] KeyCode m_LaunchTestKey;
	[SerializeField] KeyCode m_PushLeftTestKey;
	[SerializeField] KeyCode m_PushRightTestKey;

	void Start()
	{
		Application.runInBackground = true;
		if( TwitchChatClient.singleton != null )
		{
			TwitchChatClient.singleton.AddChatListener( OnChatMessage );
		}
	}

	void OnDestroy()
	{
		if( TwitchChatClient.singleton != null )
		{
			TwitchChatClient.singleton.RemoveChatListener( OnChatMessage );
		}
	}

	void OnChatMessage( ref TwitchChatMessage message )
	{
		string chatMessagePlainText = message.chatMessagePlainText;
		foreach( var text in m_LaunchTestCommands )
		{
			if( chatMessagePlainText.Equals( text ) )
			{
				Color color;
				ColorUtility.TryParseHtmlString( message.userNameColor, out color );
				m_Launcher.Launch( color );
			}
		}
		foreach( var text in m_PushLeftTestCommands )
		{
			if( chatMessagePlainText.Equals( text ) )
			{
				m_Launcher.Push( false );
			}
		}
		foreach( var text in m_PushRightCommands )
		{
			if( chatMessagePlainText.Equals( text ) )
			{
				m_Launcher.Push( true );
			}
		}
	}

	void Update()
	{
		if( Input.GetKeyDown( m_LaunchTestKey ) )
		{
			m_Launcher.Launch();
		}
		if( Input.GetKeyDown( m_PushLeftTestKey ) )
		{
			m_Launcher.Push( false );
		}
		if( Input.GetKeyDown( m_PushRightTestKey ) )
		{
			m_Launcher.Push( true );
		}
	}
}