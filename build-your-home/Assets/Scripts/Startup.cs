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
}
