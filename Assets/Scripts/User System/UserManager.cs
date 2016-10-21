using System.Collections.Generic;
using System.IO;
using TwitchChatter;
using UnityEngine;

public class UserManager : MonoBehaviour
{
	UserList m_UserList;
	Dictionary<string, User> m_Users;
	string m_UserListFilePath;

	const string usersDirectory = "/Users/";
	const string userListFileName = "User List";
	const string jsonFileExtension = ".json";

	void Start()
	{
		LoadUserList();
	}

	void OnDisable()
	{
		SaveUserList();
	}

	public void HandleChatMessage( TwitchChatMessage message )
	{
		string userName = message.userName;
		User messageUser;
		if( !m_Users.TryGetValue( userName, out messageUser ) )
		{
			string path = Application.persistentDataPath +
				usersDirectory + userName + jsonFileExtension;
			messageUser = User.Initialize( userName, path, transform );
			m_Users.Add( message.userName, messageUser );
		}
		messageUser.HandleMessage( message );
	}

	void LoadUserList()
	{
		if( !Directory.Exists( Application.persistentDataPath + usersDirectory ) )
		{
			Directory.CreateDirectory( Application.persistentDataPath + usersDirectory );
		}
		m_Users = new Dictionary<string, User>();
		m_UserListFilePath = Application.persistentDataPath + usersDirectory +
			userListFileName + jsonFileExtension;
		m_UserList = JsonScriptableObject.
			LoadFromFile<UserList>( m_UserListFilePath );
		if( m_UserList != null &&
			m_UserList.userNames != null &&
			m_UserList.userNames.Length > 0 )
		{
			foreach( var userName in m_UserList.userNames )
			{
				string userDataFilePath = Application.persistentDataPath +
					usersDirectory + userName + jsonFileExtension;
				User user = User.Initialize( userName, userDataFilePath, transform );
				m_Users.Add( userName, user );
			}
		}
	}

	void SaveUserList()
	{
		var userNames = new string[ m_Users.Keys.Count ];
		m_Users.Keys.CopyTo( userNames, 0 );
		m_UserList.userNames = userNames;
		JsonScriptableObject.SaveToFile<UserList>( m_UserList, m_UserListFilePath );
	}
}