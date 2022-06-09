using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AreaObjects : object
{
    public static AreaSetting Swamp;
    public static AreaSetting Jungle;
  
    static AreaObjects() //Создание каждого экземпляра типа территории.
    {
        //    AreaType;      //Тип почвы: болото, луг, пустыня и т.д. 
        //    ModSpeedAbs;   //Абсолютный модификатор скорости от типа почвы, влияющий на скорость животного.
        //    ModSpeedProc;  //Процентный модификатор скорости от типа почвы, влияющий на скорость животного.

        Swamp = new AreaSetting(GlobalEnumerators.AreaNameSwampEnum, GlobalEnumerators.AreaTypeEnum.Swamp,"Болото", -1, 1);
        Jungle = new AreaSetting(GlobalEnumerators.AreaNameJungleEnum, GlobalEnumerators.AreaTypeEnum.Jungle,"Джунгли", -1.5f, 1);
    }

}
