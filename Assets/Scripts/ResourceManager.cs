using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }
    private Dictionary<ResourceTypeScriptableObject, int> resourceAmountDictionary;

    public event EventHandler OnResourceAmountChanged;


    private void Awake()
    {
        Instance = this;
        resourceAmountDictionary = new Dictionary<ResourceTypeScriptableObject, int>();

        ResourceTypeListScriptableObject resourceTypeList = Resources.Load<ResourceTypeListScriptableObject>(typeof(ResourceTypeListScriptableObject).Name);

        foreach(ResourceTypeScriptableObject resourceType in resourceTypeList.list)
        {
            resourceAmountDictionary[resourceType] = 0;
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            ResourceTypeListScriptableObject resourceTypeList = Resources.Load<ResourceTypeListScriptableObject>(typeof(ResourceTypeListScriptableObject).Name);
            AddResource(resourceTypeList.list[2], 2);
        }
    }

    private void Test()
    {
        foreach(ResourceTypeScriptableObject resourceType in resourceAmountDictionary.Keys)
        {
            Debug.Log(resourceType.name + ": " + resourceAmountDictionary[resourceType]);
        }
    }

    public void AddResource(ResourceTypeScriptableObject resourceType, int amount)
    {
        resourceAmountDictionary[resourceType] += amount;
        OnResourceAmountChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetResourceAmount(ResourceTypeScriptableObject resourceType)
    {
        return resourceAmountDictionary[resourceType];
    }





}
