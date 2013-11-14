using UnityEngine;
using System.Collections;

public class Board : MonoBehaviour {

	public Level[]	levels;
	public Tile[]	tiles;
	private Tile[,]	grid		= null;
	private int[,]  trainGrid	= null;

	public void Start () {
		LoadLevel (0);
	}
		
	private void LoadLevel (int levelNumber) {
		Level level = levels [levelNumber];
		
		grid = new Tile [level.width, level.height];
		
		for (int i = 0; i < level.width; ++i) {
			for (int j = 0; j < level.height; ++j) {
				Tile tile = grid [i,j];
				tile = Instantiate (tiles [(int)TileType.Empty]) as Tile;
			}
		}
	}
	
		
}
