﻿using UnityEngine;

public class Peg : MonoBehaviour
{
	public int m_Health = 1;

	void OnCollisionEnter( Collision _ )
	{
		m_Health--;
		if( m_Health <= 0 )
		{
			Destroy( gameObject );
		}
	}
}