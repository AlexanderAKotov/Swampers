using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileSetting : AttributeModify
{
    public bool AllowStanding; // Можно ли на клетку вставать.
    public int StepCostMod;
}
public static class TileObject : object
{ 
    public static TileSetting Ground, Blocked;
    static TileObject()
    {
        Ground = new TileSetting();
        Ground.AllowStanding = true;
        Blocked = new TileSetting();
    }
}