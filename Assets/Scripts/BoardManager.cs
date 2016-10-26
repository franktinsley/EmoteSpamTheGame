using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class BoardManager : MonoBehaviour
{
	public const int startingNumberOfPegs = 100;

	const float m_SecondsBetweenShots = 0.05f;
	const float m_SecondsToLetPegsBounce = 2f;

	[ HideInInspector ] public UnityEvent boardFrozen;
	[ HideInInspector ] public UnityEvent boardReset;

	public Turret turret;
	public Animator barrelMotor;
	public Animator boardAnimator;
	public float rainbowCycleHue;
	public float rainbowCycleSpeedMin;
	public float rainbowCycleSpeedMax;
	public GameObject emotePrefab;
	public GameObject pegPrefab;
	public GameObject pegWalls;
	public Transform pegParent;
	public Transform emoteParent;

	int m_NumberOfPegsPopped;

	void Start()
	{
		SetRainbowCycleSpeed();
	}

	public void CreateBoard()
	{
		boardReset.Invoke();
		StartCoroutine( CreateBoardCoroutine() );
	}

	public void PegPopped( Peg pegPopped )
	{
		m_NumberOfPegsPopped++;
		if( m_NumberOfPegsPopped == startingNumberOfPegs )
		{
			CreateBoard();
			m_NumberOfPegsPopped = 0;
		}
		SetRainbowCycleSpeed();
	}

	void SetRainbowCycleSpeed()
	{
		float pegProgress =
			Mathf.InverseLerp( 0f, ( float )startingNumberOfPegs, ( float )m_NumberOfPegsPopped );
		float rainbowCycleAnimatorSpeed =
			Mathf.Lerp( rainbowCycleSpeedMin, rainbowCycleSpeedMax, pegProgress );
		boardAnimator.speed = rainbowCycleAnimatorSpeed;
	}

	IEnumerator CreateBoardCoroutine()
	{
		GameManager.singleton.allowMessages = false;
		barrelMotor.speed = 5f;
		pegWalls.SetActive( true );
		turret.transform.Translate( Vector3.down * 4f );
		var waitForSecondsBetweenShots = new WaitForSeconds( m_SecondsBetweenShots );
		yield return waitForSecondsBetweenShots;
		for( int i = 0; i < startingNumberOfPegs; i++ )
		{
			GameObject pegGameObject = Instantiate( pegPrefab );
			pegGameObject.transform.parent = pegParent;
			turret.Shoot( pegGameObject );
			yield return waitForSecondsBetweenShots;
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