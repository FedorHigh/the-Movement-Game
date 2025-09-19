using UnityEngine;

public class QuickSprint : StatusEffect
{
    public float threshold = 0.2f, multiplier = 2;
    BetterController player;
    public QuickSprint()
    {
        
    }
    public override void Start() {
        QuickSprint reference = GlobalVars.instance.statusEffects.GetComponent<QuickSprint>();
        duration = reference.duration;
        tickDown = reference.tickDown;
        effectName = reference.effectName;
        base.Start();
        if (!TryGetComponent<BetterController>(out player)) { 
            player = GlobalVars.instance.player;
        };

        player.sprintSpeed *= multiplier;
    }
    public override void Remove()
    {
        player.sprintSpeed /= multiplier;
        Debug.Log("trying to remove effect" + effectName);
        base.Remove();
    }
    public override void Refresh()
    {
        base.Refresh();
        duration = threshold;
    }
    public override void Update() { 
        base.Update();
        if(player.moving)duration = threshold;
     
    }

}
