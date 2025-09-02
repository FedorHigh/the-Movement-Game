using CustomClasses;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class SlamSpell : Ability, IAbility
{
    public float maxInvTime = 10, invTime = 1, damageOnFail = 30, damageOnHeavyFail = 80, downwardsForce, steeringSpeed, minHeight = 10f, maxHeight = 1000f;
    bool descending = false, hitEnemy = false, inHeavy = false, validHeight = false;
    public UnityEvent onWeakCastUnityEvent;
    //public UnityEvent onHeavyFinishUnityEvent;

    bool CheckHeight()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out hit, maxHeight, LayerMask.GetMask("Ground"), QueryTriggerInteraction.Ignore))
        {
            Debug.Log("tried slamming from height " + hit.distance + " need " + minHeight);
            if (hit.distance >= minHeight)
            {
                Debug.Log("valid height for slam");
                validHeight = true;
                return true;
            }
        }
        Debug.Log("invalid height for slam");
        return false;
    }
    public void WeakCast() {
        player.dashing = true;
        player.currentAbility = new CastInfo(ID, 0);
        descending = true;
        hitEnemy = false;

        validHeight = false;
        onWeakCastUnityEvent.Invoke();
    }
    public override void Cast()
    {
        if(!CheckHeight())
        {
            WeakCast();
            Debug.Log("weak slam");
            return;
        }
        Debug.Log("setting slam");
        base.Cast();

        player.dashing = true;
        player.currentAbility = new CastInfo(ID, 0);

        //
        descending = true;
        hitEnemy = false;
        player.hpManager.SetInvincibility(maxInvTime);
    }
    
    public override void ResetVars()
    {
       
        //Debug.Log("resetting slam");
        if (!validHeight)
        {
            
        }
        else if (hitEnemy)
        {
            if (inHeavy)
            {
                Instantiate(attackBoxes[1], player.transform.position, player.transform.rotation);
            }
            else
            {
                Instantiate(attackBoxes[0], player.transform.position, player.transform.rotation);
            }
            player.hpManager.SetInvincibility(invTime);
        }
        else {
            player.hpManager.SetInvincibility(0);
            if(!inHeavy)player.hpManager.SelfDamage(damageOnFail, true);
            else player.hpManager.SelfDamage(damageOnHeavyFail, true);
        }
        descending = false;
        inHeavy = false;
        hitEnemy=false;
        base.ResetVars();
    }
    public override void Abort()
    {
        Finish();
    }
    public override void HeavyCast()
    {
        if (!CheckHeight())
        {
            WeakCast();
            return;
        }
        
        base.HeavyCast();
        player.dashing = true;
        player.currentAbility = new CastInfo(ID, 1);
        charge = minCharge;
        BeginCharging();
    }
    public override void ReleaseCharge()
    {
        if (charge < maxCharge)
        {
            charging = true;
            return;
        }
        base.ReleaseCharge();
        inHeavy = true;
        descending = true;
        //player.queued = false;
        //player.queuedCast = null;
        //if (player.currentAbility != null) player.abilities[player.currentAbility.ID].Reset();

        //attackBoxes[1].SetActive(true);
        //Dash(CD[1], 1);
        
    }
    public override void OnSuccessfulHit(float damage)
    {
        base.OnSuccessfulHit(damage);
        hitEnemy = true;

    }
    public override void Update()
    {
        base.Update();
        if (descending)
        {
            if (player.grounded)
            {
                Finish();
            }
            else{
                Vector3 forceVector = new Vector3();

                forceVector += player.abdir.transform.right * -1 * Input.GetAxis("Horizontal") * steeringSpeed;
                forceVector.y = -1 * downwardsForce;
                if(inHeavy) forceVector.y *= 0.5f;
                forceVector += player.abdir.transform.forward * Input.GetAxis("Vertical") * steeringSpeed;
                
                

                player.rb.AddForce(forceVector, ForceMode.Acceleration);
            }
        }
    }
}
