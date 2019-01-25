using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    public float normalSpeed;
    public float dashSpeed;
    public GameObject tileMap;
    public Vector3 Direction
    {
        get;
        private set;
    }

    private Vector3 prevPos;
    private Collider2D tileMapCollider;
    new private Collider2D collider;

    private float Speed
    {
        get
        {
            if (Input.GetButton("Dash"))
            {
                return dashSpeed;
            }
            else
            {
                return normalSpeed;
            }
        }
    }

    private void Start()
    {
        tileMapCollider = tileMap.GetComponent<Collider2D>();
        collider = GetComponent<Collider2D>();
    }

    void Update()
    {
        prevPos = transform.position;
        var horizontal = Input.GetAxis("Horizontal") * Vector3.right * Time.deltaTime * Speed;
        var vertical = Input.GetAxis("Vertical") * Vector3.up * Time.deltaTime * Speed;

        // try to move horizontally
        var currentPrevPos = prevPos;
        transform.position += horizontal;
        if (!IsOnMap())
        {
            transform.position = prevPos;
        }
        else
        {
            currentPrevPos = transform.position;
        }
        // try to move vertically
        transform.position += vertical;
        if (!IsOnMap())
        {
            transform.position = currentPrevPos;
        }
    }

    private void LateUpdate()
    {
        // find the direction that lines up best with the change between previous and current position
        var directions = new[] { Vector3.right, Vector3.up, Vector3.left, Vector3.down };
        var delta = transform.position - prevPos;
        if (delta.magnitude > 0)
        {
            // just compare dot products with cardinal directions
            Direction = directions.OrderBy(v => Vector3.Dot(v, delta))
                                  .Last();
        }

    }

    private bool IsOnMap()
    {
        return (tileMapCollider.bounds.Contains(new Vector3(collider.bounds.min.x, collider.bounds.min.y, tileMapCollider.bounds.min.z))
    && tileMapCollider.bounds.Contains(new Vector3(collider.bounds.max.x, collider.bounds.max.y, tileMapCollider.bounds.max.z)));
        // bounds.contain has issues with 2D for some reason, always expecting a vector 3. Hopefully intersects is an ok replacement.
         
        //return !tileMapCollider.bounds.Intersects(collider.bounds);
    }
}