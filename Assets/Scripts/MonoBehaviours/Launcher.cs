using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
	[SerializeField] float m_ShootForce = 10f;
	[SerializeField] float m_ProjectileZPosition = 2f;
	[SerializeField] List<SpriteRenderer> m_SpriteRenderers;
	[SerializeField] Transform m_Barrel;
	[SerializeField] Transform m_Hinge;
	[SerializeField] Rigidbody2D m_Rigidbody2D;
	[SerializeField] GameObject m_ProjectilePrefab;

	void Start()
	{
		m_Rigidbody2D.centerOfMass =
			m_Rigidbody2D.transform.InverseTransformPoint( m_Hinge.position );
	}

	public void Launch()
	{
		Shoot( Spawn() );
	}

	public void Launch( Color color )
	{
		var projectile = Spawn();
		TintSpriteRenderers( color, projectile.GetComponent<SpriteRenderer>() );
		Shoot( projectile );
	}

	GameObject Spawn()
	{
		return Instantiate(
			m_ProjectilePrefab,
			new Vector3(
				m_Barrel.position.x,
				m_Barrel.position.y,
				m_ProjectileZPosition ),
			m_Barrel.rotation ) as GameObject;
	}

	void Shoot( GameObject projectile )
	{
		projectile.GetComponent<Rigidbody2D>()
			.AddRelativeForce( Vector2.down * m_ShootForce, ForceMode2D.Impulse );
	}

	void TintSpriteRenderers( Color color, SpriteRenderer spriteRenderer )
	{
		spriteRenderer.color = color;
		foreach( var _spriteRenderer in m_SpriteRenderers )
		{
			_spriteRenderer.color = color;
		}
	}
}