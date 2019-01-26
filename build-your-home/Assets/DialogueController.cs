using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour {

    private GameObject player;
    private RectTransform rt;
    private RectTransform canvasRT;
    private Vector3 guyScreenPos;
    private Camera camera;
    ItemController itemDetails;
    private float decayTime = 5f;

    // Use this for initialization
    void Start () {
        player = FindObjectOfType<MoveController>().transform.gameObject;
        transform.SetParent(FindObjectOfType<Canvas>().transform, true);
        camera = FindObjectOfType<Camera>();
        rt = GetComponent<RectTransform>();
        canvasRT = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
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
}
