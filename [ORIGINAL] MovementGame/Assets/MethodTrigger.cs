using UnityEngine;

public class MethodTrigger : MonoBehaviour
{
    public string enterMethod, exitMethod;
    public MonoBehaviour toTrigger;
    public bool inTrigger;

    private void OnTriggerEnter()
    {
        if(enterMethod != "") toTrigger.Invoke(enterMethod, 0f);
        inTrigger = true;
    }
    private void OnTriggerExit()
    {
        if (exitMethod != "") toTrigger.Invoke(exitMethod, 0f);
        inTrigger = false;
    }
}
