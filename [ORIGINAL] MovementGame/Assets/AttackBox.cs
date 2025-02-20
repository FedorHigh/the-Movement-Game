using UnityEngine;
using UnityEngine.Animations;

public class AttackBox : MonoBehaviour
{
    public float duration, finDuration;
    damager damager;
    ParentConstraint constraint;
    public GameObject target;

    //public AttackBox(float d, GameObject t) {
    //    duration = d;
    //    target = t;
    //    damager = b.GetComponent<damager>();
    //    constraint = b.GetComponent<ParentConstraint>();
    //}
    public void Start()
    {
        Debug.Log("spawned");
        damager = GetComponent<damager>();
        //constraint = GetComponent<ParentConstraint>();

        Invoke("endAttack", duration);
        Invoke("despawn", finDuration);

    }
    public void endAttack()
    {
        damager.enabled = false;
        tag = "Untagged";
    }
    public void despawn()
    {
        Debug.Log("despawned");
        Destroy(gameObject);
    }

}
