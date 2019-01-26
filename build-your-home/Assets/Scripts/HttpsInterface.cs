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
    public bool solid;
}
[System.Serializable]
public class Instance {
    public Item item;
    public string sender;
}

public class HttpsInterface: MonoBehaviour {
    private static string auth;
    private static string Auth {
        get {
            if (auth == null) {
                string path = "Assets/Resources/auth.json";
                StreamReader reader = new StreamReader(path);
                auth = reader.ReadToEnd();
            }
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
            try {
                callback(JsonUtility.FromJson<Instance>(res));
            } catch (ArgumentException e) {
                Debug.Log(e);
                callback(null);
            }
        } else {
            Debug.Log(req.error);
        }
    }

    public static IEnumerator PutAnInstance(string name) {
        // build the json string. this sucks I know sorry
        var json = Auth.Remove(Auth.Length - 2);
        json += $",\n\t\"item\": \"{name}\",\n\t\"sender\": \"{PlayerPrefs.GetString("username")}\"\n}}";
        Debug.Log(json);

        UnityWebRequest req = UnityWebRequest.Put("localhost:8080/server/put", json);
        req.method = UnityWebRequest.kHttpVerbPOST;
        req.SetRequestHeader("Content-Type", "application/json");
        req.SetRequestHeader("Accept", "text/plain");
        yield return req.SendWebRequest();

        if (req.isNetworkError || req.responseCode != 200) {
            Debug.Log(req.error);
        } else {
            Debug.Log(req.downloadHandler.text);
        }
    }
}
