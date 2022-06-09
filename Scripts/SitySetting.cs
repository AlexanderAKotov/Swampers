using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SitySetting : object, ICloneable
{
    public GlobalEnumerators.SityTypeEnum SityType;     //Тип почвы: болото, луг, пустыня и т.д. 
    public string SityName;
    public List<string> SityNameList;

    public SitySetting(List<string> SityNameList, GlobalEnumerators.SityTypeEnum SityType, string SityName)
    {
        this.SityNameList = SityNameList;
        this.SityType = SityType;
        this.SityName = SityName;
    }
    public object CreateClone()
    {
        return (SitySetting)MemberwiseClone();
    }
}
