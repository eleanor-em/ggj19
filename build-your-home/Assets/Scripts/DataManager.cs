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
    }

    private readonly static string[] firstWord = { "Fox", "Warm", "Cold", "Happy", "Sunny", "Birds", "Windy", "Sweet", "Tropical", "Cozy", "Serene", "Hedgehog", "Bear", "Rabbit", "Possum", "Koala", "Kangaroo", "Cats", "Puppy", "Frogs", "Platypus", "Hoppy", "Snuggly", "Jackalopes", "Deer", "Kangaroo", "Green", "Trees", "Daisy", "Cuckoo", "Gumtrees", "Ducks" };
    private readonly static string[] secondWord = { "Hut", "Pines", "Peaks", "Falls", "Rivers", "Creek", "Home", "Bungalow", "Lodge", "Cottage", "Homestead", "Mill", "End", "Bridgelands", "Woods", "Corner", "Cabin", "Villa", "Hill", "Copse", "Ville", "View", "Cedars", "Barn", "Rise", "Halls", "Byways", "Court", "Hedgerows", "Glade", "Stables", "Cove" };
    public static void ChooseUsername() {
        var first = firstWord[Random.Range(0, firstWord.Length)];
        var second = secondWord[Random.Range(0, secondWord.Length)];
        PlayerPrefs.SetString("username", $"{first} {second}");
    }

    public static void Save() {
        using (FileStream file = File.Create(SavePath)) {
            BinaryFormatter bf = new BinaryFormatter();
            var saveList = FindObjectsOfType<ItemController>().Select(comp => comp.data).ToList();
            bf.Serialize(file, saveList);
            Debug.Log("saved!");
        }
    }

    public static void Load(GameObject itemPrefab) {
        if (SaveExists()) {
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
                Debug.Log("loaded!");
            }
        }
    }

    public static void DeleteSave() {
        File.Delete(SavePath);
    }

    public static bool SaveExists() {
        return File.Exists(SavePath);
    }
}
