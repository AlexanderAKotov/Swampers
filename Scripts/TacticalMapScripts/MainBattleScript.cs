using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
public static class MainBattleScript : object
{

    private static CharacterSetting _ActiveC; //Персонаж, чей сейчас ход
    private static CharacterSetting _SelectedC; //Персонаж, выбранный для отображения статов на панели 
    public static List <TurnOrderStruct> TurnOrder;
    public static List<GameObject> InitiativeOrderButtons;
    public static GameObject EndRoundTextImage;
    
    public static CellMapStruct[,] _Map;
    public static List<GameObject>[] AbilityButtons;
    /*public static List<GameObject> RightArmAbilityButtons;
    public static List<GameObject> LeftArmAbilityButtons;
    public static List<GameObject> RightArmSecondAbilityButtons;
    public static List<GameObject> LeftArmSecondAbilityButtons;
    public static List<GameObject> OtherAbilityButtons; */
    public static ActiveAbilityStruct ActiveAbility;
    public static bool TacticalMapIsActive = false;
    public static bool EndTurnCriterion = false;
    public static bool EndBattleCriterion = false;
    public static int MapWidth, MapHeight;

    public class CellMapStruct
    {
        public CharacterSetting C;
        public TileSetting Property;
        
    }
    public struct TurnOrderStruct
    {
        private CharacterSetting _C;
        public float CurrentInitiative;
        private bool _Ready;
        public int _NumberInTurnOrder;
        public List<AnimationPathStruct> AnimationPath;

        public int NumberInTurnOrder
        {
            set
            {
                _NumberInTurnOrder = value;
                if (C != null)
                {
                    C.TurnOrderPosition = value;
                }
            }
            get => _NumberInTurnOrder;
        }

        public void ResetInitiative()
        {
            //CurrentInitiative = Random.Range(0, C.Initiative.Current + 1);
            CurrentInitiative = C.GetNewInitiative();
        }
        public void SetBasedOnStruct(TurnOrderStruct BaseStruct)//Меняет поля на основе структуры из аргумента
        {
            this.C = BaseStruct.C;
            this.CurrentInitiative = BaseStruct.CurrentInitiative;
            this.Ready = BaseStruct.Ready;
            this.NumberInTurnOrder = BaseStruct.NumberInTurnOrder;
            this.AnimationPath = BaseStruct.AnimationPath;
        }
        public CharacterSetting C
        {
            set
            {
                _C = value;
                _C.TurnOrderPosition = NumberInTurnOrder;
            }
            get => _C;
        }
        public bool Ready
        {
            set 
            {
                _Ready = value;
            }
            get => _Ready;
        }
    }
    public struct AnimationPathStruct
    {
        public Vector3 Point;
        public float AnimationSpeed;

        public AnimationPathStruct(Vector3 point, float animationSpeed)
        {
            Point = point;
            AnimationSpeed = animationSpeed;
        }
    }
    public static void TurnOrderSetUp(CharacterSetting[] Characters)
    {
        MapWidth = 11;
        MapHeight = 11;
        _Map = new CellMapStruct[MapWidth, MapHeight];
        int j;
        for (int i = 0; i < MapWidth; i++)
        {
            for (j = 0; j < MapHeight; j++)
            {
                Debug.Log(MainBattleBehaviour.ThisScript.map.GetTile(new Vector3Int(i, j, 0)).name);
                _Map[i, j] = new CellMapStruct();
                _Map[i, j].Property = WorkWithTile.TileProperty[MainBattleBehaviour.ThisScript.map.GetTile(new Vector3Int(i, j, 0))];
            }
        }
        EndRoundTextImage = GameObject.Find("EndRoundTextImage");
        SetActiveAbility(null);

        //OtherAbilityButtons = new List<GameObject>();
        TurnOrder = new List<TurnOrderStruct>();
        j = 0;
        foreach (CharacterSetting C in Characters)
        {
            TurnOrderStruct CurrentStruct = new TurnOrderStruct();
            CurrentStruct.C = C;
            CurrentStruct.NumberInTurnOrder = j;
            CurrentStruct.AnimationPath = new List<AnimationPathStruct>();
            TurnOrder.Add(CurrentStruct);
            C.CreateCharacterToken(MainBattleBehaviour.ThisScript.map);
            bool CStanding = false;
            while (!CStanding)
            {
                int x = Random.Range(0, MapWidth);
                int y = Random.Range(0, MapHeight);
                if (_Map[x, y].Property.AllowStanding)
                {
                    CStanding = SetCharacterOnMapCell(x, y, C, 50);
                }
            }
            j++;
        }
        SetSelectedCharacterIndex(0);
        Debug.Log("Конец turnorder");
        SetNewRound();
    }
    
