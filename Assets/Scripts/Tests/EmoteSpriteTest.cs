using TwitchChatter;
using UnityEngine;

public class EmoteSpriteTest : MonoBehaviour
{
	public SpriteRenderer m_SpriteRenderer;

	void Start()
	{
		if( TwitchChatClient.singleton != null )
		{
			TwitchChatClient.singleton.AddChatListener( OnChatMessage );
		}
	}

	void OnChatMessage( ref TwitchChatMessage message )
	{
		if( message.emoteData != null )
		{
			int emoteID = message.emoteData[ 0 ].id;
			m_SpriteRenderer.sprite =
				TwitchEmoteCache.GetSpriteForEmoteID(
					emoteID, () => SetSpriteToEmote( m_SpriteRenderer, emoteID ) );
		}
	}

	void SetSpriteToEmote( SpriteRenderer spriteRenderer, int emoteID )
	{
		spriteRenderer.sprite = TwitchEmoteCache.GetSpriteForEmoteID( emoteID, null );
	}
}