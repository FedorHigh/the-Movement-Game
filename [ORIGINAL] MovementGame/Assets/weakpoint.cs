using Interfaces;
using System.Collections;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class weakpoint : Entity
{
    public Entity body;
    public string method;
    public float damage;
    public LayerMask raycastLayer;
    public override void Start()
    {
        base.Start();
        raycastLayer = GetComponent<LayerMask>();
    }
    public override void Damage(float damage)
    {
        //Debug.Log("weakpoint hit");
        //IEnumerator tmp = body.OnWeakpointHit(damage);
        StartCoroutine(body.OnWeakpointHit(damage));
    }
    public override void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("HurtEntity"))
        {
            float val;
            //if (resistances.TryGetValue(other.gameObject, out val)) return;
            damager dmg = other.gameObject.GetComponent<damager>();

            RaycastHit tmp;
            Ray tmpray = new Ray(other.transform.position, transform.position - other.transform.position);
            Physics.Raycast(tmpray, out tmp, 100000f, raycastLayer, QueryTriggerInteraction.Ignore);
            Debug.DrawRay(other.transform.position, transform.position - other.transform.position);
            //if(tmp.collider!=null)Debug.Log(tmp.collider.ToString());
            if (tmp.collider == null)
            {
                Debug.Log("NULL");
                return;
            }
            else Debug.Log(tmp.collider.gameObject.name);
            if (tmp.collider.gameObject == gameObject)
            {
                
                Damage(dmg.dmg);
                //resistances.Add(other.gameObject, dmg.cooldown);
            }


            }
    }
}
