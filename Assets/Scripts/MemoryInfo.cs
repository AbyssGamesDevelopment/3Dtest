using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;

public class MemoryInfo : MonoBehaviour {

	private Text text;
	private int framec;
	public int updateRate = 10;

	// Use this for initialization
	void Start () {
		text = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(framec % updateRate == 0)
			text.text = "Memory: " + System.Math.Round(Profiler.GetTotalAllocatedMemoryLong ()/10000000f, 2) + "/" + System.Math.Round(Profiler.GetTotalReservedMemoryLong ()/10000000f, 2) + " MB";
		framec++;
	}
}
