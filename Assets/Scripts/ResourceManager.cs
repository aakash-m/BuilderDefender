using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ResourceManager : MonoBehaviour
{

    public event EventHandler OnResourceAmountChanged;

    public static ResourceManager Instance { get; private set; }
    
    private Dictionary<ResourceTypeScriptableObject, int> resourceAmountDictionary;

    [SerializeField] List<ResourceAmount> startingResourceAmountList;

    private void Awake()
    {
        Instance = this;
        resourceAmountDictionary = new Dictionary<ResourceTypeScriptableObject, int>();

        ResourceTypeListScriptableObject resourceTypeList = Resources.Load<ResourceTypeListScriptableObject>(typeof(ResourceTypeListScriptableObject).Name);

        foreach(ResourceTypeScriptableObject resourceType in resourceTypeList.list)
        {
            resourceAmountDictionary[resourceType] = 0;
        }

        foreach (ResourceAmount resourceAmount in startingResourceAmountList)
        {
            AddResource(resourceAmount.resourceType, resourceAmount.amount);
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

    public bool CanAfford(ResourceAmount[] resourceAmountArray)
    {
        foreach (ResourceAmount resourceAmount in resourceAmountArray)
        {
            if (GetResourceAmount(resourceAmount.resourceType) >= resourceAmount.amount)
            {
                //Can afford
            }
            else
            {
                // Cannot afford
                return false;
            }
        }

        // Can afford all
        return true;

    }

    public void SpendResources(ResourceAmount[] resourceAmountArray)
    {
        foreach (ResourceAmount resourceAmount in resourceAmountArray)
        {
            resourceAmountDictionary[resourceAmount.resourceType] -= resourceAmount.amount;
        }
    }


}
