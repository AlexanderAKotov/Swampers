using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTurnBattleScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(Click);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Click()
    {
        if (MainBattleScript.ActiveC.ControlledByPlayer)
        {
            MainBattleScript.EndTurn();
        }
    }
}
