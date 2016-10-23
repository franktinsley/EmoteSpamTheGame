using UnityEngine;
using UnityEngine.SceneManagement;

public class Peg : MonoBehaviour
{
	static int count;

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
		gameObject.isStatic = true;
		m_BoardManager.freezeBoard.RemoveListener( Freeze );
		m_Invincible = false;
	}

	void Start()
	{
		count++;
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
				count--;
				if( count < 1 )
				{
					SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex );
				}
				Destroy( gameObject );
			}
		}
	}
}