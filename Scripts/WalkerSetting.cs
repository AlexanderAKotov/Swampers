using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface ICloneable { object CreateClone(); }
public class WalkerSetting : object, ICloneable
{
    public GlobalEnumerators.WalkerTypeEnum WalkerType;
    public string Name;
    public float SpeedBase;
    public int   CrewBaseMax;
    public AreaSpeedModStruct[] AreaSpeedMod;
    public WalkerSetting NextWalker;
    public int AgeStage;
    public double InventoryWeightMaxUnderPenalty;
    public List<ItemSetting> Inventory = new List<ItemSetting> { };
    public List <CharacterSetting> Crew = new List<CharacterSetting> { };
    public struct AreaSpeedModStruct 
    {
        public GlobalEnumerators.AreaTypeEnum AreaType;
        public float ModSpeedAbs;
        public float ModSpeedProc;
    }
   public WalkerSetting(GlobalEnumerators.WalkerTypeEnum WalkerType, int AgeStage, string Name, float SpeedBase, int CrewBaseMax, WalkerSetting NextWalker = null)
    {
        this.WalkerType = WalkerType;
        if (Name == "")
        {
            this.Name = WalkerType.ToString();
        }
        else
        {
            this.Name = Name;
        }
        this.AgeStage = AgeStage;
        this.SpeedBase = SpeedBase;
        this.CrewBaseMax = CrewBaseMax;
        this.NextWalker = NextWalker;
    }
    public object CreateClone()
    {
        WalkerSetting Clone = (WalkerSetting)MemberwiseClone();
        Clone.AreaSpeedMod =  (WalkerSetting.AreaSpeedModStruct[]) this.AreaSpeedMod.Clone();
        return Clone; 
    }
    public void CheckInventoryOnNull()
    {
        for (int i = 0; i < Inventory.Count; i++)
        {
            if (Inventory[i] == null)
            {
                Inventory.RemoveAt(i);
                i--;
            }
        }
    }
}
