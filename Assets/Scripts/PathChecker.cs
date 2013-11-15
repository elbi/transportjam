using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathChecker
{	
	public List<Vector2> Search(int startX, int startY, int finishX, int finishY, int connection, int connector, Tile[,] grid, int width, int height)
    {
		bool[,] alreadySearched = new bool[width,height];
		List<Vector2> finalPath = finalPath = new List<Vector2>();
		
		bool pathFound = Solve(startX, startY, finishX, finishY, grid, width, height, connection, connector, alreadySearched, finalPath);
		
		finalPath.Reverse();
		
		return pathFound ? finalPath : null;
    }
	
	private bool Solve(int currentX, int currentY, int finishX, int finishY, Tile[,] grid, int width, int height, int connection, int connector, bool[,] alreadySearched, IList<Vector2> finalPath) {
		bool correctPath = false;
        bool shouldCheck = true;
		
		//Check for out of boundaries
        if (currentX >= width || currentX < 0 || currentY >= height || currentY < 0)
            shouldCheck = false;
        else
        {
			Tile tile = grid[currentX, currentY];
			
			//check finish
	        if (currentX == finishX && currentY == finishY) {
	            correctPath = true;
	            shouldCheck = false;
	        }
	
			
			
	        //Check obstacle
	        if (grid[currentX, currentY].type == TileType.Empty || tile.connectors[connector] == connection) {
	            shouldCheck = false;
			}
	
	        //Check if previously searched
	        if (alreadySearched[currentX, currentY])
	            shouldCheck = false;
		}

        //Search the Tile
        if (shouldCheck)
        {
            //mark tile as searched
            alreadySearched[currentX, currentY] = true;
            //Check right tile
            correctPath = correctPath || Solve(currentX + 1, currentY, finishX, finishY, grid, width, height, connection, 0, alreadySearched, finalPath);
            //Check down tile
            correctPath = correctPath || Solve(currentX, currentY + 1, finishX, finishY, grid, width, height, connection, 1, alreadySearched, finalPath);
            //check left tile
            correctPath = correctPath || Solve(currentX - 1, currentY, finishX, finishY, grid, width, height, connection, 2, alreadySearched, finalPath);
            //check up tile
            correctPath = correctPath || Solve(currentX, currentY - 1, finishX, finishY, grid, width, height, connection, 3, alreadySearched, finalPath);
        }

        if (correctPath) {
            finalPath.Add(new Vector2(currentX, currentY));
		}
		
        return correctPath;
	}
}

