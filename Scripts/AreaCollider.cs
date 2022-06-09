using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaCollider : MonoBehaviour
{
    public AreaSetting Area = AreaObjects.Swamp; 
    public string AreaName; //��� ����������� ���������� ����������.
    // Start is called before the first frame update
    void Start()
    {
        switch (this.tag) //��������� ���� ���������� ��� ����������� ���������� �� ��� ����. 
        {
            case "Swamp":
                Area = AreaObjects.Swamp;
                break;
            case "Jungle":
                Area = AreaObjects.Jungle;
                break;
        }
        int NameNumber = Random.Range(1, Area.AreaNameList.Count + GlobalEnumerators.AreaNameUniversalEnum.Count);
        if (NameNumber <= GlobalEnumerators.AreaNameUniversalEnum.Count)
        {
            AreaName = GlobalEnumerators.AreaNameUniversalEnum[NameNumber-1];
        }
        else
        {
            AreaName = Area.AreaNameList[NameNumber - GlobalEnumerators.AreaNameUniversalEnum.Count - 1];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
