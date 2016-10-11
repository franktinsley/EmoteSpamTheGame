using UnityEngine;

public class Launcher : MonoBehaviour
{
	public GameObject m_ProjectilePrefab;
	public float m_Power = 10f;

	public void Launch()
	{
		//Spawn projectile
		var projectile = Instantiate( m_ProjectilePrefab, transform.position, transform.rotation ) as GameObject;
		//Fire
		projectile.GetComponent<Rigidbody>().AddRelativeForce( Vector3.forward * m_Power, ForceMode.Impulse );
	}
}