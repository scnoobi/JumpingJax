using System.Collections;
using System.Collections.Generic;   
using UnityEngine;
using UnityEngine.UI;

public class MiscOptions : MonoBehaviour
{
    public GameObject togglePrefab;
    public Transform scrollViewContent;
    public GameObject inGameUIPrefab;
    public InGameUI inGameUI;
    public GameObject inGameUIContainer; 
    //private GameObject inGameUIObject; 
    private Transform container; 

    void Start()
    {
        //inGameUIObject = GameObject.Find("InGameUI"); 
        //container = inGameUIPrefab.transform.Find("Container");
        PopulateToggles();
        inGameUI.container.transform.Find("SpeedContainer").gameObject.SetActive(false);
    }

    private void PopulateToggles()
    {
        foreach (Transform element in inGameUIContainer.transform)
        {
            Debug.Log(element.name);
            GameObject newToggle = Instantiate(togglePrefab);
            newToggle.transform.SetParent(scrollViewContent, false);
            newToggle.name = $"{element.name}Toggle";
            Text text = newToggle.GetComponentInChildren<Text>();
            text.text = element.name;
            Toggle toggle = newToggle.GetComponent<Toggle>();
            toggle.isOn = element.gameObject.activeSelf ? true : false;
            toggle.onValueChanged.AddListener((value) =>
            {
                Debug.Log(element.name + " changed to " + value);
                element.gameObject.SetActive(value);
            });
        }
    }

    public void SetDefaults()
    {
    }
}
