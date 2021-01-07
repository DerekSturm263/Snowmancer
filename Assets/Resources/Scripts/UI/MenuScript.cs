using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    public Vector2 normalizedMousePosistion;
    public float currentAngle;
    public int angleOffset;
    public int selection;
    private int prevSelect;

    public GameObject[] menuItems;

    private MenuItemScript menuItemSc;
    private MenuItemScript prevMenuItemSc;

    public Image mouseBar;

    public int spellID;

    void Start()
    {
        
    }
    
    void Update()
    {
        normalizedMousePosistion = new Vector2(Input.mousePosition.x - Screen.width / 2, Input.mousePosition.y - Screen.height / 2);

        currentAngle = Mathf.Atan2(normalizedMousePosistion.y, normalizedMousePosistion.x) * Mathf.Rad2Deg;
        currentAngle = (currentAngle + 360) % 360;

        mouseBar.transform.rotation = Quaternion.Euler(0, 0, currentAngle + 180);

        if((currentAngle >= 342 && currentAngle <= 360) || (currentAngle >= 0 && currentAngle <= 54)){ selection = 0; }
        if(currentAngle >= 54  && currentAngle <= 126){ selection = 1; }
        if(currentAngle >= 126 && currentAngle <= 198){ selection = 2; }
        if(currentAngle >= 198 && currentAngle <= 270){ selection = 3; }
        if(currentAngle >= 270 && currentAngle <= 342){ selection = 4; }
        //selection = (int)currentAngle / 72;

        if (selection != prevSelect)
        {
            prevMenuItemSc = menuItems[prevSelect].GetComponent<MenuItemScript>();
            prevMenuItemSc.Deselect();
            prevSelect = selection;

            menuItemSc = menuItems[selection].GetComponent < MenuItemScript>();
            menuItemSc.Select();

            spellID = selection;
            
            //Debug.Log(spellID);
        }

        //Debug.Log(currentAngle);
    }
}
