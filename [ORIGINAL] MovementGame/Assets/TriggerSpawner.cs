using CustomClasses;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    public GameObject boss, door, particles, UI;
    public GameObject deathParticles, toDisable, deathDoor;
    public Interactable interact;
    public SaveBoolean saver;
    private void OnEnable()
    {
        saver.onLoad += OnLoad;
    }
    private void OnDisable()
    {
        saver.onLoad -= OnLoad;
    }
    void Start()
    {
        if(saver==null)saver = GetComponent<SaveBoolean>();
        
        
    }
    public void OnLoad(bool value) {
        if (value == false)
        {

            deathDoor.SetActive(true);
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            Activate();
        }
    }
    public void Activate() {
        door.SetActive(true);
        UI.SetActive(true);
        particles.SetActive(true);
        boss.SetActive(true);
        gameObject.SetActive(false);
        saver.value = false;
        
    }
    public void Deactivate() {
        if(interact!=null) interact.Activate();
        deathParticles.SetActive(true);
        UI.SetActive(false);
        deathDoor.SetActive(true);
        Destroy(toDisable);
    }
}
