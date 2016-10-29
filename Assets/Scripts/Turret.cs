﻿using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
	[ SerializeField ] float m_ShootForceMultiplier = 10f;
	[ SerializeField ] float m_ProjectileZPosition = 2f;
	[ SerializeField ] List<SpriteRenderer> m_SpriteRenderers;
	[ SerializeField ] Transform m_Barrel;

	public void Shoot( GameObject projectile )
	{
		var emote = projectile.GetComponentInChildren<Emote>();
		if( emote != null )
		{
			Color userNameColor;
			ColorUtility.TryParseHtmlString(
				emote.owner.userData.userNameColor, out userNameColor );
			TintSpriteRenderers( userNameColor );
		}
		projectile.transform.position = new Vector3(
			m_Barrel.position.x,
			m_Barrel.position.y,
			m_ProjectileZPosition );
		projectile.transform.rotation = m_Barrel.rotation;
		projectile.GetComponent<HiddenGameObject>().isHidden = false;
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