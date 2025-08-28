using UnityEngine;

public class Chain : MonoBehaviour
{
    public GameObject target;
    public float delay;
    void Start()
    {
        Invoke("Activate", delay);
    }
    private void OnDestroy()
    {
        CancelInvoke();
    }
    private void OnDisable()
    {
        CancelInvoke();
    }
    public void Activate() {
        target.SetActive(true);
    }
}
