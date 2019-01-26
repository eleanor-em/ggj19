using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverController : MonoBehaviour {
    public GameObject itemPrefab;
    public float minWait = 40;
    public float maxWait = 60;

    private BoxCollider2D riverCollider;
    public Vector3 spawnPos;
    public Vector3 endPos;

    private bool first = true;
    private int needToSave = 0;

    // Use this for initialization
    void Start() {
        riverCollider = transform.GetComponent<BoxCollider2D>();
        spawnPos = riverCollider.bounds.center + new Vector3(0, riverCollider.bounds.extents.y - 0.6f, -0.5f);
        endPos = riverCollider.bounds.center - new Vector3(0, riverCollider.bounds.extents.y, -0.5f);

        if (DataManager.SaveExists()) {
            first = false;
        }
        StartCoroutine(SpawnItem());
    }

    // Update is called once per frame
    void LateUpdate() {
        if (needToSave == 1) {
            DataManager.Save();
        }
        if (needToSave > 0) {
            --needToSave;
        }
    }

    public IEnumerator SpawnItem() {
        if (!first) {
            yield return new WaitForSeconds(Random.Range(minWait, maxWait));
        } else {
            first = false;
        }
        StartCoroutine(HttpsInterface.GetAnInstance(instance => {
            Debug.Log("spawn");
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
            needToSave = 2;
            StartCoroutine(SpawnItem());
        }));
    }
}
