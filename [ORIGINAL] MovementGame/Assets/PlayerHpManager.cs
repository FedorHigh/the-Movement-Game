using UnityEngine;

public class PlayerHpManager : MonoBehaviour
{
    public float hp, maxHp, invincibilityTime, invTimeLeft;
    public bool invincible = false;
    private void Update()
    {
        if (invincible) { 
            invTimeLeft -= Time.deltaTime;
            if (invTimeLeft <= 0) { 
                invincible = false;
            }
        }
    }
    public void OnDeath() {
        Debug.Log("Man im dead!");
    }
    public void CheckIsAlive() {
        if (hp <= 0) OnDeath();
    }
    public void Damage(float damage, float cooldown) {
        cooldown = cooldown*invincibilityTime;
        if (invincible) return;
        invTimeLeft = cooldown;
        invincible = true;
        
        hp -= damage;
        CheckIsAlive();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("HurtPlayer")) {
            damager dmg = other.gameObject.GetComponent<damager>();
            Damage(dmg.dmg, dmg.cooldown);
        }
    }
}
