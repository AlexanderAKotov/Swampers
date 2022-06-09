using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TacticalMenu : MonoBehaviour
{
    public GameObject TacticalMenuPanel;
    public GameObject WarriorStats;
    public GameObject ItemStats;

    // Start is called before the first frame update
    void Start()
    {
        Button btn = this.GetComponent<Button>();
        switch (name)
        {
            case "MenuWarriorButton":
                btn.onClick.AddListener(WarriorStatsPanel);
                break;
            case "MenuItemButton":
                btn.onClick.AddListener(ItemStatsPanel);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void WarriorStatsPanel()
    {
        if (WarriorStats.activeSelf == true)
        {
            WarriorStats.SetActive(false);
        }
        else
        {
            WarriorStats.SetActive(true);
        }
    }

    private void ItemStatsPanel()
    {
        if (ItemStats.activeSelf == true)
        {
            ItemStats.SetActive(false);
        }
        else
        {
            ItemStats.SetActive(true);
        }
    }
}
