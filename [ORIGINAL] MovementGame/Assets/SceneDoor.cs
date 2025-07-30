using CustomClasses;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDoor : Interactable
{
    public string scene;
    public Vector3 point;
    public override void Activate()
    {
        base.Activate();
        DataManager.instance.gameData.SetPoint(point);
        DataManager.instance.gameData.scene = scene;
        DataManager.instance.SaveGame();
        //Debug.LogError(DataManager.instance.gameData.respawnPoint);
        SceneManager.LoadScene(scene);
    }
}
