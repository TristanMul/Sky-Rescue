using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class Store : MonoBehaviour {

    public Transform SpawnTransform;
    public ItemHolder HolderPrefab;
    public List<Item> CurrentItemList = new List<Item>();
    private List<ItemHolder> HolderList = new List<ItemHolder>();

    public int ItemsCount;
    public string ResourcePath;

    private void Start()
    {
        CreateStore();
    }

    private void CreateStore()
    {
        for (int i = 0; i < ItemsCount; i++)
        {
            ItemHolder currentHolder = Instantiate(HolderPrefab, SpawnTransform, false);
            HolderList.Add(currentHolder);

            CurrentItemList.Add(new Item());

            // Add price for items
            if (i < 11)
            {
                CurrentItemList[i].ItemPrice = 100;
            }
            else if (i < 21)
            {
                CurrentItemList[i].ItemPrice = 150;
            }
            else if (i < 31)
            {
                CurrentItemList[i].ItemPrice = 200;
            }
            else if (i < 41)
            {
                CurrentItemList[i].ItemPrice = 250;
            }
            /*
            You cam add more price
            else if(i < P)
            {
                CurrentItemList[i].ItemPrice = N;
            }
            */
        }
    }

    public void UpdateItemSprite()
    {
        for (int i = 0; i < HolderList.Count; i++)
        {
            if (CurrentItemList[i].IsBough)
            {
                HolderList[i].ItemImage.sprite = Resources.Load<Sprite>(ResourcePath + "/" + i);
            }
            else
            {
                HolderList[i].ItemImage.sprite = Resources.Load<Sprite>(ResourcePath + "/Locked");
            }
        }
    }
}
