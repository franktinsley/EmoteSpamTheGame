using UnityEngine;

public class Peg : MonoBehaviour
{
	public int m_Health = 1;

	void OnCollisionEnter2D( Collision2D _ )
	{
		m_Health--;
		if( m_Health <= 0 )
		{
			Destroy( gameObject );
		}
	}
}