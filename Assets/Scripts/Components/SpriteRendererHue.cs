using UnityEngine;

[ RequireComponent( typeof( SpriteRenderer ) ) ]
public class SpriteRendererHue : MonoBehaviour
{
	public float m_Hue;

	SpriteRenderer m_SpriteRenderer;

	void Start()
	{
		m_SpriteRenderer = GetComponent<SpriteRenderer>();
	}

	void Update()
	{
		Color color = m_SpriteRenderer.color;
		m_SpriteRenderer.color = color.WithHue( m_Hue );
	}
}