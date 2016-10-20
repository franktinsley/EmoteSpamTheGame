using System.Collections.Generic;
using TwitchChatter;
using UnityEngine;

public class UserManager : MonoBehaviour
{
	Dictionary<string, User> m_Users;

	const string usersDirectoryPath = "/Users/";
	const string fileExtension = ".json";

	void Start()
	{
		/*string path = Application.persistentDataPath + usersDirectoryPath;
		if( !Directory.Exists( path ) )
		{
			Directory.CreateDirectory( path );
		}
		path += user.userName + fileExtension;

		string path = Application.persistentDataPath + usersDirectoryPath + userName + fileExtension;*/
		// need to get all the saved UserData and use it to construct all the
		// User GameObjects
		m_Users = new Dictionary<string, User>();
	}

	void LoadUsers()
	{
		/*string[] paths = Directory.GetFiles( Application.persistentDataPath + usersDirectoryPath );
		foreach( var path in paths )
		{
			UserData userData = UserData.LoadUserDataFromFile( path );

		}*/
		// get all the user files
		// create each userdata instance and user object and set everything
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