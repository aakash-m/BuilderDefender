using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance { get; private set; }

    public event EventHandler<OnActiveBuildingTypeChangedEventArgs> OnActiveBuildingTypeChanged; 

    public class OnActiveBuildingTypeChangedEventArgs : EventArgs
    {
        public BuildingTypeScriptableObject activeBuildingType;
    }

    [SerializeField] private Building hqBuilding;


    BuildingTypeScriptableObject activeBuildingType;
    BuildingTypeListScriptableObject buildingTypeList;
    private Camera mainCamera;

    private void Awake()
    {
        Instance = this;
        buildingTypeList = Resources.Load<BuildingTypeListScriptableObject>(typeof(BuildingTypeListScriptableObject).Name);
    }

    void Start()
    {
        mainCamera = Camera.main;

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (activeBuildingType != null)
            {
                if (CanSpawnBuilding(activeBuildingType, HelperClass.GetMouseWorldPosition(), out string errorMessage))
                {
                    if (ResourceManager.Instance.CanAfford(activeBuildingType.constructionResourceCostArray))
                    {
                        ResourceManager.Instance.SpendResources(activeBuildingType.constructionResourceCostArray);
                        Instantiate(activeBuildingType.prefab, HelperClass.GetMouseWorldPosition(), Quaternion.identity);
                    }
                    else
                    {
                        TooltipUI.Instance.Show("Cannot afford " + activeBuildingType.GetConstructionResourceCostString(), 
                            new TooltipUI.TooltipTimer { timer = 2f });
                    }
                }
                else
                {
                    TooltipUI.Instance.Show(errorMessage, new TooltipUI.TooltipTimer { timer = 2f });
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            Vector3 enemyPosition = HelperClass.GetMouseWorldPosition() + HelperClass.GetRandomDir() * 5f;
            Enemy.Create(enemyPosition);
        }

    }

    public void SetActiveBuildingType(BuildingTypeScriptableObject buildingType)
    {
        activeBuildingType = buildingType;
        OnActiveBuildingTypeChanged?.Invoke(this, new OnActiveBuildingTypeChangedEventArgs { activeBuildingType = activeBuildingType });
    }

    public BuildingTypeScriptableObject GetActiveBuildingType()
    {
        return activeBuildingType;
    }

    private bool CanSpawnBuilding(BuildingTypeScriptableObject buildingType, Vector3 position, out string errorMessage)
    {
        BoxCollider2D boxCollider2D = buildingType.prefab.GetComponent<BoxCollider2D>();

        Collider2D[] collider2DArray = Physics2D.OverlapBoxAll(position + (Vector3)boxCollider2D.offset, boxCollider2D.size, 0);

        bool isAreaClear = collider2DArray.Length == 0;
        if (!isAreaClear)
        {
            errorMessage = "Area is not clear!";
            return false;
        }

        collider2DArray = Physics2D.OverlapCircleAll(position, buildingType.minConstructionRadius);

        foreach (Collider2D collider2D in collider2DArray)
        {
            //colliders inside the construction radius
            BuildingTypeHolder buildingTypeHolder = collider2D.GetComponent<BuildingTypeHolder>();
            if (buildingTypeHolder != null)
            {
                //Has a buildingTypeHolder
                if (buildingTypeHolder.buildingType == buildingType)
                {
                    //Theres already a building of this type within the construction radius
                    errorMessage = "Too close to other building of same type";
                    return false;
                }
            }

        }

        float maxConstructionRadius = 25;
        collider2DArray = Physics2D.OverlapCircleAll(position, maxConstructionRadius);

        foreach (Collider2D collider2D in collider2DArray)
        {
            //colliders inside the construction radius
            BuildingTypeHolder buildingTypeHolder = collider2D.GetComponent<BuildingTypeHolder>();
            if (buildingTypeHolder != null)
            {
                //Its a building
                errorMessage = "";
                return true;
                
            }
             
        }

        errorMessage = "Too far from any other building";
        return false;
    }

    public Building GetHQBuilding()
    {
        return hqBuilding;
    }

}
