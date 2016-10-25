using UnityEngine;
using System;

public class UserData : ScriptableObject, IComparable<UserData>
{
	public string userName;
	public string userNameColor;
	public bool isSubscriber;
	public bool isTurbo;
	public bool isMod;
	public int score;

	public int CompareTo( UserData other )
	{
		return score == other.score ?
			userName.CompareTo( other.userName ) :
			other.score.CompareTo( score );
	}

	public override string ToString()
	{
		return "<color=" + userNameColor + ">" + userName + "</color>: " + score;
	}
}