using UnityEngine;
using Interfaces;

public class ThrowProjectileAction : Interfaces.Action 
{
    public float g, dist, speed, time, horizontalDist, tmpspeed, cooldown, gravMultiplier, prediction, inaccuracy, repositionCD;
    public GameObject projectile, TargetObj;
    public Vector3 groundTarget, velocity, targetPrediction;

    public override bool StartAction() {
        g = Physics.gravity.y * -1 * gravMultiplier;
        if (!base.StartAction()) return false;
        
        GameObject obj = Instantiate(projectile);
        Physics.IgnoreCollision(obj.GetComponent<Collider>(), GetComponent<Collider>());
        obj.transform.position = transform.position;
        obj.GetComponent<ConstantForce>().force = new Vector3(0, (g + Physics.gravity.y) * -1, 0);




        //dist = 50;
        //tmpspeed = dist * 2 * g;
        speed = Mathf.Sqrt(dist * 2 * g);
        time = speed / g;
        velocity = new Vector3(0, g * time, 0);

        targetPrediction = TargetObj.transform.position + (TargetObj.GetComponent<Rigidbody>().linearVelocity * time * prediction);

        groundTarget = new Vector3(targetPrediction.x + (Random.value - 0.5f) * 2 * inaccuracy, obj.transform.position.y, targetPrediction.z + (Random.value - 0.5f) * 2 * inaccuracy);
        obj.transform.LookAt(groundTarget);

        horizontalDist = (groundTarget - obj.transform.position).magnitude;
        velocity += obj.transform.forward * horizontalDist / (time * 2);

        obj.GetComponent<Rigidbody>().linearVelocity = velocity;
        return true;
    }
}
