using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour
{
	public int startingNumberOfPegs = 200;
	public int startingNumberOfTriggers = 3;
	public int startingNumberOfBumpers;
	public Turret turret;
	public Animator barrelMotor;
	public Animator boardAnimator;
	public int shotCost;
	public int pegPopScoreMin;
	public int pegPopScoreMax;
	public int bumperPopScore;
	public int triggerScore;
	public int bucketScore;
	public int jackpotIncrement;
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

	[ HideInInspector ] public UnityEvent boardFrozen;
	[ HideInInspector ] public UnityEvent boardReset;

	[ SerializeField ] float m_SecondsBetweenShots = 0.05f;
	[ SerializeField ] float m_SecondsBetweenBumperShots = 1f;
	[ SerializeField ] float m_SecondsToLetPegsBounce = 2f;

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

	void Awake()
	{
		m_JackpotLeadingText = jackpot.text;
	}

	void Start()
	{
		SetRainbowCycleSpeed();
	}

	public void CreateBoard()
	{
		jackpotScore = 0;
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
		user.ScorePoints( jackpotScore );
		PointsLabel.InstantiatePointsLabelGameObject(
			jackpotScore.ToString(), jackpotTrigger.transform.position );
		CreateBoard();
	}

	public void BucketActivated( Bucket bucket, User user )
	{
		user.ScorePoints( bucketScore );
		PointsLabel.InstantiatePointsLabelGameObject(
			bucketScore.ToString(), bucket.transform.position );
	}

	public void IncreaseJackpot()
	{
		jackpotScore += jackpotIncrement;
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
		turret.transform.Translate( Vector3.up * 4f );
		pegWalls.SetActive( false );
		barrelMotor.speed = 1f;
		boardFrozen.Invoke();
		yield return null;
	}
}