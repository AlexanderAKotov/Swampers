using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPanelScript : MonoBehaviour
{
    public List<GameObject> InventorySlotButtonList = new List<GameObject>();
    public GameObject SlotButtonPrefub;
    public static int InventorySlotButtonSide = 60; // �������� ������� ������ �������� �� ������ ���������
    public static int InventorySlotButtonMargin = 5; // ������ ������ ��������� ���� �� ����� � �� �������� ���� ������ ���������
    public static int InventoryScrollButtonWidth = 22;
    public static int NumberOfButtonsOnHeight;
    public static int NumberOfButtonsOnWidth;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void CreateSlotButtons(List<GameObject> A)
    {
        float PanelHeight = this.gameObject.GetComponent<RectTransform>().rect.height; //�������� ������ ������
        float PanelWidth  = this.gameObject.GetComponent<RectTransform>().rect.width; //�������� ������ ������
        NumberOfButtonsOnHeight = Mathf.FloorToInt((PanelHeight - InventorySlotButtonMargin) / (InventorySlotButtonSide + InventorySlotButtonMargin));// �������� ���������� ������, ������� ����� ���������� �� ������ ��������� �� ������
        NumberOfButtonsOnWidth  = Mathf.FloorToInt((PanelWidth - InventorySlotButtonMargin - InventoryScrollButtonWidth) / (InventorySlotButtonSide + InventorySlotButtonMargin));// �������� ���������� ������, ������� ����� ���������� �� ������ ��������� �� ������
        for (int i = 0; i < NumberOfButtonsOnHeight; i++)
        {
            for (int j = 0; j < NumberOfButtonsOnWidth; j++)
            {
                GameObject NewButton = Instantiate(SlotButtonPrefub, gameObject.transform);
                NewButton.GetComponent<ItemSlotButtonPrefubScript>().ItemSlot = GlobalEnumerators.ItemSlot.Inventory;
                NewButton.transform.localPosition = new Vector3(- PanelWidth / 2 + InventorySlotButtonMargin + j * (InventorySlotButtonMargin + InventorySlotButtonSide) + InventorySlotButtonSide / 2, PanelHeight / 2  - InventorySlotButtonMargin - i * (InventorySlotButtonMargin + InventorySlotButtonSide) - InventorySlotButtonSide / 2, 0);
                A.Add(NewButton);
            }
        }

    }
}
