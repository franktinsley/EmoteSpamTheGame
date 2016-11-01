using UnityEngine;

[ RequireComponent( typeof( SpriteRenderer ) ) ]
public class Peg : MonoBehaviour
{
	public User owner;

	[ SerializeField ] float m_StartingColliderRadius;
	[ SerializeField ] float m_FrozenColliderRadius;

	SpriteRenderer m_SpriteRenderer;
	CircleCollider2D m_Collider;
	BoardManager m_BoardManager;
	Color m_DefaultColor;

	void Start()
	{
		m_SpriteRenderer = GetComponent<SpriteRenderer>();
		m_DefaultColor = m_SpriteRenderer.color;
		m_Collider = GetComponent<CircleCollider2D>();
		m_Collider.radius = m_StartingColliderRadius;
		m_BoardManager = GameManager.singleton.boardManager;
		m_BoardManager.boardFrozen.AddListener( OnBoardFrozen );
		m_BoardManager.boardReset.AddListener( OnBoardReset );
	}

	void OnCollisionEnter2D( Collision2D collision )
	{
		var emote = collision.gameObject.GetComponent<Emote>();
		if( emote != null )
		{
			m_BoardManager.PegHit( this, emote );
		}
	}

	void OnDestroy()
	{
		if( m_BoardManager != null )
		{
			m_BoardManager.boardFrozen.RemoveListener( OnBoardFrozen );
			m_BoardManager.boardReset.RemoveListener( OnBoardReset );
		}
	}

	public void SetOwner( User user )
	{
		owner = user;
		if( owner == null )
		{
			m_SpriteRenderer.color = m_DefaultColor;
		}
		else
		{
			Color color;
			m_SpriteRenderer.color =
				ColorUtility.TryParseHtmlString( owner.userData.userNameColor, out color ) ?
				color : m_DefaultColor;
		}
	}

	public void Pop()
	{
		Destroy( gameObject );
	}

	void OnBoardFrozen()
	{
		var _rigidbody2D = GetComponent<Rigidbody2D>();
		if( _rigidbody2D != null )
		{
			Destroy( _rigidbody2D );
		}
		m_Collider.radius = m_FrozenColliderRadius;
		gameObject.isStatic = true;
	}

	void OnBoardReset()
	{
		Destroy( gameObject );
	}
}