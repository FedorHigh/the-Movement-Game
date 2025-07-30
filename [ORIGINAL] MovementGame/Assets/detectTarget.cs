using CustomClasses;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class detectTarget : MonoBehaviour
{
    public bool LOS_required = true;
    public float alertRadius = 20;
    public Entity toSet;
    public LayerMask raycastLayer;
    public int targetLayer;
    public string trg = "Player";
    private void Start()
    {
        targetLayer = LayerMask.NameToLayer(trg);
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (!other.gameObject.layer.Equals(targetLayer)) return;
        //Debug.Log("seen");
        RaycastHit tmp;
        Ray tmpray = new Ray(transform.position, other.transform.position - transform.position);
        Physics.Raycast(tmpray, out tmp, 100000f, raycastLayer, QueryTriggerInteraction.Ignore);
        Debug.DrawRay(transform.position, other.transform.position - transform.position);
        //Collider tmpobj = tmp.collider;
       // Debug.Log(tmpobj.ToString());
      //  Debug.Log(tmp.collider.gameObject.name);
        if (tmp.collider.gameObject.layer==targetLayer || !LOS_required) {
            //Debug.Log("detected");
            Activate(other.gameObject);
        }
        
    }
    public void Activate(GameObject obj) {
        toSet.LocateTarget(obj);
        if (alertRadius > 0)
        {
            AlertEnemies alerter = gameObject.AddComponent<AlertEnemies>();
            alerter.radius = alertRadius;
            alerter.Activate(obj);
        }
    }
}
