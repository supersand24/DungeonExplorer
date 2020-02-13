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

	public Vector2Int size; //map size
	public bool mapBorder; //true=generate walls around map
	public int wallRemovalPercent; //the number of walls to remove after generation the mazes
	public int score;

	private MapCell[,] cells; //the map
	private List<MapCellEdge> walls = new List<MapCellEdge>();
	private int cellCount;
	private Vector2Int startCoords;

	//increases the score by one
	public void IncScore() {
		this.score++;
	}

	//returns the coords of the initial cell
	public Vector2Int GetStartCoords() {
		return startCoords;
	}

	//Returns random coords within the map's coord limits
	public Vector2Int RandomCoordinates {
		get {
			return new Vector2Int((int)UnityEngine.Random.Range(0, size.x), (int)UnityEngine.Random.Range(0, size.y));
		}
	}

	//Returns true if given coords are within the map's coord limits
	public bool ContainsCoordinates(Vector2Int coords) {
		return coords.x >= 0 && coords.x < size.x && coords.y >= 0 && coords.y < size.y;
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
		newCell.transform.localPosition = new Vector2(coords.x - size.x * 0.5f + 0.5f, coords.y - size.y * 0.5f + 0.5f);
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
		walls.Add(wall);
		wall.Initialize(cell, otherCell, direction);
		if (otherCell != null) {
			wall = Instantiate(wallPrefab) as MapWall;
			wall.name = "MapWall " + direction.GetOpposite();
			wall.Initialize(otherCell, cell, direction.GetOpposite());
			//Debug.Log("Creating wall from: " + cell.coords.x + "," + cell.coords.y + " to: " + otherCell.coords.x + "," + otherCell.coords.y);
		} else {
			//Debug.Log("Creating a " + direction + " border wall at: " + cell.coords.x + "," + cell.coords.y);
		}
	}

	private void CreateScoreOrb(MapCell cell) {
		ScoreOrb orb = Instantiate(scoreOrbPrefab) as ScoreOrb;
		orb.transform.parent = cell.transform;
		orb.name = "ScoreOrb";
		orb.transform.localPosition = Vector2.zero;
	}

	//Creates the borders
	private void CreateBorders(){
		MapDirection[] directions = { (MapDirection)0, (MapDirection)1, (MapDirection)2, (MapDirection)3 };
		foreach (MapDirection direction in directions) {
			MapBorder border = Instantiate(borderPrefab) as MapBorder;
			border.name = "MapBorder " + direction;
			border.transform.parent = transform;
			border.Initialize(direction, new Vector2(size.x, size.y));
		}
	}

	//Begins and controls the generation process
	public void Generate() {
		Debug.Log("Starting map generation of " + (size.x * size.y) + " cells.");
		cells = new MapCell[size.x, size.y];
		MapCell startCell = CreateCell(new Vector2Int(RandomCoordinates.x, RandomCoordinates.y));
		CreateScoreOrb(startCell);
		startCoords = startCell.coords;
		GeneratePassagesFrom(startCell, cells, mapBorder);
		if (cellCount >= (size.x * size.y)) {
			Debug.Log("Finished map generation.");
			RemoveRandomWalls(cells, wallRemovalPercent);
		}
		CreateBorders();
	}

	//Recursive-backtracing maze algorithm implementation
	private void GeneratePassagesFrom(MapCell currentCell, MapCell[,] cells, bool mapBorder) {
		MapDirection[] directions = { (MapDirection)0, (MapDirection)1, (MapDirection)2, (MapDirection)3 };
		Shuffle.ShuffleList(directions);

		foreach (MapDirection direction in directions) {
			Vector2Int nextCellCoords = currentCell.coords + direction.ToVector2Int();
			if (ContainsCoordinates(nextCellCoords)) { //Creates wall in direction if next cell's coords are outside the map
				if (cells[nextCellCoords.x, nextCellCoords.y] == null) { //Either recurses in next cell or creates a wall in direction if there is already a cell there
					cells[nextCellCoords.x, nextCellCoords.y] = CreateCell(nextCellCoords);
					CreateScoreOrb(cells[nextCellCoords.x, nextCellCoords.y]);
					CreatePassage(currentCell, cells[nextCellCoords.x, nextCellCoords.y], direction);
					GeneratePassagesFrom(cells[nextCellCoords.x, nextCellCoords.y], cells, mapBorder);
				} else { //Sometimes getting duplicates
					if (currentCell.GetEdge(direction) == null) {
						CreateWall(currentCell, cells[nextCellCoords.x, nextCellCoords.y], direction);
					}
				}
			} else {
				if (mapBorder) {
					CreateWall(currentCell, null, direction);
				}
			}
		}
	}

	//Removes random walls as a given percentage of total walls
	private void RemoveRandomWalls(MapCell[,] cells, int wallRemovalPercentage) {
		int wallsToRemove = (int)((wallRemovalPercentage / 100.0) * walls.Count);
		Debug.Log("Removing " + wallsToRemove + " walls.");
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
