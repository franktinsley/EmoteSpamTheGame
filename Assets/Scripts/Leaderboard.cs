using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ RequireComponent( typeof( Text ) ) ]
public class Leaderboard : MonoBehaviour
{
	public string leadingText = "Leaderboard:\n";
	public int count = 50;

	Text m_Text;
	List<UserData> m_PlayerScores;

	void Awake()
	{
		m_Text = GetComponent<Text>();
		m_PlayerScores = new List<UserData>();
	}

	public void UpdateScore( UserData userData )
	{
		if( !m_PlayerScores.Contains( userData ) )
		{
			m_PlayerScores.Add( userData );
		}
		m_PlayerScores.Sort();

		m_Text.text = leadingText;
		int limit = count > m_PlayerScores.Count ? m_PlayerScores.Count : count;
		for( int i = 0; i < limit; i++ )
		{
			m_Text.text += ( i + 1 ) + " - " + m_PlayerScores[ i ] + "\n";
		}
	}
}