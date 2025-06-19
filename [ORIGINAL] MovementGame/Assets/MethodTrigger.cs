using CustomClasses;
using UnityEngine;

public class MethodTrigger : MonoBehaviour
{
    //public string enterMethod, exitMethod;
    //public MonoBehaviour toTrigger;
    public StateMachine toTrigger;
    public bool inTrigger;
    public int enterId = -1, exitId = -1;

    private void OnTriggerEnter()
    {
        if(enterId > -1) toTrigger.Trigger(enterId);
        inTrigger = true;
    }
    private void OnTriggerExit()
    {
        if (exitId > -1) toTrigger.Trigger(exitId);
        inTrigger = false;
    }
}
