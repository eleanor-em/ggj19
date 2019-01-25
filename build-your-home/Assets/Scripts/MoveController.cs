using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour {
    public float normalSpeed;
    public float dashSpeed;
    public GameObject tileMap;

    private Collider2D tileMapCollider;
    new private Collider2D collider;

    private float Speed {
        get {
            if (Input.GetButton("Dash")) {
                return dashSpeed;
            } else {
                return normalSpeed;
            }
        }
    }

    private void Start() {
        tileMapCollider = tileMap.GetComponent<Collider2D>();
        collider = GetComponent<Collider2D>();
    }

    void Update () {
        var prevPos = transform.position;
        var horizontal = Input.GetAxis("Horizontal") * Vector3.right * Time.deltaTime * Speed;
        var vertical = Input.GetAxis("Vertical") * Vector3.up * Time.deltaTime * Speed;

        // try to move horizontally
        transform.position += horizontal;
        if (!IsOnMap()) {
            transform.position = prevPos;
        } else {
            prevPos = transform.position;
        }
        // try to move vertically
        transform.position += vertical;
        if (!IsOnMap()) {
            transform.position = prevPos;
        }
	}

    private bool IsOnMap() {
        return tileMapCollider.bounds.Contains(collider.bounds.min)
            && tileMapCollider.bounds.Contains(collider.bounds.max);
    }
}
