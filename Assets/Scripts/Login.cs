using UnityEngine;
using System.Collections;

public class Login : MonoBehaviour
{
	TwitchIRC m_IRC;
	string m_User;
	string m_OAuth;
	bool connected;

	IEnumerator Start()
	{
		yield return true;
		m_User = PlayerPrefs.GetString( "user" );
		m_OAuth = PlayerPrefs.GetString( "auth" );
		m_IRC = FindObjectOfType<TwitchIRC>();
		if( m_User.Length > 2 )
			Submit();
	}

	public void Submit()
	{
		if( m_IRC == null )
			Debug.LogError( "No IRC client Found, make sure the \'TwitchPlays Client\' prefab is in the scene!" );
		else
		{
			m_IRC.Login( m_User, m_OAuth );
			m_IRC.Connected += Connected;
			StopCoroutine( "reconnect" );
			StartCoroutine( "reconnect" );
		}
	}

	void Connected()
	{
		connected = true;
		m_IRC.SendCommand( "CAP REQ :twitch.tv/tags" );
		Debug.Log( "Connected to Chat" );
	}

	void Update()
	{
		if( connected )
		{
			PlayerPrefs.SetString( "user", m_User );
			PlayerPrefs.SetString( "auth", m_OAuth );
			PlayerPrefs.Save();
		}
	}

	public void Disconnect()
	{
		connected = false;
	}

	IEnumerator reconnect()
	{
		yield return new WaitForSeconds( 5f );
		if( !connected )
		{
			Debug.Log( "Failed to connect" );
		}
	}
}