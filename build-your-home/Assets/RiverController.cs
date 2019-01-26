using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverController : MonoBehaviour {

    private BoxCollider2D riverCollider;
    private Vector3 spawnPos;

    // Use this for initialization
    void Start () {
        riverCollider = transform.GetComponent<BoxCollider2D>();
        spawnPos = riverCollider.bounds.center + new Vector3(0, riverCollider.bounds.extents.y, 0.5f);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SpawnItem()
    {
        

    }
}
