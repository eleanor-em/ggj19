using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableController : MonoBehaviour {
    public Color selectedCol = Color.green;
    public float grabbedAlpha = 0.5f;

    private SpriteRenderer sprite;

    private void Start() {
        sprite = GetComponent<SpriteRenderer>();
    }

    public void OnSelect() {
        Debug.Log("I am selected, yay");
        sprite.color = selectedCol;
    }
    public void OnDeselect() {
        Debug.Log("I am deselected, unyay");
        sprite.color = Color.white;
    }

    public void OnGrab() {
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, grabbedAlpha);
    }
    public void OnDrop() {
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1);
    }
}
