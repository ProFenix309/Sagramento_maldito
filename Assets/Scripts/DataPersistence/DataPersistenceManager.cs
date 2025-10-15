using UnityEngine;

public class DataPersistenceManager : MonoBehaviour
{
    private GameData gameData;
public static DataPersistenceManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null) 
        {
            Debug.LogError("Found more than one Data Persistence Manager in the scene");
        } 
        instance = this;
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }
    public void LoadGame()
    {
        //load any saved data from a file using the data handler
        //if no data can be loaded, initialize to a new game
        if(this.gameData == null)
        {
            Debug.Log("No data was found, initializing data to default.");
            NewGame();
        }
        // push the loaded data to all other scripts that need it
    }
    public void SaveGame()
    {
        // pass the data to other scripts so they can update it 

        // save that data to a file using the data handler
    }
}
