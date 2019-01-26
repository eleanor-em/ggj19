using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Startup : MonoBehaviour {
    public GameObject itemPrefab;


    private void Awake() {
        if (DataManager.Username == null) {
            DataManager.ChooseUsername();
        }
    }

    void Start () {
        StartCoroutine(HttpsInterface.GetAnInstance(instance => {
            if (instance != null) {
                var newItem = Instantiate(itemPrefab);

                var itemControl = newItem.GetComponent<ItemController>();
                itemControl.data.name = instance.item.name;
                itemControl.data.description = instance.item.description;
                itemControl.data.owner = instance.item.owner;
                itemControl.data.sender = instance.sender;
                itemControl.data.solid = instance.item.solid;
                itemControl.LoadSprite();

                newItem.transform.position += new Vector3(0.5f, 0.5f, 0);
            }

            StartCoroutine(HttpsInterface.PutAnInstance("Kitchen Sink"));
        }));
	}
}
