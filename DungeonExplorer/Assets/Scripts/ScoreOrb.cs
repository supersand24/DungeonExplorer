using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ScoreOrb : MonoBehaviour
{

	MapCell cell;
	private int scoreValue;

	public void OnTriggerEnter2D(Collider2D collision) {
		if (collision.gameObject.CompareTag("Player")) {
			FindObjectOfType<Map>().IncScore(scoreValue);
			//Debug.Log("Increasing Score");
			Destroy(this.gameObject);
		}
	}

	public void Initialize(MapCell cell, int value) {
		this.cell = cell;
		this.scoreValue = value;
		this.transform.parent = cell.transform;
		this.name = "ScoreOrb";
		this.transform.localPosition = Vector2.zero;
		if (value >= 5) {
			this.GetComponentInChildren<SpriteRenderer>().sprite = Resources.Load<Sprite>("score5Texture");
			//Debug.Log("Changing Score Value to 5 at " + cell.coords.x + "," + cell.coords.y);
		}
	}

}
