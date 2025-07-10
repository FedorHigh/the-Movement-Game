using UnityEngine;
using UnityEngine.UI;

public class PlayerHpManager : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider rallyhpBar;
    public float hp, maxHp,rallyhp, invincibilityTime, invTimeLeft, depletionSpeed = 4, rallyEfficiency = 0.7f;
    public bool invincible = false;
    public BetterController player;
    public LevelManager manager;
    private void Update()
    {
        if (invincible) { 
            invTimeLeft -= Time.deltaTime;
            if (invTimeLeft <= 0) { 
                invincible = false;
            }
        }
        if (rallyhp > hp)
        {
            rallyhp -= Time.deltaTime*depletionSpeed;
            UpdateRallyHealthBar();
        }
        
        
    }
    public void OnSuccesfulHit(float damage = 0, DamageTrigger damageTrigger = null) {
        if (damageTrigger != null) {
            if (damageTrigger.TriggerAbility) player.abilities[player.currentAbility.ID].OnSuccessfulHit(damage);
        }

        hp = Mathf.Min(hp + damage, rallyhp);
        if(hp>rallyhp)rallyhp = hp;
        UpdateHealthBar();
        UpdateRallyHealthBar();
    }
    public void OnDeath() {
        //Debug.Log("Man im dead!");
        manager.OnPlayerDeath();
        //Destroy(gameObject);
    }
    public void CheckIsAlive() {
        if (hp <= 0) OnDeath();
    }
    public void Stagger(Vector3 fromDir, float force) {
        Vector3 direction = transform.position - fromDir;
        if (direction.magnitude == 0) direction = transform.forward * -1;
        player.rb.AddForce((direction.normalized * 50 * force) + (transform.up * 25 * force), ForceMode.Impulse);
    }
    public void SetInvincibility(float time) {
        invTimeLeft = time;
        invincible = true;
    }
    public void Damage(damager dmg, Collider other) {

        if (invincible) return;

        /*
        if (dmg.push)
        {
            Vector3 push = (transform.position - other.gameObject.transform.position).normalized;
            push.y = 0;
            if (push.magnitude == 0) push = new Vector3(1, 0, 1).normalized;
            //Debug.Log("AAAAAAAAA" + push.ToString());
            player.rb.AddForce(push * dmg.force, ForceMode.Impulse);
        }
        */
        float cooldown = dmg.cooldown;
        float damage = dmg.dmg;

        //cooldown = cooldown * invincibilityTime;
        //invTimeLeft = cooldown;
        invTimeLeft = invincibilityTime;
        invincible = true;
        Stagger(other.transform.position, dmg.force);
        hp -= damage;
        rallyhp -= damage * (1 - rallyEfficiency);
        UpdateHealthBar();
        GetComponent<DamageIndicator>().FlashRed();
        CheckIsAlive();
    }
    private void UpdateHealthBar()
    {
        healthBar.value = hp/maxHp;
    }
    public void UpdateRallyHealthBar()
    {
        rallyhpBar.value = rallyhp / maxHp;
    }
    public void Start()
    {
        player = GetComponent<BetterController>();
        rallyhp = hp;
    }
    private void OnTriggerStay(Collider other)
    {

        
        if (other.CompareTag("HurtPlayer")) {
            damager dmg = other.gameObject.GetComponent<damager>();
            
            Damage(dmg, other);
        }
    }
}
