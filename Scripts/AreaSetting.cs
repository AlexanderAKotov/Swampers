using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaSetting : object, ICloneable
{
    public GlobalEnumerators.AreaTypeEnum AreaType;     //��� �����: ������, ���, ������� � �.�. 
    public string AreaName;
    public float ModSpeedAbs;   //���������� ����������� �������� �� ���� �����, �������� �� �������� ���������.
    public float ModSpeedProc;  //���������� ����������� �������� �� ���� �����, �������� �� �������� ���������.
    public List<string> AreaNameList;

    public AreaSetting(List<string> AreaNameList, GlobalEnumerators.AreaTypeEnum AreaType, string AreaName, float ModSpeedAbs = 0, float ModSpeedProc = 1)
    {
        this.AreaNameList = AreaNameList;
        this.AreaType = AreaType;
        this.AreaName = AreaName;
        this.ModSpeedAbs = ModSpeedAbs;
        this.ModSpeedProc = ModSpeedProc;

    }
        public object CreateClone()
    {
        return (AreaSetting)MemberwiseClone();
    }
   
}
