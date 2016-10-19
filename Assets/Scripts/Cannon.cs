using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
	[SerializeField] float m_ShootForce = 10f;
	[SerializeField] float m_ProjectileZPosition = 2f;
	[SerializeField] List<SpriteRenderer> m_SpriteRenderers;
	[SerializeField] Transform m_Barrel;
	[SerializeField] Transform m_Hinge;
	[SerializeField] GameObject m_ProjectilePrefab;

	public void Shoot()
	{
		Shoot( Spawn() );
	}

	public void Shoot( Color color )
	{
		var projectile = Spawn();
		TintSpriteRenderers( color, projectile.GetComponent<SpriteRenderer>() );
		Shoot( projectile );
	}

	public void Shoot( Color color, int emoteID )
	{
		GameObject projectile = Spawn();
		TintSpriteRenderers( color, projectile.GetComponent<SpriteRenderer>() );
		Emote emote = projectile.GetComponentInChildren<Emote>();
		emote.SetEmote( emoteID );
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

	void Shoot( List<GameObject> projectiles, string user )
	{
		
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