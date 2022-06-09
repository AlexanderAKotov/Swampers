using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public delegate void Activate(CharacterSetting C, ItemSetting Item);
public class ItemSetting : AttributeModify, ICloneable, ITacticalEvent
{
    public GlobalEnumerators.ItemTypeEnum ItemType;
    public AbilitySetting[] Abilities;
    public List<GlobalEnumerators.ItemTag> ItemTag = new List<GlobalEnumerators.ItemTag>();
    public string Name;
    public Sprite IconTop, IconBottom, IconActivated;
    public bool UseBottomIconAsPrimary = false;
    public bool AllowActivate = false;
    private bool _Activated = false;
    public Activate ActivateProcedure;
    public void Activate(CharacterSetting C)
    {
        Debug.Log("Вход в обход делегата");
        if (ActivateProcedure != null)
        {
            Debug.Log("Переменная делегата не нал");
            ActivateProcedure(C, this);
        }
    }

    //Оружие
    public GlobalEnumerators.WeaponTypeEnum WeaponType;
    public GlobalEnumerators.DamageType DamageType;
    public bool Twohand, AllowTwohandGrip, AllowBagWearing, AllowParry, CanBeParred, Ranged;
    public float DamageMin, DamageMax;
    public float DamageStrenghtMult;
    public float SpeedCostBase;
    public float AccuracyMod, DodgeMod = 0;
    public float ParryMod, ParryValueMult;
    public float InjuryThresholdMod;
    public float RangeMin, RangeMax;
    //Доспехи
    public ArmorValueStruct[] ArmorValues;
    public int NumberOfBeltSlot;
    //Пояс
    public int Capacity; //Вместимость предмета с зарядами
    public int Fullness; //Заполненность вместимости предмета с зарядами
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


    public List<GlobalEnumerators.RaceTypeEnum> ArmorExcludingRaceTypes;

    public struct ArmorValueStruct
    {
        public GlobalEnumerators.DamageType DamageType;
        public float Value;
    }
    public ItemSetting(GlobalEnumerators.ItemTypeEnum itemType, GlobalEnumerators.WeaponTypeEnum weaponType, GlobalEnumerators.DamageType damageType, AbilitySetting[] abilities, string name, Sprite icon, bool twohand, bool allowTwohandGrip, bool allowBagWearing, bool allowParry, bool canBeParred, bool ranged, float damageMin, float damageMax, float damageStrenghtMult, float speedCostBase, float accuracyMod, float parryMod, float parryValueMult, float injuryThresholdMod, float rangeMin = 1, float rangeMax = 1)
    {
        ItemType = itemType;
        WeaponType = weaponType;
        DamageType = damageType;
        Abilities = abilities;
        Name = name;
        IconTop = icon;
        Twohand = twohand;
        AllowTwohandGrip = allowTwohandGrip;
        AllowBagWearing = allowBagWearing;
        AllowParry = allowParry;
        CanBeParred = canBeParred;
        Ranged = ranged;
        DamageMin = damageMin;
        DamageMax = damageMax;
        DamageStrenghtMult = damageStrenghtMult;
        SpeedCostBase = speedCostBase;
        AccuracyMod = accuracyMod;
        ParryMod = parryMod;
        ParryValueMult = parryValueMult;
        InjuryThresholdMod = injuryThresholdMod;
        RangeMin = rangeMin;
        RangeMax = rangeMax;
    }

    public ItemSetting(GlobalEnumerators.ItemTypeEnum itemType, AbilitySetting[] abilities, string name, Sprite icon, float[] ArmorValue, float dodgeMod, float stepCostMod, float speedMod, int numberOfBeltSlot, List<GlobalEnumerators.RaceTypeEnum> excludingRaceTypes)
    {
        ItemType = itemType;
        Abilities = abilities;
        Name = name;
        IconTop = icon;
        ArmorValues = new ArmorValueStruct[Enum.GetNames(typeof(GlobalEnumerators.DamageType)).Length - 1];
        for (int i = 0; i < ArmorValues.Length; i++) // Не берем первый тип урона None
        {
            ArmorValues[i].DamageType = (GlobalEnumerators.DamageType)Enum.GetValues(typeof(GlobalEnumerators.DamageType)).GetValue(i + 1);
            ArmorValues[i].Value = ArmorValue[i];
        }
        base.Dodge = dodgeMod;
        base.StepCost = stepCostMod;
        base.Speed = speedMod;
        NumberOfBeltSlot = numberOfBeltSlot;
        ArmorExcludingRaceTypes = excludingRaceTypes;
    }

