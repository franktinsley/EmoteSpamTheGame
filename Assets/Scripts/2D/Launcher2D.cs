using UnityEngine;

public class Launcher2D : MonoBehaviour
{
	[SerializeField] float m_PushPower = 10f;
	[SerializeField] float m_ShootPower = 10f;
	[SerializeField] float m_ProjectileZPosition = 2f;
	[SerializeField] SpriteRenderer m_BarrelSpriteRenderer;
	[SerializeField] Transform m_Barrel;
	[SerializeField] Transform m_Hinge;
	[SerializeField] Rigidbody2D m_Rigidbody2D;
	[SerializeField] GameObject m_ProjectilePrefab;

	void Start()
	{
		m_Rigidbody2D.centerOfMass =
			m_Rigidbody2D.transform.InverseTransformPoint( m_Hinge.position );
	}

	public void Push( bool right )
	{
		m_Rigidbody2D.AddTorque( right ? m_PushPower : -m_PushPower );
	}

	public void Launch()
	{
		Shoot( Spawn() );
	}

	public void Launch( Color color )
	{
		var projectile = Spawn();
		SpriteRenderer spriteRenderer = projectile.GetComponent<SpriteRenderer>();
		spriteRenderer.color = color;
		m_BarrelSpriteRenderer.color = color;
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
			.AddRelativeForce( Vector2.down * m_ShootPower, ForceMode2D.Impulse );
	}
}