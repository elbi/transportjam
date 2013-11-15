using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Board : MonoBehaviour {

	public Level[]	levels;
	public Tile[]	tiles;
	public CheckPoint checkpoints;

	public Deck[]	decks;

	public GameObject rotateRight;
	public GameObject rotateLeft;
	private Tile[,]	grid		= null;
	private int[,]  trainGrid	= null;
	private Player[] players;
	//private Train[] trains;
	
	public GameObject checkpointPrefab;
	public Player	playerPrefab;
	public GameObject trainPrefab;
	
	public GameObject[] playerHands = null;
	
	private int		currentPlayer;
	private Level 	currentLevel;
	
//	private int		localPlayer;
	private Card	selectedCard;
	private Tile	selectedTile;
	private Train	selectedTrain;
	
	private bool isRotatingOnHand = false;
	
	private PathChecker pathChecker = new PathChecker();
	
	private int		actionsDone = 0;
	private bool	didRotate = false;
	

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
			startInstance.transform.Rotate (new Vector3 (0f, 0f, 180f));
			startInstance.transform.localPosition = new Vector3 (offsetX, offsetY, -0.5f);
			
			GameObject trainInstance = Instantiate(trainPrefab) as GameObject;
			trainInstance.transform.localPosition = new Vector3 (offsetX, offsetY, -1f);
			
			if (start.playerNum == 1) {
				startInstance.renderer.material.color = new Color (1f, 0.6f, 0.6f, 1f);
				trainInstance.renderer.material.color = new Color (1f, 0.2f, 0.2f, 1f);
			}
			else {
				startInstance.renderer.material.color = new Color (0.6f, 0.6f, 1f, 1f);
				trainInstance.renderer.material.color = new Color (0.2f, 0.2f, 1f, 1f);
			}
				
			
			
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
			
			
			exitInstance.transform.localPosition = new Vector3 (offsetX, offsetY, -0.5f);
			if (exit.playerNum == 1)
				exitInstance.renderer.material.color = new Color (1f, 0.1f, 0.1f, 1f);
			else
				exitInstance.renderer.material.color = new Color (0.1f, 0.1f, 1f, 1f);
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
		
		// hide enemy's hand
		foreach (GameObject go in playerHands)
			go.SetActive (false);
			
		playerHands[playerNumber].SetActive (true);
		
		
		/*
		// show all hands
		foreach (GameObject go in playerHands)
			go.SetActive (true);
		*/	
	}
	
	public void MoveTrain(Train train, Tile tile) {
		List<Vector2> path = pathChecker.Search(train.x, train.y,tile.x,tile.y, selectedTrain.connection, 0, grid, currentLevel.width, currentLevel.height);
		
		Debug.Log("Checking path from (" + train.x + "," + train.y + ") to (" + tile.x + "," + tile.y + "): " + (path != null));
		
		if (path != null) {
			Debug.Log("Extracting path: " + getPathString(path));
								
			selectedTrain = null;
			//tile.gameObject.SetActive (false);
				
			train.Move(tile.x, tile.y, path);
			
			
			Debug.Log("Moving train to destination");
		}

	}
	
	public void EndTurn () {
		
		if (selectedTile != null)
			selectedTile.ShowRotationPrefabs (false);
			
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
					if (didRotate) {
						didRotate = false;
						if (isRotatingOnHand == false)
							actionsDone += 2;
						isRotatingOnHand = false;
						if (actionsDone >= 2) {
							EndTurn ();
							return;
						}
					}			
															
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
				else if (hit.collider.gameObject.name == "EmptyTrain(Clone)")
					SelectTrainOnGrid(hit.collider.transform.GetComponent<Train>());
				else
					return;
			}
		}
	}
	
	private void SelectTrainOnGrid(Train train) {
		Debug.Log ("Selecting train");
		selectedTrain = train;
	}
	
	private void SelectTileOnGrid (Tile tile) {
		Debug.Log ("selected tile on grid: " + tile);
		
		if (selectedTile != null)
			selectedTile.ShowRotationPrefabs (false);
		
		if (selectedTrain != null) {
			MoveTrain(selectedTrain, tile);
			return;
		}
		
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
			
		tile.ShowRotationPrefabs (true, 0);
		selectedTile = tile;
		isRotatingOnHand = true;
			
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
//		Destroy (selectedCard);
		selectedCard = null;
		tile.gameObject.SetActive (false);
			
		card.tile.x = tile.x;
		card.tile.y = tile.y;
		
		grid[tile.x, tile.y] = card.tile;
				
		++actionsDone;
		if (actionsDone >= 2)
			EndTurn ();
	}
	
	private String getPathString(IList<Vector2> path) {
		var pathString = String.Empty;
		
		foreach (Vector2 pathNode in path) {
			pathString += "(" + pathNode.x + "," + pathNode.y + "),";
		}
		
		return pathString;
	}	

	void RotateTileOnGrid (Tile tile, int rotation) {
		didRotate = true;
		tile.Rotate (rotation);
	}

}
