using UnityEngine;

public class Shot
{
	public User user;
	public int emoteID;

	public Shot( User user, int emoteID )
	{
		this.user = user;
		this.emoteID = emoteID;
	}
}