using UnityEngine;
using System.Collections;
using System;

public class Board : MonoBehaviour {

	public Level[]	levels;
	public Tile[]	tiles;
	public Deck[]	decks;
	public Player	playerPrefab;
	private Tile[,]	grid		= null;
	private int[,]  trainGrid	= null;
	private Player[] players;
	
	public GameObject[] playerHands = null;
	
	private int		currentPlayer;
//	private int		localPlayer;
	private Card	selectedCard;
	private Tile	selectedTile;
	

	public void Start () {
	
		// Gets the TapRecognizer instance and registers it on the inputhandler
		TapRecognizer recognizer = FingerGestures.Instance.GetComponent<TapRecognizer>();
		recognizer.OnGesture += this.OnTap;
		
		LoadLevel (0);
	}
		
	private void LoadLevel (int levelNumber) {
		Level level = levels [levelNumber];
		
		grid = new Tile [level.width, level.height];
		
		for (int i = 0; i < level.width; ++i) {
			for (int j = 0; j < level.height; ++j) {
				Tile tile = grid [i,j];
				tile = Instantiate (tiles [(int)TileType.Empty]) as Tile;
				tile.transform.localPosition = new Vector3 (i * 1f, j * 1f, 0f);
				tile.renderer.material.color = new Color (new System.Random ().Next (2), new System.Random ().Next (2), new System.Random ().Next (2), 1f);
			}
		}
		
		LoadPlayers (level.numPlayers);
	}
	
	private void LoadPlayers (int numPlayers) {
	
		players = new Player[numPlayers];
		for (int i = 0; i < numPlayers; ++i) {
			players[i] = Instantiate (playerPrefab) as Player;
			players[i].Draw (decks[0], 5);
			
			float offset = 0;
			foreach (Card card in players[i].Cards) {
				if (card.tile != null) {
					Tile cardTile = Instantiate (card.tile) as Tile;
					cardTile.transform.parent = playerHands[i].transform;
					cardTile.transform.localPosition = new Vector3 (offset, 0f, 0f);
					offset += 1.2f;
				}
			}
		}
	}
	
	public void StartTurn (int playerNumber) {
		currentPlayer = playerNumber;
		
		selectedCard = null;
		selectedTile = null;
		
		
	}
	
	public void EndTurn () {
		
	}
	
	public void OnTap (Gesture gesture) {
		Debug.Log ("tap recognized!");
		
		// TODO: ray cast to see what we clicked on
	}
	
	
		
}
