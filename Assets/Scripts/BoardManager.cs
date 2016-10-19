using UnityEngine;

public class BoardManager : MonoBehaviour
{
	public Cannon cannon { get { return m_Cannon; } }

	[SerializeField] Cannon m_Cannon;
	[SerializeField] GameObject m_PegPrefab;
	//[SerializeField] int m_StartingNumberOfPegs = 30;

	public void CreateBoard()
	{
		
	}
}