﻿using UnityEngine;
using System.Collections;

[System.Serializable]
public class CardPercentage {
	public Card card;
	public int chance;
}

public class Deck : MonoBehaviour {

	public CardPercentage[] possibleCards;
	
}
