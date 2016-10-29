using System.Collections.Generic;
using UnityEngine;

public class JackpotTrigger : MonoBehaviour
{
	const string activatedTriggerName = "Activated";

	[ SerializeField ] Animator animator;
	[ SerializeField ] float offset;
	[ SerializeField ] Collider2D spacer;

	BoardManager m_BoardManager;
	List<GameObject> m_CollisionGameObjects;

	void Start()
	{
		m_BoardManager = GameManager.singleton.boardManager;
		m_BoardManager.boardFrozen.AddListener( OnBoardFrozen );
		m_BoardManager.boardReset.AddListener( OnBoardReset );
		m_CollisionGameObjects = new List<GameObject>();
	}

	void OnCollisionEnter2D( Collision2D collision )
	{
		if( collision.transform.position.y > transform.position.y )
		{
			m_CollisionGameObjects.Add( collision.gameObject );
		}
	}

	void OnCollisionExit2D( Collision2D collision )
	{
		if( m_CollisionGameObjects.Count > 0 )
		{
			GameObject collisionGameObject = collision.gameObject;
			if( m_CollisionGameObjects.Contains( collisionGameObject ) )
			{
				m_CollisionGameObjects.Remove( collisionGameObject );
				if( collisionGameObject.transform.position.y - offset < transform.position.y )
				{
					var emote = collisionGameObject.GetComponent<Emote>();
					if( emote != null )
					{
						var user = emote.owner;
						animator.SetTrigger( activatedTriggerName );
						m_BoardManager.JackpotTriggerActivated( this, user );
						m_CollisionGameObjects.Clear();
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