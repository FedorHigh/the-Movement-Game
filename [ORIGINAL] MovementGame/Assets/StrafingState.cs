using CustomClasses;
using UnityEngine;

public class StrafingState : State
{
    public Vector3 center, target;
    public float radius, dist, time, threshold = 1;
    


    public void Retarget() {
        while (true)
        {
            Vector3 dir = (new Vector3(Random.Range(-1, 1), transform.position.y, Random.Range(-1, 1))).normalized * dist;

            if ((transform.position + dir - center).magnitude < radius)
            {
                target = dir;
                break;
            }
        }
        
    }
    public override void Start()
    {
        Retarget();
        base.Start();
    }
    public override void Enter(string info = "")
    {
        Retarget();
        base.Enter(info);
    }
    void Update()
    {
        if (active) {
            Vector3 tmp = ((target - transform.position) / time) * Time.deltaTime;
            tmp.y = 0;
            transform.position += tmp;
            if ((target - transform.position).magnitude < threshold) Retarget();
        }
    }
}
