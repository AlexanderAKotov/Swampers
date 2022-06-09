using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StrategicCharactersButton : MonoBehaviour
{
    public GameObject StrategicMapMenuPanel;
    public GameObject CharactersPanel;
    public Button CharactersMenuButton;
    public GameObject Warrior1Button;
    private static GameObject[] ButtonArray;
    private int _CharacterNumber; //Номер выбранного бойца в массиве команды.
    public CharacterSetting C; // Выбранный персонаж.
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
    public GameObject StrenghtUpButton;
    public GameObject EnduranceUpButton;
    public GameObject AgilityUpButton;
    public static GameObject DragObject; //Объект, который тянется за мышкой
    public GameObject DragObjectBuffer;
    public static GameObject DragedObject; //Кнопка с предметом, которую потянули.
    public static GameObject MouseOnSlot;
    public static bool OnDrag;
    public GameObject CharacterItemPanel;
    public GameObject InventoryPanel;
    public GameObject[] ArmsSlots = new GameObject[4];
    public GameObject ArmorSlot;
    public GameObject[] BeltSlots = new GameObject[CharacterSetting.BeltSlotGlobalMax];
    public List<GameObject> InventorySlots = new List<GameObject>();
    private int _InventoryRowMargin = 0;
    private int _InventoryRowMarginMax;
    private List<ItemSetting> ItemBuffer = new List<ItemSetting>();
    //public static GameObject DragItemObject;
    //public static Image DragItemImage;


    // Start is called before the first frame update
    void Start()
    {
        Button btn = this.GetComponent<Button>();
        switch (name)
        {
            case "CharactersMenuButton":
                btn.onClick.AddListener(CharactersStatsPanel);
                break;
        }
        StrenghtUpButton.GetComponent<StatUpButton>().Stat = GlobalEnumerators.StatEnum.Strenght;
        EnduranceUpButton.GetComponent<StatUpButton>().Stat = GlobalEnumerators.StatEnum.Endurance;
        AgilityUpButton.GetComponent<StatUpButton>().Stat = GlobalEnumerators.StatEnum.Agility;
        DragObject = DragObjectBuffer;
    }
    // Update is called once per frame
    void Update()
    {
        if (OnDrag && DragObject != null) //Отображение иконки предмета на мышке при перетягивании
        {
            // DragObject.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Camera.main.pixelHeight - Input.mousePosition.y, Camera.main.nearClipPlane));
            Vector3 ScreenPosition = Input.mousePosition;
            ScreenPosition.z = Camera.main.nearClipPlane + 2;
            DragObject.transform.position = ScreenPosition;
            Debug.Log("Перетягивание" + DragObject.transform.position.ToString() + " / " + DragedObject.transform.position.ToString());
        }
    }
    public void CharactersStatsPanel()
    {
        if (CharactersPanel.activeSelf == true)
        {
            CharactersPanel.SetActiveRecursively(false);
            int ArrayNumber = ButtonArray.Length;
            for (int i = 0; i < ArrayNumber; i++)
            {
                //Debug.Log(ButtonArray[i].tag);
                Destroy(ButtonArray[i].gameObject);
                
            }
            while (InventorySlots.Count != 0)
            {
                Destroy(InventorySlots[0]);
                InventorySlots.RemoveAt(0);
            }
            for (int i = 0; i < BeltSlots.Length; i++)
            {
                Destroy(BeltSlots[i]);
            }
        }
        else
        {
            CharactersPanel.SetActiveRecursively(true);
            DragObject.SetActive(false);
            CharacterItemPanel.GetComponent<CharacterItemPanelScript>().CreateBeltItemSlotButton(BeltSlots);
            CharacterNumber = 0;
            PrintInfo();
            ButtonArray = new GameObject[PlayerControl.Player.Walker.Crew.Count];
            for (int i = 0; i < PlayerControl.Player.Walker.Crew.Count; i++)
            {
                GameObject WarriorButton = Instantiate(Warrior1Button, CharactersPanel.transform);
                WarriorButton.transform.localPosition = new Vector3(- CharactersPanel.GetComponent<RectTransform>().rect.width /2 + WarriorButton.GetComponent<RectTransform>().rect.width /2 + 10 + i * (WarriorButton.GetComponent<RectTransform>().rect.width + 10), CharactersPanel.GetComponent<RectTransform>().rect.height / 2 - WarriorButton.GetComponent<RectTransform>().rect.height / 2 - 10, 0);
                Button B = WarriorButton.GetComponent<Button>();
                ButtonArray[i] = WarriorButton;
                CharacterMenuCharacterButtonScript Script = WarriorButton.GetComponent<CharacterMenuCharacterButtonScript>();
                Script.CharacterNumber = i;
                Script.BoundToOnClick();
                Script.CharactersButton = this;
                WarriorButton.transform.GetChild(1).GetComponent<Image>().sprite = PlayerControl.Player.Walker.Crew[i].CharacterSprite;
                WarriorButton.transform.GetChild(0).GetComponent<Text>().text = PlayerControl.Player.Walker.Crew[i].Name;
                SetStatUpButtonActivity();
            }
            InventoryPanel.GetComponent<InventoryPanelScript>().CreateSlotButtons(InventorySlots);
            InventoryRowMargin = 0;
        }
    }
    public void PrintInfo()
    {
        // CharacterSetting C = PlayerControl.Player.Walker.Crew[CharacterNumber];
        FullNameWarriorText.text = C.Name; // Полное имя выбранного бойца.
        Debug.Log(C.Name);
        Debug.Log(CharacterNumber.ToString());
        RaceDataText.text = C.Race.RaceName; // Раса выбранного бойца.
        StrenghtDataText.text = C.Strenght.Current.ToString(); // Сила выбранного бойца.
        EnduranceDataText.text = C.Endurance.Current.ToString(); // Выносливость выбранного бойца.
        AgilityDataText.text = C.Agility.Current.ToString(); // Проворность выбранного бойца.
        SpeedDataText.text = C.SpeedMax.ToString() + " / " + C.StepCost.Current.ToString(); // Скорость и стоимость шага выбранного бойца.
        HPDataText.text = C.HP.ToString() + " / " + C.HPMax.ToString(); // Здоровье выбранного бойца.
        InjuryThresholdDataText.text = C.InjuryThreshold.Current.ToString(); // Порог ранения выбранного бойца.
        DodgeDataText.text = C.Dodge.Current.ToString(); // Шанс уклонения выбранного бойца.
        //ParryDataText; // Шанс парирования выбранного бойца.
        //DamagDataText; // Урон выбранного бойца.
        AccuracyDataText.text = C.Accuracy[0].Current.ToString(); // Точность выбранного бойца (первая правая рука).
        CritChanceDataText.text = C.CritChance.Current.ToString(); // Шанс критического удара выбранного бойца (первая правая рука).
        InitiativeDataText.text = C.Initiative.Current.ToString();
    }
    public int CharacterNumber
    {   
        set 
        {
            _CharacterNumber = value;
            C = PlayerControl.Player.Walker.Crew[value];
            for (int i = 0; i < 4; i++)
            {
                ArmsSlots[i].SetActive(C.Race.ArmsEnabled[i]);
            }
            FillEquipmentButtons();
        }
        get => _CharacterNumber;
    }
    public void SetStatUpButtonActivity()
    {
        StrenghtUpButton.SetActive((C.LeftStatPoints > 0 && C.Strenght.Max < 7) ? true : false);
        EnduranceUpButton.SetActive((C.LeftStatPoints > 0 && C.Endurance.Max < 7) ? true : false);
        AgilityUpButton.SetActive((C.LeftStatPoints > 0 && C.Agility.Max < 7) ? true : false);

        StrenghtUpButton.GetComponent<StatUpButton>().StatUpButtonText.text = C.LeftStatPoints.ToString();
        EnduranceUpButton.GetComponent<StatUpButton>().StatUpButtonText.text = C.LeftStatPoints.ToString();
        AgilityUpButton.GetComponent<StatUpButton>().StatUpButtonText.text = C.LeftStatPoints.ToString();
    }
    public bool TrySwapItem(GameObject A, GameObject B)
    {
        if (CheckSetItemOnSlot(A, B.GetComponent<ItemSlotButtonPrefubScript>().Item) && CheckSetItemOnSlot(B, A.GetComponent<ItemSlotButtonPrefubScript>().Item))
        {
            return SwapItem(A, B);
        }
        return false;
    }
    public bool SwapItem(GameObject A, GameObject B)
    {
        ItemSetting ItemA = A.GetComponent<ItemSlotButtonPrefubScript>().Item;
        ItemSetting ItemB = B.GetComponent<ItemSlotButtonPrefubScript>().Item;
        SetItemOnSlotWithArmCheck(A, ItemB);
        SetItemOnSlotWithArmCheck(B, ItemA);
        AddItemInInventoryFromBuffer();

        FillInventoryFields();
        FillEquipmentButtons();
        return true;
    }

    public void SetItemOnSlotWithArmCheck(GameObject A, ItemSetting ItemB)// А - целевой слот.
    {
        if (IsArmItemSlot(A.GetComponent<ItemSlotButtonPrefubScript>().ItemSlot) && A.GetComponent<ItemSlotButtonPrefubScript>().Item != null)
        {
            if (A.GetComponent<ItemSlotButtonPrefubScript>().Item.Twohand)
            {
                SetItemOnSlot(GetOpposedArmSlot(A), null);
            }
        }
        if (ItemB != null)
        {
            if (IsArmItemSlot(A.GetComponent<ItemSlotButtonPrefubScript>().ItemSlot) && ItemB.Twohand)
            {
                if (GetOpposedArmSlot(A).GetComponent<ItemSlotButtonPrefubScript>().Item != null)
                {
                    if (!GetOpposedArmSlot(A).GetComponent<ItemSlotButtonPrefubScript>().Item.Twohand)
                    {
                        AddItemInBuffer(GetOpposedArmSlot(A).GetComponent<ItemSlotButtonPrefubScript>().Item);
                    }
                }
                SetItemOnSlot(A, ItemB);
                SetItemOnSlot(GetOpposedArmSlot(A), ItemB);
            }
            else
            {
                SetItemOnSlot(A, ItemB);
            }
        }
        else
        {
            SetItemOnSlot(A, ItemB);
        }
    }
    public void ZeroiseItemSlot(GameObject A)
    {
        if (A.GetComponent<ItemSlotButtonPrefubScript>().Item != null)
        {
            if (IsArmItemSlot(A.GetComponent<ItemSlotButtonPrefubScript>().ItemSlot) && A.GetComponent<ItemSlotButtonPrefubScript>().Item.Twohand)
            {
                if (ReferenceEquals(GetOpposedArmSlot(A).GetComponent<ItemSlotButtonPrefubScript>().Item, A.GetComponent<ItemSlotButtonPrefubScript>().Item))
                {
                    //AddItemInBuffer(GetOpposedArmSlot(A).GetComponent<ItemSlotButtonPrefubScript>().Item);
                    SetItemOnSlot(GetOpposedArmSlot(A), null);
                }
                SetItemOnSlot(A, null);
            }
            else
            {
                SetItemOnSlot(A, null);
            }
        }
    }
    public ItemSetting ItemButtonToItemSlot(GameObject A)
    {
        switch (A.GetComponent<ItemSlotButtonPrefubScript>().ItemSlot)
        {
            case GlobalEnumerators.ItemSlot.RightArm:
                return C.Arms[0];
            case GlobalEnumerators.ItemSlot.LeftArm:
                return C.Arms[1];
            case GlobalEnumerators.ItemSlot.RightArmSecond:
                return C.Arms[2];
            case GlobalEnumerators.ItemSlot.LeftArmSecond:
                return C.Arms[3];
            case GlobalEnumerators.ItemSlot.Armor:
                return C.Armor;
            case GlobalEnumerators.ItemSlot.Inventory:
                if (A.GetComponent<ItemSlotButtonPrefubScript>().NumberItemSlot == -1)
                {
                    PlayerControl.Player.Walker.Inventory.Add(null);
                }
                return PlayerControl.Player.Walker.Inventory[A.GetComponent<ItemSlotButtonPrefubScript>().NumberItemSlot];
            default:
                return null;
        }
    }
    public void AddItemInInventoryFromBuffer()
    {
        for (int i = 0; i < ItemBuffer.Count; i++)
        {
            PlayerControl.Player.Walker.Inventory.Add(ItemBuffer[i]);
        }
        ItemBuffer.Clear();
        PlayerControl.Player.Walker.CheckInventoryOnNull();
    }
    public void AddItemInBuffer(ItemSetting B)
    {
        ItemBuffer.Add(B);
        //Walker.Inventory.Add(B);
    }
    public GameObject GetOpposedArmSlot(GameObject A) => A.GetComponent<ItemSlotButtonPrefubScript>().ItemSlot switch
    {
        GlobalEnumerators.ItemSlot.RightArm => ArmsSlots[1],
        GlobalEnumerators.ItemSlot.LeftArm => ArmsSlots[0],
        GlobalEnumerators.ItemSlot.RightArmSecond => ArmsSlots[3],
        GlobalEnumerators.ItemSlot.LeftArmSecond => ArmsSlots[2],
        _ => null,
    };
    public void SetItemOnSlot(GameObject A, ItemSetting B)
    {
        A.GetComponent<ItemSlotButtonPrefubScript>().Item = B;
        //ItemSetting C = ItemButtonToItemSlot(A);
        //C = B;
        switch (A.GetComponent<ItemSlotButtonPrefubScript>().ItemSlot)
        {
            case GlobalEnumerators.ItemSlot.RightArm:
                C.Arms[0] = B;
                break;
            case GlobalEnumerators.ItemSlot.LeftArm:
                C.Arms[1] = B;
                break;
            case GlobalEnumerators.ItemSlot.RightArmSecond:
                C.Arms[2] = B;
                break;
            case GlobalEnumerators.ItemSlot.LeftArmSecond:
                C.Arms[3] = B;
                break;
            case GlobalEnumerators.ItemSlot.Armor:
                C.Armor = B;
                break;
            case GlobalEnumerators.ItemSlot.Belt:
                C.Belt[A.GetComponent<ItemSlotButtonPrefubScript>().NumberItemSlot] = B;
                break;
            case GlobalEnumerators.ItemSlot.Inventory:
                if (A.GetComponent<ItemSlotButtonPrefubScript>().NumberItemSlot == -1)
                {
                    PlayerControl.Player.Walker.Inventory.Add(B);
                }
                else
                {
                    PlayerControl.Player.Walker.Inventory[A.GetComponent<ItemSlotButtonPrefubScript>().NumberItemSlot] = B;
                }
                break;
        }
    }
    public static bool IsArmItemSlot(GlobalEnumerators.ItemSlot Slot)
    {
        if (Slot == GlobalEnumerators.ItemSlot.RightArm || Slot == GlobalEnumerators.ItemSlot.LeftArm || Slot == GlobalEnumerators.ItemSlot.RightArmSecond || Slot == GlobalEnumerators.ItemSlot.LeftArmSecond)
        {
            return true;
        }
        return false;
    }
    public bool CheckSetItemOnSlot(GameObject A, ItemSetting B) //Возвращает истину, если предмет В можно поместить в слот А.
    {
        if (B == null)
        {
            //Debug.Log("Вернули истину");
            return true;
        }
        switch (A.GetComponent<ItemSlotButtonPrefubScript>().ItemSlot)
        {
            case GlobalEnumerators.ItemSlot.Inventory:
                //Debug.Log("Вернули истину");
                return true;
            case GlobalEnumerators.ItemSlot.RightArm:
                if (B.ItemType == GlobalEnumerators.ItemTypeEnum.Weapon)
                {
                    if (!B.Twohand || (C.Race.ArmsEnabled[1]))
                    {
                        //Debug.Log("Вернули истину");
                        return true;
                    }
                }
                return false;
            case GlobalEnumerators.ItemSlot.LeftArm:
                if (B.ItemType == GlobalEnumerators.ItemTypeEnum.Weapon)
                {
                    if (C.Race.ArmsEnabled[1])
                    {
                        //Debug.Log("Вернули истину");
                        return true;
                    }
                }
                return false;
            case GlobalEnumerators.ItemSlot.RightArmSecond:
                if (B.ItemType == GlobalEnumerators.ItemTypeEnum.Weapon)
                {
                    if (C.Race.ArmsEnabled[2] && (!B.Twohand || (C.Race.ArmsEnabled[3])))
                    {
                        //Debug.Log("Вернули истину");
                        return true;
                    }
                }
                return false;
            case GlobalEnumerators.ItemSlot.LeftArmSecond:
                if (B.ItemType == GlobalEnumerators.ItemTypeEnum.Weapon)
                {
                    if (C.Race.ArmsEnabled[3] && (!B.Twohand || (C.Race.ArmsEnabled[2])))
                    {
                        //Debug.Log("Вернули истину");
                        return true;
                    }
                }
                return false;
            case GlobalEnumerators.ItemSlot.Armor:
                if (B.ItemType == GlobalEnumerators.ItemTypeEnum.Armor)
                {
                    if (!B.ArmorExcludingRaceTypes.Contains(C.Race.RaceType))
                    {
                        //Debug.Log("Вернули истину");
                        return true;
                    }
                }
                return false;
            case GlobalEnumerators.ItemSlot.Belt:
                if (B.ItemType == GlobalEnumerators.ItemTypeEnum.Belt)
                {
                    return true;
                }     
                return false;
            default:
                return false;
        }
    }
    public void FillInventoryFields()
    {
        int NumberOfItems = PlayerControl.Player.Walker.Inventory.Count;
        InventoryRowMarginMax = Mathf.CeilToInt((float)(NumberOfItems + 1 - (InventoryPanelScript.NumberOfButtonsOnHeight * InventoryPanelScript.NumberOfButtonsOnWidth)) / InventoryPanelScript.NumberOfButtonsOnWidth);
        for (int i = 0; i < InventoryPanelScript.NumberOfButtonsOnHeight * InventoryPanelScript.NumberOfButtonsOnWidth; i++)
        {
            if (i + InventoryRowMargin * InventoryPanelScript.NumberOfButtonsOnWidth < NumberOfItems)
            {
                InventorySlots[i].GetComponent<ItemSlotButtonPrefubScript>().Item = PlayerControl.Player.Walker.Inventory[i + InventoryRowMargin * InventoryPanelScript.NumberOfButtonsOnWidth];
                InventorySlots[i].GetComponent<ItemSlotButtonPrefubScript>().NumberItemSlot = i + InventoryRowMargin * InventoryPanelScript.NumberOfButtonsOnWidth;
            }
            else 
            {
                InventorySlots[i].GetComponent<ItemSlotButtonPrefubScript>().Item = null;
                InventorySlots[i].GetComponent<ItemSlotButtonPrefubScript>().NumberItemSlot = -1;
            }
            InventorySlots[i].transform.Find("Text").GetComponent<Text>().text = InventorySlots[i].GetComponent<ItemSlotButtonPrefubScript>().NumberItemSlot.ToString();
        }
    }
    public void FillEquipmentButtons()
    {
        for (int i = 0; i < 4; i++)
        {
            ArmsSlots[i].GetComponent<ItemSlotButtonPrefubScript>().Item = C.Arms[i];
        }
        ArmorSlot.GetComponent<ItemSlotButtonPrefubScript>().Item = C.Armor;
        for (int i = 0; i < BeltSlots.Length; i++)
        {
            if (i >= C.NumberOfBeltSlot)
            {
                BeltSlots[i].SetActive(false);
            }
            else
            {
                BeltSlots[i].SetActive(true);
                BeltSlots[i].GetComponent<ItemSlotButtonPrefubScript>().Item = C.Belt[i];
            }
        }
    }
    public int InventoryRowMargin 
    {
        set
        {
            _InventoryRowMargin = value > InventoryRowMarginMax ? InventoryRowMarginMax : Mathf.Max(0, value);
            FillInventoryFields();//Добавить перезаполнение кнопок инвентаря.
        }
        get => _InventoryRowMargin;
    }
    public int InventoryRowMarginMax
    {
        set
        {
            _InventoryRowMarginMax = Mathf.Max(0, value);
            if (InventoryRowMargin > InventoryRowMarginMax)
            {
                InventoryRowMargin = InventoryRowMarginMax;
            }
        }
        get => _InventoryRowMarginMax;
    }
}
