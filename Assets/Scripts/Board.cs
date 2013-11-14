using UnityEngine;
using System.Collections;
using System;

public class Board : MonoBehaviour {

	public Level[]	levels;
	public Tile[]	tiles;
	public CheckPoint checkpoints;
	public GameObject checkpointPrefab;
	public Deck[]	decks;
	public Player	playerPrefab;
	public GameObject rotateRight;
	public GameObject rotateLeft;
	private Tile[,]	grid		= null;
	private int[,]  trainGrid	= null;
	private Player[] players;
	//private Train[] trains;
	
	public GameObject[] playerHands = null;
	
	private int		currentPlayer;
	private Level 	currentLevel;
	
//	private int		localPlayer;
	private Card	selectedCard;
	private Tile	selectedTile;
	private Train	selectedTrain;
	
	private PathChecker pathChecker = new PathChecker();
	
	private int		actionsDone = 0;
	

	public void Start () {
	
		// Gets the TapRecognizer instance and registers it on the inputhandler
		TapRecognizer recognizer = FingerGestures.Instance.GetComponent<TapRecognizer>();
		recognizer.OnGesture += this.OnTap;
		
		LoadLevel (0);
	}
		
	private void LoadLevel (int levelNumber) {
		Level level = levels [levelNumber];
		
		currentLevel = level;
		
		grid = new Tile [level.width, level.height];
		
		for (int i = 0; i < level.width; ++i) {
			for (int j = 0; j < level.height; ++j) {
				Tile tile = grid [i,j];
				tile = Instantiate (tiles [(int)TileType.Empty]) as Tile;
				tile.AddRotationPrefabs (rotateLeft, rotateRight);
				tile.transform.parent = transform;
				tile.transform.localPosition = new Vector3 (i * 1f, j * 1f, 0f);
				tile.x = i;
				tile.y = j;
				grid[i,j] = tile;
			}
		}
		
		float offsetX = 0;
		float offsetY = 0;
		
		for (int k = 0; k < level.entryPoints.Length; ++k) {
			CheckPoint start = level.entryPoints[k];
			
			offsetX = start.x * 1f;
			offsetY = start.y * 1f;
			
			switch (start.direction) {
				case DirectionType.Down: offsetY -= 1f; break;
				case DirectionType.Up: offsetY += 1f; break;
				case DirectionType.Left: offsetX += 1f; break;
				case DirectionType.Right: offsetY -= 1f; break;
			}
			
			GameObject startInstance = Instantiate(checkpointPrefab) as GameObject;
			startInstance.transform.localPosition = new Vector3 (offsetX, offsetY, 0f);
			startInstance.renderer.material.color = new Color (new System.Random ().Next (0), new System.Random ().Next (0), new System.Random ().Next (0), 1f);
		}
		
		for (int l = 0; l < level.exitPoints.Length; ++l) {
			CheckPoint exit = level.exitPoints[l];
			
			GameObject exitInstance = Instantiate(checkpointPrefab) as GameObject;
			
			offsetX = exit.x * 1f;
			offsetY = exit.y * 1f;
			
			switch (exit.direction) {
				case DirectionType.Down: offsetY -= 1f; break;
				case DirectionType.Up: offsetY += 1f; break;
				case DirectionType.Left: offsetX += 1f; break;
				case DirectionType.Right: offsetY -= 1f; break;
			}
			
			exitInstance.transform.localPosition = new Vector3 (offsetX, offsetY, 0f);
			exitInstance.renderer.material.color = new Color (new System.Random ().Next (20), new System.Random ().Next (20), new System.Random ().Next (20), 1f);
		}
		
		LoadPlayers (level.numPlayers);
		
		StartTurn (0);
	}
	
