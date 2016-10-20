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
	public Color userColor { get { return m_UserColor; } }

	Color m_UserColor;

	public void HandleMessage( TwitchChatMessage message )
	{
		ColorUtility.TryParseHtmlString( message.userNameColor, out m_UserColor );

		if( message.emoteData != null )
		{
			if( m_Cannon == null )
			{
				m_Cannon = GameManager.singleton.boardManager.cannon;
			}
			m_Cannon.ShootEmotes( message.emoteData, this );
		}
	}
}