using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject PanelPause;


    public bool isPaused; // Flag to track pause state

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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
    }
    public void ResumeGame()
    {
        // Set Time.timeScale back to 1 to resume gameplay
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor
        // Hide PauseMenu panel (deactivate its gameObject)
        PanelPause.gameObject.SetActive(false);
    }
    public void OnBossDeath() {
        Debug.Log("BOSS IS DEAD");
        SceneManager.LoadScene("Win");
        
    }
    public void OnPlayerDeath()
    {
        Debug.Log("PLAYER IS DEAD");
        SceneManager.LoadScene("Lose");
    }
}
