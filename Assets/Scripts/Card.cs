using UnityEngine;
using System.Collections;

public enum CardType {
	Tile = 0,
	Special,
}

public class Card : MonoBehaviour {
	
	public CardType cardType;
	public Tile		tile;
	
}
