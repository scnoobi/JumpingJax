using System.Collections;
using System.Collections.Generic;   
using UnityEngine;
using UnityEngine.UI;

public class MiscOptions : MonoBehaviour
{
    public GameObject togglePrefab;
    public Transform scrollViewContent;

    private InGameUI inGameUI;

    void Awake()
    {
        inGameUI = GameObject.Find("PortalBhopPlayer").GetComponentInChildren<InGameUI>();
        PopulateToggles();
    }

    private void PopulateToggles()
    {
        foreach (Transform element in inGameUI.container.transform)
        {
            GameObject newToggle = Instantiate(togglePrefab);
            newToggle.transform.SetParent(scrollViewContent, false);
            newToggle.name = $"{element.name}Toggle";
            Text text = newToggle.GetComponentInChildren<Text>();
            text.text = element.name;
            Toggle toggle = newToggle.GetComponent<Toggle>();
            toggle.isOn = element.gameObject.activeSelf ? true : false;
            toggle.onValueChanged.AddListener((value) => element.gameObject.SetActive(value));
        }
    }

    public void SetDefaults()
    {
    }
}
