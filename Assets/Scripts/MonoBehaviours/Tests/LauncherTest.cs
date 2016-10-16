using UnityEngine;

public class LauncherTest : MonoBehaviour
{
	[SerializeField] Launcher m_Launcher;
	[SerializeField] KeyCode m_LaunchTestKey;

	void Update()
	{
		if( Input.GetKeyDown( m_LaunchTestKey ) )
		{
			m_Launcher.Launch();
		}
	}
}