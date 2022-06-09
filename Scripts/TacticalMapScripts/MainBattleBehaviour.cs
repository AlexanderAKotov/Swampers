using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using System.Drawing;

public class MainBattleBehaviour : MonoBehaviour
{
    // public TileBase TileToSet;
    // public Transform Warrior;
    public Tilemap map;
    private Camera mainCamera;
    public Text CoordWarriorText;
    //public Text SpeedCurrentText;
    public Button EndTurnButton;
    public static MainBattleBehaviour ThisScript;
    public GameObject TacticalMenuPanel;
    public GameObject CharactersPanel;
    //public GameObject ItemStats;
    public GameObject InitButtonPrefab;
    public GameObject CharacterTokenPrefab;
    public GameObject InitPanel;
    public GameObject AbilityButtonPrefab;
    public GameObject Canvas;
    public GameObject ConditionIconPrefab;
    public GameObject AbilityTooltipPanel;
    public Text FullNameWarriorText; // Полное имя выбранного бойца.
    public Text RaceDataText; // Раса выбранного бойца.
    public Text StrenghtDataText; // Сила выбранного бойца.
    public Text EnduranceDataText; // Выносливость выбранного бойца.
    public Text AgilityDataText; // Проворность выбранного бойца.
    public Text SpeedDataText; // Скорость и стоимость шага выбранного бойца.
    public Text HPDataText; // Здоровье выбранного бойца.
    public Text InjuryThresholdDataText; // Порог ранения выбранного бойца.
    public Text DodgeDataText; // Шанс уклонения выбранного бойца.
    public Text ParryDataText; // Шанс парирования выбранного бойца.
    public Text DamagDataText; // Урон выбранного бойца.
    public Text AccuracyDataText; // Точность выбранного бойца (первая правая рука).
    public Text CritChanceDataText; // Шанс критического удара выбранного бойца (первая правая рука).
    public Text InitiativeDataText; // Инициатива выбранного бойца.
    public Text CoordDataText; //Координаты выбранного бойца
    public Text NumberDataText; //Номер бойца в списке инициативы
    public Text InjuriesNamesDataText;
    public Text BattleLogText;
    public static int InitiativeOrderButtonSide = 75; // Величина стороны кнопки персонажа на панели инициативы
    public static int InitiativeOrderButtonMargin = 10;// Отступ кнопок персонажа друг от друга и от верхнего края панели инициативы
    public static float InitiativeOrderButtonDeltaScaleOnFirst = 1.3f; //Увеличение размера кнопки первого по инициативе персонажа
    public static int AbilityButtonSide = 40; // Величина стороны кнопки абилки
    public static int AbilityButtonMargin = 5;// Отступ кнопок абилки друг от друга и от верхнего края тактической панели (снизу)
    public static float AbilityButtonDeltaScaleOnActivity = 1.3f; //Увеличение размера кнопки при выборе абилки
    public static int ConditionIconSide = 40; // Величина стороны иконки состояния
    public static int ConditionIconMargin = 5;// Отступ кнопок иконки состояния друг от друга 
    public List<GameObject> ConditionIcons;
    const float HeuristicsMod = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
        PlayerControl.MouseOnPanel = false;
        Time.timeScale = 1;
        CreateInitiativeOrderButtons();
        CreateAbilityButtons();
        CreateConditionIcons();
        MainBattleScript.TacticalMapIsActive = true;
        ThisScript = this;
        // map = GetComponent<Tilemap>();
        mainCamera = Camera.main;
        CharacterSetting[] CharactersArray = new CharacterSetting[PlayerControl.Player.Walker.Crew.Count + 3]; //Создаем бойцов на поле боя: количество членов команды + враги.
        for (int i = 0; i < PlayerControl.Player.Walker.Crew.Count; i++)
        {
            CharactersArray[i] = PlayerControl.Player.Walker.Crew[i];
        }
        CharactersArray[PlayerControl.Player.Walker.Crew.Count] = CharacterSetting.CreateRandomCharacter(null, 0);
        CharactersArray[PlayerControl.Player.Walker.Crew.Count + 1] = CharacterSetting.CreateRandomCharacter(null, 0);
        CharactersArray[PlayerControl.Player.Walker.Crew.Count + 2] = CharacterSetting.CreateRandomCharacter(null, 0);
        for (int i = 3; i < PlayerControl.Player.Walker.Crew.Count + 3; i++)
        {
            int WeaponRoll = Random.Range(0, 3);
            int ArmorRoll = Random.Range(0, 3);
            if (WeaponRoll == 1)
                CharactersArray[i].Arms[0] = (ItemSetting)ItemObject.BoneShiv.CreateClone();
            else if (WeaponRoll == 2)
                CharactersArray[i].Arms[0] = (ItemSetting)ItemObject.RustySword.CreateClone();
            if (ArmorRoll == 1)
                CharactersArray[i].Armor = (ItemSetting)ItemObject.Baldric.CreateClone();
            else if (ArmorRoll == 2)
                CharactersArray[i].Armor = (ItemSetting)ItemObject.HeavyScrapArmor.CreateClone();
        }
        MainBattleScript.TurnOrderSetUp(CharactersArray);
    }

    // Update is called once per frame
    void Update()
    {
        MoveAnimation();
        if (Input.GetMouseButtonDown(0))
        {
            if (!PlayerControl.MouseOnPanel)
            {
                CharacterSetting C = MainBattleScript.ActiveC;
                Vector3 clickWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int clickCellPosition = map.WorldToCell(clickWorldPosition);
                clickCellPosition.z = 0;
                if (C.ControlledByPlayer)
                {
                    if (MainBattleScript.ActiveAbility.Ability == null)
                    {
                        if (MainBattleScript._Map[clickCellPosition.x, clickCellPosition.y].C != null)
                        {
                            MainBattleScript.SetSelectedCharacterIndex(MainBattleScript._Map[clickCellPosition.x, clickCellPosition.y].C.TurnOrderPosition);
                        }
                        else
                        {
                            MoveTo(C, clickCellPosition);
                        }
                    }
                    else
                    {
                        AbilitySetting Ability = MainBattleScript.ActiveAbility.Ability;
                        if (Ability.TriggerType == GlobalEnumerators.AbilityTriggerType.Attack)
                        {
                            int Distance = GetDistanceBetweenHexes(new Vector3Int((int)C.x, (int)C.y, 0), clickCellPosition);
                            if (Distance >= MainBattleScript.ActiveAbility.Item.RangeMin + Ability.RangeMin && Distance <= MainBattleScript.ActiveAbility.Item.RangeMax + Ability.RangeMax)
                            {
                                CharacterSetting TargetC = MainBattleScript.GetMapCellCharacter(clickCellPosition.x, clickCellPosition.y);
                                if (TargetC != null)
                                {
                                    if (TargetC.BattleSide != C.BattleSide)
                                    {
                                        Ability.AbilityProcedure(C, clickCellPosition, MainBattleScript.ActiveAbility);
                                        C.SpeedCurrent -= MainBattleScript.ActiveAbility.TotalSpeedCost;
                                        C.AttackPerformedInTurn++;
                                        MainBattleScript.ActiveAbility.AbilityButton.GetComponent<AbilityButtonPrefabScript>().RaiseAdditionSpeedCost();
                                        MainBattleScript.SetActiveAbility(null);
                                    }
                                }
                            }
                        }
                    }
                }
                PrintInfo(MainBattleScript.SelectedCharacter);
                CoordWarriorText.text = clickCellPosition.x.ToString() + "/" + clickCellPosition.y.ToString() + "/" + clickCellPosition.z.ToString() + " || " + MainBattleScript.ActiveC.SpeedCurrent.ToString() + "/" + MainBattleScript.ActiveC.SpeedMax.ToString(); //Отображение текущих координат Варриора.
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (MainBattleScript.ActiveAbility.AbilityButton != null)
            {
                MainBattleScript.SetActiveAbility(null);
            }
        }

    }
    public void PrintInfo(CharacterSetting C)
    {
        FullNameWarriorText.text = C.Name; // Полное имя выбранного бойца.
        RaceDataText.text = C.Race.RaceName; // Раса выбранного бойца.
        StrenghtDataText.text = C.Strenght.Current.ToString(); // Сила выбранного бойца.
        EnduranceDataText.text = C.Endurance.Current.ToString(); // Выносливость выбранного бойца.
        AgilityDataText.text = C.Agility.Current.ToString(); // Проворность выбранного бойца.
        SpeedDataText.text = C.SpeedCurrent.ToString() + " / " + C.StepCost.Current.ToString(); // Скорость и стоимость шага выбранного бойца.
        HPDataText.text = C.HP.ToString() + " / " + C.HPMax.ToString(); // Здоровье выбранного бойца.
        InjuryThresholdDataText.text = C.InjuryThreshold.Current.ToString(); // Порог ранения выбранного бойца.
        ParryDataText.text = C.Parry[0].Current.ToString(); // Шанс базового парирования выбранного бойца.
        //DamagDataText; // Урон выбранного бойца.
        AccuracyDataText.text = C.Accuracy[0].Current.ToString(); // Точность выбранного бойца (первая правая рука).
        CritChanceDataText.text = C.CritChance.Current.ToString(); // Шанс критического удара выбранного бойца (первая правая рука).
        InitiativeDataText.text = C.Initiative.Current.ToString();
        CoordDataText.text = C.x.ToString() + " / " + C.y.ToString();
        NumberDataText.text = C.TurnOrderPosition.ToString();
        DodgeDataText.text = C.Dodge.Current.ToString(); // Шанс уклонения выбранного бойца.
        InjuriesNamesDataText.text = "";
        foreach (GameObject Icon in ConditionIcons)
        {
            Icon.GetComponent<Image>().sprite = null;
            Icon.SetActive(false);
        }
        int OccupiedIcons = 0;
        for (int i = 0; i < C.Injuries.Count; i++)
        {
            InjuriesNamesDataText.text += C.Injuries[i].Group.InjuryList[C.Injuries[i].NumberInGroup].Name + " ";
            ConditionIcons[i].GetComponent<Image>().sprite = C.Injuries[i].Group.InjuryList[C.Injuries[i].NumberInGroup].Icon;
            ConditionIcons[i].SetActive(true);
            OccupiedIcons++;
        }
        for (int i = OccupiedIcons; i < C.Conditions.Count; i++)
        {
            if (C.Conditions[i].Condition.Visible)
            {
                ConditionIcons[OccupiedIcons].GetComponent<Image>().sprite = C.Conditions[i].Condition.Icon;
                ConditionIcons[OccupiedIcons].SetActive(true);
                OccupiedIcons++;
            }
        }
    }
    public GameObject GetInitButtonPrefabInstantiated() //Создание кнопки на панели инициативы
    {
        return Instantiate(InitButtonPrefab, InitPanel.transform);
    }
    public GameObject GetAbilityButtonPrefabInstantiated() //Создание кнопки абилки
    {
        return Instantiate(AbilityButtonPrefab, Canvas.transform);
    }
    public bool CharacterMove(CharacterSetting C, Vector3Int clickCellPosition)
    {
        Vector3Int WarriorCellPosition = new Vector3Int(C.x, C.y, 0);
        float DifferenceX = clickCellPosition.x - WarriorCellPosition.x;
        float DifferenceY = clickCellPosition.y - WarriorCellPosition.y;
        float EvenY = WarriorCellPosition.y % 2;
        //Debug.Log("DifferenceX " + DifferenceX.ToString() + " DifferenceY " + DifferenceY.ToString() + " EvenY " + EvenY.ToString());
        if ((DifferenceX == 0 && Mathf.Abs(DifferenceY) == 1) || (DifferenceX == 1 - (EvenY * 2) && DifferenceY == 0) || (DifferenceX == -1 + (EvenY * 2) && Mathf.Abs(DifferenceY) <= 1)) // Шагание только на 1 клетку
        {
            //Debug.Log("Соседняя клетка попытка встать");
            if (C.SpeedCurrent >= (C.StepCost.Current + MainBattleScript._Map[clickCellPosition.x, clickCellPosition.y].Property.StepCostMod) && MainBattleScript._Map[clickCellPosition.x, clickCellPosition.y].Property.AllowStanding)
            {
                if (MainBattleScript.SetCharacterOnMapCell(clickCellPosition.x, clickCellPosition.y, C, 1 + C.Agility.Current * 0.33f))
                {
                    C.SpeedCurrent -= C.StepCost.Current;
                    C.HexMovedInTurn++;
                    return true;
                }
            }
        }
        return false;
    }
    public static int GetDistanceBetweenHexes(Vector3Int A, Vector3Int B)
    {
        int Dx = B.x - A.x;
        int Dy = B.y - A.y;
        int Distance = 0;
        while (Dx != 0 || Dy != 0)
        {
            if (Dx == 0)
            {
                Distance += Mathf.Abs(Dy);
                Dy = 0;
            }
            else
            {
                if (Dy == 0)
                {
                    Distance += Mathf.Abs(Dx);
                    Dx = 0;
                }
                else
                {
                    int EvenY = (A.y + Distance) % 2;
                    if (EvenY == 1)
                    {
                        if (Dx > 0 && Dy > 0)
                        {
                            Dx--;
                            Dy--;
                        }
                        else if (Dx > 0 && Dy < 0)
                        {
                            Dx--;
                            Dy++;
                        }
                        else if (Dx < 0 && Dy > 0)
                        {
                            Dy--;
                        }
                        else if (Dx < 0 && Dy < 0)
                        {
                            Dy++;
                        }
                    }
                    else
                    {
                        if (Dx > 0 && Dy > 0)
                        {
                            Dy--;
                        }
                        else if (Dx > 0 && Dy < 0)
                        {
                            Dy++;
                        }
                        else if (Dx < 0 && Dy > 0)
                        {
                            Dx++;
                            Dy--;
                        }
                        else if (Dx < 0 && Dy < 0)
                        {
                            Dx++;
                            Dy++;
                        }
                    }
                    Distance++;
                }
            }
        }
        return Distance;
    }
    public static int GetDistanceBetweenHexes(Vector2Int A, Vector2Int B)
    {
        int Dx = B.x - A.x;
        int Dy = B.y - A.y;
        int Distance = 0;
        while (Dx != 0 || Dy != 0)
        {
            if (Dx == 0)
            {
                Distance += Mathf.Abs(Dy);
                Dy = 0;
            }
            else
            {
                if (Dy == 0)
                {
                    Distance += Mathf.Abs(Dx);
                    Dx = 0;
                }
                else
                {
                    int EvenY = (A.y + Distance) % 2;
                    if (EvenY == 1)
                    {
                        if (Dx > 0 && Dy > 0)
                        {
                            Dx--;
                            Dy--;
                        }
                        else if (Dx > 0 && Dy < 0)
                        {
                            Dx--;
                            Dy++;
                        }
                        else if (Dx < 0 && Dy > 0)
                        {
                            Dy--;
                        }
                        else if (Dx < 0 && Dy < 0)
                        {
                            Dy++;
                        }
                    }
                    else
                    {
                        if (Dx > 0 && Dy > 0)
                        {
                            Dy--;
                        }
                        else if (Dx > 0 && Dy < 0)
                        {
                            Dy++;
                        }
                        else if (Dx < 0 && Dy > 0)
                        {
                            Dx++;
                            Dy--;
                        }
                        else if (Dx < 0 && Dy < 0)
                        {
                            Dx++;
                            Dy++;
                        }
                    }
                    Distance++;
                }
            }
        }
        return Distance;
    }

    private static void CreateInitiativeOrderButtons()
    {
        MainBattleScript.InitiativeOrderButtons = new List<GameObject>();
        float PanelHeight = GameObject.Find("InitiativeOrderPanel").GetComponent<RectTransform>().rect.height;
        int NumberOfButtons = (int)(PanelHeight - InitiativeOrderButtonMargin) / (InitiativeOrderButtonSide + InitiativeOrderButtonMargin);// Получаем количество кнопок, которые можно разместить на панели инициатив
        GameObject InitButtonFirst = GameObject.Find("Main Camera").GetComponent<MainBattleBehaviour>().GetInitButtonPrefabInstantiated();//Создание отдельной кнопки для первого по инициативе персонажа.
        MainBattleScript.InitiativeOrderButtons.Add(InitButtonFirst);
        InitButtonFirst.transform.localScale = new Vector3(InitiativeOrderButtonDeltaScaleOnFirst, InitiativeOrderButtonDeltaScaleOnFirst, 1);
        InitButtonFirst.transform.localPosition = new Vector3(InitiativeOrderButtonSide * (InitiativeOrderButtonDeltaScaleOnFirst - 1) / 2, -PanelHeight / 2 - InitiativeOrderButtonSide * InitiativeOrderButtonDeltaScaleOnFirst / 2 - InitiativeOrderButtonMargin, 0);
        InitButtonFirst.GetComponent<InitiativeOrderPrefabScript>().CharacterNumber = -1;
        InitButtonFirst.GetComponent<InitiativeOrderPrefabScript>().BoundToOnClick();
        for (int i = 0; i < NumberOfButtons; i++) //Заполнение стека инициативы для персонажей от второго по инициативе и дальше.
        {
            GameObject InitButton = GameObject.Find("Main Camera").GetComponent<MainBattleBehaviour>().GetInitButtonPrefabInstantiated();
            MainBattleScript.InitiativeOrderButtons.Add(InitButton);
            InitButton.transform.localPosition = new Vector3(0, -PanelHeight / 2 + i * InitiativeOrderButtonSide + (i + 1) * InitiativeOrderButtonMargin + InitiativeOrderButtonSide / 2, 0);
            InitButton.GetComponent<InitiativeOrderPrefabScript>().CharacterNumber = -1;
            InitButton.GetComponent<InitiativeOrderPrefabScript>().BoundToOnClick();
        }
    }
    private void CreateAbilityButtons()
    {
        MainBattleScript.AbilityButtons = new List<GameObject>[5];
        for (int i = 0; i < 4; i++)
        {
            MainBattleScript.AbilityButtons[i] = new List<GameObject>();
            CreateWeaponAbilityButton(i, MainBattleScript.AbilityButtons[i]);
        }
        float PanelHeight = TacticalMenuPanel.GetComponent<RectTransform>().rect.height;
        float CanvasHeight = Canvas.GetComponent<RectTransform>().rect.height;
        float CanvasWidth = Canvas.GetComponent<RectTransform>().rect.width;
        MainBattleScript.AbilityButtons[4] = new List<GameObject>();
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                GameObject AbilityButton = GetAbilityButtonPrefabInstantiated();
                MainBattleScript.AbilityButtons[4].Add(AbilityButton);
                AbilityButton.transform.localPosition = new Vector3(InitiativeOrderButtonMargin + InitiativeOrderButtonSide * InitiativeOrderButtonDeltaScaleOnFirst - CanvasWidth / 2 + AbilityButtonMargin + (2 * AbilityButtonSide + AbilityButtonMargin) * 4 * 2 + (j * (AbilityButtonSide + AbilityButtonMargin)) + AbilityButtonSide / 2, -CanvasHeight / 2 + PanelHeight + AbilityButtonMargin + AbilityButtonSide / 2 + i * (AbilityButtonSide + AbilityButtonMargin), 0); // 
                AbilityButton.GetComponent<AbilityButtonPrefabScript>().BoundToOnClick();
                AbilityButton.GetComponent<AbilityButtonPrefabScript>().ItemSlot = GlobalEnumerators.ItemSlot.Belt;
            }
        }
    }
    private void CreateWeaponAbilityButton(int ArmNumber, List<GameObject> AbilityButtons)
    {
        float PanelHeight = TacticalMenuPanel.GetComponent<RectTransform>().rect.height;
        float CanvasHeight = Canvas.GetComponent<RectTransform>().rect.height;
        float CanvasWidth = Canvas.GetComponent<RectTransform>().rect.width;
        for (int i = 0; i < 4; i++)
        {
            GameObject AbilityButton = GetAbilityButtonPrefabInstantiated();
            AbilityButtons.Add(AbilityButton);
            AbilityButton.transform.localPosition = new Vector3(InitiativeOrderButtonMargin + InitiativeOrderButtonSide * InitiativeOrderButtonDeltaScaleOnFirst - CanvasWidth / 2 + AbilityButtonMargin + (2 * AbilityButtonSide + AbilityButtonMargin) * ArmNumber * 2 + ((i % 2 == 1) ? (AbilityButtonSide + AbilityButtonMargin) : 0) + AbilityButtonSide / 2, -CanvasHeight / 2 + PanelHeight + AbilityButtonMargin + AbilityButtonSide / 2 + ((i == 2 || i == 3) ? (AbilityButtonSide + AbilityButtonMargin) : 0), 0);
            AbilityButton.GetComponent<AbilityButtonPrefabScript>().BoundToOnClick();
            AbilityButton.GetComponent<AbilityButtonPrefabScript>().ItemSlot = CharacterSetting.ItemSlotFromArm(ArmNumber);
            AbilityButton.GetComponent<AbilityButtonPrefabScript>().ArmNumber = ArmNumber;
        }
    }
    private void CreateConditionIcons()
    {
        float PanelHeight = CharactersPanel.GetComponent<RectTransform>().rect.height;
        float PanelWidth = CharactersPanel.GetComponent<RectTransform>().rect.width;
        ConditionIcons = new List<GameObject>();
        int IconInRow = Mathf.FloorToInt((PanelWidth - ConditionIconMargin) / (ConditionIconMargin + ConditionIconSide));
        Debug.Log(IconInRow.ToString());
        for (int i = 0; i < IconInRow; i++)
        {
            GameObject Icon = Instantiate(ConditionIconPrefab, CharactersPanel.transform);
            ConditionIcons.Add(Icon);
            Icon.transform.localPosition = new Vector3(-PanelWidth / 2 + ConditionIconMargin + ConditionIconSide / 2 + i * (ConditionIconSide + ConditionIconMargin), -PanelHeight / 2 + ConditionIconMargin + ConditionIconSide / 2, 0);
            Icon.SetActive(true);
        }
    }
    public void MoveTo(CharacterSetting C, Vector3Int Target)// Идем до целевой клетки, пока не закончится скорость или не дойдем.
    {
        int X = Target.x;
        int Y = Target.y;
        if (!MainBattleScript._Map[X, Y].Property.AllowStanding) //Если нельзя на клетке стоять
        {
            Debug.Log("Клетка недоступна, возврат");
            return;
        }
        List<Vector2Int> Path = BuildPath(C, new Vector2Int(Target.x, Target.y));//Построение пути
        foreach (Vector2Int Cell in Path)
        {
            Debug.Log(Cell.x.ToString() + " | " + Cell.y.ToString());
        }
        foreach (Vector2Int Cell in Path)//Попытки шагать
        {
            if (!CharacterMove(C, new Vector3Int(Cell.x, Cell.y, 0)))
            {
                break;
            }
        }
    }
    private void MoveAnimation()
    {
        foreach (MainBattleScript.TurnOrderStruct Order in MainBattleScript.TurnOrder)
        {
            if (Order.AnimationPath.Count > 0)
            {
                float Delta = Time.deltaTime;
                float Dx = Order.AnimationPath[0].Point.x - Order.C.CharacterToken.transform.position.x;
                float Dy = Order.AnimationPath[0].Point.y - Order.C.CharacterToken.transform.position.y;
                if (Dx == 0 && Dy == 0)
                {
                    Order.AnimationPath.RemoveAt(0);
                }
                // else if (Mathf.Sqrt(Dx * Dx + Dy * Dy) < 0.05f)
                //    Order.C.CharacterToken.transform.position = new Vector3(Order.AnimationPath[0].Point.x, Order.AnimationPath[0].Point.y, 1);
                else
                {
                    float NormX = Dx / Mathf.Sqrt(Dx * Dx + Dy * Dy);
                    float NormY = Dy / Mathf.Sqrt(Dx * Dx + Dy * Dy);
                    float MoveX = NormX * Delta * Order.AnimationPath[0].AnimationSpeed;
                    if (Mathf.Abs(MoveX) > Mathf.Abs(Dx))
                        MoveX = Dx;
                    float MoveY = NormY * Delta * Order.AnimationPath[0].AnimationSpeed;
                    if (Mathf.Abs(MoveY) > Mathf.Abs(Dy))
                        MoveY = Dy;
                    float X = Order.C.CharacterToken.transform.position.x + MoveX;
                    float Y = Order.C.CharacterToken.transform.position.y + MoveY;
                    // Debug.Log(NormX.ToString() + " " + NormY.ToString());
                    Order.C.CharacterToken.transform.position = new Vector3(X, Y, 1);
                }
            }
        }
    }
    public List<Vector2Int> GetAllHexInRadius(Vector2Int Center, int R)
    {
        if (R < 0)
        {
            R = 0;
        }
        List<Vector2Int> HexList = new List<Vector2Int>();
        return RecursiveTraversal(Center, R, new Vector2Int(Center.x + R, Center.y), HexList);
    }
    public List<Vector2Int> RecursiveTraversal(Vector2Int Center, int R, Vector2Int CurrentHex, List<Vector2Int> HexList)
    {
        HexList.Add(CurrentHex);
        Vector2Int C = CurrentHex;
        int EvenY = C.y % 2;
        List<Vector2Int> NearbyCell = new List<Vector2Int>();
        NearbyCell.Add(new Vector2Int(C.x, C.y + 1));
        NearbyCell.Add(new Vector2Int(C.x, C.y - 1));
        NearbyCell.Add(new Vector2Int(C.x - 1 + EvenY * 2, C.y));
        NearbyCell.Add(new Vector2Int(C.x + 1 - EvenY * 2, C.y));
        NearbyCell.Add(new Vector2Int(C.x - 1 + EvenY * 2, C.y + 1));
        NearbyCell.Add(new Vector2Int(C.x - 1 + EvenY * 2, C.y - 1));
        List<Vector2Int> SameR = new List<Vector2Int>();
        foreach (Vector2Int Cell in NearbyCell)
        {
            if (GetDistanceBetweenHexes(new Vector3Int(Center.x, Center.y, 0), new Vector3Int(Cell.x, Cell.y, 0)) == R)
            {
                SameR.Add(Cell);
            }
        }
        foreach (Vector2Int Cell in SameR)
        {
            if (!HexList.Contains(Cell))
            {
                RecursiveTraversal(Center, R, Cell, HexList);
                break;
            }
        }
        return HexList;
    }
    public void FillSpeedMap(Vector2Int Center, CharacterSetting C, ref int[,] SpeedMap, int CurrentR)
    {
        bool AllHexInRadiusBlocked = true;
        foreach (Vector2Int CurrentHex in GetAllHexInRadius(Center, CurrentR - 1))
        {
            if (CurrentHex.x >= 0 && CurrentHex.x < MainBattleScript.MapWidth && CurrentHex.y >= 0 && CurrentHex.y < MainBattleScript.MapHeight)
            {
                List<Vector2Int> RadiusList = GetAllHexInRadius(CurrentHex, 1);
                foreach (Vector2Int Cell in RadiusList)
                {
                    if (Cell.x >= 0 && Cell.x < MainBattleScript.MapWidth && Cell.y >= 0 && Cell.y < MainBattleScript.MapHeight && GetDistanceBetweenHexes(CurrentHex, Center) < GetDistanceBetweenHexes(Cell, Center))
                    {
                        if (MainBattleScript._Map[Cell.x, Cell.y].Property.AllowStanding && MainBattleScript._Map[Cell.x, Cell.y].C != null)
                        {
                            AllHexInRadiusBlocked = false;
                            int Speed = (int)C.StepCost.Current + (int)MainBattleScript._Map[Cell.x, Cell.y].Property.StepCost + (int)MainBattleScript._Map[CurrentHex.x, CurrentHex.y].Property.StepCostMod;
                            if (C.x >= 0 && C.x < MainBattleScript.MapWidth && C.y >= 0 && C.y < MainBattleScript.MapHeight)
                            {
                                Speed -= (int)MainBattleScript._Map[C.x, C.y].Property.StepCost;
                            }
                            Speed += SpeedMap[CurrentHex.x, CurrentHex.y];
                            if (SpeedMap[Cell.x, Cell.y] <= 0 || Speed < SpeedMap[Cell.x, Cell.y])
                            {
                                SpeedMap[Cell.x, Cell.y] = Speed;
                            }
                        }
                        else
                        {
                            SpeedMap[Cell.x, Cell.y] = -1;
                        }
                    }
                }
            }
        }
        if (!AllHexInRadiusBlocked)
        {
            FillSpeedMap(Center, C, ref SpeedMap, CurrentR + 1);
        }
    }
    public List<Vector2Int> GetNearbyHexWithMapRangeCheck(Vector2Int Center)
    {
        List<Vector2Int> Nearby = GetAllHexInRadius(Center, 1);
        List<Vector2Int> WithCheck = new List<Vector2Int>();
        foreach (Vector2Int Cell in Nearby)
        {
            if (Cell.x >= 0 && Cell.x < MainBattleScript.MapWidth && Cell.y >= 0 && Cell.y < MainBattleScript.MapHeight)
            {
                WithCheck.Add(Cell);
            }
        }
        return WithCheck;
    }
    public List<Vector2Int> BuildPath(CharacterSetting C, Vector2Int Target)
    {
        Debug.Log("Входим в BuildPath");
        List<Vector2Int> Frontier = new List<Vector2Int>();
        List<Vector2Int> Visited = new List<Vector2Int>();
        bool PathBuilded = false;
        BuildPathHex[,] PathMap = new BuildPathHex[MainBattleScript.MapWidth, MainBattleScript.MapHeight];
        Frontier.Add(new Vector2Int(C.x, C.y));
        PathMap[C.x, C.y] = new BuildPathHex();
        while (Frontier.Count != 0 && !PathBuilded)
        {
            int Index = GetBestWithPriority(Frontier, PathMap);
            VisitHex(Frontier, Visited, PathMap, Index, out PathBuilded, C, Target);
        }
        List<Vector2Int> PathList = new List<Vector2Int>();
        if (Visited.Contains(Target))
        {
            List<Vector2Int> ReversedPathList = new List<Vector2Int>();
            Vector2Int CurrentHex = new Vector2Int(Target.x, Target.y);
            while (CurrentHex.x != C.x || CurrentHex.y != C.y)
            {
                ReversedPathList.Add(CurrentHex);
                //Debug.Log(CurrentHex.x.ToString() + " | " + CurrentHex.y.ToString());
                CurrentHex = PathMap[CurrentHex.x, CurrentHex.y].PreviousHex;
            }
            for (int i = ReversedPathList.Count - 1; i > -1; i--)
            {
                PathList.Add(ReversedPathList[i]);
            }
        }
        return PathList;
    }
    private class BuildPathHex : object
    {
        public int CalculatedSpeed;
        public float Priority;
        public List<MoveCostStruct> MoveCost = new List<MoveCostStruct>();
        public Vector2Int PreviousHex;
        public struct MoveCostStruct
        {
            public Vector2Int Coord;
            public int MoveCost;
        }
    }
    private int GetBestWithPriority(List<Vector2Int> Frontier, BuildPathHex[,] PathMap)//Получение индекса гекса с лучшим (минимальным) приоритетом. 
    {
        int Index = 0;
        float CurrentPriority = float.MaxValue;
        for (int i = 0; i < Frontier.Count; i++)
        {
            if (PathMap[Frontier[i].x, Frontier[i].y].Priority < CurrentPriority)
            {
                Index = i;
                CurrentPriority = PathMap[Frontier[i].x, Frontier[i].y].Priority;
            }
        }
        return Index;
    }
    private void VisitHex(List<Vector2Int> Frontier, List<Vector2Int> Visited, BuildPathHex[,]  PathMap, int Index, out bool PathBuilded, CharacterSetting C, Vector2Int Target)
    {
        PathBuilded = Frontier[Index] == Target;
        Vector2Int PreviousHex = new Vector2Int(-1, -1);
        int TotalCost = int.MaxValue;
        for (int i = 0; i < PathMap[Frontier[Index].x, Frontier[Index].y].MoveCost.Count; i++)
        {
            if (Visited.Contains(PathMap[Frontier[Index].x, Frontier[Index].y].MoveCost[i].Coord))
            {
                if (PathMap[Frontier[Index].x, Frontier[Index].y].MoveCost[i].MoveCost + PathMap[PathMap[Frontier[Index].x, Frontier[Index].y].MoveCost[i].Coord.x, PathMap[Frontier[Index].x, Frontier[Index].y].MoveCost[i].Coord.y].CalculatedSpeed < TotalCost)
                {
                    PreviousHex = PathMap[Frontier[Index].x, Frontier[Index].y].MoveCost[i].Coord;
                    TotalCost = PathMap[Frontier[Index].x, Frontier[Index].y].MoveCost[i].MoveCost + PathMap[PathMap[Frontier[Index].x, Frontier[Index].y].MoveCost[i].Coord.x, PathMap[Frontier[Index].x, Frontier[Index].y].MoveCost[i].Coord.y].CalculatedSpeed;
                }
            }
        }
        if (TotalCost == int.MaxValue)
        { 
            TotalCost = 0;
        }
        PathMap[Frontier[Index].x, Frontier[Index].y].CalculatedSpeed = TotalCost;
        PathMap[Frontier[Index].x, Frontier[Index].y].PreviousHex = PreviousHex;
        //Debug.Log("Добавили клетку в посещенные: " + Frontier[Index].x.ToString() + " | " + Frontier[Index].y.ToString() + "Предыдущая клетка: " + PreviousHex.x.ToString() + " | " + PreviousHex.y.ToString());

        Visited.Add(new Vector2Int(Frontier[Index].x, Frontier[Index].y));
        Frontier.RemoveAt(Index);
        List<Vector2Int> NearbyHex = GetNearbyHexWithMapRangeCheck(Visited[Visited.Count - 1]);
        foreach (Vector2Int Cell in NearbyHex)
        {
            if (Frontier.Contains(Cell))
            {
                bool Finded = false;
                int MoveCost = 0;
                foreach (BuildPathHex.MoveCostStruct Struct in PathMap[Cell.x, Cell.y].MoveCost)
                {
                    if (Struct.Coord == Visited[Visited.Count - 1])
                    {
                        Finded = true;
                        MoveCost = Struct.MoveCost;
                        break;
                    }
                }
                if (Finded)
                {
                    if (PathMap[Visited[Visited.Count - 1].x, Visited[Visited.Count - 1].y].CalculatedSpeed + MoveCost + (GetDistanceBetweenHexes(Cell, new Vector2Int(C.x, C.y)) + GetDistanceBetweenHexes(Cell, Target)) * HeuristicsMod < PathMap[Cell.x, Cell.y].Priority)
                    {
                        PathMap[Cell.x, Cell.y].Priority = PathMap[Visited[Visited.Count - 1].x, Visited[Visited.Count - 1].y].CalculatedSpeed + MoveCost + (GetDistanceBetweenHexes(Cell, new Vector2Int(C.x, C.y)) + GetDistanceBetweenHexes(Cell, Target)) * HeuristicsMod;
                    }
                }
            }
            else
            {
                if (MainBattleScript._Map[Cell.x, Cell.y].Property.AllowStanding && (MainBattleScript._Map[Cell.x, Cell.y].C == null || Cell == Target))
                {
                    if (!Visited.Contains(Cell))
                    {
                        int MoveCost = 0;
                        PathMap[Cell.x, Cell.y] = new BuildPathHex();
                        List<Vector2Int> NearbyHex1 = GetNearbyHexWithMapRangeCheck(Cell);
                        foreach (Vector2Int Cell1 in NearbyHex1)
                        {
                            if (MainBattleScript._Map[Cell1.x, Cell1.y].Property.AllowStanding && (MainBattleScript._Map[Cell1.x, Cell1.y].C == null || MainBattleScript._Map[Cell1.x, Cell1.y].C == C))
                            {
                                BuildPathHex.MoveCostStruct Struct = new BuildPathHex.MoveCostStruct();
                                Struct.Coord = Cell1;
                                Struct.MoveCost = (int)GetCharacterStepCostBetweenHexes(C, Cell1, Cell);
                                PathMap[Cell.x, Cell.y].MoveCost.Add(Struct);
                                if (Cell1 == Visited[Visited.Count - 1])
                                {
                                    MoveCost = Struct.MoveCost;
                                }
                            }
                        }
                        PathMap[Cell.x, Cell.y].Priority = PathMap[Visited[Visited.Count - 1].x, Visited[Visited.Count - 1].y].CalculatedSpeed + MoveCost + (GetDistanceBetweenHexes(Cell, new Vector2Int(C.x, C.y)) + GetDistanceBetweenHexes(Cell, Target)) * HeuristicsMod;
                        Frontier.Add(Cell);
                    }
                }
            }
        }
    }
    public float GetCharacterStepCostBetweenHexes(CharacterSetting C, Vector2Int From, Vector2Int To)
    {
        float BaseStepCost = C.StepCost.Current - MainBattleScript._Map[C.x, C.y].Property.StepCost;
        return BaseStepCost + MainBattleScript._Map[From.x, From.y].Property.StepCost + MainBattleScript._Map[To.x, To.y].Property.StepCostMod;
    }
}

