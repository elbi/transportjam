using UnityEngine;
using System.Collections;

public class Board : MonoBehaviour {

	public Level[]	levels;
	public Tile[]	tiles;
	public Deck[]	decks;
	public Player	playerPrefab;
	private Tile[,]	grid		= null;
	private int[,]  trainGrid	= null;
	private Player[] players;

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
		
		LoadPlayers (level.numPlayers);
	}
	
	private void LoadPlayers (int numPlayers) {
	
		players = new Player[numPlayers];
		for (int i = 0; i < numPlayers; ++i) {
			players[i] = Instantiate (playerPrefab) as Player;
			players[i].Draw (decks[0], 5);
		}
	}
	
		
}
