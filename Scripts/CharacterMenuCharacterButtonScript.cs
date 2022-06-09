using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMenuCharacterButtonScript : MonoBehaviour
{
    public int CharacterNumber;
    public StrategicCharactersButton CharactersButton;

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
        //FullNameWarriorText = GameObject.Find("Canvas/CharactersPanel/FullNameWarriorText").GetComponent<Text>();
    }
    
    void Click()
    {
        CharactersButton.CharacterNumber = CharacterNumber;
        CharactersButton.PrintInfo();
        CharactersButton.SetStatUpButtonActivity();
    }
}
