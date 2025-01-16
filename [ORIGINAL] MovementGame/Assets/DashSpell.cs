using UnityEngine;
using Interfaces;
using UnityEngine.Splines;
using UnityEngine.Animations;
using Cinemachine;
using static UnityEngine.ParticleSystem;

public class DashSpell : MonoBehaviour, IAbility
{
    public GameObject splineObj, splineObjHeavy;
    public SplineContainer spline, splineHeavy;
    public ParentConstraint constraint, constraintHeavy;
    public BetterController player;
    public IAbility self;
    public Vector3 addedVelocity;
    public Vector3 savedVelocity;
    public Vector3 appliedVeclocity;
    public Rigidbody rb;
    public SmoothAnimate anim;
    public float duration, speed, durationHeavy;
    public bool active;
    public int ID;
    public TrailRenderer trail;
    public float BaseCD, LightCD, HeavyCD;
    public float CDleft;
    public bool ready, charging;
    public float charge, minCharge, maxCharge, chargeBoost, lightBoost;
    public KeyCode curKey;
    public bool IsReady() { return ready; }

    public int GetID()
    {
        return ID;
    }

    void Update() {
        if (!ready)
        {
            CDleft -= Time.deltaTime;
            if (CDleft <= 0)
            {
                CDleft = 0;
                ready = true;
            }
        }
        if (charging)
        {
            if (!Input.GetKey(curKey))
            {
                if (charge < minCharge) charge = minCharge;
                ReleaseCharge();
                charging = false;
            }

            charge += Time.deltaTime;
            if (charge >= maxCharge)
            {
                charge = maxCharge;
                ReleaseCharge();
                charging = false;
            }
        }
    }

    //public ParticleSystem particles;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<SmoothAnimate>();
        player = GetComponent<BetterController>();
        spline = splineObj.GetComponent<SplineContainer>();
        constraint = splineObj.GetComponent<ParentConstraint>();
        splineHeavy = splineObjHeavy.GetComponent<SplineContainer>();
        constraintHeavy = splineObjHeavy.GetComponent<ParentConstraint>();
        self = GetComponent<DashSpell>();
        trail.time = duration;
    }

    public void Reset()
    {
        constraint.enabled = true;
        constraintHeavy.enabled = true;
        player.dashing = false;
        player.currentAbility = null;
        trail.emitting = false;
    }
    public void Cast(KeyCode key)
    {
        if (player.dashing) return;

        curKey = key;

        ready = false;
        CDleft = BaseCD;

        player.dashing = true;
        constraint.enabled = false;
        player.currentAbility = self;
        trail.time = duration;
        trail.emitting = true;

        anim.duration = duration;
        anim.container = spline;
        anim.play(self);
        //particles.Play();

        rb.linearVelocity = Vector3.zero;
        appliedVeclocity = splineObj.transform.right * addedVelocity.x + splineObj.transform.up * addedVelocity.y + splineObj.transform.forward * addedVelocity.z;
        rb.AddForce(appliedVeclocity, ForceMode.Impulse);
    }

    public void HeavyCast(KeyCode key)
    {
        ready = false;
        CDleft = HeavyCD;

        curKey = key;
        charging = true;
        charge = minCharge;
    }
    public void ReleaseCharge() {
        ready = false;
        CDleft = HeavyCD;

        if (player.currentAbility != null) player.currentAbility.Reset();
        //Debug.Log("RELEASED");
        //if (player.dashing) return;
        player.dashing = true;
        constraintHeavy.enabled = false;
        player.currentAbility = self;
        trail.emitting = true;
        trail.time = durationHeavy;

        //tmpknot = spline.Spline.ToArray()[1];
        //tmpknot.Position.y = defDist + (charge / maxCharge) * addDist;
        //spline.Spline.SetKnot(1, tmpknot);

        anim.duration = durationHeavy;
        anim.container = splineHeavy;
        anim.play(self);
        //particles.Play();

        rb.linearVelocity = Vector3.zero;
        appliedVeclocity = splineObj.transform.right * addedVelocity.x + splineObj.transform.up * addedVelocity.y + splineObj.transform.forward * addedVelocity.z;
        rb.AddForce(appliedVeclocity * (charge / maxCharge) * chargeBoost, ForceMode.Impulse);
    }

    public void LightCast(KeyCode key)
    {
        ready = false;
        CDleft = LightCD;
        //Debug.Log("light cast dash");

        //rb.linearVelocity = Vector3.zero;
        appliedVeclocity = splineObj.transform.right * addedVelocity.x + splineObj.transform.up * addedVelocity.y + splineObj.transform.forward * addedVelocity.z;
        rb.AddForce(appliedVeclocity*lightBoost, ForceMode.Impulse);
    }

}
