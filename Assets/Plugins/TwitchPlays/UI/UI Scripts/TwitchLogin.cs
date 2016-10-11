using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TwitchLogin : MonoBehaviour {

	public InputField user;
	public InputField oauth;

	TwitchIRC irc;

	bool connected;

	public GameObject LoginPanel;


	IEnumerator Start()
	{
		yield return true;
		user.text = PlayerPrefs.GetString("user");
		oauth.text = PlayerPrefs.GetString("auth");
		irc = FindObjectOfType<TwitchIRC>();
		if(user.text.Length > 2)
			Submit();
	}

	public void Submit ()
	{
		if(irc == null)
			Debug.LogError("No IRC client Found, make sure the \'TwitchPlays Client\' prefab is in the scene!");
		else
		{
			irc.Login(user.text, oauth.text);
			irc.Connected += Connected;
			StopCoroutine("reconnect");
			StartCoroutine("reconnect");
		}
	}

	void Connected()
	{
		connected = true;
		Debug.Log("Connected to Chat");
	}

	void Update()
	{
		if(connected)
		{
			PlayerPrefs.SetString("user", user.text);
			PlayerPrefs.SetString("auth", oauth.text);
			PlayerPrefs.Save();
			if(LoginPanel != null)
				LoginPanel.SetActive(false);
		}
			
	}

	public void Disconnect()
	{
		 connected = false;
	}

	IEnumerator reconnect()
	{
		yield return new WaitForSeconds(5.0f);
		if(!connected)
		{
			Debug.Log("Failed to connect");
		}
	}

}
