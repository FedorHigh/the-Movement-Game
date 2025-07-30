using CustomClasses;
using UnityEngine;

public class AlertEnemies : MonoBehaviour
{
    public GameObject point;
    public float radius = 10;
    public bool oneTime = true;
    void Start()
    {
        if (point == null) point = gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Activate(other.gameObject);
        }
    }
    public void Activate(GameObject other) {
        if (point == null) point = gameObject;
        Collider[] colliders = Physics.OverlapSphere(point.transform.position, radius);
        Debug.Log("alert at " + point.transform.position);
        Entity entity;
        detectTarget detectTarget;
        foreach (Collider collider in colliders)
        {
            //Debug.Log(collider.gameObject.name + " checked by " + gameObject.name);
            if (collider.gameObject.TryGetComponent<detectTarget>(out detectTarget)) { 
                detectTarget.Activate(other);
                Debug.Log(collider.gameObject.name + " detect activated by " + gameObject.name);
            }
            else if (collider.gameObject.TryGetComponent<Entity>(out entity))
            {
                entity.LocateTarget(other);
                Debug.Log(collider.gameObject.name + " alerted by " + gameObject.name);
            }
        }
        if (oneTime) gameObject.SetActive(false);
    }
}
