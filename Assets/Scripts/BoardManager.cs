using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour
{
	public const int startingNumberOfPegs = 200;
	public const int startingNumberOfBumpers = 0;
	public const int startingNumberOfTriggers = 3;

	const float m_SecondsBetweenShots = 0.05f;
	const float m_SecondsBetweenBumperShots = 1f;
	const float m_SecondsToLetPegsBounce = 2f;

	[ HideInInspector ] public UnityEvent boardFrozen;
	[ HideInInspector ] public UnityEvent boardReset;

	public Turret turret;
	public Animator barrelMotor;
	public Animator boardAnimator;
	public int shotCost;
	public int pegPopScoreMin;
	public int pegPopScoreMax;
	public int bumperPopScore;
	public int triggerScore;
	public int bucketScore;
	public float rainbowCycleHue;
	public float rainbowCycleSpeedMin;
	public float rainbowCycleSpeedMax;
	public GameObject emotePrefab;
	public GameObject pegPrefab;
	public GameObject triggerPrefab;
	public GameObject jackpotTriggerPrefab;
	public GameObject bumperPrefab;
	public GameObject pointsLabelPrefab;
	public GameObject pegWalls;
	public Transform pegParent;
	public Transform emoteParent;
	public Text jackpot;

	public int jackpotScore
	{
		get
		{
			return m_JackpotScore;
		}
		set
		{
			m_JackpotScore = value;
			jackpot.text = m_JackpotLeadingText + value;
		}
	}

	int m_JackpotScore;
	string m_JackpotLeadingText;
	int m_NumberOfPegsPopped;

	void Start()
	{
		SetRainbowCycleSpeed();
		m_JackpotLeadingText = jackpot.text;
	}

	public void CreateBoard()
	{
		m_JackpotScore = 0;
		boardReset.Invoke();
		StartCoroutine( CreateBoardCoroutine() );
	}

	public void PegPopped( Peg pegPopped, User user )
	{
		m_NumberOfPegsPopped++;
		if( m_NumberOfPegsPopped == startingNumberOfPegs )
		{
			CreateBoard();
			m_NumberOfPegsPopped = 0;
		}
		SetRainbowCycleSpeed();
		int points = PegPopScore();
		user.ScorePoints( points );
		PointsLabel.InstantiatePointsLabelGameObject(
			points.ToString(), pegPopped.transform.position );
	}

	public void BumperPopped( Bumper bumper, User user )
	{
		user.ScorePoints( bumperPopScore );
		PointsLabel.InstantiatePointsLabelGameObject(
			bumperPopScore.ToString(), bumper.transform.position );
	}

	public void TriggerActivated( Trigger trigger, User user )
	{
		user.ScorePoints( triggerScore );
		PointsLabel.InstantiatePointsLabelGameObject(
			triggerScore.ToString(), trigger.transform.position );
	}

	public void JackpotTriggerActivated( JackpotTrigger jackpotTrigger, User user )
	{
		user.ScorePoints( m_JackpotScore );
		PointsLabel.InstantiatePointsLabelGameObject(
			m_JackpotScore.ToString(), jackpotTrigger.transform.position );
		CreateBoard();
	}

	public void BucketActivated( Bucket bucket, User user )
	{
		user.ScorePoints( bucketScore );
		PointsLabel.InstantiatePointsLabelGameObject(
			bucketScore.ToString(), bucket.transform.position );
	}

	int PegPopScore()
	{
		return ( int )Mathf.Lerp(
			( int )pegPopScoreMin, ( int )pegPopScoreMax, PegProgress() );
	}

	float PegProgress()
	{
		return Mathf.InverseLerp(
			0f, ( float )startingNumberOfPegs, ( float )m_NumberOfPegsPopped );
	}

	void SetRainbowCycleSpeed()
	{
		float rainbowCycleAnimatorSpeed =
			Mathf.Lerp( rainbowCycleSpeedMin, rainbowCycleSpeedMax, PegProgress() );
		boardAnimator.speed = rainbowCycleAnimatorSpeed;
	}

	IEnumerator CreateBoardCoroutine()
	{
		GameManager.singleton.allowMessages = false;
		barrelMotor.speed = 10f;
		pegWalls.SetActive( true );
		turret.transform.Translate( Vector3.down * 4f );
		var waitForSecondsBetweenShots = new WaitForSeconds( m_SecondsBetweenShots );
		yield return waitForSecondsBetweenShots;
		for( int i = 0; i < startingNumberOfBumpers; i++ )
		{
			GameObject bumperGameObject = Instantiate( bumperPrefab );
			bumperGameObject.transform.parent = pegParent;
			turret.Shoot( bumperGameObject );
			yield return new WaitForSeconds( m_SecondsBetweenBumperShots );
		}
		for( int i = 0; i < startingNumberOfTriggers; i++ )
		{
			GameObject triggerGameObject = Instantiate( triggerPrefab );
			triggerGameObject.transform.parent = pegParent;
			turret.Shoot( triggerGameObject );
			yield return new WaitForSeconds( m_SecondsBetweenBumperShots );
		}
		GameObject jackpotTriggerGameObject = Instantiate( jackpotTriggerPrefab );
		jackpotTriggerGameObject.transform.parent = pegParent;
		turret.Shoot( jackpotTriggerGameObject );
		yield return new WaitForSeconds( m_SecondsBetweenBumperShots );
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