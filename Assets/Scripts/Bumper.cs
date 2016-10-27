using UnityEngine;

public class Bumper : MonoBehaviour
{
	BoardManager m_BoardManager;

	void Start()
	{
		m_BoardManager = GameManager.singleton.boardManager;
		m_BoardManager.boardFrozen.AddListener( OnBoardFrozen );
	}

	void OnDestroy()
	{
		if( m_BoardManager != null )
		{
			m_BoardManager.boardFrozen.RemoveListener( OnBoardFrozen );
		}
	}

	void OnBoardFrozen()
	{
		var _rigidbody2D = GetComponent<Rigidbody2D>();
		if( _rigidbody2D != null )
		{
			Destroy( _rigidbody2D );
		}
		gameObject.isStatic = true;
	}
}