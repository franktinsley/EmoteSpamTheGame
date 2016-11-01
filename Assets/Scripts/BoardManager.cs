﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoardManager : MonoBehaviour
{
	public int startingNumberOfPegs = 99;
	public Turret turret;
	public Animator barrelMotor;
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
		m_Pegs.Clear();
		boardReset.Invoke();
		StartCoroutine( CreateBoardCoroutine() );
	}

	public void PegHit( Peg peg, Emote emote )
	{
		User user = emote.owner;
		if( peg.owner == user )
		{
			peg.SetOwner( null );
		}
		else
		{
			peg.SetOwner( user );
		}
	}

	public void BucketHit( User user )
	{
		foreach( var peg in m_Pegs )
		{
			if( peg.owner == user )
			{
				peg.Pop();
				user.points++;
			}
		}
		m_Pegs.RemoveAll( peg => peg.owner == user );
		if( user.points > ( startingNumberOfPegs / 2 ) )
		{
			UserWins( user );
			CreateBoard();
		}
	}

	void UserWins( User user )
	{
		
	}

	IEnumerator CreateBoardCoroutine()
	{
		barrelMotor.speed = 10f;
		pegWalls.SetActive( true );
		turret.transform.Translate( Vector3.down * 4f );
		var waitForSecondsBetweenShots = new WaitForSeconds( m_SecondsBetweenShots );
		yield return waitForSecondsBetweenShots;
		for( int i = 0; i < startingNumberOfPegs; i++ )
		{
			GameObject pegGameObject = Instantiate( pegPrefab );
			m_Pegs.Add( pegGameObject.GetComponent<Peg>() );
			pegGameObject.transform.parent = pegParent;
			turret.Shoot( pegGameObject );
			yield return waitForSecondsBetweenShots;
		}
		yield return new WaitForSeconds( m_SecondsToLetPegsBounce );
		turret.transform.Translate( Vector3.up * 4f );
		pegWalls.SetActive( false );
		barrelMotor.speed = 1f;
		boardFrozen.Invoke();
		yield return null;
	}
}