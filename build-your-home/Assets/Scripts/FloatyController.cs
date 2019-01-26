using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatyController : MonoBehaviour {
    public float speed = 0.6f;
    public bool shouldFloat = false;
    BoxCollider2D myCollider;
    BoxCollider2D river;
    private ItemController itemCtl;
    SpriteRenderer playerSprite;

    private bool saved = false;
    private GrabbableController grabbableCtl;
    // Use this for initialization
    void Start() {
        oscillateRange = (endRange - startRange) / 2;
        oscillateOffset = oscillateRange + startRange;
        myCollider = transform.GetComponent<BoxCollider2D>();
        river = GameObject.FindGameObjectWithTag("river").GetComponent<BoxCollider2D>();
        playerSprite = transform.GetComponent<SpriteRenderer>();
        Interact();
        itemCtl = GetComponent<ItemController>();
        grabbableCtl = GetComponent<GrabbableController>();
    }


    public void Interact() {
        shouldFloat = IsInRiver();
    }

    bool IsInRiver() {
        return (river.bounds.Contains(new Vector3(myCollider.bounds.min.x, myCollider.bounds.min.y, river.bounds.min.z))
            && river.bounds.Contains(new Vector3(myCollider.bounds.max.x, myCollider.bounds.max.y, river.bounds.max.z)));
    }
    // Update is called once per frame
    void Update() {
        if (shouldFloat) {
            transform.position += Vector3.down * speed * Time.deltaTime;

            float offset = 1f;
            if (!grabbableCtl.Grabbed) {
                offset = oscillateOffset + Mathf.Sin(Time.time * 2.5f) * oscillateRange;
            }
            transform.localScale = new Vector3(offset, offset, 1);
            if (myCollider.bounds.max.y < river.transform.GetComponent<RiverController>().endPos.y) {
                if (!saved) {
                    StartCoroutine(HttpsInterface.PutAnInstance(itemCtl.data.name, () => {
                        Destroy(gameObject);
                        gameObject.SetActive(false);
                        DataManager.Save();
                    }));
                    saved = true;
                }
            }
        } else {
            transform.localScale = Vector3.one;
        }
    }

    private float endRange = 0.8f;
    private float startRange = 1.0f;
    private float oscillateRange;
    private float oscillateOffset;
}
