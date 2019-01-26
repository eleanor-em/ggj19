using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverController : MonoBehaviour {
    public GameObject itemPrefab;

    private BoxCollider2D riverCollider;
    public Vector3 spawnPos;
    public Vector3 endPos;

    // Use this for initialization
    void Start() {
        riverCollider = transform.GetComponent<BoxCollider2D>();
        spawnPos = riverCollider.bounds.center + new Vector3(0, riverCollider.bounds.extents.y, 0.5f);
        endPos = riverCollider.bounds.center - new Vector3(0, riverCollider.bounds.extents.y, 0.5f);
    }

    // Update is called once per frame
    void Update() {

    }

    public void SpawnItem() {
        HttpsInterface.GetAnInstance(instance => {
            var newItem = Instantiate(itemPrefab);

            var itemControl = newItem.GetComponent<ItemController>();
            itemControl.data.name = instance.item.name;
            itemControl.data.description = instance.item.description;
            itemControl.data.owner = instance.item.owner;
            itemControl.data.sender = instance.sender;
            itemControl.data.solid = instance.item.solid;
            itemControl.LoadSprite();

            if (itemControl.data.owner == "") {
                itemControl.data.owner = "Unknown";
            }

            newItem.transform.position = spawnPos;
        });
    }
}
