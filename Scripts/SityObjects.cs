using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SityObjects : object
{
    public static SitySetting SitySmall;
   // public static SitySetting Jungle;

    static SityObjects() //Создание каждого экземпляра типа территории.
    {
    //    SityType;      //Тип города: городок. 
        SitySmall = new SitySetting(GlobalEnumerators.SityNameSitySmallEnum, GlobalEnumerators.SityTypeEnum.SitySmall, "Городок");
    //    Jungle = new AreaSetting(GlobalEnumerators.AreaNameJungleEnum, GlobalEnumerators.AreaTypeEnum.Jungle, "Джунгли", -1.5f, 1);
    }

}
