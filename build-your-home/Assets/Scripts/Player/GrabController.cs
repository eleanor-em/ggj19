using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GrabController : MonoBehaviour {
    public LayerMask grabbableLayer;
    [SerializeField]
    private GameObject dialogueBox;
    [SerializeField]
    private GameObject heart;

    public AudioClip grabSound;
    public AudioClip dropSound;
    public AudioClip grabWaterSound;
    public AudioClip dropWaterSound;
    private AudioSource source;
    public GameObject GrabbedObject {
        get { if (holding) { return target.gameObject; } else { return null; }}
    }

    private MoveController moveController;
    private GrabbableController target = null;
    private bool holding = false;
    private bool legalTarget = true;

    private void Start() {
        moveController = GetComponent<MoveController>();
        source = GetComponent<AudioSource>();
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
                Debug.Log(hit.collider.name);
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
        if (Input.GetButtonDown("Interact")) {
            GameObject dialogue = Instantiate(heart);
        }
    }
    private void CheckForGrabAction() {
        if (Input.GetButtonDown("Pickup") && target != null) {
            if (!holding) {
                ItemController item = target.GetComponent<ItemController>();
                GameObject dialogue = Instantiate(dialogueBox);
                dialogue.GetComponentInChildren<DialogueController>().SetItem(item);
                holding = true;

                source.clip = target.GetComponent<FloatyController>().IsInRiver() ? grabWaterSound : grabSound;
                source.volume = 1;
                source.Play();
                
                target.OnGrab();
            } else if (legalTarget) {
                holding = false;

                source.clip = target.GetComponent<FloatyController>().IsInRiver() ? dropWaterSound : dropSound;
                source.volume = 1;
                source.Play();

                target.OnDrop();
            }
        }
    }

    private void UpdateHeld() {
        if (holding) {
            Vector3 newPosition = SnapToPoint5(transform.position + moveController.Direction);

            target.transform.position = newPosition;

            // check location
            var filter = new ContactFilter2D();
            filter.layerMask = grabbableLayer;
            var results = new Collider2D[5];
            var count = Physics2D.OverlapBox(new Vector2(newPosition.x, newPosition.y), Vector2.one * 0.1f, 0, filter, results);
            count = results.Count(coll => {
                if (coll != null) {
                    var itemCtl = coll.GetComponent<ItemController>();
                    return itemCtl?.data.solid == true;
                }
                return false;
            });
            if (count > 1) {
                legalTarget = false;
                target.OnIllegalPlacement();
            } else {
                legalTarget = true;
                target.OnLegalPlacement();
            }
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
