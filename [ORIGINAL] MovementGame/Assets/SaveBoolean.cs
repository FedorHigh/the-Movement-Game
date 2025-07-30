using System;
using UnityEngine;
using UnityEngine.Events;

public class SaveBoolean : MonoBehaviour, ISaveable
{
    public string key;
    public bool value;
    public bool defaultValue = false;
    public UnityAction<bool> onLoad;
    public void SaveData(ref GameData data)
    {
        data.booleans[key] = value;
        Debug.Log("saved: " + key + " as " + value);
    }

    public void LoadData(GameData data)
    {
        if(!data.booleans.ContainsKey(key))value = defaultValue;
        else value = data.booleans[key];
        
        if(onLoad!=null)onLoad.Invoke(value);
    }
}
