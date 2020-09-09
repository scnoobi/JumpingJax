using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiscOptions : MonoBehaviour
{
    public GameObject[] toggleableUIElements; 
    public GameObject togglePrefab;
    public GameObject inGameUI;
    public Transform inGameUIContainer;
    public Transform scrollViewContent;

    void Start()
    {
        PopulateToggles(); 
    }

    private void PopulateToggles()
    {
        foreach (Transform element in inGameUIContainer)
        {
            GameObject newToggle = Instantiate(togglePrefab);
            newToggle.transform.parent = scrollViewContent;
            newToggle.name = $"{element.name}Toggle";
            Text text = newToggle.GetComponentInChildren<Text>();
            text.text = element.name; 
            Toggle toggle = newToggle.GetComponent<Toggle>();
            InGameUI _inGameUI = inGameUI.GetComponent<InGameUI>(); 
            toggle.onValueChanged.AddListener((value) => _inGameUI.ToggleIndividual(element.gameObject));
        }
    }

    public void SetDefaults()
    {
    }

    
}
