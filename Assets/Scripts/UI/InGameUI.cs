using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    // Time
    public Text completionTimeText;

    // Speed
    public SpeedSlider speed;

    // Crosshair 
    public Crosshair crosshair;

    // KeyPressed
    public KeyPressed keyPressed; 

    // Tutorial
    public Text tutorialText;
    public Text tutorialNextText;
    public GameObject tutorialPane;
    private string[] tutorialTexts;
    private int tutorialTextIndex = 0;

    public GameObject container;
    public PlayerMovement playerMovement;

    // For comparison in Update
    //private int crosshairToggleStartingValue;
    //private int speedToggleStartingValue;
    //private int timeToggleStartingValue;
    //private int keyPressedToggleStartingValue;
    //private int tutorialToggleStartingValue;

    //private Dictionary<string, int> togglePreferences = new Dictionary<string, int>()
    //{
    //    { "CrosshairToggle", OptionsPreferencesManager.GetCrosshairToggle() },
    //    { "SpeedToggle", OptionsPreferencesManager.GetSpeedToggle() },
    //    { "TimeToggle", OptionsPreferencesManager.GetTimeToggle() },
    //    { "KeyPressedToggle", OptionsPreferencesManager.GetKeyPressedToggle() },
    //    { "TutorialToggle", OptionsPreferencesManager.GetTutorialToggle() },
    //};

    private void Awake()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
        speed = GetComponentInChildren<SpeedSlider>();
        crosshair = GetComponentInChildren<Crosshair>();
        keyPressed = GetComponentInChildren<KeyPressed>();

        tutorialTexts = GameManager.GetCurrentLevel().tutorialTexts;
        LoadNextTutorial();

        SetToggleStartingValues();

        //speedToggleStartingValue = OptionsPreferencesManager.GetSpeedToggle();
        //speed.gameObject.SetActive(speedToggleStartingValue == 1 ? true : false);
        
        //container.SetActive(OptionsPreferencesManager.get)
    }

    void Update()
    {
        if(GameManager.Instance != null)
        {
            TimeSpan time = TimeSpan.FromSeconds(GameManager.Instance.currentCompletionTime);
            completionTimeText.text = time.ToString("hh':'mm':'ss");
        }

        //fpsText.text = "FPS: " + Mathf.Round(1 / Time.deltaTime);
        Vector2 directionalSpeed = new Vector2(playerMovement.newVelocity.x, playerMovement.newVelocity.z);
        speed.SetSpeed(directionalSpeed.magnitude);

        if (Input.GetKeyDown(PlayerConstants.NextTutorial))
        {
            LoadNextTutorial();
        }
        else if (InputManager.GetKeyUp(PlayerConstants.ToggleUI)) 
        {
            ToggleUI();
        }

        // This is hideous 
        bool newCrosshairTogglePref = OptionsPreferencesManager.GetCrosshairToggle() == 1 ? true : false;
        bool newSpeedTogglePref = OptionsPreferencesManager.GetSpeedToggle() == 1 ? true : false;
        bool newTimeTogglePref = OptionsPreferencesManager.GetTimeToggle() == 1 ? true : false;
        bool newKeyPressedTogglePref = OptionsPreferencesManager.GetKeyPressedToggle() == 1 ? true : false;
        bool newTutorialTogglePref = OptionsPreferencesManager.GetTutorialToggle() == 1 ? true : false;

        if (newCrosshairTogglePref != crosshair.gameObject.activeSelf)
        {
            crosshair.gameObject.SetActive(newCrosshairTogglePref);
        }
        if (newSpeedTogglePref != speed.gameObject.activeSelf)
        {
            speed.gameObject.SetActive(newSpeedTogglePref);
        }
        if (newTimeTogglePref != completionTimeText.gameObject.activeSelf)
        {
            completionTimeText.gameObject.SetActive(newTimeTogglePref);
        }
        if (newKeyPressedTogglePref != keyPressed.gameObject.activeSelf)
        {
            keyPressed.gameObject.SetActive(newKeyPressedTogglePref);
        }
        if (newTutorialTogglePref != tutorialText.gameObject.activeSelf)
        {
            tutorialText.gameObject.SetActive(newTutorialTogglePref);
        }
    }

    private void LoadNextTutorial()
    {
        if(tutorialTextIndex < tutorialTexts.Length)
        {
            tutorialPane.SetActive(true);
            tutorialText.text = tutorialTexts[tutorialTextIndex].Replace("<br>", "\n");
            Invoke("UpdateParentLayoutGroup", 0.1f);
            tutorialTextIndex++;
        }
        else
        {
            tutorialPane.SetActive(false);
        }
    }

    void UpdateParentLayoutGroup()
    {
        tutorialText.gameObject.SetActive(false);
        tutorialText.gameObject.SetActive(true);

        tutorialNextText.gameObject.SetActive(false);
        tutorialNextText.gameObject.SetActive(true);
    }
    private void ToggleUI()
    {
        container.SetActive(!container.activeSelf);
    }

    // My attempts to make this dynamic were sloppy...
    private void UpdateToggleValues()
    {
    }

    private bool PreferenceHasChanged(string key)
    {
        return MiscOptions.GetOptionPreference(key) == 1 ? true : false; 
    }

    //private bool UpdateActiveFlags(GameObject element)
    //{
    //    foreach (string key in System.Enum.GetNames(typeof(ToggleableUIElements)))
    //    {
    //        bool preferenceValue = MiscOptions.GetOptionPreference(key) == 1 ? true : false; 
    //        if (element.activeSelf != preferenceValue)
    //        {
    //            element.gameObject.SetActive(!element.activeSelf); 
    //        }
    //    }
    //}

    private void SetToggleStartingValues()
    {
        crosshair.gameObject.SetActive(OptionsPreferencesManager.GetCrosshairToggle() == 1 ? true : false);
        speed.gameObject.SetActive(OptionsPreferencesManager.GetSpeedToggle() == 1 ? true : false);
        completionTimeText.gameObject.SetActive(OptionsPreferencesManager.GetTutorialToggle() == 1 ? true : false);
        keyPressed.gameObject.SetActive(OptionsPreferencesManager.GetKeyPressedToggle() == 1 ? true : false);
        tutorialText.gameObject.SetActive(OptionsPreferencesManager.GetTutorialToggle() == 1 ? true : false);

        //foreach (Transform element in container.transform)
        //{
        //}
    }
}
