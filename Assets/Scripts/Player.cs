using UnityEngine;
using System.Collections;
using System;

public class Player : MonoBehaviour {

	public Card[]	cards		= null;
	
	public void Start () {
		cards = new Card[5];
	}
	
	public void Draw (Deck deck, int cardsToDraw) {
		int maxRoll = 0;
		foreach (CardPercentage card in deck.possibleCards) {
			maxRoll += card.chance;
		}
		
		int randomRoll = new System.Random ().Next (maxRoll);
		
		Card newCard = null;
		int cardOffset = 0;
		for (int i = 0; i < deck.possibleCards.Length; ++i) {
			if ((randomRoll - cardOffset) <= deck.possibleCards[i].chance)
				newCard = Instantiate (deck.possibleCards[i].card) as Card;
			else
				cardOffset += deck.possibleCards[i].chance;
		}
		
		if (newCard == null) {
			Debug.LogError ("Draw Card: card is null. Whyyyy?");
			return;
		}
		
		for (int slot = 0; slot < cards.Length; ++slot) {
			if (cards[slot] == null) {
				cards[slot] = newCard;
				break;
			}
		}
		
		Debug.Log ("Giving card to player: " + newCard.ToString ());
		
		--cardsToDraw;
		if (cardsToDraw > 0)
			Draw (deck, cardsToDraw);
	}
}
