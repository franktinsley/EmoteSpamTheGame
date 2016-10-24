using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class BoardManager : MonoBehaviour
{
	public const int startingNumberOfPegs = 100;

	const float m_SecondsBetweenShots = 0.05f;
	const float m_SecondsToLetPegsBounce = 2f;

	[ HideInInspector ] public UnityEvent boardFrozen;
	[ HideInInspector ] public IntEvent pegPopped;
	[ HideInInspector ] public UnityEvent boardReset;

	public Turret turret;
	public Animator barrelMotor;
	public GameObject emotePrefab;
	public GameObject pegPrefab;
	public GameObject pegWalls;
	public Transform pegParent;
	public Transform emoteParent;

	int m_remainingNumberOfPegs;

	void Start ()
	{
		if( pegPopped == null )
		{
			pegPopped = new IntEvent();
		}
	}

	public void CreateBoard()
	{
		boardReset.Invoke();
		StartCoroutine( CreateBoardCoroutine() );
		m_remainingNumberOfPegs = startingNumberOfPegs;
	}

	public void FirePegPopped()
	{
		m_remainingNumberOfPegs--;
		if( m_remainingNumberOfPegs < 1 )
		{
			CreateBoard();
		}
		else
		{
			pegPopped.Invoke( m_remainingNumberOfPegs );
		}
	}

	IEnumerator CreateBoardCoroutine()
	{
		GameManager.singleton.allowMessages = false;
		barrelMotor.speed = 5f;
		pegWalls.SetActive( true );
		turret.transform.Translate( Vector3.down * 4f );
		yield return new WaitForSeconds( m_SecondsBetweenShots );
		for( int i = 0; i < startingNumberOfPegs; i++ )
		{
			GameObject pegGameObject = Instantiate( pegPrefab );
			pegGameObject.transform.parent = pegParent;
			turret.Shoot( pegGameObject );
			yield return new WaitForSeconds( m_SecondsBetweenShots );
		}
		yield return new WaitForSeconds( m_SecondsToLetPegsBounce );
		boardFrozen.Invoke();
		turret.transform.Translate( Vector3.up * 4f );
		pegWalls.SetActive( false );
		barrelMotor.speed = 1f;
		GameManager.singleton.allowMessages = true;
		yield return null;
	}
}