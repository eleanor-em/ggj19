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
    [SerializeField]
    private Text description;
    [SerializeField]
    private Text name;
    [SerializeField]
    private Text owner;


    // Use this for initialization
    void Start () {
        player = FindObjectOfType<MoveController>().transform.gameObject;
        transform.SetParent(FindObjectOfType<Canvas>().transform, true);
        camera = FindObjectOfType<Camera>();
        UpdateText();
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
        Debug.Log(itemDetails.name);
        description.text = itemDetails.description;
        name.text = "Shared by: " + itemDetails.sender;
        owner.text = "Owned by: " + itemDetails.owner;

    }
}
