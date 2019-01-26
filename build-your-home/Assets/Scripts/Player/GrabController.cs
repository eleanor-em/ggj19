using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrabController : MonoBehaviour {
    public LayerMask grabbableLayer;
    [SerializeField]
    private GameObject dialogueBox;

    private MoveController moveController;
    private GrabbableController target = null;
    private bool holding = false;

    private void Start() {
        moveController = GetComponent<MoveController>();
    }

    void Update() {
        CheckForInteract();
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

    private void CheckForInteract() {
        if (Input.GetButtonDown("Interact") && target != null) {
            Debug.Log("Clicked Interact");
            if (!holding) {
                ItemController item = target.transform.GetComponent<ItemController>();
                GameObject dialogue = Instantiate(dialogueBox);
                dialogue.GetComponentInChildren<Text>().text = item.description;
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
            Vector3 newPosition = SnapToPoint5(transform.position + moveController.Direction);

            target.transform.position = newPosition;
        }
    }

    Vector3 SnapToPoint5(Vector3 input) {
        return new Vector3((float)(Mathf.Floor(input.x) + 0.5), (float)(Mathf.Floor(input.y) + 0.5), input.z);
    }

    Vector3 SnapPosition(Vector3 input, float factor = 1f) {
        if (factor <= 0f)
            throw new UnityException("factor argument must be above 0");

        float x = Mathf.Round(input.x / factor) * factor;
        float y = Mathf.Round(input.y / factor) * factor;
        float z = Mathf.Round(input.z / factor) * factor;

        return new Vector3(x, y, z);
    }
}
