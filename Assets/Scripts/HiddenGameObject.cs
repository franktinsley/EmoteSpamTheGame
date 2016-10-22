using UnityEngine;

public class HiddenGameObject : MonoBehaviour
{
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
				if( rigidbody2D == null )
				{
					gameObject.AddComponent<Rigidbody2D>();
				}
			}
			m_IsHidden = value;
		}
	}

	bool m_IsHidden;
}