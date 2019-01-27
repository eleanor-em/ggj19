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

    [SerializeField]
    private Sprite guyFront;
    [SerializeField]
    private Sprite guyBack;
    [SerializeField]
    private Sprite guySide;

    private Vector3 prevPos;
    private SpriteRenderer renderer;
    private Collider2D tileMapCollider;
    new private Collider2D collider;
    private GrabController grabCtl;

    private void Start() {
        renderer = GetComponent<SpriteRenderer>();
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

        if(Direction.y > 0)
        {
            renderer.sprite = guyBack;
        }else if(Direction.y < 0)
        {
            renderer.sprite = guyFront;
        }
        else
        {
            renderer.sprite = guySide;
            if (Direction.x > 0)
            {
                renderer.flipX = true;
            }
            else if (Direction.x < 0)
            {
                renderer.flipX = false;
            }
        }

    }

    private bool CheckRaycast(Vector3 movement) {
        // you are not expected to understand this
        float ext = collider.bounds.extents.x;
        var dir = movement.normalized;
        var perp = Vector3.Cross(Vector3.forward, dir);

        var hits = Physics2D.RaycastAll(transform.position, dir, movement.magnitude + ext, grabbableLayer);
        foreach (var hit in hits) {
            if (hit.collider != null) {
                var itemCtl = hit.collider.GetComponent<ItemController>();
                if (itemCtl != null && itemCtl.data.solid && grabCtl.GrabbedObject != itemCtl.gameObject) {
                    return false;
                }
            }
        }
        hits = Physics2D.RaycastAll(transform.position + ext * perp, dir, movement.magnitude + ext, grabbableLayer);
        foreach (var hit in hits) {
            if (hit.collider != null) {
                var itemCtl = hit.collider.GetComponent<ItemController>();
                if (itemCtl != null && itemCtl.data.solid && grabCtl.GrabbedObject != itemCtl.gameObject) {
                    return false;
                }
            }
        }
        hits = Physics2D.RaycastAll(transform.position - ext * perp, dir, movement.magnitude + ext, grabbableLayer);
        foreach (var hit in hits) {
            if (hit.collider != null) {
                var itemCtl = hit.collider.GetComponent<ItemController>();
                if (itemCtl != null && itemCtl.data.solid && grabCtl.GrabbedObject != itemCtl.gameObject) {
                    return false;
                }
            }
        }
        return true;
    }

    private void LateUpdate() {
        // find the direction that lines up best with the change between previous and current position
        var directions = new[] { Vector3.right, Vector3.up, Vector3.left, Vector3.down };
        var delta = Input.GetAxis("Horizontal") * Vector3.right + Input.GetAxis("Vertical") * Vector3.up;
        if (delta.magnitude > 0) {
            // just compare dot products with cardinal directions
            Direction = directions.OrderBy(v => Vector3.Dot(v, delta))
                                  .Last();
        }

    }
    private bool IsOnMap() {
        return tileMapCollider.bounds.Contains(new Vector3(collider.bounds.min.x, collider.bounds.min.y, tileMapCollider.bounds.min.z))
    && tileMapCollider.bounds.Contains(new Vector3(collider.bounds.max.x, collider.bounds.max.y, tileMapCollider.bounds.max.z));
        // bounds.contain has issues with 2D for some reason, always expecting a vector 3. Hopefully intersects is an ok replacement.
    }
}