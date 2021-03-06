﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public enum LevelSelectionTab
{
    Hop, Portal, Workshop
}

public class LevelSelectionMenu : MonoBehaviour
{
    public Transform levelButtonParent;
    public GameObject levelObjectPrefab;
    public LevelPreview levelPreview;

    public TabButton hopTabButton;
    public TabButton portalTabButton;
    public TabButton workshopTabButton;

    public List<LevelButton> hopButtonList;
    public List<LevelButton> portalButtonList;
    public List<LevelButton> workshopButtonList;

    public LevelSelectionTab currentTab;

    void Start()
    {
        levelPreview = GetComponentInChildren<LevelPreview>();
        SetupTabButtons();
        LoadButtons(GameManager.Instance.levelDataContainer.levels);
        SetTab(LevelSelectionTab.Hop);
        LoadWorkshopButtons();
    }

    void SetupTabButtons()
    {
        hopTabButton.button.onClick.RemoveAllListeners();
        hopTabButton.button.onClick.AddListener(() => SetTab(LevelSelectionTab.Hop));

        portalTabButton.button.onClick.RemoveAllListeners();
        portalTabButton.button.onClick.AddListener(() => SetTab(LevelSelectionTab.Portal));

        workshopTabButton.button.onClick.RemoveAllListeners();
        workshopTabButton.button.onClick.AddListener(() => SetTab(LevelSelectionTab.Workshop));
    }

    void SetTab(LevelSelectionTab tab)
    {
        hopButtonList.ForEach(x => x.gameObject.SetActive(tab == LevelSelectionTab.Hop));
        portalButtonList.ForEach(x => x.gameObject.SetActive(tab == LevelSelectionTab.Portal));
        workshopButtonList.ForEach(x => x.gameObject.SetActive(tab == LevelSelectionTab.Workshop));


        switch (tab)
        {
            case LevelSelectionTab.Hop:
                hopTabButton.SelectTab();
                portalTabButton.UnselectTab();
                workshopTabButton.UnselectTab();
                break;
            case LevelSelectionTab.Portal:
                hopTabButton.UnselectTab();
                portalTabButton.SelectTab();
                workshopTabButton.UnselectTab();
                break;
            case LevelSelectionTab.Workshop:
                hopTabButton.UnselectTab();
                portalTabButton.UnselectTab();
                workshopTabButton.SelectTab();
                break;
        }

        currentTab = tab;
    }
    
    void LoadButtons(Level[] levels)
    {
        for (int i = 0; i < levels.Length; i++)
        {
            GameObject newLevelButton = Instantiate(levelObjectPrefab, levelButtonParent);
            LevelButton levelButton = newLevelButton.GetComponentInChildren<LevelButton>();

            Level currentLevel = levels[i];
            levelButton.SetupButton(currentLevel);
            levelButton.button.onClick.AddListener(() => OnClickLevel(currentLevel));

            if (levelButton.tab == LevelSelectionTab.Hop)
            {
                hopButtonList.Add(levelButton);
            }
            else if(levelButton.tab == LevelSelectionTab.Portal)
            {
                portalButtonList.Add(levelButton);
            }
            else
            {
                workshopButtonList.Add(levelButton);
            }
        }
    }

    private async void LoadWorkshopButtons()
    {
        List<Level> levels = await GetWorkshopLevels();
        LoadButtons(levels.ToArray());
        // Re-toggle the correct buttons once the levels have loaded from Steam
        SetTab(currentTab);
    }

    private async Task<List<Level>> GetWorkshopLevels()
    {
        List<Steamworks.Ugc.Item> items = await WorkshopManager.DownloadAllSubscribedItems();
        List<Level> toReturn = new List<Level>();
        if (items != null && items.Count > 0)
        {
            foreach(Steamworks.Ugc.Item item in items)
            {
                Level newLevel = ScriptableObject.CreateInstance<Level>();
                newLevel.levelBuildIndex = GameManager.workshopLevelIndex;
                newLevel.filePath = item.Directory;
                newLevel.levelName = item.Title;
                newLevel.fileId = item.Id.Value;
                newLevel.gravityMultiplier = 1;
                newLevel.completionTime = await StatsManager.GetLevelCompletionTime(item.Title);
                if (newLevel.completionTime > 0)
                {
                    newLevel.isCompleted = true;
                }
                if (item.PreviewImageUrl != null && item.PreviewImageUrl != string.Empty)
                {
                    Texture2D texture = await LoadTextureFromUrl(item.PreviewImageUrl);
                    newLevel.previewSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                }
                toReturn.Add(newLevel);
            }
        }

        return toReturn;
    }

    public async Task<Texture2D> LoadTextureFromUrl(string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url, true);

        var r = request.SendWebRequest();

        while (!r.isDone)
        {
            await Task.Delay(10);
        }

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.LogError($"Error downloading texture from url: {url}");
            return new Texture2D(100, 100);
        }

        DownloadHandlerTexture dh = request.downloadHandler as DownloadHandlerTexture;
        dh.texture.name = url;

        return dh.texture;
    }

    public void OnClickLevel(Level level)
    {
        levelPreview.Init(level);
    }
}
