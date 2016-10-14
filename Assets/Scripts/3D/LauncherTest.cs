using UnityEngine;

public class LauncherTest : MonoBehaviour
{
	public Launcher m_Launcher;
	public KeyCode m_LaunchTestKey;

	void Update()
	{
		if( Input.GetKeyDown( m_LaunchTestKey ) )
		{
			m_Launcher.Launch();
		}
	}
}