    public static void SetNewRound()
    {
        SetNewRoundInitiative();
        FillInitiativeOrderPrefabButton();
        SetCurrentSpeedToMax();
        SetFirstReadyAsActive();
    }
    public static void SetNewRoundInitiative()
    {
        for  (int i = 0; i < TurnOrder.Count; i++)
        {
            TurnOrderStruct CurrentStruct = new TurnOrderStruct();
            CurrentStruct = TurnOrder[i];
            CurrentStruct.ResetInitiative();
            CurrentStruct.Ready = true;
            TurnOrder[i] = CurrentStruct;
        }
        SortTurnOrderStruct();
        //TurnOrder.Sort((p1, p2) => p1.CurrentInitiative.CompareTo(p2.CurrentInitiative));
    }
    public static  void SortTurnOrderStruct()
    {
        for (int i = 0; i < TurnOrder.Count; i++)
        {
            for (int j = 0; j<TurnOrder.Count - i-1; j++)
            {
                if (TurnOrder[j].CurrentInitiative<TurnOrder[j+1].CurrentInitiative)
                {
                    TurnOrderStruct Temp = new TurnOrderStruct();
                    Temp.SetBasedOnStruct(TurnOrder[j + 1]);
                    Temp.NumberInTurnOrder = j;
                    TurnOrderStruct NewStruct = new TurnOrderStruct();
                    NewStruct.SetBasedOnStruct(TurnOrder[j]);
                    NewStruct.NumberInTurnOrder = j + 1;
                    TurnOrder[j + 1] = NewStruct;
                    TurnOrder[j] = Temp;
                }
            }
        }
    }
    public static void SetSelectedCharacterIndex(int CharacterNumber)
    {
        SelectedCharacter = TurnOrder[CharacterNumber].C;
    }
    public static CharacterSetting SelectedCharacter
    {
        set
        {
            _SelectedC = value;
            //TacticalMenu.PrintInfo
            GameObject.Find("Main Camera").GetComponent<MainBattleBehaviour>().PrintInfo(value);
        }
        get => _SelectedC;
    }
    public static void FillInitiativeOrderPrefabButton()
    {
        ZeroizeInitiativeOrderPrefabButton();
        int j = 0;
        for (int i = 0; i < TurnOrder.Count; i++)
        {
            if (j >= InitiativeOrderButtons.Count)
            {
                break;
            }
            else
            {
                if (TurnOrder[i].Ready)
                {
                    InitiativeOrderButtons[j].GetComponent<InitiativeOrderPrefabScript>().CharacterNumber = i;
                    j++;
                }
            }
        }
        if (j < InitiativeOrderButtons.Count) //Отображение метки надвигающегося конца раунда.
        {
            float PanelHeight = GameObject.Find("InitiativeOrderPanel").GetComponent<RectTransform>().rect.height;
            float EndRoundTextImageHeight = EndRoundTextImage.GetComponent<RectTransform>().rect.height;
            EndRoundTextImage.SetActive(true);
            EndRoundTextImage.transform.localPosition = new Vector3(0, (j - 1) * MainBattleBehaviour.InitiativeOrderButtonSide + j * MainBattleBehaviour.InitiativeOrderButtonMargin - PanelHeight / 2 + EndRoundTextImageHeight / 2, 0);
        }
        else
        {
            EndRoundTextImage.SetActive(false);
        }
    }
    public static void ZeroizeInitiativeOrderPrefabButton()
    {
        foreach (GameObject InitiativeButton in InitiativeOrderButtons)
        {
            InitiativeButton.GetComponent<InitiativeOrderPrefabScript>().CharacterNumber = -1;
        }
    }
    public static void SetCurrentSpeedToMax()
    {
        foreach (TurnOrderStruct CurrentStruct in TurnOrder)
        {
            CurrentStruct.C.SpeedCurrent = CurrentStruct.C.SpeedMax;
        }
    }
    public static bool SetCharacterOnMapCell(int x, int y, CharacterSetting C, float Speed)// установка персонажа в клетку карты. Speed - скорость анимации установки персонажа.
    {
        if (GetMapCellCharacter(x, y) != null) // Если клетка занята возвращает ложь.
        {
            return false;
        }
        else  // Если не занята, устанавливаем персонажа и возвращаем истину.
        {
            _Map[(int)C.x, (int)C.y].C = null;
            _Map[x, y].C = C;
            C.x = x;
            C.y = y;
            TurnOrder[C.TurnOrderPosition].AnimationPath.Add(new AnimationPathStruct(MainBattleBehaviour.ThisScript.map.CellToWorld(new Vector3Int(x, y, 0)), Speed));
            //C.CharacterToken.transform.position = MainBattleBehaviour.ThisScript.map.CellToWorld(new Vector3Int (x, y, 0)); //Старая ходьба - телепортация на целевую клетку
            return true;
        }
    }
    public static CharacterSetting GetMapCellCharacter(int x, int y)
    {
        return _Map[x, y].C;
    }
    public static void EndTurn()
    {
        if (!EndBattleCriterion)
        {
            foreach (CharacterSetting.InjuryStruct Struct in ActiveC.Injuries)
            {
                Struct.Group.InjuryList[Struct.NumberInGroup].TurnEndProcedure(ActiveC);
            }
            ActiveC.ReduceAllConditionDuration(GlobalEnumerators.DurationMeasurementUnit.Turn, 1);
            ActiveC.CalculateAllCurrentDerived();
            TurnOrderStruct NewStruct = new TurnOrderStruct();
            NewStruct.SetBasedOnStruct(TurnOrder[ActiveC.TurnOrderPosition]);
            NewStruct.Ready = false;
            TurnOrder[ActiveC.TurnOrderPosition] = NewStruct;
            if (ExistReadyCharacter())
            {
                FillInitiativeOrderPrefabButton();
                SetFirstReadyAsActive();
            }
            else
            {
                SetNewRound();
            }
        }
    }
    public static bool ExistReadyCharacter()
    {
        foreach (TurnOrderStruct CurrentStruct in TurnOrder)
        {
            if (CurrentStruct.Ready)
            {
                return true;
            }
        }
        return false;
    }
    public static void SetFirstReadyAsActive()
    {
        for (int i = 0; i < TurnOrder.Count; i++)
        {
            if (TurnOrder[i].Ready)
            {
                ActiveC = TurnOrder[i].C;
                ActiveC.ResetStatistic();
                return;
            }
        }
    }
    public static CharacterSetting ActiveC
    {
        set
        {
            _ActiveC = value;
            EndTurnCriterion = false;
            foreach (CharacterSetting.ConditionStruct Struct in ActiveC.Conditions)
            {
                Struct.Condition.TurnStartProcedure(ActiveC);
            }
            ActiveC.CalculateAllCurrentDerived();
            if (EndTurnCriterion)
            {
                EndTurn();
                return;
            }
            SetActiveAbility(null);
            if (_ActiveC.ControlledByPlayer)
            {
                OnActiveCharacterChanged();
            }
            else
            {
                OnActiveCharacterChanged();
                AITurn();
            }
        }
        get => _ActiveC;
    }
    public static void OnActiveCharacterChanged()
    {
        SetAbilityButton();
    }
    public static void SetAbilityButton()
    {
        ZeroizeAbilityPrefabButton();
        for (int j = 0; j < ActiveC.Arms.Length; j++)
        {
            if (ActiveC.Race.ArmsEnabled[j])
            {
                ItemSetting Weapon = ActiveC.GetWeapon(j);
                for (int i = 0; i < Weapon.Abilities.Length; i++)
                {
                    if (j % 2 == 0 || (j % 2 == 1 && !Weapon.Twohand)) // проверка на двуручное оружие в левой руке
                    {
                        AbilityButtons[j][i].GetComponent<AbilityButtonPrefabScript>().Ability = Weapon.Abilities[i];
                        AbilityButtons[j][i].GetComponent<AbilityButtonPrefabScript>().ItemSlot = CharacterSetting.ItemSlotFromArm(j);
                    }
                }
            }
        }
        int OccupiedButtons = 0;
        for (int i = 0; i < ActiveC.NumberOfBeltSlot; i++)
        {
            if (ActiveC.Belt[i] != null)
            {
                for (int j = 0; j < ActiveC.Belt[i].Abilities.Length; j++)
                {
                    
                    AbilityButtons[4][OccupiedButtons].GetComponent<AbilityButtonPrefabScript>().ItemSlot = GlobalEnumerators.ItemSlot.Belt;
                    AbilityButtons[4][OccupiedButtons].GetComponent<AbilityButtonPrefabScript>().Item = ActiveC.Belt[i];
                    AbilityButtons[4][OccupiedButtons].GetComponent<AbilityButtonPrefabScript>().Ability = ActiveC.Belt[i].Abilities[j];
                    OccupiedButtons++;
                }

            }
        }
    }
    
