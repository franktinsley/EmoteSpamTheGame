using UnityEngine;

public class Peg : MonoBehaviour
{
	public int m_Health = 1;
	public bool m_Invincible;

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
}