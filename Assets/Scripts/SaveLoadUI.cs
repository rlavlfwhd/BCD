using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SaveLoadUI : MonoBehaviour
{
    public Button[] saveButtons;
    public Button[] loadButtons;
    public Image[] slotImages;
    public GameObject OptionPanel;
    public CameraParallax cameraParallax;

    void Start()
    {
        for(int i = 0; i < saveButtons.Length; i++)
        {
            int slot = i + 1;
            saveButtons[i].onClick.AddListener(() => SaveGame(slot));
            loadButtons[i].onClick.AddListener(() => LoadGame(slot));
        }

        UpdateSlotImages();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OptionPanel.SetActive(!OptionPanel.activeSelf);
            cameraParallax.enabled = !OptionPanel.activeSelf;
        }
    }

    public void SaveGame(int slot)
    {
        GameSystem.Instance.SaveGame(slot);
        UpdateSlotImages();
    }

    public void LoadGame(int slot)
    {
        GameSystem.Instance.LoadGame(slot);
        UpdateSlotImages();
    }

    private void UpdateSlotImages()
    {
        for (int i = 0; i < slotImages.Length; i++)
        {
            int slot = i + 1;
            SaveSystem.SaveData saveData = SaveSystem.LoadGame(slot);

            if (saveData != null && !string.IsNullOrEmpty(saveData.mainImagePath))
            {
                Sprite loadedSprite = LoadSpriteFromPath(saveData.mainImagePath);
                if (loadedSprite != null)
                {
                    slotImages[i].sprite = loadedSprite;
                }
            }
        }
    }

    private Sprite LoadSpriteFromPath(string path)
    {
        if (!File.Exists(path)) return null;

        byte[] imageBytes = File.ReadAllBytes(path);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(imageBytes);
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }

    public void OptionBtnClick()
    {
        OptionPanel.SetActive(true);
    }
}