using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class AbilitySetting : object
{
    public delegate void AbilityProcedureDelegate(CharacterSetting A, Vector3Int B, MainBattleScript.ActiveAbilityStruct C);
    public delegate string AbilityTooltipProcedureDelegate(CharacterSetting A, GameObject B);
    public string Name;    
    public GlobalEnumerators.WeaponTypeEnum[] WeaponInclude, WeaponExclude;
    public Sprite Icon, IconLocked; //Базовая иконка, иконка недоступной способности
    public AbilityProcedureDelegate AbilityProcedure;
    public AbilityTooltipProcedureDelegate AbilityTooltipProcedure;
    public int RangeMin, RangeMax;
    public int SpeedCost;
    public GlobalEnumerators.AbilityTriggerType TriggerType;
    public GlobalEnumerators.DamageType DamageType;
    public bool UseItemIcon; //Способность использует иконку предмета
    public bool CostlyReuse = true; //Последующее использование способности дороже предыдущего.
    // Максимальный перепад высот

    public bool CheckWeaponType(GlobalEnumerators.WeaponTypeEnum WeaponType)
    {
        if (WeaponExclude.Length == 0)
        {
            if (WeaponInclude.Length == 0)
            {
                return true;
            }
            foreach (GlobalEnumerators.WeaponTypeEnum Type in WeaponInclude)
            {
                if (WeaponType == Type)
                {
                    return true;
                }
            }
            return false;
        }
        else
        {
            foreach (GlobalEnumerators.WeaponTypeEnum Type in WeaponExclude)
            {
                if (WeaponType == Type)
                {
                    return false;
                }
            }
            return true;
        }
    }
}

public static class AbilityObjects : object
{
    public static List<AbilitySetting> AbilityList;
    public static AbilitySetting RegularUnarmedAttack;
    public static AbilitySetting ImprovedUnarmedAttack;
    public static AbilitySetting RegularSlash;
    public static AbilitySetting RegularBlunt;
    public static AbilitySetting StunAttack;
    public static AbilitySetting SwitchItemActivity;