    public ItemSetting(GlobalEnumerators.ItemTypeEnum itemType, AbilitySetting[] abilities, string name, Sprite iconTop, Sprite iconActivated, bool allowActivate, bool activated, int capacity, int fullness)
    {
        ItemType = itemType;
        Abilities = abilities;
        Name = name;
        IconTop = iconTop;
        IconActivated = iconActivated;
        AllowActivate = allowActivate;
        _Activated = activated;
        Capacity = capacity;
        Fullness = fullness;
    }

    public static string DamageTypeToString(GlobalEnumerators.DamageType Type) => Type switch
    {
        GlobalEnumerators.DamageType.Aсid => "Кислота",
        GlobalEnumerators.DamageType.Blunt => "Дробящий",
        GlobalEnumerators.DamageType.Energy => "Энергия",
        GlobalEnumerators.DamageType.Fire => "Огонь",
        GlobalEnumerators.DamageType.Frost => "Холод",
        GlobalEnumerators.DamageType.Pierce => "Колющий",
        GlobalEnumerators.DamageType.Slash => "Рубящий",
        _ => "Неизвестно"
    };

    public object CreateClone()
    {
        return (ItemSetting)MemberwiseClone();
    }
    public float GetArmorValueFromDamageType(GlobalEnumerators.DamageType DamageType)
    {
        for (int i = 0; i < ArmorValues.Length; i++)
        {
            if (ArmorValues[i].DamageType == DamageType)
                return ArmorValues[i].Value;
        }
        return 0;
    }
    public Sprite GetPrimarySprite()
    {
        if (UseBottomIconAsPrimary)
        {
            return IconBottom;
        }
        else
        {
            return IconTop;
        }
    }
    public void SetActivation(CharacterSetting C, bool value)
    {
        Debug.Log("Установка активации предмета " + value.ToString());
        if (_Activated != value)
            {
                _Activated = value;
                Activate(C);
            }
    }
    public bool GetActivation() => _Activated;
    
}
public static class ItemObject : object
{
    //Кулаки по расам.
    public static ItemSetting KwabrothFist;
    public static ItemSetting BlamkFist;
    public static ItemSetting MossFist;
    //Оружие.
    public static ItemSetting BoneShiv;
    public static ItemSetting RustySword;
    public static ItemSetting CareerGnawer;
    //Доспехи.
    public static ItemSetting Baldric; //Перевязь
    public static ItemSetting HeavyScrapArmor;
    //Пояс
    public static ItemSetting BatterySmall;

