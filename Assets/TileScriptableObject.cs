using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TileData", menuName = "ScriptableObjects/TileData", order = 1)]
public class TileScriptableObject : ScriptableObject
{
    public GameObject prefab;
    public int[] validNeighbours;
}
