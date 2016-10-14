using TwitchChatter;
using UnityEngine;

public class Launcher2DTest : MonoBehaviour
{
	[SerializeField] Launcher2D m_Launcher;
	[SerializeField] string m_LaunchTestCommand;
	[SerializeField] KeyCode m_LaunchTestKey;

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
		if( message.chatMessagePlainText.Equals( m_LaunchTestCommand ) )
		{
			m_Launcher.Launch( message );
		}
	}

	void Update()
	{
		if( Input.GetKeyDown( m_LaunchTestKey ) )
		{
			m_Launcher.Launch();
		}
	}
}