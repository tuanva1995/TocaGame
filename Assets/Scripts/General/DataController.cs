using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Pixelplacement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public class DataController : Singleton<DataController>
{
    public const int HEART_REPLENISH_TIME = 1800, MAX_ENERGY = 5;
    private string KEY = "namtsh";
    private string dataPath = "";
    public GameData gameData;
    private float timeStamp;
    protected override void OnRegistration()
    {
        dataPath = Path.Combine(Application.persistentDataPath, "data.dat");
        LoadData();
    }
    public int Gem
    {
        get { return gameData.gem; }
        set
        {
            gameData.gem = Mathf.Clamp(value, 0, Int32.MaxValue);
            SaveData();
        }
    }
    public int Coin
    {
        get { return gameData.coin; }
        set
        {
            gameData.coin = Mathf.Clamp(value, 0, Int32.MaxValue);
            SaveData();
        }
    }
    public void SaveData()
    {
        string origin = JsonUtility.ToJson(gameData);
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        string encrypted = Utils.XOROperator(origin, KEY);
        using (FileStream fileStream = File.Open(dataPath, FileMode.OpenOrCreate))
        {
            binaryFormatter.Serialize(fileStream, encrypted);
        }
        timeStamp = Time.time;
    }
    public async void LoadData()
    {
        await System.Threading.Tasks.Task.Delay(500);
        if (File.Exists(dataPath))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            string data;
            using (FileStream fileStream = File.Open(dataPath, FileMode.Open))
            {
                try
                {
                    data = (string)binaryFormatter.Deserialize(fileStream);
                    string decrypted = Utils.XOROperator(data, KEY);
                    gameData = JsonUtility.FromJson<GameData>(decrypted);
                }
                catch (System.Exception e)
                {                        
                    Debug.LogError(e.Message);
                    ResetData();
                }
            }
        }
        else
            ResetData();
        if (FindObjectOfType<IAPSilentProcesser>() != null)
            FindObjectOfType<IAPSilentProcesser>().canProcessIAP = true;
        SceneController.Instance.LoadScene("Home");
    }
    public void ResetData()
    {
        gameData = new GameData();
    }
}

