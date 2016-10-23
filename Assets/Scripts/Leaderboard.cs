using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ RequireComponent( typeof( Text ) ) ]
public class Leaderboard : MonoBehaviour
{
	public static Leaderboard singleton { get { return m_Singleton; } }
	static Leaderboard m_Singleton;

	public string leadingText = "Leaderboard:\n";
	public int count = 20;

	UserManager m_UserManager;
	Text m_Text;
	List<User> m_Users;

	public void UpdateScore()
	{
		m_Users = new List<User>( m_UserManager.m_Users.Values );
		m_Users.Sort();
		m_Text.text = leadingText;
		for( int i = 0; i < count; i++ )
		{
			m_Text.text += ( i + 1 ) + " - " + m_Users[ i ] + "\n";
		}
	}

	void Awake()
	{
		m_Singleton = this;
	}

	void Start()
	{
		m_UserManager = GameManager.singleton.userManager;
		m_Text = GetComponent<Text>();
	}
}