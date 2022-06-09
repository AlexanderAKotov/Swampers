using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkSetting : AttributeModify, ITacticalEvent
{
    public ITacticalEvent.TurnStart TurnStart;
    public ITacticalEvent.TurnEnd TurnEnd;
    public ITacticalEvent.AttackPerformed AttackPerformed;
    public ITacticalEvent.AttackReceived AttackReceived;
    public ITacticalEvent.OnKill OnKill;
    // public GlobalEnumerators.PerkType PerkType;
    public GlobalEnumerators.WeaponTypeEnum WeaponType;
    
    public string Name;
    public Sprite Icon;
    public string Discription;
    public PerkGroup Group;
    public int NumberInGroup;

    public void TurnStartProcedure(CharacterSetting C)
    {
        if (TurnStart != null)
            TurnStart(C);
    }
    public void TurnEndProcedure(CharacterSetting C)
    {
        if (TurnEnd != null)
            TurnEnd(C);
    }
    public void AttackPerformedProcedure(CharacterSetting C, CharacterSetting T)
    {
        if (AttackPerformed != null)
            AttackPerformed(C, T);
    }
    public void AttackReceivedProcedure(CharacterSetting C, CharacterSetting T)
    {
        if (AttackReceived != null)
            AttackReceived(C, T);
    }
    public void OnKillProcedure(CharacterSetting C, CharacterSetting T)
    {
        if (OnKill != null)
            OnKill(C, T);
    }
    public PerkSetting(string name, Sprite icon, PerkGroup group, int numberInGroup)
    {
        Name = name;
        Icon = icon;
        Group = group;
        NumberInGroup = numberInGroup;
    }
    public PerkSetting FillOnPerk(PerkSetting I)
    {
        string Name = this.Name;
        Sprite Icon = this.Icon;
        PerkGroup Group = this.Group;
        int NumberInGroup = this.NumberInGroup;
        PerkSetting NewPerk = (PerkSetting)I.MemberwiseClone();
        NewPerk.Name = Name;
        NewPerk.Icon = Icon;
        NewPerk.Group = Group;
        NewPerk.NumberInGroup = NumberInGroup;
        return NewPerk;
        
    }
}


public class PerkGroup : object
{
    public List<PerkSetting> PerkList;
    public List<GlobalEnumerators.DamageType> DamageTypes;
    public PerkGroup()
    {
        PerkList = new List<PerkSetting>();
        DamageTypes = new List<GlobalEnumerators.DamageType>();
    }
}
public static class PerkObject : object
{
    public static PerkSetting LegPerkSP_1, LegPerkSP_2, LegPerkSP_3;

    static PerkObject()
    {
        LegPerkSP_1 = new PerkSetting("Легкое проникающее ранение ноги", Resources.Load<Sprite>("PerkIcon/LegPerkSP_1"), PerkGroupObject.LegPerkSP, 0);
        LegPerkSP_1.StepCost = 1;
        LegPerkSP_2 = new PerkSetting("Среднее проникающее ранение ноги", Resources.Load<Sprite>("PerkIcon/LegPerkSP_2"), PerkGroupObject.LegPerkSP, 1);
        LegPerkSP_2 = LegPerkSP_2.FillOnPerk(LegPerkSP_1);
        LegPerkSP_2.Dodge = -5;
        LegPerkSP_3 = new PerkSetting("Тяжелое проникающее ранение ноги", Resources.Load<Sprite>("PerkIcon/LegPerkSP_3"), PerkGroupObject.LegPerkSP, 2);
        LegPerkSP_3 = LegPerkSP_3.FillOnPerk(LegPerkSP_2);
        LegPerkSP_3.Dodge = -5;
        LegPerkSP_3.TurnEnd = PerkProcedure.BleedIfMoved;
    }
}
public static class PerkGroupObject : object
{
    public static List<PerkGroup> PerkGroupList;
    public static PerkGroup LegPerkSP, LegPerkB;
    public static PerkGroup ArmPerk;
    public static PerkGroup HeadPerk;
    public static PerkGroup TorsoPerk;

    static PerkGroupObject()
    {
        PerkGroupList = new List<PerkGroup>();
        LegPerkSP = new PerkGroup();
        LegPerkSP.DamageTypes.Add(GlobalEnumerators.DamageType.Slash);
        LegPerkSP.DamageTypes.Add(GlobalEnumerators.DamageType.Pierce);
        LegPerkSP.PerkList.Add(PerkObject.LegPerkSP_1);
        LegPerkSP.PerkList.Add(PerkObject.LegPerkSP_2);
        LegPerkSP.PerkList.Add(PerkObject.LegPerkSP_3);
        PerkGroupList.Add(LegPerkSP);
    }
}
public static class PerkProcedure : object
{
    public static void BleedIfMoved(CharacterSetting C)
    {
        if (C.HexMovedInTurn > 0)
        {
            C.HP -= 2;
        }
    }
}