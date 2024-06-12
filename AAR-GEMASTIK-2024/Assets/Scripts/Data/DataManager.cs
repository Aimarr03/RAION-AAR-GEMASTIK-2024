using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    [SerializeField] private string directivePath;
    [SerializeField] private string fileName;
    public static DataManager instance;
    public GameData gameData;
    private FileDataHandler fileHandler;
    List<IDataPersistance> ListDataPersistance;
    public void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("There is one more! Destroy newest one");
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        ListDataPersistance = new List<IDataPersistance>();
        fileHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        DontDestroyOnLoad(this.gameObject);
    }

    public void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ListDataPersistance = GetListDataPersistance();
        Debug.Log(ListDataPersistance.Count);
        LoadGame();
    }

    private List<IDataPersistance> GetListDataPersistance()
    {
        IEnumerable<IDataPersistance> ListDataPersistance = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistance>();
        return new List<IDataPersistance>(ListDataPersistance);
    }

    public void NewGame()
    {
        gameData = new GameData();
        Debug.Log("Created new Game Data");
        fileHandler.NewData();
        fileHandler.SaveData(gameData);
    }
    public void LoadGame()
    {
        gameData = fileHandler.LoadData();
        if (gameData != null)
        {
            foreach (IDataPersistance iDataPersistance in ListDataPersistance)
            {
                iDataPersistance.LoadScene(gameData);
            }
            return;
        }
        else
        {
            Debug.LogWarning("No Data was Found!");
        }
    }
    public void SaveGame()
    {
        if (gameData == null)
        {
            Debug.LogWarning("No data was found!");
            return;
        }
        Debug.Log("MONEY: " + gameData.money);
        foreach (IDataPersistance iDataPersistance in ListDataPersistance)
        {
            iDataPersistance.SaveScene(ref gameData);
        }
        fileHandler.SaveData(gameData);
    }

    /*public void OnApplicationQuit()
    {
        SaveGame();
    }*/
    public bool HasGameData()
    {
        return gameData != null;
    }
}
