using TwitchChatter;
using UnityEngine;

public class User : MonoBehaviour
{
	[SerializeField] public UserData userData;

	string m_UserDataFilePath;
	Cannon m_Cannon;

	public static User Initialize( string userName, string userDataFilePath, Transform parent )
	{
		var userGameObject = new GameObject( userName );
		userGameObject.transform.parent = parent;
		var user = userGameObject.AddComponent<User>();
		user.userData = JsonScriptableObject.LoadFromFile<UserData>( userDataFilePath );
		user.m_UserDataFilePath = userDataFilePath;
		return user;
	}

	public void HandleMessage( TwitchChatMessage message )
	{
		UpdateUserData( message );

		if( message.emoteData != null )
		{
			if( m_Cannon == null )
			{
				m_Cannon = GameManager.singleton.boardManager.cannon;
			}
			m_Cannon.ShootEmotes( message.emoteData, this );
		}
	}

	void UpdateUserData( TwitchChatMessage message )
	{
		userData.userName = message.userName;
		userData.userNameColor = message.userNameColor;
		userData.isSubscriber = message.isSubscriber;
		userData.isTurbo = message.isTurbo;
		userData.isMod = message.isMod;
	}

	void OnDisable()
	{
		JsonScriptableObject.SaveToFile<UserData>( userData, m_UserDataFilePath );
	}
}