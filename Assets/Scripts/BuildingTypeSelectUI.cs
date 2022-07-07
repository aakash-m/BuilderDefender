using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingTypeSelectUI : MonoBehaviour
{
    [SerializeField] Transform buttonTemplate;
    [SerializeField] Sprite arrowSprite;
    [SerializeField] List<BuildingTypeScriptableObject> ignoreBuildingTypeList;

    BuildingTypeListScriptableObject buildingListType;

    private Dictionary<BuildingTypeScriptableObject, Transform> buttonTransformDictionary;
    private Transform arrowButton;
    
    private void Awake()
    {
        //Transform buttonTemplate = transform.Find("buttonTemplate");
        buttonTemplate.gameObject.SetActive(false);
        buildingListType = Resources.Load<BuildingTypeListScriptableObject>(typeof(BuildingTypeListScriptableObject).Name);

        buttonTransformDictionary = new Dictionary<BuildingTypeScriptableObject, Transform>();

        int index = 0;

        arrowButton = Instantiate(buttonTemplate, transform);
        arrowButton.gameObject.SetActive(true);

        float offset = 130f;
        arrowButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(offset * index, 0);

        arrowButton.Find("image").GetComponent<Image>().sprite = arrowSprite;
        arrowButton.Find("image").GetComponent<RectTransform>().sizeDelta = new Vector2(0, -25);

        arrowButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            BuildingManager.Instance.SetActiveBuildingType(null);
        });

        index++;



        foreach (BuildingTypeScriptableObject buildingType in buildingListType.list)
        {
            if (ignoreBuildingTypeList.Contains(buildingType)) continue;

            Transform buttonTransform = Instantiate(buttonTemplate, transform);
            buttonTransform.gameObject.SetActive(true);

            offset = 130f;
            buttonTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(offset * index, 0);

            buttonTransform.Find("image").GetComponent<Image>().sprite = buildingType.sprite;

            buttonTransform.GetComponent<Button>().onClick.AddListener(() =>
            {
                BuildingManager.Instance.SetActiveBuildingType(buildingType);
            });

            buttonTransformDictionary[buildingType] = buttonTransform;

            index++;
        }


    }

    private void Start()
    {
        BuildingManager.Instance.OnActiveBuildingTypeChanged += BuildingManager_OnActiveBuildingTypeChanged;
        UpdateActiveBuildingTypeButton();
    }

    private void BuildingManager_OnActiveBuildingTypeChanged(object sender, BuildingManager.OnActiveBuildingTypeChangedEventArgs e)
    {
        UpdateActiveBuildingTypeButton();
    }

    private void UpdateActiveBuildingTypeButton()
    {
        arrowButton.Find("selected").gameObject.SetActive(false);
        foreach (BuildingTypeScriptableObject buildingType in buttonTransformDictionary.Keys)
        {
            Transform buttonTransform = buttonTransformDictionary[buildingType];
            buttonTransform.Find("selected").gameObject.SetActive(false);
        }

        BuildingTypeScriptableObject activeBuildingType = BuildingManager.Instance.GetActiveBuildingType();

        if (activeBuildingType == null)
        {
            arrowButton.Find("selected").gameObject.SetActive(true);
        }
        else
        {
            buttonTransformDictionary[activeBuildingType].Find("selected").gameObject.SetActive(true);
        }
    }



}
