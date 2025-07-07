using UnityEngine;
using UnityEngine.Animations;

public class AttackBox : MonoBehaviour
{
    public float duration = 0.3f, finDuration = 0.3f;
    damager damager;
    public bool deleteOnTimeout = true;
    public string storedTag;

    //public AttackBox(float d, GameObject t) {
    //    duration = d;
    //    target = t;
    //    damager = b.GetComponent<damager>();
    //    constraint = b.GetComponent<ParentConstraint>();
    //}
    public void OnEnable()
    {
        //Debug.Log("spawned");
        damager = GetComponent<damager>();
        //constraint = GetComponent<ParentConstraint>();

        Invoke("endAttack", duration);
        Invoke("despawn", finDuration);

    }
    public void Start()
    {
        //OnEnable();
    }
    public void endAttack()
    {
        damager.enabled = false;
        storedTag = tag;
        tag = "Untagged";
    }
    public void despawn()
    {
        //Debug.Log("despawned");
        if (deleteOnTimeout) Destroy(gameObject);
        else {
            damager.enabled = true;
            tag = storedTag;
            gameObject.SetActive(false);
        }
    }

}
