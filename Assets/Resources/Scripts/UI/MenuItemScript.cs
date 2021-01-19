using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuItemScript : MonoBehaviour
{
    public Color baseColor;
    public Color hoverColor;
    public Color lockedBaseColor;
    public Color lockedHoverColor;
    public Image background;

    public TMPro.TMP_Text nameDisplay;
    public string runeName;
    public Image currentSpell;
    public Sprite icon;

    public bool isLocked = true;

    void Start()
    {
        if (isLocked)
        {
            background.color = lockedBaseColor;
        }
        else
        {
            background.color = baseColor;
        }
    }
    
    public void Select()
    {
        if (!isLocked)
        {
            nameDisplay.text = runeName;
            background.color = hoverColor;
        } else
        {
            nameDisplay.text = "LOCKED";
            background.color = lockedHoverColor;
        }
    }

    public void Deselect()
    {
        if (isLocked)
        {
            background.color = lockedBaseColor;
        }
        else
        {
            background.color = baseColor;
        }
    }

    public void SetCurrentSpell()
    {
        if(!isLocked)
            currentSpell.sprite = icon;
    }
    
}
