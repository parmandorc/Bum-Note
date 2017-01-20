﻿using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public float movementSpeed = 1.0f;
    public float jumpSpeed = 20f;
    public GameObject topFloor;
    public Camera camera;
    public float cameraZoomSpeed = 1.0f;
    private SpriteRenderer spr;
    private Rigidbody2D rb;
    private Instrument isBelowInstrument = null;
    private bool isOnTopFloor = false;
    private Vector3 cameraDefaultPosition;
    private float cameraDefaultSize;
    private float cameraLerpPosition;
    private bool isZoomed;
    Animator anim;
	// Use this for initialization
	void Start () {

        rb = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        cameraDefaultPosition = camera.transform.position;
        cameraDefaultSize = camera.orthographicSize;
	}
	
	// Update is called once per frame
	void Update () {
        float dx = Input.GetAxis("Horizontal");

        if (dx > 0.1f)
        {
            spr.flipX = false;
        }
        else if (dx < -0.1f)
        {
            spr.flipX = true;
        }

        //simple player animations...
        if (rb.velocity.y == 0)
        {
            if (dx != 0)
            {
                anim.Play("Walk");
            }
            else
            {
                anim.Play("Idle");
            }
        }
        else
        {
            anim.Play("Jump");
        }


        if (dx != 0.0f && !isOnTopFloor)
        {
            gameObject.transform.Translate(dx * movementSpeed * Time.deltaTime, 0, 0);
        }

        //such lazy floor """""detection"""""
        if (rb.velocity.y == 0.0f)
        {
            if (transform.position.y < -3.17f && isBelowInstrument)               //on bottom layer
            {
                if (Input.GetButtonDown("Jump"))
                {
                    rb.AddForce(Vector2.up * jumpSpeed);
                    topFloor.SetActive(false);
                    isOnTopFloor = true;
                }
            }

            if (transform.position.y > -0.26f)
            {
                if (Input.GetButtonDown("Jump"))
                {
                    rb.AddForce(Vector2.up * (jumpSpeed * 0.3f));
                    topFloor.SetActive(false);
                    isOnTopFloor = false;
                }

                if (Input.GetButtonDown("Fix") && isBelowInstrument)
                {
                    isBelowInstrument.Fix();
                }
            }
        }
        if (transform.position.y > 0.02f)
        {
            topFloor.SetActive(true);
        }

        // Camera animation
        if (isOnTopFloor)
        {
            cameraLerpPosition = Mathf.Clamp01(cameraLerpPosition + cameraZoomSpeed * Time.deltaTime);
            camera.transform.position = new Vector3(
                    Mathf.Lerp(camera.transform.position.x, isBelowInstrument.transform.position.x, cameraZoomSpeed * Time.deltaTime),
                    Mathf.Lerp(camera.transform.position.y, isBelowInstrument.transform.position.y, cameraZoomSpeed * Time.deltaTime),
                    cameraDefaultPosition.z
                );
            camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, 4, cameraZoomSpeed * Time.deltaTime);
        }
        else
        {
            cameraLerpPosition = Mathf.Clamp01(cameraLerpPosition - cameraZoomSpeed * Time.deltaTime);
            camera.transform.position = new Vector3(
                    Mathf.Lerp(camera.transform.position.x, cameraDefaultPosition.x, cameraZoomSpeed * Time.deltaTime),
                    Mathf.Lerp(camera.transform.position.y, cameraDefaultPosition.y, cameraZoomSpeed * Time.deltaTime),
                    cameraDefaultPosition.z
                );
            camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, cameraDefaultSize, cameraZoomSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isBelowInstrument = collision.gameObject.GetComponent<Instrument>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isBelowInstrument = null;
    }
}
