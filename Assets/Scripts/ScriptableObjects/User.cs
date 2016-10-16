using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class User : ScriptableObject
{
	public string userName;
	public string userNameColor;
	public bool isSubscriber;
	public bool isTurbo;
	public bool isMod;

	const string usersDirectoryPath = "/Users";

	public static void SaveUserToFile( User user )
	{
		string path = Application.persistentDataPath + usersDirectoryPath;
		if( !Directory.Exists( path ) )
		{
			Directory.CreateDirectory( path );
		}
		path += "/" + user.userName + ".json";
		string jsonUser = JsonUtility.ToJson( user );
		File.WriteAllText( path, jsonUser );
	}

	/*public static User LoadUserFromFile( string userName )
	{
		
	}*/
}