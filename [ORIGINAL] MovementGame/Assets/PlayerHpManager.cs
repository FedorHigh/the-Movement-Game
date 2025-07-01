using UnityEngine;

public class PlayerHpManager : MonoBehaviour
{
    public float hp, maxHp, invincibilityTime, invTimeLeft;
    public bool invincible = false;
    public BetterController player;
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
    public void Damage(damager dmg, Collider other) {

        if (invincible) return;


        if (dmg.push)
        {
            Vector3 push = (transform.position - other.gameObject.transform.position).normalized;
            push.y = 0;
            if (push.magnitude == 0) push = new Vector3(1, 0, 1).normalized;
            Debug.Log("AAAAAAAAA" + push.ToString());
            player.rb.AddForce(push * dmg.force, ForceMode.Impulse);
        }
        float cooldown = dmg.cooldown;
        float damage = dmg.dmg;

        cooldown = cooldown * invincibilityTime;
        invTimeLeft = cooldown;
        invincible = true;
        
        hp -= damage;
        CheckIsAlive();
    }

    private void OnTriggerStay(Collider other)
    {

        
        if (other.CompareTag("HurtPlayer")) {
            damager dmg = other.gameObject.GetComponent<damager>();
            
            Damage(dmg, other);
        }
    }
}
