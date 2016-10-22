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
		GameObject emoteGameObject =
			Instantiate( GameManager.singleton.boardManager.emotePrefab );
		Emote emote = emoteGameObject.GetComponentInChildren<Emote>();
		emote.owner = owner;
		emote.SetEmote( id );
		return emoteGameObject;
	}

	void Awake()
	{
		m_SpriteRenderer = GetComponent<SpriteRenderer>();
	}

	public void SetEmote( int emoteID )
	{
		m_EmoteID = emoteID;
		m_SpriteRenderer.sprite = TwitchEmoteCache.GetSpriteForEmoteID(
			m_EmoteID, EmoteSize.Large, HandleOnLoadCallBack );
	}

	void HandleOnLoadCallBack( Sprite sprite )
	{
		m_SpriteRenderer.sprite = sprite;
	}
}