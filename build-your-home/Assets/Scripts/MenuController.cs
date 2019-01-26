using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {
    public Text text;

    private int selected = 0;
    private bool changed = false;

    private void Start() {
        transform.GetChild(selected).GetComponent<SpriteRenderer>().color = Color.gray;
        if (DataManager.Username != null) {
            text.text = DataManager.Username;
        } else {
            ResetName();
        }
    }

    private void Update() {
        if (Input.GetAxisRaw("Vertical") != 0) {
            if (!changed) {
                changed = true;
                Toggle();
            }
        } else {
            changed = false;
        }

        if (Input.GetButtonDown("Pickup")) {
            Press();
        }
    }

    void Toggle() {
        // good luck
        transform.GetChild(selected).GetComponent<SpriteRenderer>().color = Color.white;
        selected += 1;
        selected %= 2;
        transform.GetChild(selected).GetComponent<SpriteRenderer>().color = Color.gray;
    }

    void Press() {
        if (selected == 0) {
            ResetName();
        } else {
            SceneManager.LoadScene("home");
        }
    }

    private void ResetName() {
        DataManager.ChooseUsername();
        text.text = DataManager.Username;
    }
}
