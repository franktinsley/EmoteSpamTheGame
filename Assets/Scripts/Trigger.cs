using UnityEngine;

public class Trigger : MonoBehaviour
{
	const string activatedTriggerName = "Activated";

	[ SerializeField ] Animator animator;
	[ SerializeField ] float offset;
	[ SerializeField ] Collider2D spacer;

	BoardManager m_BoardManager;
	Collision2D m_Collision;

	void Start()
	{
		m_BoardManager = GameManager.singleton.boardManager;
		m_BoardManager.boardFrozen.AddListener( OnBoardFrozen );
		m_BoardManager.boardReset.AddListener( OnBoardReset );
	}

	void OnCollisionEnter2D( Collision2D collision )
	{
		if( collision.transform.position.y > transform.position.y )
		{
			m_Collision = collision;
		}
	}

	void OnCollisionExit2D( Collision2D collision )
	{
		if( m_Collision != null )
		{
			if( collision.gameObject == m_Collision.gameObject )
			{
				m_Collision = null;
				if( collision.transform.position.y - offset < transform.position.y )
				{
					var emote = collision.gameObject.GetComponent<Emote>();
					if( emote != null )
					{
						var user = emote.owner;
						animator.SetTrigger( activatedTriggerName );
						m_BoardManager.TriggerActivated( this, user );
					}
				}
			}
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

	void OnBoardFrozen()
	{
		var _rigidbody2D = GetComponentInParent<Rigidbody2D>();
		if( _rigidbody2D != null )
		{
			Destroy( _rigidbody2D );
		}
		Destroy( spacer );
		gameObject.isStatic = true;
	}

	void OnBoardReset()
	{
		Destroy( gameObject );
	}
}