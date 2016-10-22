using UnityEngine;

public class Peg : MonoBehaviour
{
	[SerializeField] bool m_Invincible;
	[SerializeField] int m_Health = 1;

	BoardManager m_BoardManager;

	public void Freeze()
	{
		var _rigidbody2D = GetComponent<Rigidbody2D>();
		if( _rigidbody2D != null )
		{
			Destroy( _rigidbody2D );
		}
		m_BoardManager.freezeBoard.RemoveListener( Freeze );
		m_Invincible = false;
	}

	void Start()
	{
		m_BoardManager = GameManager.singleton.boardManager;
		m_BoardManager.freezeBoard.AddListener( Freeze );
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
				Destroy( gameObject );
			}
		}
	}
}