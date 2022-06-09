using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SityControl : MonoBehaviour
{
    public GameObject Panel;
    public GameObject SityPanel;
    // Start is called before the first frame update
    void Start()
    {
        Button btn = this.GetComponent<Button>();
        switch (name)
        {
            case "ExitSityButton":
                btn.onClick.AddListener(ExitSity);
                break;
            case "TacticalMapButton":
                btn.onClick.AddListener(TacticalMapLoad);
                break;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    private void ExitSity()
    {
        PlayerControl.MouseOnPanel = false;
        SityPanel.SetActive(false);
        Panel.SetActive(true);
    }
    private void TacticalMapLoad()
    {
        PlayerControl.GlobalMapIsActive = false;
        SceneManager.LoadScene(2);
    }
}
