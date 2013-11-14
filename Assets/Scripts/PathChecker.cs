using System;
using System.Collections;
using UnityEngine;

public class PathChecker
{	
	public bool Search(Vector2 start, Vector2 finish, int connection, Tile[,] grid, int width, int height)
    {
		bool[,] alreadySearched = new bool[width,height];
		bool pathFound = false;//Solve(start.x, start.y, finish.x, finish.y, grid, width, height, connection, alreadySearched);
		
		return pathFound;
    }
	
	private bool Solve(int currentX, int currentY, int finishX, int finishY, Tile[,] grid, int width, int height, int connection, bool[,] alreadySearched) {
		bool correctPath = false;
        bool shouldCheck = true;

		Tile tile = grid[currentX, currentY];
		
		//Check for out of boundaries
        if (currentX >= width || currentX < 0 || currentY >= height || currentY < 0)
            shouldCheck = false;
        else
        {
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
            correctPath = correctPath || Solve(currentX + 1, currentY, finishX, finishY, grid, width, height, connection, alreadySearched);
            //Check down tile
            correctPath = correctPath || Solve(currentX, currentY + 1, finishX, finishY, grid, width, height, connection, alreadySearched);
            //check left tile
            correctPath = correctPath || Solve(currentX - 1, currentY, finishX, finishY, grid, width, height, connection, alreadySearched);
            //check up tile
            correctPath = correctPath || Solve(currentX, currentY - 1, finishX, finishY, grid, width, height, connection, alreadySearched);
        }
        //make correct path gray
        /*if (correctPath)
            add to vector*/
		
        return correctPath;
	}
}

