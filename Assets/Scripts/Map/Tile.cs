using UnityEngine;
using System.Collections.Generic;

public enum TileType {
	Empty	= 0,
	Track,
	Special
}

public class Tile : MonoBehaviour {

	public TileType type		= TileType.Empty;
	public int[]	connectors	= new int[4];
	public int x;
	public int y;
	
	private bool wantsToShowRotation = false;
	
//	private GameObject left;
	private GameObject right;
	
	public void AddRotationPrefabs (GameObject rotateLeft, GameObject rotateRight)
	{
//		left = Instantiate (rotateLeft) as GameObject;
//		left.name = "RotateLeft";
//		left.transform.parent = transform;
//		left.transform.localPosition = new Vector3 (-0.5f, 0f, -1f);
		
		right = Instantiate (rotateRight) as GameObject;
		right.name = "RotateRight";
		right.transform.parent = transform;
		right.transform.localPosition = new Vector3 (0f, 0f, -1f);
	}
	
	public void Update () {
		if (wantsToShowRotation == true)
			ShowRotationPrefabs (true, 0);
	}
	
	public void ShowRotationPrefabs (bool doShow, int actionsDone = 0) {
		if (actionsDone > 0)
			doShow = false;
		
		if (right != null) {
			right.SetActive (doShow);
			wantsToShowRotation = false;
		}
		else
			wantsToShowRotation = true;
//		left.SetActive (doShow);
	}

	public void Rotate (int rotation) {
		if (rotation == -1) {
			int tempConnector = connectors[0];
			connectors[0] = connectors[1];
			connectors[1] = connectors[2];
			connectors[2] = connectors[3];
			connectors[3] = tempConnector;
			
			transform.Rotate (new Vector3 (0f, 0f, -90f));
		}
		else {
			int tempConnector = connectors[3];
			connectors[1] = connectors[0];
			connectors[2] = connectors[1];
			connectors[3] = connectors[2];
			connectors[0] = tempConnector;
			
			transform.Rotate (new Vector3 (0f, 0f, 90f));
		}
	}
}
