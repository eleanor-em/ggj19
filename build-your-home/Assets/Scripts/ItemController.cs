﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour {
    // name of the item
    new public string name;
    // description of the item
    public string description;
    // the character that owns this item
    public string owner;
    // the user that sent the item
    public string sender;
    // internal; a database primary key
    public string id;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
