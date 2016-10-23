using TwitchChatter;
using UnityEngine;

[RequireComponent( typeof( SpriteRenderer ) )]
public class Emote : MonoBehaviour
{
	public User owner;

	SpriteRenderer m_SpriteRenderer;
	int m_EmoteID;

	public static GameObject InstantiateEmoteGameObject( int id, User owner )
	{
		BoardManager boardManager = GameManager.singleton.boardManager;
		GameObject emoteGameObject =
			Instantiate( boardManager.emotePrefab );
		emoteGameObject.transform.parent = boardManager.emoteParent;
		Emote emote = emoteGameObject.GetComponent<Emote>();
		emote.owner = owner;
		emote.SetEmote( id );
		return emoteGameObject;
	}
		
	void Awake()
	{
		m_SpriteRenderer = GetComponent<SpriteRenderer>();
	}

	void SetEmote( int emoteID )
	{
		m_EmoteID = emoteID;
		m_SpriteRenderer.sprite = TwitchEmoteCache.GetSpriteForEmoteID(
			m_EmoteID, EmoteSize.Large, HandleOnLoadCallBack );
	}

	void HandleOnLoadCallBack( Sprite sprite )
	{
		if( m_SpriteRenderer != null )
		{
			m_SpriteRenderer.sprite = sprite;
		}
	}
}