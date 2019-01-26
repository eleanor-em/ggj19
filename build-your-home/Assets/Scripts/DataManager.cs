using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class DataManager : MonoBehaviour {
    public GameObject itemPrefab;

    public static string Username {
        get { return PlayerPrefs.GetString("username", null); }
    }

    private static string SavePath {
        get { return Application.persistentDataPath + "/data"; }
    }

    private void Awake() {
        DontDestroyOnLoad(gameObject);
        Debug.Log(SavePath);
    }

    private readonly static string[] firstWord = { "apple", "banana", "celery", "durian", "eggplant" };
    private readonly static string[] secondWord = { "abode", "boudoir", "castle", "dwelling" };
    public static void ChooseUsername() {
        var first = firstWord[Random.Range(0, firstWord.Length)];
        var second = secondWord[Random.Range(0, secondWord.Length)];
        PlayerPrefs.SetString("username", $"{first} {second}");
    }

    public void Save() {
        using (FileStream file = File.Create(SavePath)) {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(file, FindObjectsOfType<ItemController>().Select(comp => comp.data).ToList());
        }
    }

    public void Load() {
        if (File.Exists(SavePath)) {
            using (FileStream file = File.Open(SavePath, FileMode.Open)) {
                BinaryFormatter bf = new BinaryFormatter();
                List<ItemData> items = (List<ItemData>)bf.Deserialize(file);
                foreach (var item in items) {
                    var newItem = Instantiate(itemPrefab);
                    var itemController = newItem.GetComponent<ItemController>();
                    itemController.data = item;
                    itemController.LoadSprite();
                    newItem.transform.position = new Vector3(item.x, item.y, item.z);
                }
            }
        }
    }
}
