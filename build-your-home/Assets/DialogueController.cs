using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour {

    private GameObject player;
    private Vector3 guyScreenPos;
    private Camera camera;
    ItemController itemDetails;
    private float decayTime = 5f;

    // Use this for initialization
    void Start () {
        player = FindObjectOfType<MoveController>().transform.gameObject;
        transform.SetParent(FindObjectOfType<Canvas>().transform, true);
        camera = FindObjectOfType<Camera>();
    }
	
	// Update is called once per frame
	void Update () {
        guyScreenPos = camera.WorldToScreenPoint(player.transform.position);
        transform.position = guyScreenPos;
        decayTime -= Time.deltaTime;
        if(decayTime< 0)
        {
            Destroy(gameObject);
        }
    }

    public void SetItem(ItemController item)
    {
        itemDetails = item;
        UpdateText();
        
    }

    private void UpdateText()
    {
        GameObject.Find("Description").GetComponent<Text>().text = itemDetails.description;
        GameObject.Find("SenderText").GetComponent<Text>().text = "Shared by: " + itemDetails.sender;
        GameObject.Find("OwnerText").GetComponent<Text>().text = "Owned by: " + itemDetails.owner;

    }
}
