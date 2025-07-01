using CustomClasses;
using UnityEngine;

public class GrasshopperBoss : StateEntity
{
    public GameObject whole;
    public override void OnDeath()
    {
        whole.SetActive(false);
        base.OnDeath();
    }
}
