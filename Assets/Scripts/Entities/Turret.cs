using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
	public Transform barrel;

	[ SerializeField ] float m_ShootForceMultiplier = 10f;
	[ SerializeField ] List<SpriteRenderer> m_SpriteRenderers;

	public void Shoot( GameObject projectile )
	{
		var emote = projectile.GetComponentInChildren<Emote>();
		if( emote != null )
		{
			Color userNameColor;
			ColorUtility.TryParseHtmlString(
				emote.owner.model.userNameColor, out userNameColor );
			TintSpriteRenderers( userNameColor );
		}
		projectile.transform.position = new Vector3(
			barrel.position.x,
			barrel.position.y,
			projectile.transform.position.z );
		projectile.transform.rotation = barrel.rotation;
		projectile.SetActive( true );
		Rigidbody2D projectileRigidbody =
			projectile.GetComponent<Rigidbody2D>();
		projectileRigidbody.AddRelativeForce(
			Vector2.down * m_ShootForceMultiplier, ForceMode2D.Impulse );
	}

	void TintSpriteRenderers( Color color )
	{
		foreach( var _spriteRenderer in m_SpriteRenderers )
		{
			_spriteRenderer.color = color;
		}
	}
}