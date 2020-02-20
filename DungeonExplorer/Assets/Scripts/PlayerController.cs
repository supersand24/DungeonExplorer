using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{

	public float speed;
	//private BoxCollider2D collider;
	private Rigidbody2D rigidBody;

	public void Generate(float startX, float startY) {
		this.transform.position = new Vector2(startX + 0.5f, startY + 0.5f);
		rigidBody = GetComponent<Rigidbody2D>();
	}
	public void Update(){
		this.transform.position += new Vector3(Input.GetAxis("Horizontal") * this.speed * Time.deltaTime, Input.GetAxis("Vertical") * this.speed * Time.deltaTime, 0);
		// update to give it a sprint function: speed=speed*2;
	}
}
