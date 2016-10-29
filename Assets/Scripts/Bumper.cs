using UnityEngine;

public class Bumper : MonoBehaviour
{
	const string flashTriggerName = "Flash";

	public bool invincible;
	public int health = 10;

	[ SerializeField ] Animator animator;

	BoardManager m_BoardManager;

	void Start()
	{
		m_BoardManager = GameManager.singleton.boardManager;
		m_BoardManager.boardFrozen.AddListener( OnBoardFrozen );
		m_BoardManager.boardReset.AddListener( OnBoardReset );
	}

	void OnCollisionEnter2D( Collision2D collision )
	{
		if( !invincible )
		{
			animator.SetTrigger( flashTriggerName );
			health--;
			if( health <= 0 )
			{
				var emote = collision.gameObject.GetComponent<Emote>();
				if( emote != null )
				{
					var user = emote.owner;
					m_BoardManager.BumperPopped( this, user );
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
			m_BoardManager.boardReset.RemoveListener( OnBoardReset );
		}
	}

	void OnBoardFrozen()
	{
		var _rigidbody2D = GetComponent<Rigidbody2D>();
		if( _rigidbody2D != null )
		{
			Destroy( _rigidbody2D );
		}
		gameObject.isStatic = true;
		invincible = false;
	}

	void OnBoardReset()
	{
		Destroy( gameObject );
	}
}