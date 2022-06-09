using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AreaObjects : object
{
    public static AreaSetting Swamp;
    public static AreaSetting Jungle;
  
    static AreaObjects() //�������� ������� ���������� ���� ����������.
    {
        //    AreaType;      //��� �����: ������, ���, ������� � �.�. 
        //    ModSpeedAbs;   //���������� ����������� �������� �� ���� �����, �������� �� �������� ���������.
        //    ModSpeedProc;  //���������� ����������� �������� �� ���� �����, �������� �� �������� ���������.

        Swamp = new AreaSetting(GlobalEnumerators.AreaNameSwampEnum, GlobalEnumerators.AreaTypeEnum.Swamp,"������", -1, 1);
        Jungle = new AreaSetting(GlobalEnumerators.AreaNameJungleEnum, GlobalEnumerators.AreaTypeEnum.Jungle,"�������", -1.5f, 1);
    }

}
