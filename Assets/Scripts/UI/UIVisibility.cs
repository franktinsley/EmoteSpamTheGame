using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIVisibility : MonoBehaviour
{
	[ SerializeField ] bool m_Visible;
	public bool visible { get { return m_Visible; } set { SetBool( ref m_Visible, value ); } }

	List<MaskableGraphic> m_MaskableGraphics;
	bool m_Dirty = true;

	void Start()
	{
		m_MaskableGraphics = new List<MaskableGraphic>();
		GetComponentsInChildren<MaskableGraphic>( true, m_MaskableGraphics );
	}

	void Update()
	{
		if( m_Dirty )
		{
			m_Dirty = false;
			if( m_MaskableGraphics != null )
			{
				foreach( var maskableGraphic in m_MaskableGraphics )
				{
					maskableGraphic.enabled = m_Visible;
				}
			}
		}
	}

	void SetBool( ref bool currentValue, bool newValue )
	{
		if( currentValue == newValue )
		{
			return;
		}
		currentValue = newValue;
		m_Dirty = true;
	}

	#if UNITY_EDITOR
	void OnValidate()
	{
		m_Dirty = true;
	}
	#endif
}