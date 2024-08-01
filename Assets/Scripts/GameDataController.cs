using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameDataController : MonoBehaviour
{
    public GameObject player;
    public GameObject cameraControl;

    public string saveFile;

    public GameData gameData = new GameData();

    private bool dataSaved;

    public static int distanceRecordData;


    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cameraControl = GameObject.FindGameObjectWithTag("MainCamera");

        saveFile = Application.dataPath + "/gameData.json";


    }

    void LoadData()
    {
        if (File.Exists(saveFile))
        {
            string content = File.ReadAllText(saveFile);
            gameData = JsonUtility.FromJson<GameData>(content);
            distanceRecordData = gameData.distanceRecord;
        }
        else
        {
            Debug.Log("save file doesn't exist");
        }
    }

    void SaveData()
    {
        GameData newData = new GameData()
        {
            distanceRecord = CameraController.distance
        };

        string stringJSON = JsonUtility.ToJson(newData);

        File.WriteAllText(saveFile, stringJSON);
    }

    void PlayerDead(Notification notificacion)
    {
        if (!dataSaved)
        {
            LoadData();

            if (distanceRecordData < CameraController.distance)
            {
                SaveData();     //if the you get a higher record than previous, the data is saved
                LoadData();     //in JSON file and loaded again to show current value in UI
            }

            dataSaved = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        NotificationCenter.DefaultCenter().AddObserver(this, "PlayerDead");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
