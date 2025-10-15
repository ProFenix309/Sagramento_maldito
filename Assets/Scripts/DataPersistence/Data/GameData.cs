using UnityEngine;
[System.Serializable]

public class GameData
{
    //add variables that contain progress EXAPMPLE: public int deathcount, public gameobject inventory 
    public float vidaActual;
    public GameData()
    {
        //the valuess defined in this constructor will be the default ones
        //the game starts with when there's no data to load
        //EXAMPLE this.deathCount = 0;
    }

}
