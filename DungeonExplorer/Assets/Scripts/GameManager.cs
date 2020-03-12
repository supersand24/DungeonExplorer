using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public Map mapPrefab;
	public PlayerController playerPrefab;
	public GhostEnemy ghostEnemyPrefab;
	public SkeletonEnemy skeletonEnemyPrefab;

	private Map mapInstance;
	private PlayerController playerInstance;
	private GhostEnemy ghostEnemyInstance;
	private SkeletonEnemy skeletonEnemyInstance;

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

		//Create the ghost enemy
		ghostEnemyInstance = Instantiate(ghostEnemyPrefab) as GhostEnemy;
		ghostEnemyInstance.name = "GhostEnemy";
		ghostEnemyInstance.Generate(playerInstance.gameObject, mapInstance);

		//Create the skeleton enemy
		skeletonEnemyInstance = Instantiate(skeletonEnemyPrefab) as SkeletonEnemy;
		skeletonEnemyInstance.name = "GhostEnemy";
		skeletonEnemyInstance.Generate(playerInstance.gameObject, mapInstance);
		StartCoroutine(skeletonEnemyInstance.Move());
		

		//Resize the camera to fit map size
		Camera.main.orthographicSize = (5 * (mapInstance.size/10f));
	}

	//Delete all elements and restart
	private void NewMap() {
		Debug.Log("------------------------");
		StopAllCoroutines();
		Destroy(mapInstance.gameObject);
		Destroy(playerInstance.gameObject);
		Destroy(ghostEnemyInstance.gameObject);
		Destroy(skeletonEnemyInstance.gameObject);
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

		if (ghostEnemyInstance.HitPlayer || skeletonEnemyInstance.HitPlayer) {
			//NewMap();
		}

		if (mapInstance.score > 5) {
			ghostEnemyInstance.Move();
		}

		ScoreUI.score = mapInstance.score;
	}

}
