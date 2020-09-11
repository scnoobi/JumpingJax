using System.Collections;
using System.Collections.Generic;   
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MiscOptions : MonoBehaviour
{
    public GameObject togglePrefab;
    public Transform scrollViewContent;

    public ToggleItem speedToggle;
    public ToggleItem timeToggle;
    public ToggleItem keyPressedToggle;
    public ToggleItem tutorialToggle;

    void Awake()
    {
        Populate();
    }

    private void Populate()
    {
        GameObject newToggle = Instantiate(togglePrefab, scrollViewContent);
        ToggleItem item = newToggle.GetComponent<ToggleItem>();
        //item.Init("speed", OptionsPreferencesManager.get)
    }

    public void SetDefaults()
    {
    }
}
