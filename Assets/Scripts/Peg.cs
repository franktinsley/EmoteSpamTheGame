using UnityEngine;

public class Peg : MonoBehaviour
{
	[SerializeField] int m_Health = 1;
	[SerializeField] bool m_Invincible;

	BoardManager m_BoardManager;

	void Start()
	{
		m_BoardManager = GameManager.singleton.boardManager;
		m_BoardManager.freezeBoard.AddListener( Freeze );
	}

	void OnCollisionEnter2D( Collision2D _ )
	{
		if( !m_Invincible )
		{
			m_Health--;
			if( m_Health <= 0 )
			{
				Destroy( gameObject );
			}
		}
	}

	public void Freeze()
	{
		
	}
}