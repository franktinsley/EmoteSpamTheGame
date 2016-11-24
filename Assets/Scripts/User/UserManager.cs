using System.Collections.Generic;
using System.IO;
using TwitchChatter;
using UnityEngine;

public class UserManager : MonoBehaviour
{
	public Dictionary<string, User> users;
	public Transform usersParent;

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
		if( !users.TryGetValue( userName, out user ) )
		{
			string path = Application.persistentDataPath +
				usersDirectory + userName + jsonFileExtension;
			user = User.CreateInstance( userName, path, usersParent );
			users.Add( message.userName, user );
		}
		user.HandleMessage( message );
	}

	void LoadUserFiles()
	{
		if( !Directory.Exists( Application.persistentDataPath + usersDirectory ) )
		{
			Directory.CreateDirectory( Application.persistentDataPath + usersDirectory );
		}
		users = new Dictionary<string, User>();
		string[] userDataFilePaths = Directory.GetFiles(
			Application.persistentDataPath + usersDirectory, "*" + jsonFileExtension );
		if( userDataFilePaths != null && userDataFilePaths.Length > 0 )
		{
			foreach( var userDataFilePath in userDataFilePaths )
			{
				string userName = Path.GetFileNameWithoutExtension( userDataFilePath );
				User user = User.CreateInstance( userName, userDataFilePath, usersParent );
				users.Add( user.model.userName, user );
			}
		}
	}
}