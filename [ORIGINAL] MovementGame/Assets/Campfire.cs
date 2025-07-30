using CustomClasses;
using UnityEngine;
using static UnityEngine.Analytics.IAnalytic;

public class Campfire : Interactable, ISaveable
{
    public GameObject openParticles, initParticles, positionMarker;
    public bool activated = false;
    public string newDescription = "rest at campfire";
    

    public override void Activate()
    {
        if (!activated)
        {
            activated = true;
            initParticles.SetActive(true);

            LoadActivated();
        }
        DataManager.instance.gameData.SetPoint(positionMarker.transform.position);
        DataManager.instance.SaveGame();

        //SaveData(ref DataManager.instance.gameData);
    }
    public void LoadActivated()
    {
        openParticles.SetActive(true);
        description = newDescription;
    }

    public void SaveData(ref GameData data)
    {
        data.booleans[ID + " lit"] = activated;
        //data.SetPoint(positionMarker.transform.position);
        
    }

    public void LoadData(GameData data)
    {
        activated = data.booleans.ContainsKey(ID + " lit") && data.booleans[ID + " lit"];
        Debug.Log("Loaded: " + activated);
        if (activated)
        {
            LoadActivated();

        }
    }
}
