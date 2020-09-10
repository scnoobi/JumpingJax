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
            Debug.Log(element.name); 
            GameObject newToggle = Instantiate(togglePrefab/*, scrollViewContent*/);
            newToggle.transform.SetParent(scrollViewContent, false);
            newToggle.name = $"{element.name}Toggle";
            Text text = newToggle.GetComponentInChildren<Text>();
            text.text = element.name; 
            Toggle toggle = newToggle.GetComponent<Toggle>();
            InGameUI _inGameUI = inGameUI.GetComponent<InGameUI>(); 
            toggle.onValueChanged.AddListener((value) => element.gameObject.SetActive(value));
        }
    }

    public void SetDefaults()
    {
    }

    
}
