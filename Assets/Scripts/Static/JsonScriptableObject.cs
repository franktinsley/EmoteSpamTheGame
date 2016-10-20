using System.IO;
using UnityEngine;

public static class JsonScriptableObject
{
	public static T LoadFromFile<T>( string path ) where T : ScriptableObject
	{
		T instance = ScriptableObject.CreateInstance<T>();
		if( File.Exists( path ) )
		{
			string json = File.ReadAllText( path );
			JsonUtility.FromJsonOverwrite( json, instance );
		}
		return instance;
	}

	public static void SaveToFile<T>( T instance, string path ) where T : ScriptableObject
	{
		string json = JsonUtility.ToJson( instance );
		File.WriteAllText( path, json );
	}
}