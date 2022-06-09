using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryRowButtonsScript : MonoBehaviour
{
    public bool Down;
    public GameObject CharacterButton;

    // Start is called before the first frame update
    void Start()
    {
        Button btn = this.GetComponent<Button>();
        btn.onClick.AddListener(Click);
    }

    // Update is called once per frame
    void Update()
    {
        float Scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Scroll > 0 && !Down)
        {
            ChangeRow();
        }
        if (Scroll < 0 && Down)
        {
            ChangeRow();
        }
    }
    public void Click()
    {
        ChangeRow();
    }
    public void ChangeRow()
    {
        CharacterButton.GetComponent<StrategicCharactersButton>().InventoryRowMargin += Down ? -1 : 1;
    }
}
