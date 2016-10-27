using TwitchChatter;
using UnityEngine;

[ RequireComponent( typeof( SpriteRenderer ) ) ]
public class Emote : MonoBehaviour
{
	public User owner;

	SpriteRenderer m_SpriteRenderer;
	int m_EmoteID;

	public static GameObject InstantiateEmoteGameObject( int id, User owner )
	{
		BoardManager boardManager = GameManager.singleton.boardManager;
		var emoteGameObject = Instantiate(
			boardManager.emotePrefab,
			boardManager.emoteParent ) as GameObject;
		var emote = emoteGameObject.GetComponent<Emote>();
		GameManager.singleton.boardManager.boardReset.AddListener(
			emote.OnBoardReset );
		emote.owner = owner;
		emote.SetEmote( id );
		return emoteGameObject;
	}
		
	void Awake()
	{
		m_SpriteRenderer = GetComponent<SpriteRenderer>();
	}

	void OnDestroy()
	{
		if( GameManager.singleton.boardManager != null )
		{
			GameManager.singleton.boardManager.boardReset.RemoveListener(
				OnBoardReset );
		}
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

	void OnBoardReset()
	{
		Destroy( gameObject );
	}
}