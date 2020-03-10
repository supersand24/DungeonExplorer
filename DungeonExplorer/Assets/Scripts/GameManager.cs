using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public Map mapPrefab;
	public PlayerController playerPrefab;
	public Enemy enemyPrefab;

	private Map mapInstance;
	private PlayerController playerInstance;
	private Enemy enemyInstance;

	private void BeginGame() {
		Physics2D.gravity = Vector2.zero;

		//Create the map
		mapInstance = Instantiate(mapPrefab) as Map;
		mapInstance.name = "Map";
		mapInstance.Generate();

		//Create the player
		playerInstance = Instantiate(playerPrefab) as PlayerController;
		playerInstance.name = "Player";
		playerInstance.Generate(mapInstance.StartCoords.x - (mapInstance.size / 2), mapInstance.StartCoords.y - (mapInstance.size / 2));

		//Create the enemy
		enemyInstance = Instantiate(enemyPrefab) as Enemy;
		enemyInstance.name = "Enemy";
		enemyInstance.Generate(playerInstance.gameObject, mapInstance);

		//Resize the camera to fit map size
		Camera.main.orthographicSize = (5 * (mapInstance.size/10f));
	}

	//Delete all elements and restart
	private void NewMap() {
		Debug.Log("------------------------");
		StopAllCoroutines();
		Destroy(mapInstance.gameObject);
		Destroy(playerInstance.gameObject);
		Destroy(enemyInstance.gameObject);
		BeginGame();
	}

	// Start is called before the first frame update
	private void Start() {
		BeginGame();
    }

    // Update is called once per frame
    private void Update() {
		if (Input.GetKeyDown(KeyCode.R)) { //temporary new map generation key
			NewMap();
		}
		
		if (mapInstance.score >= (mapInstance.size * mapInstance.size)) {
			NewMap();
		}

		if (enemyInstance.HitPlayer) {
			NewMap();
		}

		if (mapInstance.score > 5) {
			enemyInstance.Move4();
		}

		ScoreUI.score = mapInstance.score;
	}

	

}
