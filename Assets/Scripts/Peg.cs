using UnityEngine;

public class Peg : MonoBehaviour
{
	[ SerializeField ] bool m_Invincible;
	[ SerializeField ] int m_Health = 1;

	BoardManager m_BoardManager;

	void Start()
	{
		m_BoardManager = GameManager.singleton.boardManager;
		m_BoardManager.boardFrozen.AddListener( OnBoardFrozen );
		m_BoardManager.pegPopped.AddListener( OnPegPopped );
	}

	void OnCollisionEnter2D( Collision2D collision )
	{
		if( !m_Invincible )
		{
			m_Health--;
			if( m_Health <= 0 )
			{
				var emote = collision.gameObject.GetComponent<Emote>();
				if( emote != null )
				{
					emote.owner.ScorePop();
				}
				m_BoardManager.FirePegPopped();
				Destroy( gameObject );
			}
		}
	}

	void OnDestroy()
	{
		if( m_BoardManager != null )
		{
			m_BoardManager.boardFrozen.RemoveListener( OnBoardFrozen );
			m_BoardManager.pegPopped.RemoveListener( OnPegPopped );
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
		m_Invincible = false;
	}

	void OnPegPopped( int remainingCount )
	{
		//int startingNumberOfPegs = BoardManager.startingNumberOfPegs;
	}
}