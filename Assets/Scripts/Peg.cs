using UnityEngine;

[ RequireComponent( typeof( SpriteRenderer ) ) ]
public class Peg : MonoBehaviour
{
	public int health = 1;
	public bool invincible;

	[ SerializeField ] float m_StartingColliderRadius;
	[ SerializeField ] float m_FrozenColliderRadius;

	CircleCollider2D m_Collider;
	BoardManager m_BoardManager;

	void Start()
	{
		m_Collider = GetComponent<CircleCollider2D>();
		m_BoardManager = GameManager.singleton.boardManager;
		m_BoardManager.boardFrozen.AddListener( OnBoardFrozen );

		m_Collider.radius = m_StartingColliderRadius;
	}

	void OnCollisionEnter2D( Collision2D collision )
	{
		if( !invincible )
		{
			health--;
			if( health <= 0 )
			{
				var emote = collision.gameObject.GetComponent<Emote>();
				if( emote != null )
				{
					var user = emote.owner;
					m_BoardManager.PegPopped( this, user );
					Destroy( gameObject );
				}
			}
		}
	}

	void OnDestroy()
	{
		if( m_BoardManager != null )
		{
			m_BoardManager.boardFrozen.RemoveListener( OnBoardFrozen );
		}
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
}