using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="ScriptableObjects/BuildingType")]
public class BuildingTypeScriptableObject : ScriptableObject
{
    public string nameString;
    public Transform prefab;
    public ResourceGeneratorData resourceGeneratorData;
    public Sprite sprite;
    public float minConstructionRadius;

}
