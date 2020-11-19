using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuItemScript : MonoBehaviour
{
    public Color baseColor;
    public Color hoverColor;
    public Image background;

    public Text nameDisplay;
    public string runeName;
    public Image currentSpell;
    public Sprite icon;

    void Start()
    {
        background.color = baseColor;
    }
    
    public void Select()
    {
        nameDisplay.text = runeName;
        background.color = hoverColor;
    }

    public void Deselect()
    {
        background.color = baseColor;
    }

    public void SetCurrentSpell()
    {
        currentSpell.sprite = icon;
    }
}
