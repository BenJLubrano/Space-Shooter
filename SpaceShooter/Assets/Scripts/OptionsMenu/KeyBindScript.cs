using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeyBindScript : MonoBehaviour
{
    private Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();
    public TextMeshProUGUI up, left, down, right, shoot;
    private GameObject currentKey;

    // Start is called before the first frame update
    void Start()
    {
        keys.Add("Up", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Up", "W")));
        keys.Add("Down", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Down", "S")));
        keys.Add("Left", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Left", "A")));
        keys.Add("Right", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Right", "D")));
        keys.Add("Shoot", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Shoot", "Space")));

        up.text = keys["Up"].ToString();
        down.text = keys["Down"].ToString();
        left.text = keys["Left"].ToString();
        right.text = keys["Right"].ToString();
        shoot.text = keys["Shoot"].ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keys["Up"]))
        {
            // Do a move action
            Debug.Log("Up");
        }
        if (Input.GetKeyDown(keys["Down"]))
        {
            // Do a move action
            Debug.Log("Down");
        }
        if (Input.GetKeyDown(keys["Left"]))
        {
            // Do a move action
            Debug.Log("Left");
        }
        if (Input.GetKeyDown(keys["Right"]))
        {
            // Do a move action
            Debug.Log("Right");
        }
        if (Input.GetKeyDown(keys["Shoot"]))
        {
            // Do a move action
            Debug.Log("Shoot");
        }
    }

    void OnGUI()
    {
        if (currentKey != null)
        {
            Event e = Event.current;
            if (e.isKey)
            {
                keys[currentKey.name] = e.keyCode;
                currentKey.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = e.keyCode.ToString();
                currentKey = null;
            }
        }
    }

    public void ChangeKey(GameObject clicked)
    {
        currentKey = clicked;
    }

    public void SaveKeys()
    {
        foreach (var key in keys)
        {
            PlayerPrefs.SetString(key.Key, key.Value.ToString());
            
        }

        PlayerPrefs.Save();
    }
}
