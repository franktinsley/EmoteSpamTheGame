using System.Collections.Generic;
using TwitchChatter;
using UnityEngine;

public class UserManager : MonoBehaviour
{
	Dictionary<string, User> m_Users;

	void Start()
	{
		m_Users = new Dictionary<string, User>();
	}

	public void HandleChatMessage( TwitchChatMessage message )
	{
		User messageUser;
		if( !m_Users.TryGetValue( message.userName, out messageUser ) )
		{
			// This should probably be part of a static function on User that handles its own creation and
			// is passed all the intial crap
			var userGameObject = new GameObject( message.userName );
			userGameObject.transform.parent = transform;
			messageUser = userGameObject.AddComponent<User>();
			m_Users.Add( message.userName, messageUser );
		}
		messageUser.HandleMessage( message );
	}
}