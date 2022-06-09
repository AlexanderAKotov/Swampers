using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class CharacterSetting : object
{
    public string Name;
    private FloatStatList _HP;
    public FloatStatList Strenght;
    public FloatStatList Agility;
    public FloatStatList Endurance;
    private FloatStatList _Speed;
    public FloatStatList[] Accuracy;
    //public FloatStatList AccuracyRight, AccuracyLeft, AccuracyRightSecond, AccuracyLeftSecond;
    public FloatStatList CritChance;
    public FloatStatList Dodge;
    public FloatStatList[] Parry;
    //public FloatStatList ParryRight, ParryLeft, ParryRightSecond, ParryLeftSecond;
    public FloatStatList InjuryThreshold;
    public FloatStatList StepCost, AttackCost;
    public FloatStatList Initiative;
    public FloatStatList OffHandDamagePenalty;
    private int _NumberOfBeltSlot;
    public RaceSetting Race;
    public static float SpeedGlobalMod = 11;
    public static float AccuracyPerAgilityGlobalMult = 2;
    public static float CritChanceGlobalMod = 0;
    public static float CritChancePerAgilityGlobalMult = 2;
    public static float DodgeGlobalMod = 0;
    public static float DodgePerAgilityGlobalMult = 2;
    public static float ParryPerAgilityGlobalMult = 3;
    public static float StepCostGlobalMod = 0;
    public static float InitiativeGlobalMod = 0;
    public static float InitiativePerAgilityGlobalMult = 2;
    public static float OffHandAccuracyPenaltyGlobal = 30; //Глобальный штраф точности для не ведущей руки (со второй по четвертую)
    public static float OffHandDamagePenaltyGlobal = 0.3f; //Глобальный штраф урона в процентах для не ведущей руки (со второй по четвертую)
    public static float ReduceOffHandAccuracyPenaltyPerAgilityGlobal = 2;
    public static float ReduceOffHandDamagePenaltyPerStrenghtGlobal = 0.02f;
    public static float OffHandParryPenaltyGlobal = 15; //Глобальный штраф парирования для не ведущей руки (со второй по четвертую)
    public static float ReduceOffHandParryPenaltyPerAgilityGlobal = 3;
    public static int BeltSlotGlobalMod = 2; //Базовое количество слотов на поясе
    public static int BeltSlotGlobalMax = 8; //Максимально возможное количество предметов на поясе.
    public int LeftStatPoints; // Оставшиеся нераспределенными очки характеристик.
    public Sprite CharacterSprite;
    public bool ControlledByPlayer; //Находится ли персонаж под контролем игрока.
    public int x, y;
    public int TurnOrderPosition;
    public GameObject CharacterToken;
    private ItemSetting _Armor;
    public ItemSetting[] Arms; //    RightArm, LeftArm, RightArmSecond, LeftArmSecond,
    public ItemSetting[] Belt;
    public int BattleSide; //Номер стороны в битве. Все под одним номером друг-другу союзники, по отношению к другим номерам - враги. Игрок всегда = 0.
    public int HexMovedInTurn; //Сколько клеток прошел за ход.
    public int AttackPerformedInTurn; //Сколько атак совершил за ход.
    public List<InjuryStruct> Injuries;
    public List<ConditionStruct> Conditions;
    public WalkerSetting Walker;
    
    /* Craft;
     XP
     Percs */

    public struct FloatStatList
    {
        public float Current; //
        public float Max; //
        public void SetBoth(float Value)
        {
            Current = Value;
            Max = Value;
        }
        public void SetMax(float Value)
        {
            Max = Value;
        }
        public void SetCurrent(float Value)
        {
            Current = Value;
        }
    }
    public struct ConditionStruct
    {
        public ConditionSetting Condition;
        public int Duration;
        public GlobalEnumerators.DurationMeasurementUnit DurationUnit;

        public void SetDuration(int Duration)
        {
            this.Duration = Duration;
        }
    }
    public int GetConditionIndex(ConditionSetting Condition)
    {
        for (int i = 0; i < Conditions.Count; i++)
        {
            if (Condition == Conditions[i].Condition)
            {
                return i;
            }
        }
        return -1;
    }
    public CharacterSetting(string name, float strenght, float agility, float endurance, RaceSetting race, WalkerSetting walker, int leftStatPoints = 0, bool controlledByPlayer = false)
    {
        Injuries = new List<InjuryStruct>();
        Conditions = new List<ConditionStruct>();
        Accuracy = new FloatStatList[4];
        Parry = new FloatStatList[4];
        Arms = new ItemSetting[4];
        Belt = new ItemSetting[BeltSlotGlobalMax];
        Name = name;
        Strenght.SetBoth(strenght);
        Agility.SetBoth(agility);
        Endurance.SetBoth(endurance);
        Race = race;
        Walker = walker; 
        CalculateAll();
        CalculateNumberOfBeltSlot();
        HP = HPMax;
        LeftStatPoints = leftStatPoints;
        CharacterSprite = Race.GetRandomSprite();
        ControlledByPlayer = controlledByPlayer;
        if (ControlledByPlayer)
        {
            BattleSide = 0;
        }
        else
        {
            BattleSide = 1;
        }
    }
    public void CalculateAll()
    {
        CalculateAllStats();
        CalculateAllCurrentDerived();
        
    }
    private void CalculateAllStats()
    {
        CalculateStrenght();
        CalculateAgility();
        CalculateEndurance();
        foreach (InjuryStruct Struct in Injuries)
        {
            Struct.Group.InjuryList[Struct.NumberInGroup].ApplyStatModify(this);
        }
    }
    private void CalculateStrenght()
    {
        Strenght.Current = Strenght.Max + Race.StrenghtBonus;
    }
    private void CalculateAgility()
    {
        Agility.Current = Agility.Max + Race.AgilityBonus;
    }
    private void CalculateEndurance()
    {
        Endurance.Current = Endurance.Max + Race.EnduranceBonus;
    }
    // Создание персонажа со случайными статами
    public static CharacterSetting CreateRandomCharacter(WalkerSetting Walker, int LeftStatPoints = 0, bool ControlledByPlayer = false) 
    {
        LeftStatPoints = Mathf.Clamp(LeftStatPoints, 0, 9);
        int[] StatArray = new int[3] {1, 1, 1};
        while (StatArray[0] + StatArray[1] + StatArray[2] < 12 - LeftStatPoints) //Добавляем единицу к случайному стату, пока их сумма не превышает 12 за вычетом свободных очков.
        {
            int StatNumber = Random.Range(0, 3);
            StatArray[StatNumber] = StatArray[StatNumber] < 7 ? StatArray[StatNumber] + 1 : 7; // единица добавляется пока значение стата не превышает 7.
        }
        RaceSetting Race = RaceObject.GetRandomRace();
        string Name = Race.GetRandomName();
        return new CharacterSetting(Name, StatArray[0], StatArray[1], StatArray[2], Race, Walker, LeftStatPoints, ControlledByPlayer);
    }
    public void StatUp(GlobalEnumerators.StatEnum Stat, int UpNumber = 1)
    {
        switch (Stat)
        {
            case GlobalEnumerators.StatEnum.Agility:
                Agility.Max += UpNumber;
                break;
            case GlobalEnumerators.StatEnum.Strenght:
                Strenght.Max += UpNumber;
                break;
            case GlobalEnumerators.StatEnum.Endurance:
                Endurance.Max += UpNumber;
                break;
        }
        CalculateAllStats();
        CalculateAllCurrentDerived();
    }
    public void CalculateAllBaseDerived()
    {
        HPMax = Race.HPBase + Race.HPEnduranceMult * Endurance.Current;
        SpeedMax = Agility.Current + SpeedGlobalMod;
        for (int i = 0; i < Accuracy.Length; i++)
        {
            if (i == 0)
                Accuracy[i].SetMax(Race.AccuracyBase + AccuracyPerAgilityGlobalMult * Agility.Current);
            else
                Accuracy[i].SetMax(Race.AccuracyBase + AccuracyPerAgilityGlobalMult * Agility.Current - Mathf.Max(0, OffHandAccuracyPenaltyGlobal - Agility.Current * ReduceOffHandAccuracyPenaltyPerAgilityGlobal));
        }
        /* AccuracyRight.SetMax(Race.AccuracyBase + AccuracyPerAgilityGlobalMult * Agility.Current);
        AccuracyLeft.SetMax(Race.AccuracyBase + AccuracyPerAgilityGlobalMult * Agility.Current - Mathf.Max(0, OffHandAccuracyPenaltyGlobal - Agility.Current * ReduceOffHandAccuracyPenaltyPerAgilityGlobal));
        AccuracyRightSecond.SetMax(Race.AccuracyBase + AccuracyPerAgilityGlobalMult * Agility.Current - Mathf.Max(0, OffHandAccuracyPenaltyGlobal - Agility.Current * ReduceOffHandAccuracyPenaltyPerAgilityGlobal));
        AccuracyLeftSecond.SetMax(Race.AccuracyBase + AccuracyPerAgilityGlobalMult * Agility.Current - Mathf.Max(0, OffHandAccuracyPenaltyGlobal - Agility.Current * ReduceOffHandAccuracyPenaltyPerAgilityGlobal));*/
        CritChance.SetMax(CritChanceGlobalMod + CritChancePerAgilityGlobalMult * Agility.Current);
        Dodge.SetMax(DodgeGlobalMod + Race.DodgeBase + DodgePerAgilityGlobalMult * Agility.Current);
        for (int i = 0; i < Parry.Length; i++)
        {
            if (i == 0)
                Parry[i].SetMax(Race.ParryBase + ParryPerAgilityGlobalMult * Agility.Current);
            else
                Parry[i].SetMax(Race.ParryBase + ParryPerAgilityGlobalMult * Agility.Current - Mathf.Max(0, OffHandParryPenaltyGlobal - Agility.Current * ReduceOffHandParryPenaltyPerAgilityGlobal));
        }
        /* ParryRight.SetMax(Race.ParryBase + ParryPerAgilityGlobalMult * Agility.Current);
        ParryLeft.SetMax(Race.ParryBase + ParryPerAgilityGlobalMult * Agility.Current - Mathf.Max(0, OffHandParryPenaltyGlobal - Agility.Current * ReduceOffHandParryPenaltyPerAgilityGlobal));
        ParryRightSecond.SetMax(Race.ParryBase + ParryPerAgilityGlobalMult * Agility.Current - Mathf.Max(0, OffHandParryPenaltyGlobal - Agility.Current * ReduceOffHandParryPenaltyPerAgilityGlobal));
        ParryLeftSecond.SetMax(Race.ParryBase + ParryPerAgilityGlobalMult * Agility.Current - Mathf.Max(0, OffHandParryPenaltyGlobal - Agility.Current * ReduceOffHandParryPenaltyPerAgilityGlobal));*/
        InjuryThreshold.SetMax(Race.InjuryThresholdBase + Race.InjuryThresholdEnduranceMult * Endurance.Current + Race.InjuryThresholdStrenghtMult * Strenght.Current);
        StepCost.SetMax(StepCostGlobalMod + Race.StepCostBase);
        Initiative.SetMax(InitiativeGlobalMod + Race.InitiativeBase + InitiativePerAgilityGlobalMult * Agility.Current);
        OffHandDamagePenalty.SetMax(Mathf.Max(0, OffHandDamagePenaltyGlobal -  ReduceOffHandDamagePenaltyPerStrenghtGlobal * Strenght.Current));
        AttackCost.Max = 0;
    }
    public void CalculateAllCurrentDerived()
    {
        CalculateAllBaseDerived();
        for (int i = 0; i < Accuracy.Length; i++)
            Accuracy[i].SetCurrent(Accuracy[i].Max);
        /* AccuracyRight.SetCurrent(AccuracyRight.Max);
        AccuracyLeft.SetCurrent(AccuracyLeft.Max);
        AccuracyRightSecond.SetCurrent(AccuracyRightSecond.Max);
        AccuracyLeftSecond.SetCurrent(AccuracyLeftSecond.Max);*/
        CritChance.SetCurrent(CritChance.Max);
        Dodge.SetCurrent(Dodge.Max);
        for (int i = 0; i < Parry.Length; i++)
            Parry[i].SetCurrent(Parry[i].Max);
        /* ParryRight.SetCurrent(ParryRight.Max);
        ParryLeft.SetCurrent(ParryLeft.Max);
        ParryRightSecond.SetCurrent(ParryRightSecond.Max);
        ParryLeftSecond.SetCurrent(ParryLeftSecond.Max);*/
        InjuryThreshold.SetCurrent(InjuryThreshold.Max);
        StepCost.SetCurrent(StepCost.Max);
        Initiative.SetCurrent(Initiative.Max);
        OffHandDamagePenalty.SetCurrent(OffHandDamagePenalty.Max);
        AttackCost.SetCurrent(AttackCost.Max);

        AddInjuryModify();
        AddArmorModify();
        AddConditionModify();
    }
    public void CalculateNumberOfBeltSlot()
    {
        int Temp = BeltSlotGlobalMod + Race.NumberOfSlotBase;
        if (Armor != null)
        {
            Temp += Armor.NumberOfBeltSlot;
        }
        NumberOfBeltSlot = Temp;
    }
    public int NumberOfBeltSlot
    {
        set
        {
            value = Mathf.Clamp(value, 0, BeltSlotGlobalMax);
            _NumberOfBeltSlot = value;
            for (int i = value; i < BeltSlotGlobalMax; i++)
            {
                if (Belt[i] != null)
                {
                    if (Walker != null)
                    {
                        Walker.Inventory.Add(Belt[i]);
                    }
                    Belt[i] = null;
                }
            }
        }
        get => _NumberOfBeltSlot;
    }
    public ItemSetting Armor
    {
        set
        {
            _Armor = value;
            CalculateNumberOfBeltSlot();
        }
        get => _Armor;
    }
    private void AddInjuryModify()
    {
        foreach (InjuryStruct Struct in Injuries)
        {
            Struct.Group.InjuryList[Struct.NumberInGroup].ApplyDerivedModify(this);
        }
    }
    private void AddArmorModify()
    {
        if (Armor != null)
        {
            Armor.ApplyDerivedModify(this);
        }
    }
    private void AddConditionModify()
    {
        foreach (ConditionStruct Struct in Conditions)
        {
            Struct.Condition.ApplyDerivedModify(this);
        }
    }
    private void AddInjuryStatModify()
    {
        foreach (InjuryStruct Struct in Injuries)
        {
            Struct.Group.InjuryList[Struct.NumberInGroup].ApplyStatModify(this);
        }
    }
    private void AddArmorStatModify()
    {
        if (Armor != null)
        {
            Armor.ApplyStatModify(this);
        }
    }
    private void AddConditionStatModify()
    {
        foreach (ConditionStruct Struct in Conditions)
        {
            Struct.Condition.ApplyStatModify(this);
        }
    }
    public float HP
    {
        set
        {
            _HP.Current = Mathf.Clamp(value, 0, HPMax);
            if (_HP.Current == 0 && MainBattleScript.TacticalMapIsActive)
            {
                MainBattleScript.TurnOrder.RemoveAt(TurnOrderPosition);
                MainBattleScript.RemoveCharacterFromMapCell(this);
                MainBattleScript.CorrectingPosition();
                MainBattleScript.FillInitiativeOrderPrefabButton();
                if (MainBattleScript.AllControledByPlayerCharactersDie())
                {
                    MainBattleScript.EndBattleCriterion = true;
                }
            }
        }
        get => _HP.Current;
    }
    public float HPMax
    {
        set
        {
            float Percentage = _HP.Current / _HP.Max;
            _HP.Max = value;
            _HP.Current = _HP.Max * Percentage;
        }
        get => _HP.Max;
    }
    public float GetNewInitiative()
    {
        return Random.Range(0, Initiative.Current + 1);
    }
    public void CreateCharacterToken(Tilemap map)
    {
        this.CharacterToken = (GameObject)GameObject.Instantiate(Resources.Load("CharacterTokenPrefab"), map.transform);
        CharacterToken.transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite = CharacterSprite;
        if (BattleSide != 0)
        {
            CharacterToken.transform.Find("Sprite").GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 1);
        }
        else
        {
            CharacterToken.transform.Find("Sprite").GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }
    }
    public float SpeedCurrent
    {
        set
        {
            _Speed.Current = value;
            if (MainBattleScript.TacticalMapIsActive)
            {
                if (MainBattleScript.ActiveC == this)
                {
                    MainBattleScript.ResetAllAbilitiesEnabilities();
                }
            }
        }
        get => _Speed.Current;
    }
    public float SpeedMax
    {
        set
        {
            _Speed.Max = value;
            if (SpeedCurrent > value)
            {
                SpeedCurrent = value;                
            }
        }
        get => _Speed.Max;
    }
    public void ResetStatistic()
    {
        HexMovedInTurn = 0;
        AttackPerformedInTurn = 0;
    }
    public struct InjuryStruct
    {
        public InjuryGroup Group;
        private int _NumberInGroup;
        
        public int NumberInGroup 
        {
            set
            {
                if (Group != null)
                    _NumberInGroup = Mathf.Clamp(value, 0, Group.InjuryList.Count -1);
                else
                    _NumberInGroup = value;
            }
            get => _NumberInGroup;
        }
        public void SetNumberInGroup(int Number)
        {
            NumberInGroup = Number;
        }
    }
    public float GetAccuracyFromItemSlot(GlobalEnumerators.ItemSlot ItemSlot)
    {
        switch (ItemSlot)
        {
            case GlobalEnumerators.ItemSlot.RightArm:
                return Accuracy[0].Current;
            case GlobalEnumerators.ItemSlot.LeftArm:
                return Accuracy[1].Current;
            case GlobalEnumerators.ItemSlot.RightArmSecond:
                return Accuracy[2].Current;
            case GlobalEnumerators.ItemSlot.LeftArmSecond:
                return Accuracy[3].Current;
            default:
                return 0;
        }
    }
    public ItemSetting GetWeapon(int ArmNumber)
    {
        if (Arms[ArmNumber] != null)
            return Arms[ArmNumber];
        else
            return Race.Fist;
    }
    public float GetRandomDamage(int ArmNumber)
    {
        ItemSetting Weapon = GetWeapon(ArmNumber);
        float DamageMin = Weapon.DamageMin;
        float DamageMax = Weapon.DamageMax;
        float SMult = Weapon.DamageStrenghtMult;
        float Damage = Random.Range(DamageMin, DamageMax) + SMult * Strenght.Current;
        if (ArmNumber != 0)
        {
            Damage = Damage * (1 - OffHandDamagePenalty.Current);
        }
        return Damage;
    }
    public static int ArmFromItemSlot(GlobalEnumerators.ItemSlot ItemSlot) => ItemSlot switch
    {
        GlobalEnumerators.ItemSlot.RightArm => 0,
        GlobalEnumerators.ItemSlot.LeftArm => 1,
        GlobalEnumerators.ItemSlot.RightArmSecond => 2,
        GlobalEnumerators.ItemSlot.LeftArmSecond => 3,
        _ => 0,
    };
    public ItemSetting GetItemFromSlot(GlobalEnumerators.ItemSlot ItemSlot, int SlotNumber = 0)
    {
        switch (ItemSlot)
        {
            case GlobalEnumerators.ItemSlot.RightArm:
                return Arms[0];
            case GlobalEnumerators.ItemSlot.LeftArm:
                return Arms[1];
            case GlobalEnumerators.ItemSlot.RightArmSecond:
                return Arms[2];
            case GlobalEnumerators.ItemSlot.LeftArmSecond:
                return Arms[3];
            case GlobalEnumerators.ItemSlot.Armor:
                return Armor;
            case GlobalEnumerators.ItemSlot.Belt:
                return Belt[SlotNumber];
            default:
                return null;
        }
    }
    public static GlobalEnumerators.ItemSlot ItemSlotFromArm(int Arm) => Arm switch
    {
        0 => GlobalEnumerators.ItemSlot.RightArm,
        1 => GlobalEnumerators.ItemSlot.LeftArm,
        2 => GlobalEnumerators.ItemSlot.RightArmSecond,
        3 => GlobalEnumerators.ItemSlot.LeftArmSecond,
        _ => 0,
    };
    public void ReduceAllConditionDuration(GlobalEnumerators.DurationMeasurementUnit Unit, int ReduceValue)
    {
        for (int i = 0; i < Conditions.Count; i++)
        {
            Debug.Log(Conditions[i].Condition.Name);
            Debug.Log(Conditions[i].DurationUnit.ToString());
            Debug.Log(Conditions[i].Duration.ToString());
            if (Conditions[i].DurationUnit < Unit)
            {
                ConditionStruct NewStruct = new ConditionStruct();
                NewStruct = Conditions[i];
                NewStruct.SetDuration(0);
                Conditions[i] = NewStruct;
            }
            if (Conditions[i].DurationUnit == Unit)
            {
                ConditionStruct NewStruct = new ConditionStruct();
                NewStruct = Conditions[i];
                NewStruct.SetDuration(Conditions[i].Duration - ReduceValue);
                Conditions[i] = NewStruct;
            }
            Debug.Log(Conditions[i].Condition.Name);
            Debug.Log(Conditions[i].DurationUnit.ToString());
            Debug.Log(Conditions[i].Duration.ToString());
            if (Conditions[i].Duration < 1)
            {
                Conditions.RemoveAt(i);
                i--;
            }
        }
    }
    public void AddCondition(ConditionSetting Condition, GlobalEnumerators.DurationMeasurementUnit Unit, int Duration, bool Replace)
    {
        ConditionStruct Struct = new ConditionStruct();
        if (Replace)
        {
            int Index = GetConditionIndex(Condition);
            if (Index != -1)
            {
                if (Conditions[Index].DurationUnit < Unit || (Conditions[Index].DurationUnit == Unit && Conditions[Index].Duration < Duration))
                {
                    Struct.Condition = Condition;
                    Struct.DurationUnit = Unit;
                    Struct.Duration = Duration;
                    Conditions[Index] = Struct;
                    return;
                }
            }
        }
        Struct.Condition = Condition;
        Struct.DurationUnit = Unit;
        Struct.Duration = Duration;
        Conditions.Add(Struct);
    }
    public int RemoveCondition(ConditionSetting Condition, bool All = true, bool GreatestDuration = true, int NumberOfConditions = 1)
    {
        int Removed = 0;
        if (All)
        {
            for (int i = 0; i < Conditions.Count; i++)
            {
                if (Conditions[i].Condition == Condition)
                {
                    Conditions.RemoveAt(i);
                    i--;
                    Removed++;
                }
            }
        }
        return Removed;
    }
    public float ApplyBleed(float A) // Процедура кровотечения
    {
        float Rounded = MainBattleScript.ProbabilisticRounding(A);
        HP -= Rounded;
        return Rounded;
    }
}
