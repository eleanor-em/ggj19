using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemData {
    // name of the item
    public string name;
    // description of the item
    public string description;
    // the character that owns this item
    public string owner;
    // the user that sent the item
    public string sender;
    public bool solid;
    public float x;
    public float y;
    public float z;
}
public class ItemController : MonoBehaviour {
    public ItemData data;
    private SpriteRenderer spriteRenderer;

    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void LateUpdate() {
        spriteRenderer.sortingOrder = -(int)(transform.position.y * 10);

        data.x = transform.position.x;
        data.y = transform.position.y;
        data.z = transform.position.z;
    }

    public void LoadSprite() {
        GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(data.name);
    }
}
