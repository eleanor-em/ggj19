using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableController : MonoBehaviour {
    public Color selectedCol = Color.green;
    public float grabbedAlpha = 0.5f;
    FloatyController floatyController;
    private SpriteRenderer sprite;
    public bool Grabbed {
        get; private set;
    }

    private void Start() {
        sprite = GetComponent<SpriteRenderer>();
        floatyController = transform.GetComponent<FloatyController>();
    }

    public void OnIllegalPlacement() {
        sprite.color = new Color(1, 0, 0, grabbedAlpha);
    }
    public void OnLegalPlacement() {
        sprite.color = new Color(0, 1, 0, grabbedAlpha);
    }

    public void OnSelect() {
        sprite.color = selectedCol;
    }
    public void OnDeselect() {
        sprite.color = Color.white;
    }

    public void OnGrab() {
        sprite.sortingLayerName = "Selected";
        Grabbed = true;
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, grabbedAlpha);
    }
    public void OnDrop() {
        DataManager.Save();
        sprite.sortingLayerName = "Default";
        Grabbed = false;
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1);
        floatyController.Interact();
    }
}
