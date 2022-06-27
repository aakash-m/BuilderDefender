using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="ScriptableObjects/BuildingTypeList")]
public class BuildingTypeListScriptableObject : ScriptableObject
{
    public List<BuildingTypeScriptableObject> list;
}
