using UnityEngine;
using UnityEngine.UI;

public class PointsLabel : MonoBehaviour
{
	public Text text;

	public static PointsLabel InstantiatePointsLabelGameObject(
		string labelText, Vector3 position )
	{
		BoardManager boardManager = GameManager.singleton.boardManager;
		var pointsLabelGameObject = Instantiate(
			boardManager.pointsLabelPrefab,
			position,
			boardManager.pointsLabelPrefab.transform.rotation ) as GameObject;
		var pointsLabel = pointsLabelGameObject.GetComponent<PointsLabel>();
		pointsLabel.text.text = labelText;
		return pointsLabel;
	}

	public void Kill()
	{
		Destroy( gameObject );
	}
}