using UnityEngine;
using System.Collections;
using System;

public class Player : MonoBehaviour {

	private Card[]	cards		= null;
	private Deck	storedDeck	= null;
	private GameObject hand	= null;
	
	private GameObject rotateLeft;
	private GameObject rotateRight;

	public Card[] Cards {
		get {
			return cards;
		}
		set {
			cards = value;
		}
	}
	
	public void Init (Board board) {
		rotateLeft = board.rotateLeft;
		rotateRight = board.rotateRight;
	}
	
	public void Draw (int cardsToDraw, Deck deck = null) {
	
		if (storedDeck == null)
			storedDeck = deck;
		if (cards == null)
			cards = new Card[5];
	
		int maxRoll = 0;
		foreach (CardPercentage card in storedDeck.possibleCards) {
			maxRoll += card.chance;
		}
		
		int randomRoll = new System.Random ().Next (maxRoll);
		
		Card newCard = null;
		int cardOffset = 0;
		for (int i = 0; i < storedDeck.possibleCards.Length; ++i) {
			if ((randomRoll - cardOffset) <= storedDeck.possibleCards[i].chance) {
				newCard = Instantiate (storedDeck.possibleCards[i].card) as Card;
				newCard.transform.parent = transform;
				break;
			}
			else
				cardOffset += storedDeck.possibleCards[i].chance;
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
			Draw (cardsToDraw);
	}

	public void PlayCard (int slot) {
		cards[slot] = null;
	}

	public Card SelectCard (Tile tile) {
		for (int slot = 0; slot < cards.Length; ++slot) {
			if (cards[slot].tile == tile)
				return cards[slot];
		}
		return null;
	}
	
	public void RefillHand () {
		int missingCards = 0;
		foreach (Card card in cards)
			if (card == null)
				++missingCards;
		
		Draw (missingCards);
		RenderCards ();
	}
	
	public void RenderCards (GameObject playerHand = null) {
		
		if (hand == null)
			hand = playerHand;
	
		float offset = 1.2f;
		for (int i = 0; i < cards.Length; ++i) {
			Card card = cards[i];
			if (card.tile != null && card.isRenderedAlready == false) {
				card.isRenderedAlready = true;
				Card cardObject = Instantiate (card) as Card;
				cardObject.transform.parent = hand.transform;
				cardObject.transform.localPosition = new Vector3 (offset * i, 0f, 0f);
				cardObject.slot = i;
				
				Tile cardTile = Instantiate (card.tile) as Tile;
				cardTile.AddRotationPrefabs (rotateLeft, rotateRight);
				cardTile.transform.parent = cardObject.transform;
				cardTile.transform.localPosition = Vector3.zero;
			}
		}
	}
}
