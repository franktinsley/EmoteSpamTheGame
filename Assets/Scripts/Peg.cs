using UnityEngine;

[ RequireComponent( typeof( SpriteRenderer ) ) ]
[ RequireComponent( typeof( CircleCollider2D ) ) ]
public class Peg : MonoBehaviour
{
	public int health = 10;

	[ SerializeField ] float m_StartingColliderRadius;
	[ SerializeField ] float m_FrozenColliderRadius;

	SpriteRenderer m_SpriteRenderer;
	CircleCollider2D m_Collider;
	BoardManager m_BoardManager;

	void Start()
	{
		m_SpriteRenderer = GetComponent<SpriteRenderer>();
		m_Collider = GetComponent<CircleCollider2D>();
		m_Collider.radius = m_StartingColliderRadius;
		m_BoardManager = GameManager.singleton.boardManager;
		m_BoardManager.boardFrozen.AddListener( OnBoardFrozen );
		m_BoardManager.boardReset.AddListener( OnBoardReset );
	}
		
	void OnDestroy()
	{
		if( m_BoardManager != null )
		{
			m_BoardManager.boardFrozen.RemoveListener( OnBoardFrozen );
			m_BoardManager.boardReset.RemoveListener( OnBoardReset );
		}
	}

	public void TakeDamage( int damage, Emote emote )
	{
		health -= damage;
		if( health < 1 )
		{
			Pop( emote );
		}
	}

	void Pop( Emote emote )
	{
		emote.owner.ScorePop( this );
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