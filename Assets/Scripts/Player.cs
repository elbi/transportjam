using UnityEngine;
using System.Collections;
using System;

public class Player : MonoBehaviour {

	private Card[]	cards		= null;

	public Card[] Cards {
		get {
			return cards;
		}
		set {
			cards = value;
		}
	}
	
	public void Start () {
	}
	
	public void Draw (Deck deck, int cardsToDraw) {
	
		if (cards == null)
			cards = new Card[5];
	
		int maxRoll = 0;
		foreach (CardPercentage card in deck.possibleCards) {
			maxRoll += card.chance;
		}
		
		int randomRoll = new System.Random ().Next (maxRoll);
		
		Card newCard = null;
		int cardOffset = 0;
		for (int i = 0; i < deck.possibleCards.Length; ++i) {
			if ((randomRoll - cardOffset) <= deck.possibleCards[i].chance) {
				newCard = Instantiate (deck.possibleCards[i].card) as Card;
				break;
			}
			else
				cardOffset += deck.possibleCards[i].chance;
		}
		
		if (newCard == null) {
			Debug.LogError ("Draw Card: card is null. Whyyyy?");
			return;
		}
		
		for (int slot = 0; slot < 5; ++slot) {
			if (cards[slot] == null) {
				Debug.Log ("Giving card to player: " + newCard.ToString ());
				cards[slot] = newCard;
				break;
			}
//			else {
//				Debug.Log ("slot filled with: " + cards[slot].ToString());
//			}
		}
		
		
		--cardsToDraw;
		if (cardsToDraw > 0)
			Draw (deck, cardsToDraw);
	}
}
