using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    private HealthSystem healthSystem;
    private BuildingTypeScriptableObject buildingType;

    private void Start()
    {
        healthSystem = GetComponent<HealthSystem>();
        buildingType = GetComponent<BuildingTypeHolder>().buildingType;

        healthSystem.SetHealthAmountMax(buildingType.healthAmountMax, true);
        healthSystem.OnDied += HealthSystem_OnDied;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            healthSystem.Damage(10);
        }
    }

    private void HealthSystem_OnDied(object sender, EventArgs e)
    {
        Destroy(gameObject);
    }
}
