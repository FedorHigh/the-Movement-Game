using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static UnityEngine.Analytics.IAnalytic;
using UnityEngine.SceneManagement;

public interface ISaveable
{
    void SaveData(ref GameData data);
    void LoadData(GameData data);
}
public class DataManager : MonoBehaviour
{
    public static DataManager instance { get; private set; }
    public GameData gameData;
    public List<ISaveable> savedObjects;
    public DataFileWriter writer;
    public BetterController player;

    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
            //gameData = new GameData(Vector3.zero, 0);
            
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        savedObjects = GetSavedObjects();
        LoadGame();
    }
    public void SaveGame() {
        savedObjects = GetSavedObjects();
        Debug.Log("GAME SAVED");
        foreach(ISaveable saveable in savedObjects)
        {
            try
            {
                saveable.SaveData(ref gameData);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error saving data for {saveable.GetType().Name}: {e.Message}");
            }
            //saveable.SaveData(ref gameData);
        }

        // write to file
        if (gameData == null)
        {
            Debug.LogError("No game data found to... save?..");
            gameData = new GameData(Vector3.zero, 0);
        }
        Debug.Log("booleans "+gameData.booleans.ToString());
        writer.Save(gameData);
    }
    
    public void LoadGame() {
        // load from file
        writer = new DataFileWriter(Application.persistentDataPath);
        gameData = writer.Load();
        Debug.Log("GAME LOADED");
        if (gameData == null)
        {
            Debug.LogError("No game data found to load.");
            gameData = new GameData(new Vector3(20, 18, 75), 0);
        }

        foreach (ISaveable saveable in savedObjects)
        {
            saveable.LoadData(gameData);
        }
        //player.rb.MovePosition(new Vector3(gameData.respawnPoint[0], gameData.respawnPoint[1], gameData.respawnPoint[2]));
    }

    private void Start()
    {
        writer = new DataFileWriter(Application.persistentDataPath);
        savedObjects = GetSavedObjects();
        LoadGame();
    }
    private void OnApplicationQuit()
    {
        //SaveGame();
    }


    private List<ISaveable> GetSavedObjects() { 
        IEnumerable<ISaveable> saveables = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<ISaveable>();
        return saveables.ToList();
    }
}
