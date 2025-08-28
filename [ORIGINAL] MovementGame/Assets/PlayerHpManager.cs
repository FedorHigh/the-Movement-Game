using UnityEngine;
using UnityEngine.UI;

public class PlayerHpManager : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider rallyhpBar;
    [SerializeField] private Slider manaBar;
    [SerializeField] private Slider staminaBar;
    public float hp, maxHp,rallyhp, invincibilityTime, invTimeLeft, depletionSpeed = 4, rallyEfficiency = 0.7f, mana = 100, manaMax = 100, stamina = 100, staminaMax = 100, staminaRegen = 20;
    public bool invincible = false;
    public BetterController player;
    public LevelManager manager;
    public float manaExponent = 1.1f, manaMultiplier = 0.1f; 
    public DamageIndicator damageIndicator;
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
        if (stamina < staminaMax) {
            stamina += staminaRegen * Time.deltaTime;
            if(stamina>staminaMax) stamina = staminaMax;
            UpdateStaminaBar();
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
        damageIndicator.FlashRed();
        CheckIsAlive();
    }
    public void Heal(float amount) {
        hp += amount;
        if(hp>maxHp) hp = maxHp;
        UpdateHealthBar();
        rallyhp = hp;
        UpdateRallyHealthBar();
    }
    private void UpdateHealthBar()
    {
        healthBar.value = hp/maxHp;
    }
    public void UpdateRallyHealthBar()
    {
        rallyhpBar.value = rallyhp / maxHp;
    }
    public bool UseMana(float amount)
    {
        if (mana >= amount)
        {
            mana -= amount;
            UpdateManaBar();
            return true;
        }
        else return false;
    }
    public void AddMana(float amount) {
        mana += amount;
        if(mana > manaMax) mana = manaMax;
        damageIndicator.FlashBlue(0.1f);

        UpdateManaBar();
    }
    public void AddManaFromKill(float amount) {
        float initial = amount;
        amount = Mathf.Pow(amount, manaExponent) * manaMultiplier;
        Debug.Log(amount + " mana added, initial was " + initial);
        AddMana(amount);
    } 
    private void UpdateManaBar()
    {
        manaBar.value = mana / manaMax;
    }
    public bool UseStamina(float amount)
    {
        if (stamina >= amount)
        {
            stamina -= amount;
            UpdateStaminaBar();
            return true;
        }
        else return false;
    }
    public void UpdateStaminaBar()
    {
        staminaBar.value = stamina / staminaMax;
    }
    public bool UseManaAndStamina(float manaAmount, float staminaAmount) {
        if (mana >= manaAmount && stamina >= staminaAmount)
        {
            mana -= manaAmount;
            UpdateManaBar();

            stamina -= staminaAmount;
            UpdateStaminaBar();

            return true;
        }
        else return false;
    }


    public void Start()
    {
        player = GetComponent<BetterController>();
        damageIndicator = GetComponent<DamageIndicator>();
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
