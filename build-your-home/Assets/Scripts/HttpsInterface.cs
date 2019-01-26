using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class Item {
    public string name;
    public string description;
    public string owner;
    public string _id;
}
[System.Serializable]
public class Instance {
    public Item item;
    public string sender;
}

public class HttpsInterface {

    private static string auth;
    private static string Auth {
        get {
            if (auth == null) {
                string path = "Assets/Resources/auth.json";
                StreamReader reader = new StreamReader(path);
                auth = reader.ReadToEnd();
            }
            Debug.Log(auth);
            return auth;
        }
    }
    
    public static IEnumerator GetAnInstance(Action<Instance> callback) {
        UnityWebRequest req = UnityWebRequest.Put("localhost:8080/server/get", Auth);
        req.method = UnityWebRequest.kHttpVerbPOST;
        req.SetRequestHeader("Content-Type", "application/json");
        req.SetRequestHeader("Accept", "application/json");
        yield return req.SendWebRequest();

        if (!req.isNetworkError && req.responseCode == 200) {
            var res = req.downloadHandler.text;
            Debug.Log(res);
            callback(JsonUtility.FromJson<Instance>(res));
        } else {
            Debug.Log(req.error);
            Debug.Log(req.downloadHandler.text);
        }
    }
}
