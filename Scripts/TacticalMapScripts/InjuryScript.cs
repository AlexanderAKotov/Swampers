using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InjurySetting : AttributeModify, ITacticalEvent
{
    public ITacticalEvent.TurnStart TurnStart;
    public ITacticalEvent.TurnEnd TurnEnd;
    public ITacticalEvent.AttackPerformed AttackPerformed; //Атака совершена.
    public ITacticalEvent.AttackReceived AttackReceived; //Атака получена.
    public ITacticalEvent.OnKill OnKill;
    public string Name;
    public Sprite Icon;
    public string Discription;
    public InjuryGroup Group;
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
    public InjurySetting(string name, Sprite icon, InjuryGroup group, int numberInGroup)
    {
        Name = name;
        Icon = icon;
        Group = group;
        NumberInGroup = numberInGroup;
    }
    public InjurySetting FillOnInjury(InjurySetting I)
    {
        string Name = this.Name;
        Sprite Icon = this.Icon;
        InjuryGroup Group = this.Group;
        int NumberInGroup = this.NumberInGroup;
        InjurySetting NewInjury = (InjurySetting)I.MemberwiseClone();
        NewInjury.Name = Name;
        NewInjury.Icon = Icon;
        NewInjury.Group = Group;
        NewInjury.NumberInGroup = NumberInGroup;
        return NewInjury;
        /* 
        Discription = I.Discription;
        StepCost = I.StepCost;
        Speed = I.Speed;
        Accuracy = I.Accuracy;
        Damage = I.Damage;
        Strenght = I.Strenght;
        Endurance = I.Endurance;
        Agility = I.Agility;
        Dodge = I.Dodge;
        Parry = I.Parry; */
    }
}


public class InjuryGroup : object
{
    public List<InjurySetting> InjuryList;
    public List<GlobalEnumerators.DamageType> DamageTypes;
    public InjuryGroup()
    {
        InjuryList = new List<InjurySetting>();
        DamageTypes = new List<GlobalEnumerators.DamageType>();
    }
}
public static class InjuryObject : object
{
    public static InjurySetting LegInjurySP_1, LegInjurySP_2, LegInjurySP_3, ArmInjurySP_1, ArmInjurySP_2, ArmInjurySP_3;

    static InjuryObject()
    {
        LegInjurySP_1 = new InjurySetting("Легкое проникающее ранение ноги", Resources.Load<Sprite>("InjuryIcon/LegInjurySP_1"), InjuryGroupObject.LegInjurySP, 0);
        LegInjurySP_1.StepCost = 1;
        LegInjurySP_2 = new InjurySetting("Среднее проникающее ранение ноги", Resources.Load<Sprite>("InjuryIcon/LegInjurySP_2"), InjuryGroupObject.LegInjurySP, 1);
        LegInjurySP_2 = LegInjurySP_2.FillOnInjury(LegInjurySP_1);
        LegInjurySP_2.Dodge = -5;
        LegInjurySP_3 = new InjurySetting("Тяжелое проникающее ранение ноги", Resources.Load<Sprite>("InjuryIcon/LegInjurySP_3"), InjuryGroupObject.LegInjurySP, 2);
        LegInjurySP_3 = LegInjurySP_3.FillOnInjury(LegInjurySP_2);
        LegInjurySP_3.Dodge = -5;
        LegInjurySP_3.TurnEnd = InjuryProcedure.BleedIfMoved;

        ArmInjurySP_1 = new InjurySetting("Легкое проникающее ранение руки", Resources.Load<Sprite>("InjuryIcon/ArmInjurySP_1"), InjuryGroupObject.ArmInjurySP, 0);
        ArmInjurySP_1.Accuracy = -5;
        ArmInjurySP_2 = new InjurySetting("Среднее проникающее ранение руки", Resources.Load<Sprite>("InjuryIcon/ArmInjurySP_2"), InjuryGroupObject.ArmInjurySP, 1);
        ArmInjurySP_2 = ArmInjurySP_2.FillOnInjury(ArmInjurySP_1);
        ArmInjurySP_2.AttackCost = 1;
        ArmInjurySP_3 = new InjurySetting("Тяжелое проникающее ранение руки", Resources.Load<Sprite>("InjuryIcon/ArmInjurySP_3"), InjuryGroupObject.ArmInjurySP, 2);
        ArmInjurySP_3 = ArmInjurySP_3.FillOnInjury(ArmInjurySP_2);
        ArmInjurySP_3.TurnEnd = InjuryProcedure.BleedIfAttack;

    }
}
public static class InjuryGroupObject : object
{
    public static List<InjuryGroup> InjuryGroupList;
    public static InjuryGroup LegInjurySP, LegInjuryB;
    public static InjuryGroup ArmInjury, ArmInjurySP;
    public static InjuryGroup HeadInjury;
    public static InjuryGroup TorsoInjury;

    static InjuryGroupObject()
    {
        InjuryGroupList = new List<InjuryGroup>();
        LegInjurySP = new InjuryGroup();
        LegInjurySP.DamageTypes.Add(GlobalEnumerators.DamageType.Slash);
        LegInjurySP.DamageTypes.Add(GlobalEnumerators.DamageType.Pierce);
        LegInjurySP.InjuryList.Add(InjuryObject.LegInjurySP_1);
        LegInjurySP.InjuryList.Add(InjuryObject.LegInjurySP_2);
        LegInjurySP.InjuryList.Add(InjuryObject.LegInjurySP_3);
        InjuryGroupList.Add(LegInjurySP);

        ArmInjurySP = new InjuryGroup();
        ArmInjurySP.DamageTypes.Add(GlobalEnumerators.DamageType.Slash);
        ArmInjurySP.DamageTypes.Add(GlobalEnumerators.DamageType.Pierce);
        ArmInjurySP.InjuryList.Add(InjuryObject.ArmInjurySP_1);
        ArmInjurySP.InjuryList.Add(InjuryObject.ArmInjurySP_2);
        ArmInjurySP.InjuryList.Add(InjuryObject.ArmInjurySP_3);
        InjuryGroupList.Add(ArmInjurySP);
    }
}
public static class InjuryProcedure : object
{
    public static void BleedIfMoved(CharacterSetting C)
    {
        if (C.HexMovedInTurn > 1)
        {
            C.ApplyBleed(Random.Range(0, (float) C.HexMovedInTurn - 1));//Кровотечение увеличивается за каждый шаг после первого
        }
    }
    public static void BleedIfAttack(CharacterSetting C)
    {
        if (C.AttackPerformedInTurn > 1)
        {
            C.ApplyBleed(Random.Range(0, (float)C.AttackPerformedInTurn - 1));//Кровотечение увеличивается за каждую атаку/способность после первой
        }
    }
}