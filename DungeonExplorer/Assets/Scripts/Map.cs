using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

	public MapCell cellPrefab;
	public MapPassage passagePrefab;
	public MapWall wallPrefab;
	public MapBorder borderPrefab;
	public ScoreOrb scoreOrbPrefab;

	public int size; //map size
	public bool mapBorder; //true=generate walls around map
	public int wallRemovalPercent; //the number of walls to remove after generation the mazes
	public int score;

	private MapCell[,] cells; //the map
	private List<MapCellEdge> walls = new List<MapCellEdge>();
	private int cellCount;
	private Vector2Int startCoords;
	private int scoreOrbCount;

	//increases the score by one
	public void IncScore(int x) {
		this.score += x;
	}

	//returns the coords of the initial cell
	public Vector2Int GetStartCoords() {
		return startCoords;
	}

	//Returns random coords within the map's coord limits
	public Vector2Int RandomCoordinates {
		get {
			return new Vector2Int((int)UnityEngine.Random.Range(0, size), (int)UnityEngine.Random.Range(0, size));
		}
	}

	//Returns true if given coords are within the map's coord limits
	public bool ContainsCoordinates(Vector2Int coords) {
		return coords.x >= 0 && coords.x < size && coords.y >= 0 && coords.y < size;
	}

	//Returns the map cell at given coords
	public MapCell GetCell(Vector2Int coords) {
		return cells[coords.x, coords.y];
	}

	//Returns a cell at given location and adds it to the map grid
	private MapCell CreateCell(Vector2Int coords) {
		MapCell newCell = Instantiate(cellPrefab) as MapCell;
		cells[coords.x, coords.y] = newCell;
		newCell.coords = coords;
		newCell.name = "MapCell " + coords.x + ", " + coords.y;
		//Debug.Log("Creating " + newCell.name);
		newCell.transform.parent = transform;
		newCell.transform.localPosition = new Vector2(coords.x - size * 0.5f + 0.5f, coords.y - size * 0.5f + 0.5f);
		this.cellCount += 1;
		return newCell;
	}

	//Creates a passage (gap between cells) at given cells and direction
	private void CreatePassage(MapCell cell, MapCell otherCell, MapDirection direction) {
		MapPassage passage = Instantiate(passagePrefab) as MapPassage;
		passage.name = "MapPassage " + direction;
		passage.Initialize(cell, otherCell, direction);
		passage = Instantiate(passagePrefab) as MapPassage;
		passage.name = "MapPassage " + direction.GetOpposite();
		passage.Initialize(otherCell, cell, direction.GetOpposite());
		//Debug.Log("Creating passage from: " + cell.coords.x + "," + cell.coords.y + " to: " + otherCell.coords.x + "," + otherCell.coords.y);
	}

	//Creates a wall between given cells and direction
	private void CreateWall(MapCell cell, MapCell otherCell, MapDirection direction) {
		MapWall wall = Instantiate(wallPrefab) as MapWall;
		wall.name = "MapWall " + direction;
		wall.Initialize(cell, otherCell, direction);
		if (otherCell != null) {
			walls.Add(wall);
			wall = Instantiate(wallPrefab) as MapWall;
			wall.name = "MapWall " + direction.GetOpposite();
			wall.Initialize(otherCell, cell, direction.GetOpposite());
			//Debug.Log("Creating wall from: " + cell.coords.x + "," + cell.coords.y + " to: " + otherCell.coords.x + "," + otherCell.coords.y);
		} else {
			//Debug.Log("Creating a " + direction + " border wall at: " + cell.coords.x + "," + cell.coords.y);
		}
	}

	//Creates a score orb at given cell with given score value
	private void CreateScoreOrb(MapCell cell) {
		ScoreOrb orb = Instantiate(scoreOrbPrefab) as ScoreOrb;
		if (scoreOrbCount % 25 == 0) { //Temporary higher value score placement determination
			orb.Initialize(cell, 5);
		} else {
			orb.Initialize(cell, 1);
		}
		scoreOrbCount++;
	}

	//Creates the teleporting borders
	private void CreateBorders(){
		MapDirection[] directions = { (MapDirection)0, (MapDirection)1, (MapDirection)2, (MapDirection)3 };
		foreach (MapDirection direction in directions) {
			MapBorder border = Instantiate(borderPrefab) as MapBorder;
			border.name = "MapBorder " + direction;
			border.transform.parent = transform;
			border.Initialize(direction, size);
		}
		Debug.Log("Finished generating teleporting map borders.");
	}

	//Begins and controls the generation process
	public void Generate() {
		Debug.Log("Starting map generation of " + (size * size) + " cells.");
		CreateBorders();
		cells = new MapCell[size, size];
		MapCell startCell = CreateCell(new Vector2Int(RandomCoordinates.x, RandomCoordinates.y));
		CreateScoreOrb(startCell);
		startCoords = startCell.coords;
		GeneratePassagesFrom(startCell, cells, mapBorder);
		if (cellCount >= (size * size)) {
			Debug.Log("Finished map generation.");
			RemoveRandomWalls(cells, wallRemovalPercent);
		}
	}

	//Recursive-backtracing maze algorithm implementation
	private void GeneratePassagesFrom(MapCell currentCell, MapCell[,] cells, bool mapBorder) {
		MapDirection[] directions = { (MapDirection)0, (MapDirection)1, (MapDirection)2, (MapDirection)3 };
		Shuffle.ShuffleList(directions); //Randomly shuffles the directions

		foreach (MapDirection direction in directions) { //Loops through each direction at current cell
			Vector2Int nextCellCoords = currentCell.coords + direction.ToVector2Int();
			if (ContainsCoordinates(nextCellCoords)) { //Checks if next cell's coords are outside the map
				if (cells[nextCellCoords.x, nextCellCoords.y] == null) { //Checks if there is already a cell at next cell's coords
					cells[nextCellCoords.x, nextCellCoords.y] = CreateCell(nextCellCoords); //Creates a cell at next cell's coords
					CreateScoreOrb(cells[nextCellCoords.x, nextCellCoords.y]); //Creates a score orb at next cell
					CreatePassage(currentCell, cells[nextCellCoords.x, nextCellCoords.y], direction); //Creates a passage between current cell and next cell
					GeneratePassagesFrom(cells[nextCellCoords.x, nextCellCoords.y], cells, mapBorder); //Recurses at next cell
				} else if (currentCell.GetEdge(direction) == null) { //Checks if there is already a wall in that direction
						CreateWall(currentCell, cells[nextCellCoords.x, nextCellCoords.y], direction); //Creates a wall between current cell and next cell
				}
			} else {
				if (mapBorder) { //Checks if mapBorder option is enabled
					CreateWall(currentCell, null, direction); //Creates a wall between current cell and map edge
				} else {
					//CreatePassage(currentCell, null, direction); //Creates a passage between current cell and map edge
				}
			}
		}
	}

	//Removes random walls as a given percentage of total walls
	private void RemoveRandomWalls(MapCell[,] cells, int wallRemovalPercentage) {
		int wallsToRemove = (int)((wallRemovalPercentage / 100.0) * walls.Count);
		for (int i=0; i<wallsToRemove; i++) {
			MapCellEdge randomWall = walls[UnityEngine.Random.Range(0, walls.Count)];
			if (randomWall.otherCell != null) {
				MapCellEdge otherWall = randomWall.otherCell.GetEdge(randomWall.direction.GetOpposite());
				walls.Remove(otherWall);
				Destroy(otherWall.gameObject);
			}
			//Debug.Log("Removing " + randomWall.direction +  " Wall at: " + randomWall.cell.coords.x + "," + randomWall.cell.coords.y);
			walls.Remove(randomWall);
			Destroy(randomWall.gameObject);
		}
		Debug.Log("Finished removing " + wallsToRemove + " walls.");
	}

}

//Generic Shuffle Class
public static class Shuffle {
	private static System.Random random = new System.Random();

	//Shuffles given list
	public static void ShuffleList<E>(IList<E> list) {
		if (list.Count > 1) {
			for (int i = list.Count - 1; i >= 0; i--) {
				E tmp = list[i];
				int randomIndex = random.Next(i + 1);

				//Swap elements
				list[i] = list[randomIndex];
				list[randomIndex] = tmp;
			}
		}
	}
}
