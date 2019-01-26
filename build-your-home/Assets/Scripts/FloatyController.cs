using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatyController : MonoBehaviour {
    public bool shouldFloat = false;
    BoxCollider2D myCollider;
    BoxCollider2D river;

    private ItemController itemCtl;

    // Use this for initialization
    void Start () {
        myCollider = transform.GetComponent<BoxCollider2D>();
        river = GameObject.FindGameObjectWithTag("river").GetComponent<BoxCollider2D>();
        Interact();

        itemCtl = GetComponent<ItemController>();
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

            if (myCollider.bounds.max.y < river.transform.GetComponent<RiverController>().endPos.y)
            {
                Destroy(gameObject);
                HttpsInterface.PutAnInstance(itemCtl.data.name);
            }
        }
	}
}
