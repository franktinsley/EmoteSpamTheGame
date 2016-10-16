using System.IO;
using UnityEngine;

public class RandomTests : MonoBehaviour
{
	void Start()
	{
		var testUser = ScriptableObject.CreateInstance<User>();
		testUser.userName = "franktinsley";
		testUser.isMod = false;
		User.SaveUserToFile( testUser );
	}
}