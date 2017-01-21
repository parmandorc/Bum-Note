﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Instrument : MonoBehaviour {

    public float health;
    float randDetoriation;
    TextMesh text;
    public bool det = false;
    Renderer rend;

    private int timesDeteriorated;
     
	// Use this for initialization
	void Start () {
        randDetoriation = Random.Range(0.8f, 3f);
        text = transform.GetChild(0).GetChild(0).GetComponent<TextMesh>();
        rend = transform.GetChild(0).GetComponent<Renderer>();

        //how much time until an instrument starts detoriating
        if (!det)
            Invoke("Det", Random.Range(3f, Mathf.Clamp(14f - timesDeteriorated, 4f, 100f)));
	}

    void Det()
    {
        det = true;
        timesDeteriorated++;
    }
	
	// Update is called once per frame
	void Update () {
        if (GameState.Paused) return;

        text.text = "" + health.ToString("F2");

        if (det) //if instrumet has started detoriating, det by randDetoriation (set in Start());
        {
            health -= Time.deltaTime * randDetoriation * (1 + timesDeteriorated);
            if (health < 0f)
                health = 0f;
        }
        else
        {
            health = 100f;
        }

        // Handle wave texture
        if (health * 0.01f > 2f / 3f)
            rend.material.color = Color.Lerp(Color.yellow, Color.green, Mathf.InverseLerp(2f / 3f, 1f, health * 0.01f));
        else
            rend.material.color = Color.Lerp(Color.red, Color.yellow, Mathf.InverseLerp(0f, 2f / 3f, health * 0.01f));
        rend.material.SetTextureScale("_MainTex", new Vector2(2f - health * 0.01f, Mathf.Lerp(0.75f, 1.35f, health * 0.01f)));
        Vector2 offset = rend.material.GetTextureOffset("_MainTex");
        rend.material.SetTextureOffset("_MainTex", new Vector2(offset.x + Time.deltaTime * (2.5f - health * 0.02f), Mathf.Lerp(0.125f, -0.185f, health * 0.01f)));
    }

    public void Fix()
    {
        health += 20f;
        if (health >= 100f) // Fully fixed
        {
            det = false;
            Invoke("Start", 0);
        }
    }
}
