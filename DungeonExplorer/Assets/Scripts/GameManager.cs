using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public Map mapPrefab;

	private Map mapInstance;

	private void BeginGame() {
		mapInstance = Instantiate(mapPrefab) as Map;
		mapInstance.name = "Map";
		mapInstance.Generate();
	}

	private void NewMap() {
		Debug.Log("------------------------");
		StopAllCoroutines();
		Destroy(mapInstance.gameObject);
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
    }

}
