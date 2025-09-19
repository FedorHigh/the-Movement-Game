using UnityEngine;

public class StatusEffect : MonoBehaviour
{
    public bool doesStack = false, tickDown = true;
    public int stacks = 0;
    public float duration = 0;
    public string effectName = "base";
    public StatusEffectManager manager;
    public virtual void Start()
    {
        if(manager==null)manager = GlobalVars.instance.player.gameObject.GetComponent<StatusEffectManager>();
    }
    public virtual void Update()
    {
        if (tickDown) { 
            duration -= Time.deltaTime;
            if (duration <= 0) {
                tickDown = false;
                duration = 0;
                RemoveStack();
            }
        }
    }
    public virtual void RemoveStack() { 
        stacks--;
        if (stacks <= 0)
        {
            Remove();
        }
    }
    public virtual void Remove()
    {
        manager.RemoveEffect(this);
        Debug.Log("player effects " + manager.activeEffects);
        Destroy(this);
    }
    public virtual void Refresh() { 
        
    }
    public virtual void AddStack() {
        if (!doesStack) { 
            Refresh();
            return;
        }
        stacks++;
    }
}