    public static void ZeroizeAbilityPrefabButton()
    {
        for (int i = 0; i < AbilityButtons.Length; i++)
        {
            foreach (GameObject AbilityButton in AbilityButtons[i])
            {
                AbilityButton.GetComponent<AbilityButtonPrefabScript>().Ability = null;
            }
        }
    }
    public static void AITurn()
    {
        /*HideAbilityButtons();
        bool ReadyEndTurn = false;
        while (!ReadyEndTurn)
        {
            AIScript.TargetStruct Target = AIScript.GetClosestEnemy(ActiveC);
            int AttackCost = (int)AbilityButtons[0][0].GetComponent<AbilityButtonPrefabScript>().TotalSpeedCost;
            if (Target.Target is null)
            {
                EndTurn();
                return;
            }    
            if (Target.Distance > 2)
            {
                if (ActiveC.SpeedCurrent >= ActiveC.StepCost.Current)
                    MainBattleBehaviour.ThisScript.MoveTo(ActiveC, new Vector3Int(Target.x, Target.y, 0));
                else
                    ReadyEndTurn = true;
            }
            else if (Target.Distance == 2)
            {
                if (ActiveC.SpeedCurrent >= ActiveC.StepCost.Current + AttackCost)
                    MainBattleBehaviour.ThisScript.MoveTo(ActiveC, new Vector3Int(Target.x, Target.y, 0));
                else
                    ReadyEndTurn = true;
            }
            else if (Target.Distance == 1)
            {
                if (ActiveC.SpeedCurrent >= AttackCost)
                {
                    SetActiveAbility(AbilityButtons[0][0]);
                    ActiveAbility.Ability.AbilityProcedure(ActiveC, new Vector3Int(Target.x, Target.y, 0), ActiveAbility);
                    ActiveC.SpeedCurrent -= ActiveAbility.TotalSpeedCost;
                    ActiveAbility.AbilityButton.GetComponent<AbilityButtonPrefabScript>().RaiseAdditionSpeedCost();
                    SetActiveAbility(null);
                }
                else
                    ReadyEndTurn = true;
            }
        }*/
            EndTurn();
    }
    public struct ActiveAbilityStruct
    {
        public AbilitySetting Ability;
        public GlobalEnumerators.ItemSlot ItemSlot;
        public ItemSetting Item;
        public int ArmNumber;
        public GameObject AbilityButton;
        public float TotalSpeedCost;
    }
    public static void ResetAllAbilitiesEnabilities()
    {
        for (int i = 0; i < AbilityButtons.Length; i++)
        {
            foreach (GameObject AbilityButton in AbilityButtons[i])
            {
                if (AbilityButton.GetComponent<AbilityButtonPrefabScript>().Visible)
                {
                    AbilityButton.GetComponent<AbilityButtonPrefabScript>().SetEnability();
                }
            }
        }
    }
    public static void SetActiveAbility(GameObject AbilityButton)
    {
        if (ActiveAbility.AbilityButton != null)
        {
            ActiveAbility.AbilityButton.transform.localScale = new Vector3(1, 1, 1);
        }
        if (AbilityButton == null)
        {
            ActiveAbilityStruct NullAbility = new ActiveAbilityStruct();
            NullAbility.Ability = null;
            NullAbility.AbilityButton = null;
            NullAbility.ItemSlot = GlobalEnumerators.ItemSlot.None;
            ActiveAbility = NullAbility;
        }
        else
        {
            ActiveAbilityStruct Ability = new ActiveAbilityStruct();
            Ability.Ability = AbilityButton.GetComponent<AbilityButtonPrefabScript>().Ability;
            Ability.AbilityButton = AbilityButton;
            Ability.ItemSlot = AbilityButton.GetComponent<AbilityButtonPrefabScript>().ItemSlot;
            Ability.TotalSpeedCost = AbilityButton.GetComponent<AbilityButtonPrefabScript>().TotalSpeedCost;
            Ability.Item = AbilityButton.GetComponent<AbilityButtonPrefabScript>().Item;
            Ability.ArmNumber = AbilityButton.GetComponent<AbilityButtonPrefabScript>().ArmNumber;
            ActiveAbility = Ability;
            AbilityButton.transform.localScale = new Vector3(MainBattleBehaviour.AbilityButtonDeltaScaleOnActivity, MainBattleBehaviour.AbilityButtonDeltaScaleOnActivity, 1);
        }
    }
    public static void RemoveCharacterFromMapCell(CharacterSetting C) // Очищение клетки карты, удаление токена персонажа
    {
        _Map[C.x, C.y].C = null;
        C.x = -1;
        C.y = -1;
        GameObject.Destroy(C.CharacterToken);
    }
    public struct AttackParam
    {
        public CharacterSetting C, Target;
        public ItemSetting Item;
        public GlobalEnumerators.ItemSlot ItemSlot;
        public AbilitySetting Ability;
        public GlobalEnumerators.DamageType DamageType;
        public int ArmNumber;
        public float Accuracy, Dodge, Parry;
        public float InjuryTresholdMod;
        public bool IgnoreArmor;
        public float ArmorMod;

