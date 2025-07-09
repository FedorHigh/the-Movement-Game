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

    public bool isPaused; // Flag to track pause state

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && (EndGame != true))
        {
            // Toggle pause state on Escape key press
            isPaused = !isPaused;
            if (isPaused)
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
        // Set Time.timeScale to 0 to pause gameplay
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
        // Make PauseMenu panel visible (activate its gameObject)
        PanelPause.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }
    public void ResumeGame()
    {
        // Set Time.timeScale back to 1 to resume gameplay
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor
        // Hide PauseMenu panel (deactivate its gameObject)
        PanelPause.gameObject.SetActive(false);
    }
    public void Win()
    {
        // Set Time.timeScale to 0 to pause gameplay
        Time.timeScale = 0;
        // Make PauseMenu panel visible (activate its gameObject)
        Winpanel.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        EndGame = true;
    }
    public void Lose()
    {
        // Set Time.timeScale to 0 to pause gameplay
        Time.timeScale = 0;
        // Make PauseMenu panel visible (activate its gameObject)
        Losepanel.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        EndGame = true;
    }
    public void Settings()
    {
        PanelPause.gameObject.SetActive(false);
        CanvasAbility.gameObject.SetActive(false);
        Settingspanel.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }
    public void ToPause()
    {
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
