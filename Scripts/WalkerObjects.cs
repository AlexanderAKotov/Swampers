using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WalkerObjects : object
{
    public static WalkerSetting TurtleYoung; // Стартовое животное молодое.
    public static WalkerSetting TurtleMiddle;
    public static WalkerSetting TurtleOld;


    static WalkerObjects()
    {
        TurtleYoung = new WalkerSetting(GlobalEnumerators.WalkerTypeEnum.TurtleYoung, 1, "Черепаха", 5, 8, TurtleMiddle);
        TurtleMiddle = new WalkerSetting(GlobalEnumerators.WalkerTypeEnum.TurtleMiddle, 2, "Черепаха", 5, 9, TurtleOld);
        TurtleMiddle.AreaSpeedMod = new WalkerSetting.AreaSpeedModStruct[1];
        TurtleMiddle.AreaSpeedMod[0].AreaType = GlobalEnumerators.AreaTypeEnum.Swamp;
        TurtleMiddle.AreaSpeedMod[0].ModSpeedAbs = - 0.4f;
        TurtleMiddle.AreaSpeedMod[0].ModSpeedProc = 0.5f;
        TurtleOld = new WalkerSetting(GlobalEnumerators.WalkerTypeEnum.TurtleOld, 3, "Черепаха", 5, 10);
    }

   
}
