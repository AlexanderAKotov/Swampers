using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlotButtonPrefubScript : MonoBehaviour, IPointerClickHandler, IDragHandler,
  IPointerEnterHandler, IPointerExitHandler,
  IEndDragHandler
{
    private ItemSetting _Item;
    public int NumberItemSlot;
    public GlobalEnumerators.ItemSlot ItemSlot;
    public GameObject CharacterButton;

    // Start is called before the first frame update
    void Start()
    {
        CharacterButton = GameObject.Find("CharactersMenuButton");
    }

    // Update is called once per frame
    void Update()
    {

    }
    public ItemSetting Item 
    {
        set
        {
            if (value == null)
            {
                _Item = null;
                GetComponent<Image>().sprite = null;
            }
            else
            {
                _Item = value;
                if (StrategicCharactersButton.IsArmItemSlot(ItemSlot) && _Item.Twohand)
                {
                    if (ItemSlot == GlobalEnumerators.ItemSlot.RightArm || ItemSlot == GlobalEnumerators.ItemSlot.RightArmSecond)
                    {
                        GetComponent<Image>().sprite = _Item.IconTop;
                    }
                    else
                    {
                        GetComponent<Image>().sprite = _Item.IconBottom;
                    }
                }
                else
                {
                    GetComponent<Image>().sprite = _Item.GetPrimarySprite();
                }
            }
        }
        get => _Item;
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (Item != null && !StrategicCharactersButton.OnDrag)
        {
            StrategicCharactersButton.OnDrag = true;
            StrategicCharactersButton.DragedObject = gameObject;
            StrategicCharactersButton.DragObject.SetActive(true);
            StrategicCharactersButton.DragObject.GetComponent<Image>().sprite = Item.GetPrimarySprite();
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        StrategicCharactersButton.DragObject.SetActive(false);
        if (StrategicCharactersButton.MouseOnSlot != null && StrategicCharactersButton.OnDrag == true)
        {
            CharacterButton.GetComponent<StrategicCharactersButton>().TrySwapItem(StrategicCharactersButton.MouseOnSlot, StrategicCharactersButton.DragedObject);
        }
        StrategicCharactersButton.OnDrag = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (gameObject != StrategicCharactersButton.DragObject)
        {
            StrategicCharactersButton.MouseOnSlot = gameObject;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (gameObject != StrategicCharactersButton.DragObject)
        {
            StrategicCharactersButton.MouseOnSlot = null;
        }
    }
}
