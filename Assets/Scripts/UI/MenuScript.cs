using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
    public Vector2 normalizedMousePosistion;
    public float currentAngle;
    public int selection;
    private int prevSelect;

    public GameObject[] menuItems;

    private MenuItemScript menuItemSc;
    private MenuItemScript prevMenuItemSc;

    public int spellID;

    void Start()
    {
        
    }
    
    void Update()
    {
        normalizedMousePosistion = new Vector2(Input.mousePosition.x - Screen.width / 2, Input.mousePosition.y - Screen.height / 2);

        currentAngle = Mathf.Atan2(normalizedMousePosistion.y, normalizedMousePosistion.x) * Mathf.Rad2Deg;
        currentAngle = (currentAngle + 360) % 360;

        selection = (int)currentAngle / 72;

        if (selection != prevSelect)
        {
            prevMenuItemSc = menuItems[prevSelect].GetComponent<MenuItemScript>();
            prevMenuItemSc.Deselect();
            prevSelect = selection;

            menuItemSc = menuItems[selection].GetComponent < MenuItemScript>();
            menuItemSc.Select();

            spellID = selection;
            Debug.Log(spellID);
        }

        //Debug.Log(currentAngle);
    }
}
