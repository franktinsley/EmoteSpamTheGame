using System.Collections;
using System.Collections.Generic;
using TwitchChatter;
using UnityEngine;

public class Cannon : MonoBehaviour
{
	[SerializeField] float m_SecondsBetweenShots = 0.2f;
	[SerializeField] float m_ShootForceMultiplier = 10f;
	[SerializeField] float m_ProjectileZPosition = 2f;
	[SerializeField] List<SpriteRenderer> m_SpriteRenderers;
	[SerializeField] Transform m_Barrel;
	[SerializeField] GameObject m_ProjectilePrefab;

	Queue<Shot> m_Shots = new Queue<Shot>();
	bool m_Shooting;

	public void ShootEmotes( TwitchChatMessage.EmoteData[] emoteData, User user )
	{
		EnqueueEmoteShots( emoteData, user );
		if( !m_Shooting )
		{
			StartCoroutine( ShootCoroutine() );
		}
	}

	void Shoot( Shot shot )
	{
		GameObject projectile = Spawn();
		Emote emote = projectile.GetComponentInChildren<Emote>();
		emote.SetEmote( shot.emoteID );
		Color userNameColor;
		ColorUtility.TryParseHtmlString( shot.user.userData.userNameColor, out userNameColor );
		TintSpriteRenderers( userNameColor );
		Rigidbody2D projectileRigidbody =
			projectile.GetComponent<Rigidbody2D>();
		projectileRigidbody.AddRelativeForce(
			Vector2.down * m_ShootForceMultiplier, ForceMode2D.Impulse );
	}

	IEnumerator ShootCoroutine()
	{
		while( m_Shots.Count > 0 )
		{
			m_Shooting = true;
			Shoot( m_Shots.Dequeue() );
			yield return new WaitForSeconds( m_SecondsBetweenShots );
		}
		m_Shooting = false;
		yield return null;
	}

	GameObject Spawn()
	{
		return Instantiate(
			m_ProjectilePrefab,
			new Vector3(
				m_Barrel.position.x,
				m_Barrel.position.y,
				m_ProjectileZPosition ),
			m_Barrel.rotation ) as GameObject;
	}
		
	void EnqueueEmoteShots( TwitchChatMessage.EmoteData[] emoteData, User user )
	{
		foreach( var emote in emoteData )
		{
			var shot = new Shot( user, emote.id );
			m_Shots.Enqueue( shot );
		}
	}

	void TintSpriteRenderers( Color color )
	{
		foreach( var _spriteRenderer in m_SpriteRenderers )
		{
			_spriteRenderer.color = color;
		}
	}
}