using UnityEngine;
using Interfaces;

public class LobberEnemy : Entity
{
    public GameObject projectile;
    public float g, dist, speed, time, horizontalDist, tmpspeed, cooldown, gravMultiplier, prediction;
    public Vector3 groundTarget, velocity, targetPrediction;
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
        groundTarget = new Vector3(targetPrediction.x, obj.transform.position.y, targetPrediction.z);
        obj.transform.LookAt(groundTarget);

        horizontalDist = (groundTarget - obj.transform.position).magnitude;
        velocity += obj.transform.forward * horizontalDist / (time*2);

        obj.GetComponent<Rigidbody>().linearVelocity = velocity;
    }
    public override void Start()
    {
        locateAnyTarget(10000);
        base.Start();
        g = Physics.gravity.y * -1 * gravMultiplier;
        InvokeRepeating("ThrowProjectile", cooldown, cooldown);
    }
}
