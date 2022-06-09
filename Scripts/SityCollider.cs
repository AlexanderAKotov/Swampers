using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SityCollider : MonoBehaviour
{
    public SitySetting Sity = SityObjects.SitySmall;
    public string SityName; //»м€ уникального экземпл€ра города.
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        switch (this.tag) //”становка типа территории дл€ конкретного коллайдера по его тегу. 
        {
            case "SitySmall":
                Sity = SityObjects.SitySmall;
                break;
        }
        int NameNumber = Random.Range(1, Sity.SityNameList.Count + GlobalEnumerators.SityNameUniversalEnum.Count);
        if (NameNumber <= GlobalEnumerators.SityNameUniversalEnum.Count)
        {
            SityName = GlobalEnumerators.SityNameUniversalEnum[NameNumber - 1];
        }
        else
        {
            SityName = Sity.SityNameList[NameNumber - GlobalEnumerators.SityNameUniversalEnum.Count - 1];
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
