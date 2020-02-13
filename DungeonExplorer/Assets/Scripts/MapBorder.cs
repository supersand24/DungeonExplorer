using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class MapBorder : MonoBehaviour {

	private GameObject thePlayer;
	private Vector2 mapSize;
	private MapDirection direction;
	private float teleportExtra = 0.55f;

	//Teleports the player if they collide with the border to the opposite side with an offset multiplied by the mapsize normalized to 10
	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.gameObject.CompareTag("Player")) {
			thePlayer = collision.gameObject;
		} else {
			Debug.Log("Non-player collision detected");
		}
		switch (direction) {
			case ((MapDirection) 0): //North
				thePlayer.transform.position = new Vector2(thePlayer.transform.position.x, thePlayer.transform.position.y - 10*(mapSize.y/10) + teleportExtra*(mapSize.y/10));
				Debug.Log("Teleporting from North Border to South Border");
				break;
			case ((MapDirection) 1): //East
				thePlayer.transform.position = new Vector2(thePlayer.transform.position.x - 10*(mapSize.x/10) + teleportExtra*(mapSize.x/10), thePlayer.transform.position.y);
				Debug.Log("Teleporting from East Border to West Border");
				break;
			case ((MapDirection) 2): //South
				thePlayer.transform.position = new Vector2(thePlayer.transform.position.x, thePlayer.transform.position.y + 10*(mapSize.y/10) - teleportExtra*(mapSize.y/10));
				Debug.Log("Teleporting from South Border to North Border");
				break;
			case ((MapDirection) 3): //West
				thePlayer.transform.position = new Vector2(thePlayer.transform.position.x + 10*(mapSize.x/10) - teleportExtra*(mapSize.x/10), thePlayer.transform.position.y);
				Debug.Log("Teleporting from West Border to East Border");
				break;
			default:
				Debug.Log("Map Border does not have a designated direction");
				break;
		}

	}

	public void Initialize(MapDirection direction, Vector2 mapSize) {
		this.direction = direction;
		this.mapSize = mapSize;
		this.tag = "MapBorder";
		transform.localPosition = Vector2.zero;
		transform.localRotation = direction.ToRotation();
	}

}
