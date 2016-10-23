using System;

public class PlayerScore : IComparable<PlayerScore>
{
	public int score;
	public string name;

	public int CompareTo( PlayerScore other )
	{
		if( score == other.score )
		{
			return name.CompareTo( other.name );
		}

		return other.score.CompareTo( score );
	}

	public override string ToString()
	{
		return name + " - " + score;
	}
}