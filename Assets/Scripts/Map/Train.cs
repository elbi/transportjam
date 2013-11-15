using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Train : MonoBehaviour
{
	public int connection = 1;
	public int connector = 3;
	public int x;
	public int y;
	
	public int viewX;
	public int viewY;
	
	public bool moving = false;
	
	public List<Vector2> path = null;
	
	private IEnumerator pathIterator;
	
	private float lastUpdate;
	
	public void Move(int targetX, int targetY, List<Vector2> newPath) {
		x = targetX;
		y = targetY;
		moving = true;
		path = newPath;
	}
	
	public void Update() {
		if (moving) {
			transform.localPosition = new Vector3 (x * 1f, y * 1f, -1f);
			moving = false;
			/*if (path != null && pathIterator == null) {
				pathIterator = path.GetEnumerator();
			}
			
			pathIterator.MoveNext();
						
			Vector2 pathNode = pathIterator.Current as Vector2;
						
			if (pathNode != null) {
				trainInstance.transform.localPosition = new Vector3 (pathNode.x, pathNode.y, 0f);
			} else {
				path = null;
				pathIterator = null;
				moving = false;
			}*/
		}
	}
}


