using System.Collections.Generic;
using System.IO;
using TwitchChatter;
using UnityEngine;

public class UserManager : MonoBehaviour
{
	public Dictionary<string, User> m_Users;

	const string usersDirectory = "/Users/";
	const string jsonFileExtension = ".json";

	void Start()
	{
		LoadUserFiles();
	}

	public void HandleChatMessage( TwitchChatMessage message )
	{
		string userName = message.userName;
		User user;
		if( !m_Users.TryGetValue( userName, out user ) )
		{
			string path = Application.persistentDataPath +
				usersDirectory + userName + jsonFileExtension;
			user = User.CreateInstance( userName, path, transform );
			m_Users.Add( message.userName, user );
		}
		user.HandleMessage( message );
	}

	void LoadUserFiles()
	{
		if( !Directory.Exists( Application.persistentDataPath + usersDirectory ) )
		{
			Directory.CreateDirectory( Application.persistentDataPath + usersDirectory );
		}
		m_Users = new Dictionary<string, User>();
		string[] userDataFilePaths = Directory.GetFiles( Application.persistentDataPath + usersDirectory );
		if( userDataFilePaths != null && userDataFilePaths.Length > 0 )
		{
			foreach( var userDataFilePath in userDataFilePaths )
			{
				string userName = Path.GetFileNameWithoutExtension( userDataFilePath );
				User user = User.CreateInstance( userName, userDataFilePath, transform );
				m_Users.Add( user.userData.userName, user );
			}
		}
	}
}