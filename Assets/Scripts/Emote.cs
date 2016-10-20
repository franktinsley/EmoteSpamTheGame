using TwitchChatter;
using UnityEngine;

[RequireComponent( typeof( SpriteRenderer ) )]
public class Emote : MonoBehaviour
{
	SpriteRenderer m_SpriteRenderer;
	int m_EmoteID;

	void Awake()
	{
		m_SpriteRenderer = GetComponent<SpriteRenderer>();
	}

	public void SetEmote( int emoteID )
	{
		m_EmoteID = emoteID;
		m_SpriteRenderer.sprite = TwitchEmoteCache.GetSpriteForEmoteID( m_EmoteID, EmoteSize.Large, HandleOnLoadCallBack );
	}

	void HandleOnLoadCallBack( Sprite sprite )
	{
		m_SpriteRenderer.sprite = sprite;
	}
}