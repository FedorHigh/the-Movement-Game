using CustomClasses;
using UnityEngine;

public class EnemyDeathTrigger : MonoBehaviour
{
    public Interactable interactable;
    public Entity[] entities;

    int count;

    public void Start()
    {
        count = entities.Length;
        foreach (Entity e in entities) { e.onDeathEvent += Decrement; }
    }
    public void Decrement(GameObject ignored) { 
        count--;
        Debug.Log("enemy kills left: " + count);
        if(count <= 0) interactable.Activate();
    }
}
