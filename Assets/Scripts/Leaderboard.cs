using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ RequireComponent( typeof( Text ) ) ]
public class Leaderboard : MonoBehaviour
{
	public static Leaderboard singleton { get { return m_Singleton; } }
	static Leaderboard m_Singleton;

	public string leadingText = "Leaderboard:\n";
	public int count = 50;

	Text m_Text;
	SortedList<string, UserData> m_PlayerScores;

	public void UpdateScore( UserData userData )
	{
		if( !m_PlayerScores.ContainsKey( userData.userName ) )
		{
			m_PlayerScores.Add( userData.userName, userData );
		}

		m_Text.text = leadingText;
		int limit = count > m_PlayerScores.Count ? m_PlayerScores.Count : count;
		for( int i = 0; i < limit; i++ )
		{
			m_Text.text += ( i + 1 ) + " - " + m_PlayerScores.Values[ i ] + "\n";
		}
	}

	void Awake()
	{
		m_Singleton = this;
		m_PlayerScores = new SortedList<string, UserData>();
	}

	void Start()
	{
		m_Text = GetComponent<Text>();
	}
}