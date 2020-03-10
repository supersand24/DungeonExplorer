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

	//private int score;
	//private Time time;

	private void BeginGame() {
		Physics2D.gravity = Vector2.zero;

		mapInstance = Instantiate(mapPrefab) as Map;
		mapInstance.name = "Map";
		mapInstance.Generate();

		playerInstance = Instantiate(playerPrefab) as PlayerController;
		playerInstance.name = "Player";
		playerInstance.Generate(mapInstance.StartCoords.x - (mapInstance.size / 2), mapInstance.StartCoords.y - (mapInstance.size / 2));

		enemyInstance = Instantiate(enemyPrefab) as Enemy;
		enemyInstance.name = "Enemy";
		enemyInstance.Generate(playerInstance.gameObject, mapInstance);

		Camera.main.orthographicSize = (5 * (mapInstance.size/10f));
	}

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
		//time = new Time();
		//score = 0;
    }

    // Update is called once per frame
    private void Update() {
		//temporary new map generation key
		if (Input.GetKeyDown(KeyCode.R)) {
			NewMap();
		}
		
		if (mapInstance.score >= (mapInstance.size * mapInstance.size)) {
			NewMap();
		}

		if (enemyInstance.HitPlayer) {
			NewMap();
		}

		//if (score != mapInstance.score) {
		//	enemyInstance.Move1();
		//	score = mapInstance.score;
		//}

		if (mapInstance.score > 5) {
			enemyInstance.Move4();
		}
	}

	

}
