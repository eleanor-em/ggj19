using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableController : MonoBehaviour {
    public Color selectedCol = Color.green;
    public float grabbedAlpha = 0.5f;
    FloatyController floatyController;
    private SpriteRenderer sprite;

    private void Start() {
        sprite = GetComponent<SpriteRenderer>();
        floatyController = transform.GetComponent<FloatyController>();
    }

    public void OnSelect() {
        sprite.color = selectedCol;
    }
    public void OnDeselect() {
        sprite.color = Color.white;
    }

    public void OnGrab() {
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, grabbedAlpha);
    }
    public void OnDrop() {
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1);
        floatyController.Interact();
    }
}
