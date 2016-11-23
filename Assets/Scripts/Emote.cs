using TwitchChatter;
using UnityEngine;
using UnityEngine.UI;

[ RequireComponent( typeof( SpriteRenderer ) ) ]
public class Emote : MonoBehaviour
{
	public User owner;
	public int power;

	//SpriteRenderer m_SpriteRenderer;
	Image m_Image;
	int m_EmoteID;

	public static GameObject InstantiateEmoteGameObject( int id, User owner )
	{
		BoardManager boardManager = GameManager.singleton.boardManager;
		var emoteGameObject = Instantiate( boardManager.emotePrefab );
		emoteGameObject.transform.SetParent( boardManager.emoteParent, true );
		var emote = emoteGameObject.GetComponent<Emote>();
		GameManager.singleton.boardManager.boardReset.AddListener(
			emote.OnBoardReset );
		emote.owner = owner;
		emote.SetEmote( id );
		return emoteGameObject;
	}

	void Awake()
	{
		//m_SpriteRenderer = GetComponent<SpriteRenderer>();
		m_Image = GetComponent<Image>();
	}

	void OnCollisionEnter2D( Collision2D collision )
	{
		Peg peg = collision.gameObject.GetComponent<Peg>();
		if( peg != null )
		{
			peg.TakeDamage( power, this );
		}
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
		//m_SpriteRenderer.sprite = TwitchEmoteCache.GetSpriteForEmoteID(
			//m_EmoteID, EmoteSize.Large, HandleOnLoadCallBack );
		m_Image.sprite = TwitchEmoteCache.GetSpriteForEmoteID(
			m_EmoteID, EmoteSize.Large, HandleOnLoadCallBack );
	}

	void HandleOnLoadCallBack( Sprite sprite )
	{
		/*if( m_SpriteRenderer != null )
		{
			m_SpriteRenderer.sprite = sprite;
		}*/
		if( m_Image != null )
		{
			m_Image.sprite = sprite;
		}
	}

	void OnBoardReset()
	{
		Destroy( gameObject );
	}
}