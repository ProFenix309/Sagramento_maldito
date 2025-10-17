using UnityEngine;

public interface IDataPersistence
{
    void LoadData(GameData data);
}

//for saving data in scripts you have to do this:


//public class EXAMPLE : MonoBehaviour, IDatapersistence

/*
 * public void LoadData(GameData data)
 * {
 *     this.exampleVariable = data.exampleVariable 
 * }
 * 
 * 
 * public void SaveData(ref GameData data)
 * {
 *     data.exampleVariable = this.exampleVariable
 * }
 * 
 */

//you have to create the exampleVariable in GameData Script as well