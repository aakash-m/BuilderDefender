using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGhost : MonoBehaviour
{

    [SerializeField] GameObject spriteGameobject;

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
        }
        else
        {
            Show(e.activeBuildingType.sprite);  
        }
    }

    private void Update()
    {
        transform.position = UtilsClass.GetMouseWorldPosition();
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
