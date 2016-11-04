using UnityEngine;

public class HiddenGameObject : MonoBehaviour
{
	[ SerializeField ] bool m_StartHidden;
	[ SerializeField ] bool m_SetRotation;
	[ SerializeField ] float m_RotationZAngle;
	[ SerializeField ] float m_GravityScale;
	[ SerializeField ] bool m_UseAutoMass;
	[ SerializeField ] RigidbodyConstraints2D m_Constraints;
	[ SerializeField ] CollisionDetectionMode2D m_CollisionDetectionMode;

	bool m_IsHidden;

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
				if( m_SetRotation )
				{
					Vector3 eulerAngles = transform.rotation.eulerAngles;
					eulerAngles = new Vector3( eulerAngles.x, eulerAngles.y, m_RotationZAngle );
					transform.rotation = Quaternion.Euler( eulerAngles );
				}
				if( rigidbody2D == null )
				{
					rigidbody2D = gameObject.AddComponent<Rigidbody2D>();
					rigidbody2D.useAutoMass = m_UseAutoMass;
					rigidbody2D.gravityScale = m_GravityScale;
					rigidbody2D.constraints = m_Constraints;
					rigidbody2D.collisionDetectionMode = m_CollisionDetectionMode;
				}
			}
			m_IsHidden = value;
		}
	}

	void Awake()
	{
		isHidden = m_StartHidden;
	}
}