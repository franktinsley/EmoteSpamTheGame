﻿using TwitchChatter;
using UnityEngine;

public class Launcher2D : MonoBehaviour
{
	public GameObject ProjectilePrefab
	{
		get
		{
			return m_ProjectilePrefab;
		}
		set
		{
			m_ProjectilePrefab = value;
		}
	}

	public float Power
	{
		get
		{
			return m_Power;
		}
		set
		{
			m_Power = value;
		}
	}

	[SerializeField] GameObject m_ProjectilePrefab;
	[SerializeField] float m_Power = 10f;

	[SerializeField] Transform m_Barrel;

	public void Launch()
	{
		Shoot( Spawn() );
	}

	public void Launch( TwitchChatMessage message )
	{
		string hexColor = message.userNameColor;
		var projectile = Spawn();
		SpriteRenderer spriteRenderer = projectile.GetComponent<SpriteRenderer>();
		Color color;
		ColorUtility.TryParseHtmlString( hexColor, out color );
		spriteRenderer.color = color;
		Shoot( projectile );
	}

	GameObject Spawn()
	{
		return Instantiate(
			m_ProjectilePrefab,
			m_Barrel.position,
			m_Barrel.rotation
		) as GameObject;
	}

	void Shoot( GameObject projectile )
	{
		projectile.GetComponent<Rigidbody2D>()
			.AddRelativeForce( Vector2.down * m_Power, ForceMode2D.Impulse );
	}
}