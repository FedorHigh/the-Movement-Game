using CustomClasses;
using UnityEngine;
using UnityEngine.UI;

public class BossEntity : StateEntity
{
    [SerializeField] public Slider healthbarboss;
    public LevelManager manager;
    public override void Start()
    {
        TargetObj = GameObject.Find("playerBody");
        healthbarboss = GameObject.Find("healthbarboss").GetComponent<Slider>();
        manager = TargetObj.GetComponent<LevelManager>();
        base.Start();
    }

    public override void OnDeath()
    {
        manager.OnBossDeath();
        base.OnDeath();
    }
    private void UpdateHealthBar()
    {
        healthbarboss.value = hp / maxHp;
    }

    public override void Damage(float damage)
    {
        base.Damage(damage);
        
        UpdateHealthBar();
    }

}
