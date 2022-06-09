using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalEnumerators : object
{
    public enum StatEnum {Strenght, Agility, Endurance};
    public enum AreaTypeEnum {Swamp, Jungle, Desert};
    public enum SityTypeEnum {SitySmall};
    public enum WalkerTypeEnum {TurtleYoung, TurtleMiddle, TurtleOld};
    public enum AgeStageEnum {Young = 1, Middle = 2, Old = 3};
    public enum RaceTypeEnum {Kwabroth =1, Blamk, Moss};
    public enum ItemTypeEnum {Weapon, Armor, Belt };
    public enum ItemTag { Battery };
    public enum ExperienceTypeEnum {Bladed, Armor, Dodge, Fortitude};
    public enum WeaponTypeEnum {Unarmed, Shiv, Sword, Saber, Sledgehammer};
    public enum AbilityTriggerType {Attack, Move, OpenArea, AnyCharacter, Instance};
    public enum ItemSlot {None, RightArm, LeftArm, RightArmSecond, LeftArmSecond, Armor, Belt, Inventory };
    public enum DamageType {None = -1, Slash = 0, Blunt, Pierce, Fire, Energy, Frost, Aсid};
    public enum PerkType {None, Weapon };
    public enum DurationMeasurementUnit {Turn = 0, Hour, Day, Endless};


    public enum WalkerNameEnum { };
    public static List<string> AreaNameUniversalEnum = new List<string> { "Чужая земля", "Нет мух", "Обещанный рай", "Не вдыхай туман", "Колыбель" };
    public static List<string> AreaNameSwampEnum = new List <string> { "Густая вода", "Утонувшее солнце", "Кружка чистой воды", " Шрамы на воде", "Кочки", "Сухие клочки", "Купальни бога" };
    public static List<string> AreaNameJungleEnum = new List<string> { "Топь под сетью", "Затмение", "Палки-листья", "Скошенный лес", "Крючья", "Бурелом" };
    public static List<string> AreaNameDesertEnum = new List<string> { "t", "t" };
    
    public enum SityNameEnum { };
    public static List<string> SityNameUniversalEnum = new List<string> { "Точка на карте", "Место посадки", "Вторая столица", "Еще один дом", "Выселки" };
    public static List<string> SityNameSitySmallEnum = new List<string> { "Точка на карте", "Место посадки", "Вторая столица", "Еще один дом", "Выселки" };
    public static List<string> KwabrothRaceNameEnum = new List<string> {"Кваброт Первый", "Второй кваброт", "Рум-рум", "Все пальцы" };
    public static List<string> BlamkRaceNameEnum = new List<string> { "Бламк Первый", "Бламки", "Молчун" };
    public static List<string> MossRaceNameEnum = new List<string> { "Гнет-шелест", "Чуть-слышно", "Прут-хруст", "Шух-цветок", "Листья-капли" };
}
