using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatyController : MonoBehaviour {
    public bool shouldFloat = false;
    BoxCollider2D myCollider;
    BoxCollider2D river;
    SpriteRenderer playerSprite;
    // Use this for initialization
    void Start () {
        oscillateRange = (endRange - startRange) / 2;
        oscillateOffset = oscillateRange + startRange;
        myCollider = transform.GetComponent<BoxCollider2D>();
        river = GameObject.FindGameObjectWithTag("river").GetComponent<BoxCollider2D>();
        playerSprite = transform.GetComponent<SpriteRenderer>();
        Interact();
	}


    public void Interact()
    {
        shouldFloat = IsInRiver();
    }

    bool IsInRiver()
    {
        return (river.bounds.Contains(new Vector3(myCollider.bounds.min.x, myCollider.bounds.min.y))
            && river.bounds.Contains(new Vector3(myCollider.bounds.max.x, myCollider.bounds.max.y)));
    }
	// Update is called once per frame
	void Update () {
        if (shouldFloat)
        {
            transform.position -= new Vector3(0,1,0) * Time.deltaTime;

            float offset = oscillateOffset + Mathf.Sin(Time.time * 2.5f) * oscillateRange;
            transform.localScale = new Vector3(offset, offset, 0);

            if (myCollider.bounds.max.y < river.transform.GetComponent<RiverController>().endPos.y)
            {
                Destroy(gameObject);//Should send not destroy
            }
        }
	}

    private float endRange = 0.8f;
    private float startRange = 1.0f;
    private float oscillateRange;
    private float oscillateOffset;
}
