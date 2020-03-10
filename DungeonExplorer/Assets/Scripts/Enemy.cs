using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public float speed;

	private GameObject thePlayer;
	private MapCell currentCell;
	private bool hitPlayer = false;
	private int mapSize;
	private Rigidbody2D rigidBody;

	public bool HitPlayer {
		get {
			return hitPlayer;
		}
	}

	public void Generate(GameObject player, Map map) {
		this.thePlayer = player;
		this.currentCell = map.GetCell(map.RandomCoordinates);
		while (currentCell.GetDistance(map.StartCoords) < 5) {
			this.currentCell = map.GetCell(map.RandomCoordinates);
		}
		this.mapSize = map.size;
		this.transform.position = new Vector2(currentCell.coords.x - (mapSize / 2) + 0.5f, currentCell.coords.y - (mapSize / 2) + 0.5f);
		this.rigidBody = GetComponent<Rigidbody2D>();
		Debug.Log("Generating Enemy at " + this.transform.position);
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.gameObject.CompareTag("Player")) {
			this.hitPlayer = true;
		} else {
			//Debug.Log("Non-player collision detected");
		}
	}

	private List<MapDirection> GetPossibleMoves() { //Returns a list of directions that are passages from current cell
		MapDirection[] directions = { (MapDirection)0, (MapDirection)1, (MapDirection)2, (MapDirection)3 };
		List<MapDirection> possibles = new List<MapDirection>();
		foreach (MapDirection direction in directions){
			if (currentCell.GetEdge(direction) is MapPassage) {
				possibles.Add(direction);
			}
		}
		return possibles;
	}

	public void Move1() {
		List<MapDirection> possibleMoves = GetPossibleMoves();
		List<MapCell> orderedMoves = new List<MapCell>();

		foreach (MapDirection direction in possibleMoves) { //Orders possible moves by distance to player
			MapCell nextCell = currentCell.GetEdge(direction).otherCell;
			Vector2 playerCoords = new Vector2(thePlayer.transform.position.x, thePlayer.transform.position.y);
			if (orderedMoves.Count > 0 && (nextCell.GetDistance(playerCoords) < orderedMoves[0].GetDistance(playerCoords))) { 
				orderedMoves.Insert(0, nextCell);
			} else {
				orderedMoves.Add(nextCell);
			}
		}

		this.transform.position = new Vector2(orderedMoves[0].coords.x - (mapSize / 2) + 0.5f, orderedMoves[0].coords.y - (mapSize / 2) + 0.5f);
		this.currentCell = orderedMoves[0];

		Debug.Log("Moving Enemy to " + this.transform.position);
	}

	public void Move2() {
		float dx = thePlayer.transform.position.x - this.transform.position.x;
		float dy = thePlayer.transform.position.y - this.transform.position.y;

		if (dx > dy && dx > 0 && currentCell.GetEdge(MapDirection.East) is MapPassage) {
			MapCell nextCell = currentCell.GetEdge(MapDirection.East).otherCell;
			this.transform.position = new Vector2(nextCell.coords.x - (mapSize / 2) + 0.5f, nextCell.coords.y - (mapSize / 2) + 0.5f);
			this.currentCell = nextCell;
			Debug.Log("Moving Enemy to " + this.transform.position);
		} else if (dx > dy && dx < 0 && currentCell.GetEdge(MapDirection.West) is MapPassage) {
			MapCell nextCell = currentCell.GetEdge(MapDirection.West).otherCell;
			this.transform.position = new Vector2(nextCell.coords.x - (mapSize / 2) + 0.5f, nextCell.coords.y - (mapSize / 2) + 0.5f);
			this.currentCell = nextCell;
			Debug.Log("Moving Enemy to " + this.transform.position);
		} else if (dx < dy && dy > 0 && currentCell.GetEdge(MapDirection.North) is MapPassage) {
			MapCell nextCell = currentCell.GetEdge(MapDirection.North).otherCell;
			this.transform.position = new Vector2(nextCell.coords.x - (mapSize / 2) + 0.5f, nextCell.coords.y - (mapSize / 2) + 0.5f);
			this.currentCell = nextCell;
			Debug.Log("Moving Enemy to " + this.transform.position);
		} else if (dx < dy && dy < 0 && currentCell.GetEdge(MapDirection.South) is MapPassage) {
			MapCell nextCell = currentCell.GetEdge(MapDirection.South).otherCell;
			this.transform.position = new Vector2(nextCell.coords.x - (mapSize / 2) + 0.5f, nextCell.coords.y - (mapSize / 2) + 0.5f);
			this.currentCell = nextCell;
			Debug.Log("Moving Enemy to " + this.transform.position);
		} else {
			Debug.Log("Enemy has no valid move.");
		}
	}

	public void Move3() {
		float dx = thePlayer.transform.position.x - this.transform.position.x;
		float dy = thePlayer.transform.position.y - this.transform.position.y;

		//if (dx >= dy) {
		//	this.transform.position = new Vector2(this.transform.position.x + dx * 2 * Time.deltaTime, this.transform.position.y);
		//} else {
		//	this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + dy * 2 * Time.deltaTime);
		//}
		this.transform.position += new Vector3(dx * 1 * Time.deltaTime, dy * 1 * Time.deltaTime, 0);

		//Debug.Log("Moving Enemy to " + this.transform.position);
	}

	public void Move4() {
		this.transform.position = Vector2.MoveTowards(this.transform.position, thePlayer.transform.position, speed * Time.deltaTime);
	}

}
