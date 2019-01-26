using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Startup : MonoBehaviour {
    public GameObject itemPrefab;

	void Start () {
        StartCoroutine(HttpsInterface.GetAnInstance(instance => {
            if (instance != null) {
                var newItem = Instantiate(itemPrefab);

                var itemControl = newItem.GetComponent<ItemController>();
                itemControl.name = instance.item.name;
                itemControl.description = instance.item.description;
                itemControl.owner = instance.item.owner;
                itemControl.sender = instance.sender;
                itemControl.solid = instance.item.solid;

                var spriteRenderer = newItem.GetComponent<SpriteRenderer>();
                spriteRenderer.sprite = Resources.Load<Sprite>(itemControl.name);

                newItem.transform.position += new Vector3(0.5f, 0.5f, 0);
            }

            StartCoroutine(HttpsInterface.PutAnInstance("Kitchen Sink"));
        }));
	}
}
