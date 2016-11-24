using UnityEngine;
using UnityEngine.UI;

public class UserActivityTableCell : MonoBehaviour
{
	[ ReadOnly ] public UserData model;

	const float m_SecondsBeforeHiding = 120f;
	const string m_ColorCloseTag = "</color>";

	[ SerializeField ] UIVisibility m_UIVisibility;
	[ SerializeField ] Text m_NameLabel;
	[ SerializeField ] Text m_ScoreLabel;
	[ SerializeField ] Slider m_AmmoMeter;
	[ SerializeField ] Image m_AmmoMeterFill;

	Color m_Color;
	string m_ColorOpenTag;
	float m_HideTime;

	public static UserActivityTableCell InstantiateUserActivityTableCellGameObject( UserData model )
	{
		UserActivityTable userActivityTable = GameManager.singleton.userActivityTable;
		Transform parent = userActivityTable.parent;
		var userActivityTableCellGameObject = Instantiate( 
			userActivityTable.userActivityTableCellPrefab,
			parent.position,
			parent.rotation ) as GameObject;
		userActivityTableCellGameObject.name = model.userName;
		userActivityTableCellGameObject.transform.SetParent( parent );
		var userActivityTableCell =
			userActivityTableCellGameObject.GetComponent<UserActivityTableCell>();
		userActivityTableCell.model = model;
		if( ColorUtility.TryParseHtmlString( model.userNameColor, out userActivityTableCell.m_Color ) )
		{
			userActivityTableCell.m_ColorOpenTag = "<color=" + model.userNameColor + ">";
		}
		else
		{
			Debug.LogError( "Couldn't parse the html for " +
				model.userName + "'s user name color. Setting to white." );
			userActivityTableCell.m_Color = Color.white;
			userActivityTableCell.m_ColorOpenTag = "<color=" + ColorUtility.ToHtmlStringRGB( Color.white ) + ">";
		}
		return userActivityTableCell;
	}

	public void Activity()
	{
		m_UIVisibility.visible = true;
		transform.SetAsLastSibling();
		m_HideTime = Time.time + m_SecondsBeforeHiding;
	}

	void Update()
	{
		m_NameLabel.text = AddUserColorTags( model.userName );
		m_ScoreLabel.text = AddUserColorTags( model.score.ToString() );
		m_AmmoMeter.value = ( float )model.ammo;
		m_AmmoMeterFill.color = m_Color;

		if( m_UIVisibility.visible && Time.time > m_HideTime )
		{
			transform.SetAsFirstSibling();
			m_UIVisibility.visible = false;
		}
	}

	string AddUserColorTags( string value )
	{
		return m_ColorOpenTag + value + m_ColorCloseTag;
	}
}