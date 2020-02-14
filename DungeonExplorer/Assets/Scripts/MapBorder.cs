using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class MapBorder : MonoBehaviour {

	private GameObject thePlayer;
	private int mapSize;
	private MapDirection direction;
	private float teleportExtra;

	//Teleports the player if they collide with the border to the opposite side with an offset multiplied by the mapsize normalized to 10
	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.gameObject.CompareTag("Player")) {
			thePlayer = collision.gameObject;
		} else {
			Debug.Log("Non-player collision detected");
		}
		switch (direction) {
			case ((MapDirection) 0): //North
				thePlayer.transform.position = new Vector2(thePlayer.transform.position.x, thePlayer.transform.position.y*-1 + teleportExtra);
				//Debug.Log("Teleporting from North Border to South Border");
				break;
			case ((MapDirection) 1): //East
				thePlayer.transform.position = new Vector2(thePlayer.transform.position.x*-1 + teleportExtra, thePlayer.transform.position.y);
				//Debug.Log("Teleporting from East Border to West Border");
				break;
			case ((MapDirection) 2): //South
				thePlayer.transform.position = new Vector2(thePlayer.transform.position.x, thePlayer.transform.position.y*-1 - teleportExtra);
				//Debug.Log("Teleporting from South Border to North Border");
				break;
			case ((MapDirection) 3): //West
				thePlayer.transform.position = new Vector2(thePlayer.transform.position.x*-1 - teleportExtra, thePlayer.transform.position.y);
				//Debug.Log("Teleporting from West Border to East Border");
				break;
			default:
				Debug.Log("Map Border does not have a designated direction");
				break;
		}

	}

	public void Initialize(MapDirection direction, int mapSize) {
		this.direction = direction;
		this.mapSize = mapSize;
		this.transform.localPosition = Vector2.zero;
		this.transform.localRotation = direction.ToRotation();
		this.GetComponent<BoxCollider2D>().size = new Vector3(mapSize, 1, 1);
		this.GetComponent<BoxCollider2D>().offset = new Vector2(0, mapSize/2 + 0.5f);
		this.teleportExtra = (mapSize/10) * 0.05f;
	}

}
