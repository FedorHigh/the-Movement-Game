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
        inTrigger = true;
        if (enterId > -1) toTrigger.Trigger(enterId);
        
    }
    private void OnTriggerExit()
    {
        inTrigger = false;
        if (exitId > -1) toTrigger.Trigger(exitId);
        
    }
}
