using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SaveLoadUI : MonoBehaviour
{
    public static SaveLoadUI Instance;

    public Button[] saveButtons;
    public Button[] loadButtons;
    public Image[] saveSlotImages1;
    public Image[] saveSlotImages2;
    public Image[] loadSlotImages1;
    public Image[] loadSlotImages2;    


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 필요 시
        }
        else
        {
            Destroy(gameObject);
        }
    }


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
        for (int i = 0; i < saveSlotImages1.Length; i++)
        {
            int slot = i + 1;
            SaveSystem.SaveData saveData = SaveSystem.LoadGame(slot);

            // 저장용 첫 번째 이미지 (퍼즐이면 puzzleImagePath 사용)
            if (saveData != null)
            {
                string imagePath = saveData.sceneName.StartsWith("P") ? saveData.puzzleImagePath : saveData.mainImagePath;

                if (!string.IsNullOrEmpty(imagePath))
                {
                    Sprite loadedSprite = LoadSpriteFromPath(imagePath);
                    if (loadedSprite != null && saveSlotImages1.Length > i)
                    {
                        saveSlotImages1[i].sprite = loadedSprite;
                    }
                }
            }

            // 저장용 두 번째 이미지는 그대로 유지
            if (saveData != null && !string.IsNullOrEmpty(saveData.mainImagePath2))
            {
                Sprite loadedSprite2 = LoadSpriteFromPath(saveData.mainImagePath2);
                if (loadedSprite2 != null && saveSlotImages2.Length > i)
                {
                    saveSlotImages2[i].sprite = loadedSprite2;
                }
            }

            // 불러오기용 첫 번째 이미지도 동일하게 처리
            if (saveData != null)
            {
                string imagePath = saveData.sceneName.StartsWith("P") ? saveData.puzzleImagePath : saveData.mainImagePath;

                if (!string.IsNullOrEmpty(imagePath))
                {
                    Sprite loadedSprite = LoadSpriteFromPath(imagePath);
                    if (loadedSprite != null && loadSlotImages1.Length > i)
                    {
                        loadSlotImages1[i].sprite = loadedSprite;
                    }
                }
            }

            // 불러오기용 두 번째 이미지는 그대로 유지
            if (saveData != null && !string.IsNullOrEmpty(saveData.mainImagePath2))
            {
                Sprite loadedSprite2 = LoadSpriteFromPath(saveData.mainImagePath2);
                if (loadedSprite2 != null && loadSlotImages2.Length > i)
                {
                    loadSlotImages2[i].sprite = loadedSprite2;
                }
            }

            // 버튼 썸네일도 업데이트
            if (loadButtons[i].GetComponent<Image>() != null && loadSlotImages1.Length > i)
            {
                loadButtons[i].GetComponent<Image>().sprite = loadSlotImages1[i].sprite;
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
}