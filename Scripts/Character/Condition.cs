using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionSetting : AttributeModify, ITacticalEvent
{
    public string Name;
    public Sprite Icon;
    public bool Visible; //Видно ли состояние игроку.

    //События ITacticalEvent
    public ITacticalEvent.TurnStart TurnStart;
    public ITacticalEvent.TurnEnd TurnEnd;
    public ITacticalEvent.AttackPerformed AttackPerformed;
    public ITacticalEvent.AttackReceived AttackReceived;
    public ITacticalEvent.OnKill OnKill;

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

    public ConditionSetting(string name, Sprite icon)
    {
        Name = name;
        Icon = icon;
    }
}
public static class ConditionObject : object
{ 
    public static ConditionSetting Stun;
    public static ConditionSetting Hunger;
    public static ConditionSetting BatteryActivated;

    static ConditionObject()
    {
        Stun = new ConditionSetting("Оглушение", Resources.Load<Sprite>("ConditionIcon/Stun"));
        Stun.Dodge = -15;
        Stun.Parry = -15;
        Stun.TurnStart = ConditionProcedures.StunStartTurn;
        Stun.Visible = true;

        BatteryActivated = new ConditionSetting("Батарейка активирована", Resources.Load<Sprite>("BeltIcon/BatteryOn"));
        BatteryActivated.Visible = true;
    }
}
public static class ConditionProcedures : object
{
    public static void StunStartTurn(CharacterSetting C)
    {
        MainBattleScript.EndTurnCriterion = true;
    }
}