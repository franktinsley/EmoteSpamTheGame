using UnityEngine;

public class Launcher2DTest : MonoBehaviour
{
	[SerializeField] Launcher2D m_Launcher;
	[SerializeField] KeyCode m_LaunchTestKey;

	void Update()
	{
		if( Input.GetKeyDown( m_LaunchTestKey ) )
		{
			m_Launcher.Launch();
		}
	}
}