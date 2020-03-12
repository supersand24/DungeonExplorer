using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostEnemy : MonoBehaviour
{
	public float speed;

	private GameObject thePlayer;
	private MapCell currentCell;
	private bool hitPlayer = false;
	private int mapSize;

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
	
	public void Move() {
		this.transform.position = Vector2.MoveTowards(this.transform.position, thePlayer.transform.position, speed * Time.deltaTime);
	}

}
