using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SityObjects : object
{
    public static SitySetting SitySmall;
   // public static SitySetting Jungle;

    static SityObjects() //�������� ������� ���������� ���� ����������.
    {
    //    SityType;      //��� ������: �������. 
        SitySmall = new SitySetting(GlobalEnumerators.SityNameSitySmallEnum, GlobalEnumerators.SityTypeEnum.SitySmall, "�������");
    //    Jungle = new AreaSetting(GlobalEnumerators.AreaNameJungleEnum, GlobalEnumerators.AreaTypeEnum.Jungle, "�������", -1.5f, 1);
    }

}
