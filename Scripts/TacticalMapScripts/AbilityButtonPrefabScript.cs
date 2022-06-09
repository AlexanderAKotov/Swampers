using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AbilityButtonPrefabScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private AbilitySetting _Ability;
    public GlobalEnumerators.ItemSlot ItemSlot;
    public ItemSetting Item;
    public int ArmNumber;
    private bool _Enabled;
    public bool Visible;
    public float TotalSpeedCost;
    public float AdditionSpeedCost;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BoundToOnClick()
    {
        Button Btn = GetComponent<Button>();
        Btn.onClick.AddListener(Click);
    }
    public void Click()
    {
        if (Enabled)
        {
            if (Ability.TriggerType != GlobalEnumerators.AbilityTriggerType.Instance)
            {
                if (MainBattleScript.ActiveAbility.AbilityButton == gameObject)
                {
                    MainBattleScript.SetActiveAbility(null);
                }
                else
                {
                    MainBattleScript.SetActiveAbility(gameObject);
                }
            }
            else
            {
                MainBattleScript.SetActiveAbility(gameObject);
                Ability.AbilityProcedure(MainBattleScript.ActiveC, new Vector3Int(0, 0, 0), MainBattleScript.ActiveAbility);
                MainBattleScript.SetActiveAbility(null);
                MainBattleScript.ResetAllAbilitiesEnabilities();
                MainBattleBehaviour.ThisScript.PrintInfo(MainBattleScript.SelectedCharacter);
            }
        }
    }
    public AbilitySetting Ability
    {
        set
        {
            _Ability = value;
            AdditionSpeedCost = 0;
            if (value != null)
            {
                SetEnability();
                gameObject.SetActive(true);
                Visible = true;
            }
            else
            {
                gameObject.SetActive(false);
                Visible = false;
            }
        }
        get => _Ability;
    }
    public bool Enabled
    {
        set
        {
            _Enabled = value;
            if (Ability.UseItemIcon)
            {
                if (Item != null)
                {
                    if (Item.GetActivation())
                    {
                        gameObject.GetComponent<Image>().sprite = Item.IconActivated;
                    }
                    else
                    {
                        gameObject.GetComponent<Image>().sprite = Item.IconTop;
                    }
                }
                else
                {
                    gameObject.GetComponent<Image>().sprite = null;
                }
            }
            else
            {
                if (value)
                {
                    gameObject.GetComponent<Image>().sprite = Ability.Icon;
                }
                else
                {
                    gameObject.GetComponent<Image>().sprite = Ability.IconLocked;
                }
            }
        }
        get => _Enabled;
    }
    public void SetEnability()
    {
        /* Нет скорости
     Закончились снаряды
     В зависимости от поломки и ее влияния на доступность продвинутого удара
        */
        //Item = null;
        CharacterSetting C = MainBattleScript.ActiveC;
        if (StrategicCharactersButton.IsArmItemSlot(ItemSlot))
        {
            Item = C.GetWeapon(CharacterSetting.ArmFromItemSlot(ItemSlot));
        }
        TotalSpeedCost = Ability.SpeedCost;
        if (Item != null)
        {
            TotalSpeedCost += Item.SpeedCostBase;
            if (Item.ItemType == GlobalEnumerators.ItemTypeEnum.Weapon)
            {
                TotalSpeedCost += MainBattleScript.ActiveC.AttackCost.Current;
            }
        }
        if (Ability.CostlyReuse)
        {
            TotalSpeedCost += AdditionSpeedCost;
        }

        Enabled = C.SpeedCurrent >= TotalSpeedCost;
    }
    public void RaiseAdditionSpeedCost()
    {
        AdditionSpeedCost++;
        SetEnability();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        string TooltipText = Ability.AbilityTooltipProcedure(MainBattleScript.ActiveC, gameObject);
        MainBattleBehaviour.ThisScript.AbilityTooltipPanel.transform.Find("AbilityTooltipText").GetComponent<Text>().text = TooltipText;
        MainBattleBehaviour.ThisScript.AbilityTooltipPanel.SetActive(true);
        float AbilityButtonsPositionX = gameObject.transform.localPosition.x;
        float AbilityButtonsPositionY = gameObject.transform.localPosition.y;
        MainBattleBehaviour.ThisScript.AbilityTooltipPanel.transform.localPosition = new Vector3(AbilityButtonsPositionX, AbilityButtonsPositionY + 50, 0);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        MainBattleBehaviour.ThisScript.AbilityTooltipPanel.SetActive(false);
    }
}