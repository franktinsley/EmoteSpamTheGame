using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Turret : MonoBehaviour
{
	public Transform muzzle;

	[ SerializeField ] float m_ShootForceMultiplier = 10f;
	[ SerializeField ] List<Image> m_Images;

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
			muzzle.position.x,
			muzzle.position.y,
			projectile.transform.position.z );
		projectile.transform.rotation = muzzle.rotation;
		projectile.GetComponent<HiddenGameObject>().isHidden = false;
		Rigidbody2D projectileRigidbody =
			projectile.GetComponent<Rigidbody2D>();
		projectileRigidbody.AddRelativeForce(
			Vector2.down * m_ShootForceMultiplier, ForceMode2D.Impulse );
	}

	void TintSpriteRenderers( Color color )
	{
		foreach( var image in m_Images )
		{
			image.color = color;
		}
	}
}