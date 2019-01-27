using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class KeepAlive : MonoBehaviour {
    private SpriteRenderer spriteRenderer;

	void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = new Color(0, 0, 0, 0);
        StartCoroutine(CheckServer());
    }
	
    private IEnumerator CheckServer() {
        UnityWebRequest req = UnityWebRequest.Get("noneuclideangirl.net:13337/server/");
        yield return req.SendWebRequest();
        if (req.isNetworkError || req.downloadHandler.text != "Server online") {
            spriteRenderer.color = new Color(0, 0, 0, 0);
        } else {
            spriteRenderer.color = Color.white;
        }
        yield return new WaitForSeconds(1);
        StartCoroutine(CheckServer());
    }
}
