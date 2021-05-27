using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveLoad : MonoBehaviour {

    public Store CurrentStore;

    public List<Item> SavedList = new List<Item>();

    public string DataNeme;

    private void OnEnable()
    {
        Invoke("Load", 0.05f);
    }

    public void Save()
    {
        SavedList.Clear();

        for (int i = 0; i < CurrentStore.CurrentItemList.Count; i++)
        {
            SavedList.Add(CurrentStore.CurrentItemList[i]);
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + "/" + DataNeme + ".data", FileMode.Create);

        bf.Serialize(stream, SavedList);
        stream.Close();

        CurrentStore.UpdateItemSprite();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/" + DataNeme + ".data"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + "/" + DataNeme + ".data", FileMode.Open);

            SavedList = (List<Item>)bf.Deserialize(stream);
            stream.Close();

            for (int i = 0; i < SavedList.Count; i++)
            {
                CurrentStore.CurrentItemList[i] = SavedList[i];
            }

            if (PlayerPrefs.GetInt("FirstStart") == 0)
            {
                CurrentStore.CurrentItemList[0].IsBough = true;
                CurrentStore.CurrentItemList[0].IsSelected = true;
            }
        }
        CurrentStore.UpdateItemSprite();
    }

    public void ClearData()
    {
        for (int i = 0; i < SavedList.Count; i++)
        {
            CurrentStore.CurrentItemList[i].IsBough = false;
            CurrentStore.CurrentItemList[i].IsSelected = false;
        }
        PlayerPrefs.DeleteAll();
        Save();
    }
}