        public AttackParam(CharacterSetting c, CharacterSetting target, ActiveAbilityStruct abilityStruct)
        {
            C = c;
            Target = target;
            Item = abilityStruct.Item;
            ItemSlot = abilityStruct.ItemSlot;
            Ability = abilityStruct.Ability;
            DamageType = (abilityStruct.Ability.DamageType == GlobalEnumerators.DamageType.None) ? Item.DamageType : abilityStruct.Ability.DamageType;
            ArmNumber = abilityStruct.ArmNumber;
            Accuracy = 0;
            Dodge = 0;
            Parry = 0;
            InjuryTresholdMod = 0;
            IgnoreArmor = false;
            ArmorMod = 0;
        }
    }
    public static void Attack(AttackParam Param)
    {
        string LogText = (Param.C.Name + " атакует " + Param.Target.Name + ", используя " + Param.Ability.Name) + "\n";
        int AccuracyRoll = Random.Range(1, 101);
        float Accuracy = Param.C.GetAccuracyFromItemSlot(Param.ItemSlot) + Param.Item.AccuracyMod + Param.Accuracy;
        if (AccuracyRoll <= Accuracy)
        {
            /* Debug.Log(Param.Target.Dodge.Current.ToString());
            Debug.Log(Param.Item.DodgeMod.ToString());
            Debug.Log(Param.Dodge.ToString());*/
            LogText += ("Не промах, выбросил " + AccuracyRoll.ToString() + ", точность " + Accuracy.ToString() + ".") + "\n";
            int DodgeRoll = Random.Range(1, 101);
            float Dodge = Param.Target.Dodge.Current + Param.Item.DodgeMod + Param.Dodge;
            if (DodgeRoll >= Dodge)
            {
                LogText += ("Попадание, выбросил " + DodgeRoll.ToString() + ", уклонение " + Dodge.ToString() + ".") + "\n";
                bool Parred = false;
                int ParredDamage = 0;
                if (Param.Item.CanBeParred)
                {
                    LogText += ("Оружие может быть отпарировано") + "\n";
                    for (int i = 0; i < Param.Target.Arms.Length; i++)
                    {
                        if (Param.Target.Race.ArmsEnabled[i] && Param.Target.GetWeapon(i).AllowParry)
                        {
                            LogText += ("Оружие в " + i.ToString() + " руке может парировать.") + "\n";
                            int ParryRoll = Random.Range(1, 101);
                            float Parry = Param.Target.Parry[i].Current + Param.Target.GetWeapon(i).ParryMod + Param.Parry;
                            if (ParryRoll <= Parry)
                            {
                                int CurrentParryDamage = ProbabilisticRounding(Param.Target.GetRandomDamage(i) * Param.Target.GetWeapon(i).ParryValueMult);
                                ParredDamage += CurrentParryDamage;
                                Parred = true;
                                LogText += ("Парирование, выбросил " + ParryRoll.ToString() + ", парирование " + Parry.ToString() + ". Отпарировал " + CurrentParryDamage.ToString() + " урона.") + "\n";
                            }
                            else
                            {
                                LogText += ("Парирование, выбросил " + ParryRoll.ToString() + ", парирование " + Parry.ToString() + ".") + "\n";
                            }
                        }
                    }
                }
                int Damage = ProbabilisticRounding(Param.C.GetRandomDamage(Param.ArmNumber));
                LogText += ("Потенциальный урон " + Damage.ToString() + ".") + "\n";
                int DamageAfterParry = Parred ? Mathf.Max(0, Damage - ParredDamage) : Damage;
                if (DamageAfterParry > 0)
                {
                    if (Param.Target.Armor != null && !Param.IgnoreArmor)
                    {
                        int ArmorValue = Mathf.Max(0, ProbabilisticRounding(Param.Target.Armor.GetArmorValueFromDamageType(Param.DamageType) + Param.ArmorMod));
                        if (ArmorValue > 0)
                        {
                            Damage = Mathf.Max(0, Damage - ArmorValue);
                            if (Damage > 0)
                                LogText += ("Доспех поглотил " + ArmorValue.ToString() + " урона.") + "\n";
                            else
                                LogText += ("Урон " + Damage.ToString() + ". Поглощено доспехом полностью.") + "\n";
                        }
                    }
                    if (Damage > 0)
                    {
                        Param.Target.HP -= Damage;
                        int InjuryLevel = Mathf.FloorToInt(Damage / (Param.Target.InjuryThreshold.Current + Param.InjuryTresholdMod + Param.Item.InjuryThresholdMod));
                        if (InjuryLevel > 0)
                        {
                            List<InjuryGroup> InjuryList = new List<InjuryGroup>();
                            foreach (InjuryGroup Group in InjuryGroupObject.InjuryGroupList)
                            {
                                foreach (GlobalEnumerators.DamageType DamageType in Group.DamageTypes)
                                {
                                    if (DamageType == Param.DamageType)
                                        InjuryList.Add(Group);
                                }
                            }
                            bool InjurySelected = false;
                            while (InjuryList.Count > 0 && !InjurySelected)
                            {
                                int GroupNumber = Random.Range(0, InjuryList.Count);
                                InjuryGroup CurrentGroup = InjuryList[GroupNumber];
                                bool CurrentGroupUnavailable = false;
                                for (int i = 0; i < Param.Target.Injuries.Count; i++)
                                {
                                    if (Param.Target.Injuries[i].Group == CurrentGroup)
                                    {
                                        if (Param.Target.Injuries[i].NumberInGroup == Param.Target.Injuries[i].Group.InjuryList.Count - 1)
                                        {
                                            CurrentGroupUnavailable = true;
                                            InjuryList.RemoveAt(GroupNumber);
                                        }
                                        else
                                        {
                                            InjurySelected = true;
                                            CharacterSetting.InjuryStruct injuryStruct = new CharacterSetting.InjuryStruct();
                                            injuryStruct = Param.Target.Injuries[i];
                                            injuryStruct.SetNumberInGroup(Param.Target.Injuries[i].NumberInGroup + InjuryLevel);
                                            Param.Target.Injuries[i] = injuryStruct;
                                            //Param.Target.Injuries[i].SetNumberInGroup(Param.Target.Injuries[i].NumberInGroup + InjuryLevel);
                                        }
                                    }
                                }
                                if (!CurrentGroupUnavailable && !InjurySelected)
                                {
                                    InjurySelected = true;
                                    CharacterSetting.InjuryStruct InjuryStruct = new CharacterSetting.InjuryStruct();
                                    InjuryStruct.Group = CurrentGroup;
                                    InjuryStruct.SetNumberInGroup(InjuryLevel - 1);
                                    Param.Target.Injuries.Add(InjuryStruct);
                                    //Param.Target.Injuries[Param.Target.Injuries.Count - 1].SetNumberInGroup(InjuryLevel - 1);
                                }
                            }
                        }
                    }
                }
                else
                    LogText += ("Урон " + Damage.ToString() + ". Отпарировано полностью.") + "\n";
            }
            else
                LogText += ("Уклонение, выбросил " + DodgeRoll.ToString() + ", уклонение " + Dodge.ToString() + ".") + "\n";
        }
        else
            LogText += ("Промах, выбросил " + AccuracyRoll.ToString() + ", точность " + Accuracy.ToString() + ".") + "\n";
        Param.C.CalculateAll();
        Param.Target.CalculateAll();
        MainBattleBehaviour.ThisScript.PrintInfo(SelectedCharacter);
        Debug.Log(LogText);
        MainBattleBehaviour.ThisScript.BattleLogText.text += LogText;
    }
    public static int ProbabilisticRounding(float Value)
    {
        int Int = Mathf.FloorToInt(Value);
        float Part = Value - Int;
        float RPart = Random.Range(0f, 1f);
        if (RPart <= Part)
            Int++;
        return Int;
    }
    public static void HideAbilityButtons()
    {
        for (int i = 0; i < AbilityButtons.Length; i++)
        {
            foreach (GameObject AbilityButton in AbilityButtons[i])
            {
                AbilityButton.SetActive(false);
            }
        }
    }
    public static void CorrectingPosition()
    {
        for (int i = 0; i < TurnOrder.Count; i++)
        {
            TurnOrderStruct NewStruct = new TurnOrderStruct();
            NewStruct.SetBasedOnStruct(TurnOrder[i]);
            NewStruct.NumberInTurnOrder = i;
            TurnOrder[i] = NewStruct;
        }
    }
    public static bool AllControledByPlayerCharactersDie()
    {
        foreach (TurnOrderStruct Struct in TurnOrder)
        {
            if (Struct.C.ControlledByPlayer)
                return false;
        }
        return true;
    }
}