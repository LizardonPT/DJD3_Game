using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class LoadAvaliable : MonoBehaviour
{
    void Start()
    {
        string path = Application.persistentDataPath + "/player.nothingimportant";
        if(File.Exists(path))
        {
            gameObject.GetComponent<Button>().interactable = true;
        }
    }
}