    static ItemObject()
    {
        AbilitySetting[] FistAbilities = new AbilitySetting[2];
        FistAbilities[0] = AbilityObjects.RegularUnarmedAttack;
        FistAbilities[1] = AbilityObjects.ImprovedUnarmedAttack;
        KwabrothFist = new ItemSetting(GlobalEnumerators.ItemTypeEnum.Weapon, GlobalEnumerators.WeaponTypeEnum.Unarmed, GlobalEnumerators.DamageType.Blunt, FistAbilities, "Кулак кваброта", Resources.Load<Sprite>("WeaponIcon/SharpeningIcon"), false, false, false, false, true, false, 2f, 5f, 0.5f, 3, -5, 0, 0, 1);
        BlamkFist    = new ItemSetting(GlobalEnumerators.ItemTypeEnum.Weapon, GlobalEnumerators.WeaponTypeEnum.Unarmed, GlobalEnumerators.DamageType.Blunt, FistAbilities, "Кулак бламка"  , Resources.Load<Sprite>("WeaponIcon/SharpeningIcon"), false, false, false, false, true, false, 2f, 4f, 0.4f, 3, -5, 0, 0, 1);
        MossFist     = new ItemSetting(GlobalEnumerators.ItemTypeEnum.Weapon, GlobalEnumerators.WeaponTypeEnum.Unarmed, GlobalEnumerators.DamageType.Pierce, FistAbilities, "Шипы мха"    ,  Resources.Load<Sprite>("WeaponIcon/MossFistIcon"), false, false, false, true, true, false , 3f, 6f, 0.5f, 4, -5, 5, 0, 1);
        //Оружие
        AbilitySetting[] BladeAbilities = new AbilitySetting[1];
        BladeAbilities[0] = AbilityObjects.RegularSlash;
        //BladeAbilities[1] = AbilityObjects.StunAttack;
        BoneShiv     = new ItemSetting(GlobalEnumerators.ItemTypeEnum.Weapon, GlobalEnumerators.WeaponTypeEnum.Shiv, GlobalEnumerators.DamageType.Pierce, BladeAbilities, "Костяная заточка", Resources.Load<Sprite>("WeaponIcon/SharpeningIcon"), false, false, true, true, true, false, 4f, 8f, 0.5f, 3, 5, 5, 0.5f, -2);
        RustySword   = new ItemSetting(GlobalEnumerators.ItemTypeEnum.Weapon, GlobalEnumerators.WeaponTypeEnum.Sword, GlobalEnumerators.DamageType.Slash, BladeAbilities, "Ржавый меч", Resources.Load<Sprite>("WeaponIcon/RustySwordIcon"), false, true, false, true, true, false, 5f, 10f, 0.8f, 4, 0, 5, 0.5f, 0);
        AbilitySetting[] BluntAbilities = new AbilitySetting[2];
        BluntAbilities[0] = AbilityObjects.RegularBlunt;
        BluntAbilities[1] = AbilityObjects.StunAttack;
        CareerGnawer = new ItemSetting(GlobalEnumerators.ItemTypeEnum.Weapon, GlobalEnumerators.WeaponTypeEnum.Sledgehammer, GlobalEnumerators.DamageType.Blunt, BluntAbilities, "Карьерный грызун", Resources.Load<Sprite>("WeaponIcon/CareerGnawerIconTop"), true, false, false, true, true, false, 5f, 10f, 1f, 6, 0, 0, 0.7f, 0);
        CareerGnawer.IconBottom = Resources.Load<Sprite>("WeaponIcon/CareerGnawerIconBottom");
        //CareerGnawer.UseBottomIconAsPrimary = true;
        //Доспехи 
        Baldric         = new ItemSetting(GlobalEnumerators.ItemTypeEnum.Armor, null, "Перевязь", Resources.Load<Sprite>("ArmorIcon/BaldricIcon"),                        new float[] { 2, 2, 2, 2, 2, 2, 2 }, 0, 0, 0, 2, new List<GlobalEnumerators.RaceTypeEnum>());
        HeavyScrapArmor = new ItemSetting(GlobalEnumerators.ItemTypeEnum.Armor, null, "Тяжелый мусорный доспех", Resources.Load<Sprite>("ArmorIcon/HeawyScrapArmorIcon"), new float[] { 5, 5, 5, 0, 0, 0, 0 }, -20, 1, -1, 0, new List<GlobalEnumerators.RaceTypeEnum>());
        //Пояс
        AbilitySetting[] ActivateItem = new AbilitySetting[1];
        ActivateItem[0] = AbilityObjects.SwitchItemActivity;
        BatterySmall = new ItemSetting(GlobalEnumerators.ItemTypeEnum.Belt, ActivateItem, "Энергоблок малый", Resources.Load<Sprite>("BeltIcon/BatteryOff"), Resources.Load<Sprite>("BeltIcon/BatteryOn"), true, false, 10, 8);
        BatterySmall.ItemTag.Add(GlobalEnumerators.ItemTag.Battery);
        BatterySmall.ActivateProcedure = ItemProcedure.BatteryActivated;
    }
}
public class ItemProcedure : object
{
    public static void BatteryActivated(CharacterSetting C, ItemSetting Item)
    {
        Debug.Log("Вход в процедуру батарейки ");
        if (Item.GetActivation())
        {
            Debug.Log("Предмет включен ");
            int Index = C.GetConditionIndex(ConditionObject.BatteryActivated);
            if (Index != -1)
            {
                Debug.Log("Есть состояние ");
                for (int i = 0; i < C.NumberOfBeltSlot; i++)
                {
                    if (C.Belt[i] != null)
                    {
                        if (C.Belt[i].GetActivation() && C.Belt[i].ItemTag.Contains(GlobalEnumerators.ItemTag.Battery) && C.Belt[i] != Item)
                        {
                            C.Belt[i].SetActivation(C, false);
                            break;
                        }
                    }
                }
            }
            C.AddCondition(ConditionObject.BatteryActivated, GlobalEnumerators.DurationMeasurementUnit.Endless, 1, true);
        }
        else
        {
            Debug.Log("Предмет выключен ");
            C.RemoveCondition(ConditionObject.BatteryActivated);
        }
    }
}
