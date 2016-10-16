using UnityEngine;

public class Pit : MonoBehaviour
{
	void OnTriggerEnter2D( Collider2D collider )
	{
		Destroy( collider.gameObject );
	}
}