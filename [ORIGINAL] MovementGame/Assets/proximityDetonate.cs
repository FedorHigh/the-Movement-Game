using UnityEngine;

public class proximityDetonate : MonoBehaviour
{
    public Explosive toDetonate;
    public float delay = 0;
    private void OnTriggerEnter(Collider other)
    {
        toDetonate.Invoke("Explode", delay);
    }
}
