using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour {

    private GameObject player;
    private Vector3 guyScreenPos;
    new private Camera camera;
    ItemController itemDetails;
    private float decayTime = 5f;
    [SerializeField]
    private Text description;
    [SerializeField]
    new private Text name;
    [SerializeField]
    private Text owner;


    // Use this for initialization
    void Start() {
        player = FindObjectOfType<MoveController>().transform.gameObject;
        transform.SetParent(FindObjectOfType<Canvas>().transform, true);
        camera = FindObjectOfType<Camera>();
        UpdateText();
    }

    // Update is called once per frame
    void Update() {
        
        guyScreenPos = camera.WorldToScreenPoint(player.transform.position);
        transform.position = guyScreenPos - new Vector3(0, 0, -1); ;
        decayTime -= Time.deltaTime;
        if (decayTime < 0) {
            Destroy(gameObject);
        }
    }

    public void SetItem(ItemController item) {
        itemDetails = item;
        UpdateText();
    }

    private void UpdateText() {
        if (description != null)
        {
            description.text = itemDetails.data.description;
            name.text = "Shared by: " + itemDetails.data.sender;
            owner.text = "Owned by: " + itemDetails.data.owner;
        }
    }
}
