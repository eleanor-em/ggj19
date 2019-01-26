using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoveController : MonoBehaviour {
    public LayerMask grabbableLayer;
    public float Speed = 5.0f;
    public GameObject tileMap;
    public Vector3 Direction {
        get;
        private set;
    }

    private Vector3 prevPos;
    private Collider2D tileMapCollider;
    new private Collider2D collider;
    private GrabController grabCtl;

    private void Start() {
        tileMapCollider = tileMap.GetComponent<Collider2D>();
        collider = GetComponent<Collider2D>();
        grabCtl = GetComponent<GrabController>();
    }

    void Update() {
        prevPos = transform.position;
        var horizontal = Input.GetAxis("Horizontal") * Vector3.right * Time.deltaTime * Speed;
        var vertical = Input.GetAxis("Vertical") * Vector3.up * Time.deltaTime * Speed;

        // try to move horizontally
        var currentPrevPos = prevPos;
        if (CheckRaycast(horizontal)) {
            transform.position += horizontal;
            if (!IsOnMap()) {
                transform.position = prevPos;
            } else {
                currentPrevPos = transform.position;
            }
        }
        // try to move vertically
        if (CheckRaycast(vertical)) {
            transform.position += vertical;
            if (!IsOnMap()) {
                transform.position = currentPrevPos;
            }
        }
    }

    private bool CheckRaycast(Vector3 movement) {
        // you are not expected to understand this
        float ext = collider.bounds.extents.x;
        var dir = movement.normalized;
        var perp = Vector3.Cross(Vector3.forward, dir);
        Debug.Log(perp);

        var hit = Physics2D.Raycast(transform.position, dir, movement.magnitude + ext, grabbableLayer);
        if (hit.collider != null) {
            var itemCtl = hit.collider.GetComponent<ItemController>();
            if (itemCtl != null && itemCtl.data.solid && grabCtl.GrabbedObject != itemCtl.gameObject) {
                return false;
            }
        }
        hit = Physics2D.Raycast(transform.position + ext * perp, dir, movement.magnitude + ext, grabbableLayer);
        if (hit.collider != null) {
            var itemCtl = hit.collider.GetComponent<ItemController>();
            if (itemCtl != null && itemCtl.data.solid && grabCtl.GrabbedObject != itemCtl.gameObject) {
                return false;
            }
        }
        hit = Physics2D.Raycast(transform.position - ext * perp, dir, movement.magnitude + ext, grabbableLayer);
        if (hit.collider != null) {
            var itemCtl = hit.collider.GetComponent<ItemController>();
            if (itemCtl != null && itemCtl.data.solid && grabCtl.GrabbedObject != itemCtl.gameObject) {
                return false;
            }
        }
        return true;
    }

    private void LateUpdate() {
        // find the direction that lines up best with the change between previous and current position
        var directions = new[] { Vector3.right, Vector3.up, Vector3.left, Vector3.down };
        var delta = transform.position - prevPos;
        if (delta.magnitude > 0) {
            // just compare dot products with cardinal directions
            Direction = directions.OrderBy(v => Vector3.Dot(v, delta))
                                  .Last();
        }

    }
    private bool HackyIntersect(Bounds other) {
        // 2D colliders don't play nicely with z
        return other.Contains(new Vector3(collider.bounds.min.x, collider.bounds.min.y, other.min.z))
            && other.Contains(new Vector3(collider.bounds.max.x, collider.bounds.max.y, other.max.z));
    }
    private bool IsOnMap() {
        return HackyIntersect(tileMapCollider.bounds);
    }
}