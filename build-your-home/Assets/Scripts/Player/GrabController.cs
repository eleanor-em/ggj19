using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabController : MonoBehaviour {
    public LayerMask grabbableLayer;

    private MoveController moveController;
    private GrabbableController target = null;
    private bool holding = false;

    private void Start() {
        moveController = GetComponent<MoveController>();
    }

    void Update () {
        CheckForGrabbable();
        CheckForGrabAction();
        UpdateHeld();
	}

    private void CheckForGrabbable() {
        if (!holding) {
            // check the facing direction for a grabbable object
            var hit = Physics2D.Raycast(transform.position, moveController.Direction, 1, grabbableLayer.value);
            if (hit.collider != null) {
                // if we hit something, check if it's both a grabbable and a different target
                var hitController = hit.collider.GetComponent<GrabbableController>();
                if (hitController?.gameObject != target?.gameObject) {
                    target?.OnDeselect();
                    target = hitController;
                    target.OnSelect();
                }
            } else {
                if (target != null) {
                    target.OnDeselect();
                    target = null;
                }
            }
        }
    }

    private void CheckForGrabAction() {
        if (Input.GetButtonDown("Pickup") && target != null) {
            if (!holding) {
                holding = true;
                target.OnGrab();
            } else {
                holding = false;
                target.OnDrop();
            }
        }
    }

    private void UpdateHeld() {
        if (holding) {
            // TODO: make object snap to grid
            target.transform.position = transform.position + moveController.Direction;
        }
    }
}
