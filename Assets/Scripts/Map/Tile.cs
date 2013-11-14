using UnityEngine;
using System.Collections;

public enum TileType {
	Empty	= 0,
	Straight,
	SimpleCurve,
}

public class Tile : MonoBehaviour {

		public 	int[]	connectors	= new int[4];
}
