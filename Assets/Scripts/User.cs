using System.Collections;
using System.Collections.Generic;
using TwitchChatter;
using UnityEngine;

public class User : MonoBehaviour
{
	public UserData m_UserData;
	public Queue<int> m_Emotes = new Queue<int>();

	Cannon m_Cannon;
	bool m_Shooting;

	// put these in userdata
	Color m_UserColor;
	float m_ShotCooldown = 0.4f;

	public void HandleMessage( TwitchChatMessage message )
	{
		ColorUtility.TryParseHtmlString( message.userNameColor, out m_UserColor );

		if( message.emoteData != null )
		{
			if( m_Cannon == null )
			{
				m_Cannon = GameManager.singleton.boardManager.cannon;
			}
			foreach( var emote in message.emoteData )
			{
				m_Emotes.Enqueue( emote.id );
			}
			if( !m_Shooting )
			{
				StartCoroutine( Shoot() );
			}
		}
	}
		
	IEnumerator Shoot()
	{
		while( m_Emotes.Count > 0 )
		{
			m_Shooting = true;
			m_Cannon.Shoot( m_UserColor, m_Emotes.Dequeue() );
			yield return new WaitForSeconds( m_ShotCooldown );
		}
		m_Shooting = false;
		yield return null;
	}
}