using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatUpButton : MonoBehaviour
{
    public Text StatUpButtonText;
    public StrategicCharactersButton CharactersButton;
    public GlobalEnumerators.StatEnum Stat;

    // Start is called before the first frame update
    void Start()
    {
        Button btn = this.GetComponent<Button>();
        btn.onClick.AddListener(Click);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Click()
    {
        CharactersButton.C.LeftStatPoints--;
        CharactersButton.C.StatUp(Stat, 1);
        CharactersButton.SetStatUpButtonActivity();
        CharactersButton.PrintInfo();
    }
}
