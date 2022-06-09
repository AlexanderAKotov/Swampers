using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class AttributeModify : object
{
    public float StepCost = 0, Speed = 0, AttackCost = 0;
    public float Accuracy = 0, Damage = 0;
    public float Dodge = 0, Parry = 0;
    public float Strenght = 0, Endurance = 0, Agility = 0;
    public void ApplyDerivedModify(CharacterSetting C)
    {
        C.SpeedMax += Speed;
        for (int i = 0; i < C.Accuracy.Length; i++)
        {
            C.Accuracy[i].SetCurrent(C.Accuracy[i].Current + Accuracy);
        }
        for (int i = 0; i < C.Parry.Length; i++)
        {
            C.Parry[i].SetCurrent(C.Parry[i].Current + Parry);
        }
            C.Dodge.SetCurrent(C.Dodge.Current + Dodge);
        C.StepCost.SetCurrent(C.StepCost.Current + StepCost);
        C.AttackCost.SetCurrent(C.AttackCost.Current + AttackCost);
    }
    public void ApplyStatModify(CharacterSetting C)
    {
        C.Strenght.Current += Strenght;
        C.Endurance.Current += Endurance;
        C.Agility.Current += Agility;
    }
}

