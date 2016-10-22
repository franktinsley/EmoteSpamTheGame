using UnityEngine;
using UnityEngine.Events;

public class BoardManager : MonoBehaviour
{
	[HideInInspector] public UnityEvent freezeBoard;

	public Cannon cannon;
	public GameObject emotePrefab;
	public GameObject pegPrefab;

	[SerializeField] int m_StartingNumberOfPegs = 30;

	public void CreateBoard()
	{
		for( int i = 0; i < m_StartingNumberOfPegs; i++ )
		{
		}
	}
}