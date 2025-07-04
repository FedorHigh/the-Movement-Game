using UnityEngine;

public class FunctionTrigger : MonoBehaviour
{
    //public string enterMethod, exitMethod;
    //public MonoBehaviour toTrigger;
    public MonoBehaviour toTrigger;
    public bool inTrigger;
    public string enter = "", exit = "";

    private void OnTriggerEnter()
    {
        inTrigger = true;
        if (enter != "") toTrigger.Invoke(enter, 0);

    }
    private void OnTriggerExit()
    {
        inTrigger = false;
        if (exit != "") toTrigger.Invoke(exit, 0);

    }
}
