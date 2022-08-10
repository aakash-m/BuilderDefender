using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGhost : MonoBehaviour
{

    [SerializeField] GameObject spriteGameobject;
    [SerializeField] ResourceNearbyOverlay resourceNearbyOverlay;

    private void Awake()
    {
        
        Hide();
    }

    private void Start()
    {
        BuildingManager.Instance.OnActiveBuildingTypeChanged += BuildingManager_OnActiveBuildingTypeChanged;
    }

    private void BuildingManager_OnActiveBuildingTypeChanged(object sender, BuildingManager.OnActiveBuildingTypeChangedEventArgs e)
    {
        if (e.activeBuildingType == null)
        {
            Hide();
            resourceNearbyOverlay.Hide();
        }
        else
        {
            Show(e.activeBuildingType.sprite);
            if (e.activeBuildingType.hasResourceGeneratorData)
            {
                resourceNearbyOverlay.Show(e.activeBuildingType.resourceGeneratorData);
            }
            else
            {
                resourceNearbyOverlay.Hide();
            }
        }
    }

    private void Update()
    {
        transform.position = HelperClass.GetMouseWorldPosition();
    }

    private void Show(Sprite ghostSprite)
    {
        spriteGameobject.SetActive(true);
        spriteGameobject.GetComponent<SpriteRenderer>().sprite = ghostSprite;
    }

    private void Hide()
    {
        spriteGameobject.SetActive(false);
    }

}
