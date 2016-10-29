using UnityEngine;

public class HiddenGameObject : MonoBehaviour
{
	public bool setRotation;
	public float rotationZAngle;
	public float gravityScale;
	public RigidbodyConstraints2D constraints;
	public CollisionDetectionMode2D collisionDetectionMode;

	public bool isHidden
	{
		get
		{
			return m_IsHidden;
		}
		set
		{
			var spriteRenderer = GetComponent<SpriteRenderer>();
			if( spriteRenderer != null )
			{
				spriteRenderer.enabled = !value;
			}
			var collider2D = GetComponent<Collider2D>();
			if( collider2D != null )
			{
				collider2D.enabled = !value;
			}
			var rigidbody2D = GetComponent<Rigidbody2D>();
			if( value )
			{
				if( rigidbody2D != null )
				{
					Destroy( rigidbody2D );
				}
			}
			else
			{
				if( setRotation )
				{
					Vector3 eulerAngles = transform.rotation.eulerAngles;
					eulerAngles = new Vector3( eulerAngles.x, eulerAngles.y, rotationZAngle );
					transform.rotation = Quaternion.Euler( eulerAngles );
				}
				if( rigidbody2D == null )
				{
					rigidbody2D = gameObject.AddComponent<Rigidbody2D>();
					rigidbody2D.gravityScale = gravityScale;
					rigidbody2D.constraints = constraints;
					rigidbody2D.collisionDetectionMode = collisionDetectionMode;
				}
			}
			m_IsHidden = value;
		}
	}

	[ SerializeField ] bool m_StartHidden;

	bool m_IsHidden;

	void Awake()
	{
		isHidden = m_StartHidden;
	}
}