using UnityEngine;

public class OverlapArea : MonoBehaviour
{
	void OnTriggerExit2D( Collider2D otherCollider )
	{
		otherCollider.isTrigger = false;
	}
}