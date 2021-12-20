using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using anogamelib;
using UnityEngine.SceneManagement;
using System;
#pragma warning disable CS0649
#pragma warning disable CS0414

public class SystemSave : gamesystem.GameSystem
{
    [SerializeField]
    private SaveGame cachedSaveData;

    [SerializeField]
    private EventSave onGameSave;

    [SerializeField]
    private EventSave onGameLoad;

    [SerializeField]
    private IntReference saveSlot;
    [SerializeField]
    private StringReference playerName;
    [System.NonSerialized]
    private bool isNewGame;

    public void ReqLoad()
    {
        onGameLoad?.Invoke(cachedSaveData);
    }
    public void ReqSave()
    {
        onGameSave?.Invoke(cachedSaveData);
        WriteSaveToFile();
    }

    private void Start()
    {
        Debug.Log(cachedSaveData);
        Debug.Log(onGameLoad);
        onGameLoad.Invoke(cachedSaveData);
    }
    public override void OnLoadSystem()
    {
        Debug.Log("SaveSystem.OnLoadSystem");
        cachedSaveData = SaveUtility.LoadSave(saveSlot.Value);
        //Debug.Log(cachedSaveData);
        if (cachedSaveData == null)
        {
            CreateNewSave();
            isNewGame = true;
        }
    }
    private void CreateNewSave()
    {
        cachedSaveData = new SaveGame();

        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene getScene = SceneManager.GetSceneAt(i);

            if (getScene.name != "Core")
            {
                cachedSaveData.lastScene = getScene.name;
                cachedSaveData.playerName = playerName.Value;
            }
        }
        WriteSaveToFile();
    }
    private void WriteSaveToFile()
    {
        TimeSpan currentTimePlayed = DateTime.Now - cachedSaveData.saveDate;
        TimeSpan allTimePlayed = cachedSaveData.timePlayed;
        cachedSaveData.timePlayed = allTimePlayed + currentTimePlayed;

        SaveUtility.WriteSave(cachedSaveData, saveSlot.Value);
    }


    public override void OnTick()
    {
        onGameSave?.Invoke(cachedSaveData);
#if UNITY_WEBGL && !UNITY_EDITOR
        WriteSaveToFile();
#endif
    }


}
