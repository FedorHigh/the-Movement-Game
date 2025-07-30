using CustomClasses;
using UnityEngine;
using UnityEngine.UI;

public class BossEntity : StateEntity
{
    [SerializeField] public Slider healthbarboss;
    public LevelManager manager;
    
    public BossSpawner spawner;
    public override void Start()
    {
        TargetObj = GameObject.Find("playerBody");
        healthbarboss = GameObject.Find("healthbarboss").GetComponent<Slider>();
        manager = TargetObj.GetComponent<LevelManager>();
        base.Start();
    }

    public override void OnDeath()
    {
        //manager.OnBossDeath();
        //particles.SetActive(true);
        //door.SetActive(true);
        base.OnDeath();
        spawner.Deactivate();
        
        //Destroy(toDisable);
    }
    private void UpdateHealthBar()
    {
        healthbarboss.value = hp / maxHp;
    }

    public override void Damage(HitInfo hit)
    {
        base.Damage(hit);
        
        UpdateHealthBar();
    }

}
