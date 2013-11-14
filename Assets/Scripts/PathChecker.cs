using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathChecker
{	
	public IList<Vector2> Search(int startX, int startY, int finishX, int finishY, int connection, Tile[,] grid, int width, int height)
    {
		bool[,] alreadySearched = new bool[width,height];
		IList<Vector2> finalPath = new List<Vector2>();
		
		Solve(startX, startY, finishX, finishY, grid, width, height, connection, alreadySearched, finalPath);
		
		return finalPath;
    }
	
	private bool Solve(int currentX, int currentY, int finishX, int finishY, Tile[,] grid, int width, int height, int connection, bool[,] alreadySearched, IList<Vector2> finalPath) {
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
	        if (grid[currentX, currentY].type == TileType.Empty) {
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
            correctPath = correctPath || Solve(currentX + 1, currentY, finishX, finishY, grid, width, height, connection, alreadySearched, finalPath);
            //Check down tile
            correctPath = correctPath || Solve(currentX, currentY + 1, finishX, finishY, grid, width, height, connection, alreadySearched, finalPath);
            //check left tile
            correctPath = correctPath || Solve(currentX - 1, currentY, finishX, finishY, grid, width, height, connection, alreadySearched, finalPath);
            //check up tile
            correctPath = correctPath || Solve(currentX, currentY - 1, finishX, finishY, grid, width, height, connection, alreadySearched, finalPath);
        }

        if (correctPath) {
            finalPath.Add(new Vector2(currentX, currentY));
		}
		
        return correctPath;
	}
}

