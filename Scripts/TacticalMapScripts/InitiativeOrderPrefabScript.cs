using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitiativeOrderPrefabScript : MonoBehaviour
{
    private int _CharacterNumber;
    private bool _Visibility; //Отвечает за отображение или неотображение кнопки персонажа на панели инициативы.
    public Text CharacterName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public int CharacterNumber
    {
        set 
        {
            _CharacterNumber = value;
            if (value == -1)
            {
                Visibility = false;
            }
            else
            {
                PrintInfo();
                Visibility = true;
            }
        }
        get => _CharacterNumber;
    }
    public bool Visibility
    {
        set
        {
            _Visibility = value;
            gameObject.SetActive(value);
        }
        get => _Visibility;
    }
    public void PrintInfo()
    {
        gameObject.transform.Find("CharacterIcon").GetComponent<Image>().sprite = MainBattleScript.TurnOrder[CharacterNumber].C.CharacterSprite;
        gameObject.transform.Find("CharacterNameText").GetComponent<Text>().text = MainBattleScript.TurnOrder[CharacterNumber].C.Name;
        gameObject.transform.Find("CurrentInitiativeText").GetComponent<Text>().text = MainBattleScript.TurnOrder[CharacterNumber].CurrentInitiative.ToString();
        if (MainBattleScript.TurnOrder[CharacterNumber].C.BattleSide != 0)
        {
            gameObject.GetComponent<Image>().color = new Color(1, 0, 0, 1);
        }
        else
        {
            gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }
    }
    public void BoundToOnClick()
    {
        Button Btn = GetComponent<Button>();
        Btn.onClick.AddListener(Click);
    }
    public void Click()
    {
        MainBattleScript.SetSelectedCharacterIndex(CharacterNumber);
    }
}