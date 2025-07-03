using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelManager : MonoBehaviour
{
    
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
