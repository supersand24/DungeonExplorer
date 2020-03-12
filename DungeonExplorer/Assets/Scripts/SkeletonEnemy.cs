using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonEnemy : MonoBehaviour
{
	public float speed;

	private GameObject thePlayer;
	private MapCell currentCell;
	private bool hitPlayer = false;
	private int mapSize;
	private List<MapCell> orderedMoves = new List<MapCell>();
	private List<MapDirection> possibleMoves = new List<MapDirection>();

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
		Debug.Log("Generating GhostEnemy at " + this.transform.position);
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.gameObject.CompareTag("Player")) {
			this.hitPlayer = true;
		} else {
			//Debug.Log("Non-player collision detected");
		}
	}

	//public void Move() {
	//	orderedMoves.Clear();
	//	possibleMoves.Clear();
	//	foreach (MapDirection direction in MapDirections.directions) { //Finds the directions that have passages
	//		if (currentCell.GetEdge(direction) is MapPassage) {
	//			possibleMoves.Add(direction);
	//		}
	//	}

	//	foreach (MapDirection direction in possibleMoves) { //Orders possible moves by future distance to player
	//		MapCell nextCell = currentCell.GetEdge(direction).otherCell;
	//		Vector2 playerCoords = new Vector2(thePlayer.transform.position.x, thePlayer.transform.position.y);
	//		if (orderedMoves.Count > 0 && (nextCell.GetDistance(playerCoords) < orderedMoves[0].GetDistance(playerCoords))) {
	//			orderedMoves.Insert(0, nextCell);
	//		} else {
	//			orderedMoves.Add(nextCell);
	//		}
	//	}

	//	this.transform.position = new Vector2(orderedMoves[0].coords.x - (mapSize / 2) + 0.5f, orderedMoves[0].coords.y - (mapSize / 2) + 0.5f);
	//	this.currentCell = orderedMoves[0];

	//	Debug.Log("Moving Enemy to " + this.transform.position);
	//}

	public IEnumerator Move() {
		while (!(HitPlayer)) {
			orderedMoves.Clear();
			possibleMoves.Clear();
			foreach (MapDirection direction in MapDirections.directions) { //Finds the directions that have passages
				if (currentCell.GetEdge(direction) is MapPassage) {
					possibleMoves.Add(direction);
				}
			}

			foreach (MapDirection direction in possibleMoves) { //Orders possible moves by future distance to player
				MapCell nextCell = currentCell.GetEdge(direction).otherCell;
				Vector2 playerCoords = new Vector2(thePlayer.transform.position.x, thePlayer.transform.position.y);
				if (orderedMoves.Count > 0 && (nextCell.GetDistance(playerCoords) < orderedMoves[0].GetDistance(playerCoords))) {
					orderedMoves.Insert(0, nextCell);
				} else {
					orderedMoves.Add(nextCell);
				}
			}

			//Debug.Log(possibleMoves);
			//Debug.Log(orderedMoves);
			this.transform.position = new Vector2(orderedMoves[0].coords.x - (mapSize / 2) + 0.5f, orderedMoves[0].coords.y - (mapSize / 2) + 0.5f);
			this.currentCell = orderedMoves[0];
			Debug.Log("Moving Enemy to " + this.transform.position);

			yield return new WaitForSeconds(10f - speed);
		}
	}
}
