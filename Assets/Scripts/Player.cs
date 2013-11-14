using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public Card[]	cards		= null;
	
	public void Start () {
		cards = new Card[5];
		
	}
}
