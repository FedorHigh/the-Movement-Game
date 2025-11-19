using UnityEngine;

public class Player : MonoBehaviour
{
    public string playerName = "Hero";
    public int health = 100;
    public float speed = 5f;
    public bool isActive = true;
    public Action action;

    public void ResetStats()
    {
        health = 100;
        speed = 5f;
        Debug.Log($"{playerName} stats reset!");
    }
    public void PrintStats() { 
        Debug.Log($"Name: {playerName}, Health: {health}, Speed: {speed}, Active: {isActive}");
    }
}
