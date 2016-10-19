using UnityEngine;

public class Peg : MonoBehaviour
{
	public int m_Health = 1;

	[SerializeField] Rigidbody2D m_Rigidbody2D;

	bool m_Frozen;

	public void Freeze()
	{
		if( !m_Frozen )
		{
			Destroy( m_Rigidbody2D );
			gameObject.isStatic = true;
			m_Frozen = true;
		}
	}

	void OnCollisionEnter2D( Collision2D _ )
	{
		if( m_Frozen )
		{
			m_Health--;
			if( m_Health <= 0 )
			{
				Destroy( gameObject );
			}
		}
	}
}