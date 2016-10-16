//using TwitchChatter;
using UnityEngine;

public class LauncherTest : MonoBehaviour
{
	[SerializeField] Launcher m_Launcher;
	[SerializeField] KeyCode m_LaunchTestKey;

	void Update()
	{
		if( Input.GetKeyDown( m_LaunchTestKey ) )
		{
			m_Launcher.Launch();
		}
	}

	/*void Start()
	{
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
	}*/
}