using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour {

	private Text text;
	private int framec = 0;
	public int updateRate = 10;

	// Use this for initialization
	void Start () {
		text = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (framec % updateRate == 0) {
			text.text = "FPS: " + System.Math.Round ((1 / Time.unscaledDeltaTime)).ToString ();
		}
		framec++;
	}
}
