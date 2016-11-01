using UnityEngine;

public class Bucket : MonoBehaviour
{
	const string activatedTriggerName = "Activated";

	[ SerializeField ] Animator animator;

	BoardManager m_BoardManager;

	void Start()
	{
		m_BoardManager = GameManager.singleton.boardManager;
	}

	void OnTriggerEnter2D( Collider2D otherCollider )
	{
		var emote = otherCollider.gameObject.GetComponent<Emote>();
		if( emote != null )
		{
			var user = emote.owner;
			animator.SetTrigger( activatedTriggerName );
			m_BoardManager.BucketHit( user );
		}
	}
}