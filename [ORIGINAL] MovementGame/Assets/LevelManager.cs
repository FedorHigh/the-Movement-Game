using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject PanelPause;
    [SerializeField] private GameObject Losepanel;
    [SerializeField] private GameObject Winpanel;
    [SerializeField] private GameObject Settingspanel;
    [SerializeField] private GameObject CanvasAbility;
    [SerializeField] private GameObject CanvasinMenu;
    public bool EndGame = false;

    public bool isPaused, isInSettings = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && (EndGame != true))
        {
            if (isInSettings) {
                ToPause();
            }
            else if (!isPaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        PanelPause.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }
    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        PanelPause.gameObject.SetActive(false);
        
    }
    public void Win()
    {
        Time.timeScale = 0;
        Winpanel.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        EndGame = true;
    }
    public void Lose()
    {
        Time.timeScale = 0;
        Losepanel.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        EndGame = true;
    }
    public void Settings()
    {
        isInSettings = true;
        PanelPause.gameObject.SetActive(false);
        CanvasAbility.gameObject.SetActive(false);
        Settingspanel.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }
    public void ToPause()
    {
        isInSettings = false;
        CanvasAbility.gameObject.SetActive(true);
        Settingspanel.gameObject.SetActive(false);
        PanelPause.gameObject.SetActive(true);

    }
    public void BacktoMain()
    {
        
        Settingspanel.SetActive(false);
        CanvasinMenu.gameObject.SetActive(true);


    }
    public void SettingMenu()
    {
        CanvasinMenu.gameObject.SetActive(false);
        Settingspanel.gameObject.SetActive(true);
    }
    public void OnBossDeath() {
        Debug.Log("BOSS IS DEAD");
        Win();
        
    }
    public void OnPlayerDeath()
    {
        Debug.Log("PLAYER IS DEAD");
        Lose();
    }
}
