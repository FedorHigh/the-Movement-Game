using UnityEngine;
using Interfaces;
using UnityEngine.Splines;
using UnityEngine.Animations;
using Cinemachine;


public class LeapSpell : MonoBehaviour, IAbility
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
    public float BaseCD, LightCD, HeavyCD;
    public float CDleft;
    public bool ready, charging, inHeavy, charged;
    public float charge, minCharge, maxCharge, chargeBoost;
    public KeyCode curKey;
    public BezierKnot knot;
    public bool IsReady() { return ready; }

    //public ParticleSystem particles;
    public TrailRenderer trail, trailHeavy;

    public void ReleaseCharge() {
        anim.speedOverwrite = chargeBoost;
        trail.emitting = false;
        trailHeavy.emitting = true;
        charged = true;
    }
    void Update()
    {

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
            //if (!Input.GetKey(curKey))
            //{
                //if (charge < minCharge) charge = minCharge;
                //ReleaseCharge();
                //charging = false;
            //}

            charge += Time.deltaTime;
            if (charge >= maxCharge)
            {
                charge = maxCharge;
                ReleaseCharge();
                charging = false;
            }
        }
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<SmoothAnimate>();
        player = GetComponent<BetterController>();
        spline = splineObj.GetComponent<SplineContainer>();
        splineHeavy = splineObjHeavy.GetComponent<SplineContainer>();
        constraint = splineObj.GetComponent<ParentConstraint>();
        constraintHeavy = splineObjHeavy.GetComponent<ParentConstraint>();
        self = GetComponent<LeapSpell>();
        trail.time = duration;
    
    }
    public int GetID() { 
        return ID;
    }
    public void Reset()
    {
        if (charged) {
            Debug.Log("BOOM");
        }
        ResetVars();
    }
    public void ResetVars()
    {
        constraint.enabled = true;
        constraintHeavy.enabled = true;
        player.dashing = false;
        player.currentAbility = null;
        trail.emitting = false;
        trailHeavy.emitting = false;
        inHeavy = false;
        charged = false;
        anim.speedOverwrite = 1;
    }
    public void Cast(KeyCode key)
    {
        if (player.dashing | inHeavy) return;

        ready = false;
        CDleft = BaseCD;
        curKey = key;

        player.dashing = true;
        constraint.enabled = false;
        player.currentAbility = self;
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

        //if (player.currentAbility != null) player.currentAbility.Reset();
        //Debug.Log("RELEASED");

        
        if (inHeavy) {
            charging = true;
            //anim.duration = durationHeavy * 10;
            anim.speedOverwrite = 0.1f;
            charge = minCharge;

            return;
        }
        if (player.dashing) return;
        curKey = key;
        inHeavy = true;
        trail.emitting = true;
        player.dashing = true;
        constraintHeavy.enabled = false;
        player.currentAbility = self;

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
        if(player.dashing | inHeavy)
        CDleft = LightCD;
        ready = false;

        rb.linearVelocity = Vector3.zero;
        appliedVeclocity = splineObj.transform.right * addedVelocity.x + splineObj.transform.up * addedVelocity.y + splineObj.transform.forward * addedVelocity.z;
        rb.AddForce(appliedVeclocity, ForceMode.Impulse);
    }
}
