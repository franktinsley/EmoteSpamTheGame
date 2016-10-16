using TwitchChatter;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	void Start()
	{
		SetUnityPreferences();
		SubscribeToEvents();
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
		Debug.Log( message.userName + ": " + message.chatMessagePlainText );
	}
}