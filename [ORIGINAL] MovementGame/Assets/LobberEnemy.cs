using UnityEngine;
using Interfaces;

public class LobberEnemy : Entity
{
    public GameObject projectile;
    public float g, dist, speed, time, horizontalDist, tmpspeed, cooldown, gravMultiplier, prediction, inaccuracy, repositionCD;
    public Vector3 groundTarget, velocity, targetPrediction;
    public RangedReposition rep;
    void ThrowProjectile() {
        GameObject obj = Instantiate(projectile);
        Physics.IgnoreCollision(obj.GetComponent<Collider>(), GetComponent<Collider>());
        obj.transform.position = transform.position;
        obj.GetComponent<ConstantForce>().force = new Vector3(0, (g + Physics.gravity.y) * -1, 0);

        

        
        //dist = 50;
        //tmpspeed = dist * 2 * g;
        speed = Mathf.Sqrt(dist * 2 * g);
        time = speed / g;
        velocity = new Vector3(0, g * time, 0);

        targetPrediction = TargetObj.transform.position + (TargetObj.GetComponent<Rigidbody>().linearVelocity * time * 2);

        groundTarget = new Vector3(targetPrediction.x + (Random.value-0.5f) * 2 * inaccuracy, obj.transform.position.y, targetPrediction.z + (Random.value - 0.5f) * 2 * inaccuracy);
        obj.transform.LookAt(groundTarget);

        horizontalDist = (groundTarget - obj.transform.position).magnitude;
        velocity += obj.transform.forward * horizontalDist / (time*2);

        obj.GetComponent<Rigidbody>().linearVelocity = velocity;
    }
    public override void OnLocateTarget(GameObject target)
    {
        base.OnLocateTarget(target);
        InvokeRepeating("ThrowProjectile", cooldown, cooldown);
        rep.InvokeRepeating("Reposition", repositionCD, repositionCD);
        followTarget = false;
        
    }
    public void OutOfRange() {
        followTarget = true;
        rep.CancelInvoke();
    }
    public void InRange() {
        followTarget = false;
        rep.InvokeRepeating("Reposition", 0, repositionCD);
    }
    public override void Start()
    {
        //locateAnyTarget(10000);
        base.Start();
        g = Physics.gravity.y * -1 * gravMultiplier;
        rep = GetComponent<RangedReposition>();
        //InvokeRepeating("ThrowProjectile", cooldown, cooldown);
    }
}
