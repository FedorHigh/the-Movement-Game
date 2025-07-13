using UnityEngine;

public class Delocker : MonoBehaviour
{
    public float dur = 0.1f;
    //BetterController player;
    LockOnManager lockOn;
    void Start()
    {
        //player = GetComponent<BetterController>();
        lockOn = GetComponent<LockOnManager>();
        Invoke("End", dur);
    }

    void Update()
    {
        //player.lockedOn = false;
        lockOn.ResetLockOn();
    }

    public void End() {
        Destroy(this);
    }
}
