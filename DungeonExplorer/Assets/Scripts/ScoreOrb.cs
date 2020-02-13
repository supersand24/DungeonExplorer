using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ScoreOrb : MonoBehaviour
{

	public void OnTriggerEnter2D(Collider2D collision) {
		if (collision.gameObject.CompareTag("Player")) {
			FindObjectOfType<Map>().IncScore();
			Debug.Log("Increasing Score");
			Destroy(this.gameObject);
		}
	}

}
