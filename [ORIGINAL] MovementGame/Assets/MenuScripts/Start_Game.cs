using UnityEngine;
using UnityEngine.SceneManagement;

public class Start_Game : MonoBehaviour
{
    public void LevelMenu()
    {
        SceneManager.LoadScene("LevelMenu");
    }
    public void Level1()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("SampleScene");
    }
    public void Level2()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Interface");
    }
    public void Level3()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Level 3");

    }
    public void ReloadScene ()
    {
        Time.timeScale = 1;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);

    }

}