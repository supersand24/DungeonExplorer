using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

	public Vector2Int size; //map size
	public MapCell cellPrefab;
	public MapPassage passagePrefab;
	public MapWall wallPrefab;

	private MapCell[,] cells; //the map

	//Returns random coords within the map's coord limits
	public Vector2Int RandomCoordinates {
		get {
			return new Vector2Int((int)Random.Range(0, size.x), (int)Random.Range(0, size.y));
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
		Debug.Log("Creating " + newCell.name);
		newCell.transform.parent = transform;
		newCell.transform.localPosition = new Vector2(coords.x - size.x * 0.5f + 0.5f, coords.y - size.y * 0.5f + 0.5f);
		return newCell;
	}

	//Creates a passage (gap between cells) at given cells and direction
	private void CreatePassage(MapCell cell, MapCell otherCell, MapDirection direction) {
		MapPassage passage = Instantiate(passagePrefab) as MapPassage;
		passage.Initialize(cell, otherCell, direction);
		passage = Instantiate(passagePrefab) as MapPassage;
		passage.Initialize(otherCell, cell, direction.GetOpposite());
		Debug.Log("Creating passage from: " + cell.coords.x + "," + cell.coords.y + " to: " + otherCell.coords.x + "," + otherCell.coords.y);
	}

	//Creates a wall between given cells and direction
	private void CreateWall(MapCell cell, MapCell otherCell, MapDirection direction) {
		MapWall wall = Instantiate(wallPrefab) as MapWall;
		wall.Initialize(cell, otherCell, direction);
		if (otherCell != null) {
			wall = Instantiate(wallPrefab) as MapWall;
			wall.Initialize(otherCell, cell, direction.GetOpposite());
			Debug.Log("Creating wall from: " + cell.coords.x + "," + cell.coords.y + " to: " + otherCell.coords.x + "," + otherCell.coords.y);
		} else {
			Debug.Log("Creating a " + direction + " border wall at: " + cell.coords.x + "," + cell.coords.y);
		}
	}

	//Begins the generation process
	public void Generate() {
		cells = new MapCell[size.x, size.y];
		MapCell startCell = CreateCell(new Vector2Int(RandomCoordinates.x, RandomCoordinates.y));
		GeneratePassagesFrom(startCell, cells);
		Debug.Log("Finished.");
	}

	//Generation algorithm implementation
	private void GeneratePassagesFrom(MapCell currentCell, MapCell[,] cells) {
		MapDirection[] directions = { (MapDirection)0, (MapDirection)1, (MapDirection)2, (MapDirection)3 };
		Shuffle.ShuffleList(directions);

		foreach (MapDirection direction in directions) { 
			Vector2Int nextCellCoords = currentCell.coords + MapDirections.vectors[(int)direction];
			if (ContainsCoordinates(nextCellCoords)) { //Creates wall in direction if next cell's coords are outside the map
				if (cells[nextCellCoords.x, nextCellCoords.y] == null) { //Either recurses in next cell or creates a wall between current and next cells
					cells[nextCellCoords.x, nextCellCoords.y] = CreateCell(nextCellCoords);
					CreatePassage(currentCell, cells[nextCellCoords.x, nextCellCoords.y], direction);
					GeneratePassagesFrom(cells[nextCellCoords.x, nextCellCoords.y], cells);
				} else {
					if (currentCell.GetEdge(direction) == null) {
						CreateWall(currentCell, cells[nextCellCoords.x, nextCellCoords.y], direction);
					}
				}
			} else {
				CreateWall(currentCell, null, direction);
			}
		}
	}

}

public static class Shuffle {
	private static System.Random random = new System.Random();

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
