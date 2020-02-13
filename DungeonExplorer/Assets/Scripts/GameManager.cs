using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public Map mapPrefab;
	//public PlayerController playerPrefab;

	private Map mapInstance;
	//private PlayerController playerInstance;

	private void BeginGame() {
		Physics2D.gravity = Vector2.zero;
		mapInstance = Instantiate(mapPrefab) as Map;
		mapInstance.name = "Map";
		mapInstance.Generate();
		//playerInstance = Instantiate(playerPrefab) as PlayerController;
		//playerInstance.name = "Player";
		//playerInstance.Generate(mapInstance.GetStartCoords().x / 2, mapInstance.GetStartCoords().y / 2);
	}

	private void NewMap() {
		Debug.Log("------------------------");
		StopAllCoroutines();
		Destroy(mapInstance.gameObject);
		//Destroy(playerInstance.gameObject);
		BeginGame();
	}

	// Start is called before the first frame update
	private void Start() {
		BeginGame();
    }

    // Update is called once per frame
    private void Update() {
		//temporary new map generation key
		if (Input.GetKeyDown(KeyCode.R)) {
			NewMap();
		}
		//playerInstance.transform.position += new Vector3(Input.GetAxis("Horizontal") * playerInstance.speed * Time.deltaTime, Input.GetAxis("Vertical") * playerInstance.speed * Time.deltaTime, 0);
	}

}
