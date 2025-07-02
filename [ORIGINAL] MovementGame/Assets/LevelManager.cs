using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public void OnBossDeath() {
        Debug.Log("BOSS IS DEAD");
    }
    public void OnPlayerDeath()
    {
        Debug.Log("PLAYER IS DEAD");
    }
}
