using CustomClasses;
using UnityEngine;

public class FloaterStateMachine : StateMachine
{
   public bool charging = false;
   public float value = 0f, attackCD = 3;

    public override void Switch(int target)
    {
        if(target == 1) charging = true;
        else charging = false;

        base.Switch(target);
    }

    private void Update()
    {
        if (charging) {
            value += Time.deltaTime;
            if (value > attackCD) {
                value = 0;
                charging = false;
                Trigger(1);
            }
        }
    }
    public override void Start()
    {
        base.Start();
        entity.onDetectionEvent += Detect;
    }

    public void Detect(GameObject tmp)
    {
        Trigger(0);
    }
}
