using UnityEngine;
using System.Collections;

[System.Serializable]
public class CheckPoint {
	public int x			= 0;
	public int y			= 0;
	public int playerNum	= 1;
	public bool isExit		= false;
}

public class Level : MonoBehaviour {

	public int	width = 7;
	public int  height = 7;
	public int  numPlayers = 2;
	public CheckPoint[] entryPoints = null;
	public CheckPoint[] exitPoints  = null;

}
