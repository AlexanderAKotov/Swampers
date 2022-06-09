using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RaceSetting : object, ICloneable
{
    public GlobalEnumerators.RaceTypeEnum RaceType;
    public string RaceName;
    public float InjuryThresholdBase, InjuryThresholdEnduranceMult, InjuryThresholdStrenghtMult;
    public float HPBase, HPEnduranceMult;
    public float StrenghtBonus, AgilityBonus, EnduranceBonus;
    public float StepCostBase;
    public float AccuracyBase;
    public float DodgeBase;
    public float ParryBase = 0;
    public float InitiativeBase;
    public int NumberOfSlotBase = 0; //���������� ������ �� �����
    private List<string> NameList;
    public ItemSetting Fist;
    public bool[] ArmsEnabled;
    /* public bool RightArmEnabled = true;
    public bool LeftArmEnabled = true;
    public bool RightArmSecondEnabled = false;
    public bool LeftArmSecondEnabled = false; */
  
    public RaceSetting(GlobalEnumerators.RaceTypeEnum raceType, string raceName, List<string> nameList, float injuryThresholdBase, float injuryThresholdEnduranceMult, float injuryThresholdStrenghtMult, float hPBase, float hPEnduranceMult, float strenghtBonus, float agilityBonus, float enduranceBonus, float stepCostBase, float accuracyBase, float dodgeBase, float initiativeBase, ItemSetting fist)
    {
        ArmsEnabled = new bool[4] { true, true, false, false };
        RaceType = raceType; // ��� ���� ��� ���� ��-���������.
        RaceName = raceName; // ��� ���� ��� ���������� ��-������.
        NameList = nameList; // ���� � ������� ���������� �� ����.
        InjuryThresholdBase = injuryThresholdBase; // ����� ������� �������.
        InjuryThresholdEnduranceMult = injuryThresholdEnduranceMult; //����� ������� ��������� �� ������������ (Endurance).
        InjuryThresholdStrenghtMult = injuryThresholdStrenghtMult; //����� ������� ��������� �� ���� (Strenght).
        HPBase = hPBase; //���� �������� �������.
        HPEnduranceMult = hPEnduranceMult; // ���� �������� ��������� �� ������������ (Endurance).
        StrenghtBonus = strenghtBonus; // ���� �������� ��������� �� ���� (Strenght).
        AgilityBonus = agilityBonus; // ����� ��������.
        EnduranceBonus = enduranceBonus; // ����� ���� (Strenght).
        StepCostBase = stepCostBase; // ��������� ���� � �������� �������� �������.
        AccuracyBase = accuracyBase; // �������� ����� �������.
        DodgeBase = dodgeBase; //���� ���������� �� ����� �������.
        InitiativeBase = initiativeBase; //������� � ����������� �����.
        Fist = (ItemSetting)fist.CreateClone();
    }

    public object CreateClone()
    {
        return (RaceSetting)MemberwiseClone();
    }
    public string GetRandomName()
    {
        return NameList[UnityEngine.Random.Range(0, NameList.Count)];
    }
    public Sprite GetRandomSprite()
    {
        string path = "";
        switch (RaceType)
        {
            case  GlobalEnumerators.RaceTypeEnum.Blamk:
                path = "Blamk";
                break;
            case GlobalEnumerators.RaceTypeEnum.Kwabroth:
                path = "Kwabroth";
                break;
            case GlobalEnumerators.RaceTypeEnum.Moss:
                path = "Moss";
                break;
        }
        return (Sprite)Resources.Load<Sprite>(path);

    }
}
public static class RaceObject : object
{
    private static RaceSetting[] RaceArray;
    public static RaceSetting Kwabroth, Blamk, Moss;
    static RaceObject()
    {
        RaceArray = new RaceSetting[Enum.GetNames(typeof(GlobalEnumerators.RaceTypeEnum)).Length]; //������� ������ � ���������� ��������� = RaceTypeEnum 
        int RaceArrayCounter = 0;
        
        Kwabroth = new RaceSetting(GlobalEnumerators.RaceTypeEnum.Kwabroth, "�������", GlobalEnumerators.KwabrothRaceNameEnum,4, 0.8f, 0.3f, 20, 3, 1, 0, 0, 4, 75, 10, 0, ItemObject.KwabrothFist);
        RaceArray[RaceArrayCounter] = Kwabroth;
        RaceArrayCounter++;
        
        Blamk =           new RaceSetting(GlobalEnumerators.RaceTypeEnum.Blamk, "�����", GlobalEnumerators.BlamkRaceNameEnum, 2, 0.6f, 0.3f, 15, 2.5f, 1, 1, 0, 4, 80, 15, 0, ItemObject.BlamkFist);
        Blamk.ArmsEnabled[2] = true;
        Blamk.ArmsEnabled[3] = true;
        RaceArray[RaceArrayCounter] = Blamk;
        RaceArrayCounter++;

        Moss =                new RaceSetting(GlobalEnumerators.RaceTypeEnum.Moss, "���", GlobalEnumerators.MossRaceNameEnum, 5, 0.8f, 0.4f, 20, 2, 1, 0, 1, 5, 70, 10, 0, ItemObject.MossFist);
        RaceArray[RaceArrayCounter] = Moss;
        RaceArrayCounter++;
    }
    public static RaceSetting GetRandomRace()
    {
        return RaceArray[UnityEngine.Random.Range(0, RaceArray.Length)];
    }
    
}
