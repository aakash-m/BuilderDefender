using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    ResourceGeneratorData resourceGeneratorData;
    float timer;
    float timerMax;

    private void Awake()
    {
        resourceGeneratorData = GetComponent<BuildingTypeHolder>().buildingType.resourceGeneratorData;
        timerMax = resourceGeneratorData.timerMax;
    }

    public ResourceGeneratorData GetResourceGeneratorData()
    {
        return resourceGeneratorData;
    }

    public float GetTimerNormalized()
    {
        return timer / timerMax;
    }

    public float GetAmountGeneratedPerSecond()
    {
        return 1 / timerMax;
    }

    public static int GetNearbyResourceAmount(ResourceGeneratorData resourceGeneratorData, Vector3 position)
    {
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(position, resourceGeneratorData.resourceDetectionRadius);

        int nearByResourceAmount = 0;
        foreach (Collider2D collider2D in collider2DArray)
        {
            ResourceNode resourceNode = collider2D.GetComponent<ResourceNode>();
            if (resourceNode != null)
            {
                //Its a resource node;
                if (resourceNode.resourceType == resourceGeneratorData.resourceType)
                {
                    //Same type
                    nearByResourceAmount++;
                }

            }
        }

        nearByResourceAmount = Mathf.Clamp(nearByResourceAmount, 0, resourceGeneratorData.maxResourceAmount);
        return nearByResourceAmount;
    }

    private void Start()
    {

        int nearByResourceAmount = GetNearbyResourceAmount(resourceGeneratorData, transform.position);

        if (nearByResourceAmount == 0)
        {
            //No resource node nearby
            //Disable resource generator
            enabled = false;
        }
        else
        {
            timerMax = (resourceGeneratorData.timerMax / 2f) +
                resourceGeneratorData.timerMax *
                (1 - (float)nearByResourceAmount / resourceGeneratorData.maxResourceAmount);
        }
        Debug.Log("nearbyResourceAmount :" + nearByResourceAmount + "; timerMax" + timerMax); 

    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = timerMax; 
            ResourceManager.Instance.AddResource(resourceGeneratorData.resourceType, 1);
        }
    }

}
