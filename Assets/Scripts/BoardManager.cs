using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoardManager : MonoBehaviour
{
	public bool allowUserShooting;
	public int startingNumberOfPegs = 99;
	public Turret turret;
	//public Animator barrelMotor;
	public GameObject emotePrefab;
	public GameObject pegPrefab;
	public GameObject pointsLabelPrefab;
	public GameObject pegWalls;
	public Transform pegParent;
	public Transform emoteParent;

	[ HideInInspector ] public UnityEvent boardFrozen;
	[ HideInInspector ] public UnityEvent boardReset;

	[ SerializeField ] float m_SecondsBetweenShots = 0.05f;
	[ SerializeField ] float m_SecondsToLetPegsBounce = 2f;

	List<Peg> m_Pegs;

	void Awake()
	{
		m_Pegs = new List<Peg>();
	}

	public void CreateBoard()
	{
		StartCoroutine( CreateBoardCoroutine() );
	}

	IEnumerator CreateBoardCoroutine()
	{
		allowUserShooting = false;
		m_Pegs.Clear();
		boardReset.Invoke();
		//barrelMotor.speed = 10f;
		pegWalls.SetActive( true );
		turret.transform.Translate( Vector3.down * 4f );
		var waitForSecondsBetweenShots = new WaitForSeconds( m_SecondsBetweenShots );
		yield return waitForSecondsBetweenShots;
		for( int i = 0; i < startingNumberOfPegs; i++ )
		{
			var pegGameObject = Instantiate( pegPrefab );
			pegGameObject.transform.SetParent( pegParent, true );
			m_Pegs.Add( pegGameObject.GetComponent<Peg>() );
			turret.Shoot( pegGameObject );
			yield return waitForSecondsBetweenShots;
		}
		yield return new WaitForSeconds( m_SecondsToLetPegsBounce );
		turret.transform.Translate( Vector3.up * 4f );
		pegWalls.SetActive( false );
		//barrelMotor.speed = 1f;
		allowUserShooting = true;
		boardFrozen.Invoke();
		yield return null;
	}
}