	private void LoadPlayers (int numPlayers) {
	
		players = new Player[numPlayers];
		for (int i = 0; i < numPlayers; ++i) {
			players[i] = Instantiate (playerPrefab) as Player;
			players[i].Draw (5, decks[0]);
			
			float offset = 0;
			players[i].Init (this);
			players[i].RenderCards (playerHands[i]);
		}
	}
	
	public void StartTurn (int playerNumber) {
		currentPlayer = playerNumber;
		
		selectedCard = null;
		selectedTile = null;
		actionsDone	 = 0;
		
		foreach (GameObject go in playerHands)
			go.SetActive (false);
			
		playerHands[playerNumber].SetActive (true);
	}
	
	public void MoveTrain() {
		bool path = pathChecker.Search(0,0,1,1, selectedTrain.connection, grid, currentLevel.width, currentLevel.height);
	}
	
	public void EndTurn () {
		players[currentPlayer].RefillHand ();
		currentPlayer = (currentPlayer + 1) % players.Length;
		StartTurn (currentPlayer);
		
	}
	
	public void OnTap (Gesture gesture) {
		
		RaycastHit hit = new RaycastHit ();
		if (Physics.Raycast(Camera.main.ScreenPointToRay (gesture.Position), out hit, 5000.0f))
		{
			if (hit.collider != null) {
				Tile tile = hit.collider.transform.GetComponent<Tile>();
				
				if (tile != null) {														
					// did we click on a card in hand or on a tile on the grid?
					if (tile.transform.parent.GetComponent<Board>() != null)
						SelectTileOnGrid (tile);
					else
						SelectTileOnHand (tile);
				}
				else if (hit.collider.gameObject.name == "RotateLeft")
					RotateTileOnGrid (hit.collider.transform.parent.GetComponent<Tile>(), 1);
				else if (hit.collider.gameObject.name == "RotateRight")
					RotateTileOnGrid (hit.collider.transform.parent.GetComponent<Tile>(), -1);
				else
					return;
			}
		}
	}
	
	private void SelectTileOnGrid (Tile tile) {
		Debug.Log ("selected tile on grid: " + tile);
		
		if (selectedTile != null)
			selectedTile.ShowRotationPrefabs (false);
		
		if (selectedCard != null && tile.type == TileType.Empty) {
			PlaceCardOnGrid (selectedCard, tile);
		}
		else if (tile.type != TileType.Empty) {
			selectedCard = null;
			tile.ShowRotationPrefabs (true, actionsDone);
			selectedTile = tile;
		}
	}
	
	private void SelectTileOnHand (Tile tile) {
		if (selectedTile != null)
			selectedTile.ShowRotationPrefabs (false);
			
		Debug.Log ("selected tile on hand: " + tile);
		selectedCard = tile.transform.parent.GetComponent<Card>();
	}
	
	private void PlaceCardOnGrid (Card card, Tile tile) {
		Debug.Log ("Placing tile on grid: " + card.tile);
		
		// move card's tile to new position
		Transform tileTransform = null;
		foreach (Transform child in card.transform) {
			if (child.GetComponent<Tile>() != null)
				tileTransform = child.transform;
		}
		tileTransform.parent = tile.transform.parent;
		tileTransform.localPosition = tile.transform.localPosition;
						
		players[currentPlayer].PlayCard (selectedCard.slot);
//		tile = card.tile;
		Destroy (selectedCard);
		selectedCard = null;
		tile.gameObject.SetActive (false);
		
			
		card.tile.x = tile.x;
		card.tile.y = tile.y;
		
		grid[tile.x, tile.y] = card.tile;
		
		//DEBUG ONLY: CHECK PATH TO TEST
		bool path = pathChecker.Search(0,0,1,1, 1, grid, currentLevel.width, currentLevel.height);
		Debug.Log("Checking path from (0,0) to (1,1): " + path);
		
		++actionsDone;
		if (actionsDone >= 2)
			EndTurn ();
	}
	
	void RotateTileOnGrid (Tile tile, int rotation) {
		tile.Rotate (rotation);
	}
}
