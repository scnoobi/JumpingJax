using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleItem : MonoBehaviour
{
    public GameObject uiElement; // rn 
    public MiscOptions miscOptions;
    //private MiscOptions miscOptions;

    // rename
    private Toggle toggle; 
    private Button button;
    private Text text;
    private bool on;

    //public Text itemText;
    public Text itemText; 
    public Toggle itemToggle;
    public Text toggleText;

    void Start()
    {
        button = GetComponent<Button>();
        //miscOptions = GetComponentInParent<MiscOptions>();
    }

    void Select()
    {
        if (on)
        {
            text.text = "Off"; 
        }
        else
        {
            text.text = "On"; 
        }
    }

    public void SetItemText(string text)
    {
        itemText.text = text;
    }

    public void SetToggleText(string text)
    {
        toggleText.text = text;
    }

    public Text GetToggleText()
    {
        return toggleText;
    }

    void Update()
    {
        
    }
}
