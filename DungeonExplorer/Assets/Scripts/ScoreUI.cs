using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
	public static int score;
	private Text scoreText;

	private void Start() {
		this.scoreText = GetComponent<Text>();
		scoreText.text = "Score: 0";
	}

    // Update is called once per frame
    void Update(){
		scoreText.text = "Score: " + score;
    }
}
