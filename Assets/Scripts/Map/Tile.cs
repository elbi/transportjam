using UnityEngine;
using System.Collections;

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
}
