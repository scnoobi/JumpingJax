using System.Collections;
using System.Collections.Generic;   
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public enum ToggleableUIElements
{
    CrosshairToggle, SpeedToggle, TimeToggle, KeyPressedToggle, TutorialToggle
}

public class MiscOptions : MonoBehaviour
{
    public GameObject togglePrefab;
    public Transform scrollViewContent;

    public delegate void OnToggle();
    public static event OnToggle onToggle; 

    void Awake()
    {
        string[] keys = System.Enum.GetNames(typeof(ToggleableUIElements));
        Populate(keys);
    }

    private void Populate(string[] keys /*ToggleableUIElements toggleItem*/)
    {

        for (int i = 0; i < keys.Length; i++)
        {
            GameObject newToggle = Instantiate(togglePrefab, scrollViewContent);
            ToggleItem item = newToggle.GetComponent<ToggleItem>();
            int optionPreferenceValue = GetOptionPreference(keys[i]);
            item.Init(keys[i], optionPreferenceValue == 1 ? true : false);
            item.toggle.onValueChanged.AddListener((value) => onToggle?.Invoke());
        }

        //foreach (ToggleableUIElements element in System.Enum.GetValues(typeof(ToggleableUIElements)))
        //{
        //}
    }

    public static int GetOptionPreference(string key /*ToggleableUIElements element*/)
    {
        switch (key)
        {
            case "CrosshairToggle": /*ToggleableUIElements.CrosshairToggle:*/
                return OptionsPreferencesManager.GetCrosshairToggle();
            case "SpeedToggle": /*ToggleableUIElements.SpeedToggle:*/
                return OptionsPreferencesManager.GetSpeedToggle();
            case "TimeToggle": /*ToggleableUIElements.TimeToggle:*/
                return OptionsPreferencesManager.GetTimeToggle();
            case "KeyPressedToggle": /*ToggleableUIElements.KeyPressedToggle:*/
                return OptionsPreferencesManager.GetKeyPressedToggle();
            case "TutorialToggle": /*ToggleableUIElements.TutorialToggle:*/
                return OptionsPreferencesManager.GetTutorialToggle();
            default:
                return 0;
        }
    }

    public void SetDefaults()
    {
    }

    private void Update()
    {
    }
}
