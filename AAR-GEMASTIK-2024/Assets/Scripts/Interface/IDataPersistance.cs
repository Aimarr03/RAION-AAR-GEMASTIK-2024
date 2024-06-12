using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public interface IDataPersistance
{
    public void LoadScene(GameData gameData);
    public void SaveScene(ref GameData gameData);
}
