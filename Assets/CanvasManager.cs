﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour {

    private Vector3 closedPosition;
    public float time = -1f;

	// Use this for initialization
	void Start () {
        closedPosition = transform.position;
        Debug.Log(closedPosition);
	}
	
	// Update is called once per frame
	void Update () {
        if (time >= 0)
        {
            time += Time.deltaTime;
            GameState.Paused = true;
        }
        if (time > 1)
        {
            transform.position = Vector3.Lerp(transform.position, closedPosition, Time.deltaTime * 3);
        }
        if (time > 5f)
            time = -1f;
    }
}