    static AbilityObjects()
    {
        AbilityList = new List<AbilitySetting>();
        // Инициализация обычной атаки без оружия
        {
            GlobalEnumerators.WeaponTypeEnum[] RegularUnarmedAttackWeaponInclude = new GlobalEnumerators.WeaponTypeEnum[1];
            RegularUnarmedAttackWeaponInclude[0] = GlobalEnumerators.WeaponTypeEnum.Unarmed;
            RegularUnarmedAttack = new AbilitySetting();
            RegularUnarmedAttack.Name = "Хук";
            RegularUnarmedAttack.WeaponInclude = RegularUnarmedAttackWeaponInclude;
            RegularUnarmedAttack.AbilityProcedure = AbilityProcedure.RegularUnarmedAttackProcedure;
            RegularUnarmedAttack.AbilityTooltipProcedure = AbilityProcedure.BaseAttackAbilityTooltip;
            RegularUnarmedAttack.Icon = Resources.Load<Sprite>("AbilityIcon/UnarmedAttack");
            RegularUnarmedAttack.IconLocked = Resources.Load<Sprite>("AbilityIcon/UnarmedAttackLocked");
            RegularUnarmedAttack.TriggerType = GlobalEnumerators.AbilityTriggerType.Attack;
            RegularUnarmedAttack.SpeedCost = 0;
            RegularUnarmedAttack.RangeMin = 0;
            RegularUnarmedAttack.RangeMax = 0;
            RegularUnarmedAttack.DamageType = GlobalEnumerators.DamageType.None;

            AbilityList.Add(RegularUnarmedAttack);
        }
        // Инициализация улучшенной атаки без оружия
        {
            GlobalEnumerators.WeaponTypeEnum[] ImprovedUnarmedAttackWeaponInclude = new GlobalEnumerators.WeaponTypeEnum[1];
            ImprovedUnarmedAttackWeaponInclude[0] = GlobalEnumerators.WeaponTypeEnum.Unarmed;
            ImprovedUnarmedAttack = new AbilitySetting();
            ImprovedUnarmedAttack.Name = "Апперкот";
            ImprovedUnarmedAttack.WeaponInclude = ImprovedUnarmedAttackWeaponInclude;
            ImprovedUnarmedAttack.AbilityProcedure = AbilityProcedure.ImprovedUnarmedAttackProcedure;
            ImprovedUnarmedAttack.AbilityTooltipProcedure = AbilityProcedure.BaseAttackAbilityTooltip;
            ImprovedUnarmedAttack.Icon = Resources.Load<Sprite>("AbilityIcon/ImprovedUnarmedAttack");
            ImprovedUnarmedAttack.IconLocked = Resources.Load<Sprite>("AbilityIcon/ImprovedUnarmedAttackLocked");
            ImprovedUnarmedAttack.TriggerType = GlobalEnumerators.AbilityTriggerType.Attack;
            ImprovedUnarmedAttack.SpeedCost = 1;
            ImprovedUnarmedAttack.RangeMin = 0;
            ImprovedUnarmedAttack.RangeMax = 0;
            ImprovedUnarmedAttack.DamageType = GlobalEnumerators.DamageType.None;

            AbilityList.Add(RegularUnarmedAttack);
        }
        // Инициализация обычной атаки клинкового оружия
        {
            GlobalEnumerators.WeaponTypeEnum[] RegularSlashWeaponInclude = new GlobalEnumerators.WeaponTypeEnum[3];
            RegularSlashWeaponInclude[0] = GlobalEnumerators.WeaponTypeEnum.Shiv;
            RegularSlashWeaponInclude[1] = GlobalEnumerators.WeaponTypeEnum.Sword;
            RegularSlashWeaponInclude[2] = GlobalEnumerators.WeaponTypeEnum.Saber;
            RegularSlash = new AbilitySetting();
            RegularSlash.Name = "Атака";
            RegularSlash.WeaponInclude = RegularSlashWeaponInclude;
            RegularSlash.AbilityProcedure = AbilityProcedure.RegularSlashProcedure;
            RegularSlash.AbilityTooltipProcedure = AbilityProcedure.BaseAttackAbilityTooltip;
            RegularSlash.Icon = Resources.Load<Sprite>("AbilityIcon/SlashAttack");
            RegularSlash.IconLocked = Resources.Load<Sprite>("AbilityIcon/SlashAttackLocked");
            RegularSlash.TriggerType = GlobalEnumerators.AbilityTriggerType.Attack;
            RegularSlash.SpeedCost = 0;
            RegularSlash.RangeMin = 0;
            RegularSlash.RangeMax = 0;
            RegularSlash.DamageType = GlobalEnumerators.DamageType.Slash;

            AbilityList.Add(RegularSlash);
        }
        // Инициализация обычной атаки тупого оружия
        {
            GlobalEnumerators.WeaponTypeEnum[] RegularBluntWeaponInclude = new GlobalEnumerators.WeaponTypeEnum[1];
            RegularBluntWeaponInclude[0] = GlobalEnumerators.WeaponTypeEnum.Sledgehammer;
            RegularBlunt = new AbilitySetting();
            RegularBlunt.Name = "Атака";
            RegularBlunt.WeaponInclude = RegularBluntWeaponInclude;
            RegularBlunt.AbilityProcedure = AbilityProcedure.RegularBluntProcedure;
            RegularBlunt.AbilityTooltipProcedure = AbilityProcedure.BaseAttackAbilityTooltip;
            RegularBlunt.Icon = Resources.Load<Sprite>("AbilityIcon/BluntAttack");
            RegularBlunt.IconLocked = Resources.Load<Sprite>("AbilityIcon/BluntAttackLocked");
            RegularBlunt.TriggerType = GlobalEnumerators.AbilityTriggerType.Attack;
            RegularBlunt.SpeedCost = 0;
            RegularBlunt.RangeMin = 0;
            RegularBlunt.RangeMax = 0;
            RegularBlunt.DamageType = GlobalEnumerators.DamageType.Blunt;

            AbilityList.Add(RegularBlunt);
        }
        // Инициализация оглушающей атаки
        {
            StunAttack = new AbilitySetting();
            StunAttack.Name = "Оглушающая атака";
            StunAttack.AbilityProcedure = AbilityProcedure.StunAttackProcedure;
            StunAttack.AbilityTooltipProcedure = AbilityProcedure.BaseAttackAbilityTooltip;
            StunAttack.Icon = Resources.Load<Sprite>("AbilityIcon/StunAttack");
            StunAttack.IconLocked = Resources.Load<Sprite>("AbilityIcon/StunAttackLocked");
            StunAttack.TriggerType = GlobalEnumerators.AbilityTriggerType.Attack;
            StunAttack.SpeedCost = 2;
            StunAttack.RangeMin = 0;
            StunAttack.RangeMax = 0;
            StunAttack.DamageType = GlobalEnumerators.DamageType.Blunt;

            AbilityList.Add(StunAttack);
        }
        
        {
            SwitchItemActivity = new AbilitySetting();
            SwitchItemActivity.Name = "";
            SwitchItemActivity.AbilityProcedure = AbilityProcedure.ActivateItemAbilityProcedure;
            SwitchItemActivity.AbilityTooltipProcedure = AbilityProcedure.ActivateItemAbilityTooltip;
            SwitchItemActivity.TriggerType = GlobalEnumerators.AbilityTriggerType.Instance;
            SwitchItemActivity.UseItemIcon = true;
            SwitchItemActivity.CostlyReuse = false;

            AbilityList.Add(SwitchItemActivity);
        }
    }
}
public static class AbilityProcedure : object
{
    public static void RegularUnarmedAttackProcedure(CharacterSetting C, Vector3Int Cell, MainBattleScript.ActiveAbilityStruct AbilityStruct)
    {
        Debug.Log("Атака");
        MainBattleScript.AttackParam Param = new MainBattleScript.AttackParam(C, MainBattleScript.GetMapCellCharacter(Cell.x, Cell.y), AbilityStruct);
        MainBattleScript.Attack(Param);
    }
    public static void ImprovedUnarmedAttackProcedure(CharacterSetting C, Vector3Int Cell, MainBattleScript.ActiveAbilityStruct AbilityStruct)
    {
        Debug.Log("Атака");
        MainBattleScript.AttackParam Param = new MainBattleScript.AttackParam(C, MainBattleScript.GetMapCellCharacter(Cell.x, Cell.y), AbilityStruct);
        MainBattleScript.Attack(Param);
    }
    public static void RegularSlashProcedure(CharacterSetting C, Vector3Int Cell, MainBattleScript.ActiveAbilityStruct AbilityStruct)
    {
        Debug.Log("Атака");
        MainBattleScript.AttackParam Param = new MainBattleScript.AttackParam(C, MainBattleScript.GetMapCellCharacter(Cell.x, Cell.y), AbilityStruct);
        MainBattleScript.Attack(Param);
    }
    public static void RegularBluntProcedure(CharacterSetting C, Vector3Int Cell, MainBattleScript.ActiveAbilityStruct AbilityStruct)
    {
        Debug.Log("Атака");
        MainBattleScript.AttackParam Param = new MainBattleScript.AttackParam(C, MainBattleScript.GetMapCellCharacter(Cell.x, Cell.y), AbilityStruct);
        MainBattleScript.Attack(Param);
    }
    public static void StunAttackProcedure(CharacterSetting C, Vector3Int Cell, MainBattleScript.ActiveAbilityStruct AbilityStruct)
    {
        Debug.Log("Атака");
        MainBattleScript.AttackParam Param = new MainBattleScript.AttackParam(C, MainBattleScript.GetMapCellCharacter(Cell.x, Cell.y), AbilityStruct);
        MainBattleScript.Attack(Param);
        CharacterSetting.ConditionStruct Stun = new CharacterSetting.ConditionStruct();
        Stun.Condition = ConditionObject.Stun;
        Stun.DurationUnit = GlobalEnumerators.DurationMeasurementUnit.Turn;
        Stun.Duration = 1;
        MainBattleScript.GetMapCellCharacter(Cell.x, Cell.y).Conditions.Add(Stun);
    }
    public static string BaseAttackAbilityTooltip(CharacterSetting C, GameObject AbilityButton)
    {
        string TooltipText;
        TooltipText = AbilityButton.GetComponent<AbilityButtonPrefabScript>().Ability.Name + "\n";
        TooltipText += "Скорость: " + AbilityButton.GetComponent<AbilityButtonPrefabScript>().TotalSpeedCost.ToString() + "\n";
        ItemSetting Weapon = C.GetWeapon(AbilityButton.GetComponent<AbilityButtonPrefabScript>().ArmNumber);
        int MinDamage = Mathf.FloorToInt(Weapon.DamageMin + Weapon.DamageStrenghtMult * C.Strenght.Current);
        int MaxDamage = Mathf.CeilToInt(Weapon.DamageMax + Weapon.DamageStrenghtMult * C.Strenght.Current);
        string DamageType = AbilityButton.GetComponent<AbilityButtonPrefabScript>().Ability.DamageType == GlobalEnumerators.DamageType.None ? ItemSetting.DamageTypeToString(Weapon.DamageType) : ItemSetting.DamageTypeToString(AbilityButton.GetComponent<AbilityButtonPrefabScript>().Ability.DamageType);
        TooltipText += "Урон: " + MinDamage.ToString() + " - " + MaxDamage.ToString() + " (" + DamageType + ")\n";
        return TooltipText;
    }
    public static string ActivateItemAbilityTooltip(CharacterSetting C, GameObject AbilityButton)
    {
        string TooltipText;
        TooltipText = AbilityButton.GetComponent<AbilityButtonPrefabScript>().Item.Name;
        TooltipText += AbilityButton.GetComponent<AbilityButtonPrefabScript>().Item.GetActivation()? ": Выключить" : ": Включить";
        return TooltipText;
    }
    public static void ActivateItemAbilityProcedure(CharacterSetting C, Vector3Int Cell, MainBattleScript.ActiveAbilityStruct AbilityStruct)
    {
        Debug.Log("Способность активирована");
        if (AbilityStruct.Item != null)
        {
            Debug.Log("Предмет не нал");
            AbilityStruct.Item.SetActivation(C, !AbilityStruct.Item.GetActivation());
        }
    }
}