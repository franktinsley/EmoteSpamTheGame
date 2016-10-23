using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class BoardManager : MonoBehaviour
{
	const float m_SecondsBetweenShots = 0.05f;
	const float m_SecondsToLetPegsBounce = 2f;
	const int m_StartingNumberOfPegs = 200;

	[HideInInspector] public UnityEvent freezeBoard;

	public Cannon cannon;
	public Animator barrelMotor;
	public GameObject emotePrefab;
	public GameObject pegPrefab;
	public GameObject pegWalls;
	public Transform pegParent;
	public Transform emoteParent;

	public void CreateBoard()
	{
		StartCoroutine( ShootCoroutine() );
	}

	IEnumerator ShootCoroutine()
	{
		GameManager.singleton.allowMessages = false;
		barrelMotor.speed = 5f;
		cannon.transform.Translate( Vector3.down * 4f );
		yield return new WaitForSeconds( m_SecondsBetweenShots );
		for( int i = 0; i < m_StartingNumberOfPegs; i++ )
		{
			GameObject pegGameObject = Instantiate( pegPrefab );
			pegGameObject.transform.parent = pegParent;
			cannon.Shoot( pegGameObject );
			yield return new WaitForSeconds( m_SecondsBetweenShots );
		}
		yield return new WaitForSeconds( m_SecondsToLetPegsBounce );
		freezeBoard.Invoke();
		cannon.transform.Translate( Vector3.up * 4f );
		pegWalls.SetActive( false );
		barrelMotor.speed = 1f;
		GameManager.singleton.allowMessages = true;
		yield return null;
	}
}