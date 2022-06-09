using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterItemPanelScript : MonoBehaviour
{
    public GameObject ItemSlotButtonPrefub;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CreateBeltItemSlotButton(GameObject[] Belt)
    {
        float PanelHeight = this.gameObject.GetComponent<RectTransform>().rect.height; //Получаем высоту панели
        float PanelWidth = this.gameObject.GetComponent<RectTransform>().rect.width; //Получаем ширину панели
        int Side = InventoryPanelScript.InventorySlotButtonSide;
        int Margin = InventoryPanelScript.InventorySlotButtonMargin;
        
        for (int i = 0; i < Belt.Length; i++)
        {
            GameObject NewButton = Instantiate(ItemSlotButtonPrefub, gameObject.transform);
            NewButton.transform.localPosition = new Vector3((i % 4) * (Side + Margin) + Margin + Side / 2 - PanelWidth / 2, (i > 3 ? 0 : 1) * (Side + Margin) + Margin + Side / 2 - PanelHeight / 2, 0);
            NewButton.GetComponent<ItemSlotButtonPrefubScript>().ItemSlot = GlobalEnumerators.ItemSlot.Belt;
            NewButton.GetComponent<ItemSlotButtonPrefubScript>().NumberItemSlot = i;
            Belt[i] = NewButton;
        }
    }
}
