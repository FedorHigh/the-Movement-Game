using CustomClasses;
using UnityEngine;

public class InteractablePass : Interactable
{
    public Interactable passTo;
    public override void Activate()
    {
        passTo.Activate();
    }
    public override void HoverOn()
    {
        passTo.HoverOn();
    }
    public override void HoverOff()
    {
        passTo.HoverOff();

    }
}
