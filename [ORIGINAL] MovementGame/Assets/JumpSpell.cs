using UnityEngine;
using Interfaces;
using UnityEngine.Splines;
using UnityEngine.Animations;
using Cinemachine;

public class JumpSpell : MonoBehaviour, IAbility
{
    public GameObject splineObj;
    public SplineContainer spline;
    public ParentConstraint constraint;
    public BetterController player;
    public IAbility self;
    public Vector3 addedVelocity;
    public Vector3 savedVelocity;
    public Vector3 appliedVeclocity;
    public Rigidbody rb;
    public SmoothAnimate anim;
    public float duration, speed, defDist, addDist, chargeBoost;
    public bool charging;
    public int ID;
    public BezierKnot tmpknot;

    public KeyCode curKey;
    public float charge, minCharge, maxCharge;
    //public ParticleSystem particles;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<SmoothAnimate>();
        player = GetComponent<BetterController>();
        spline = splineObj.GetComponent<SplineContainer>();
        constraint = splineObj.GetComponent<ParentConstraint>();
        self = GetComponent<JumpSpell>();

    }
    void Update() {
        if (charging) {
            if (!Input.GetKey(curKey)) {
                if (charge < minCharge) charge = minCharge;
                ReleaseCharge();
                charging = false;
            }

            charge += Time.deltaTime;
            if (charge >= maxCharge) {
                charge = maxCharge;
                ReleaseCharge();
                charging = false;
            }
        }
    }
    public void ReleaseCharge() {
        player.currentAbility.Reset();
        Debug.Log("RELEASED");
        //if (player.dashing) return;
        player.dashing = true;
        constraint.enabled = false;

        tmpknot = spline.Spline.ToArray()[1];
        tmpknot.Position.y = defDist + (charge/maxCharge) * addDist;
        spline.Spline.SetKnot(1, tmpknot);

        anim.duration = duration;
        anim.container = spline;
        anim.play(self);
        //particles.Play();

        rb.linearVelocity = Vector3.zero;
        appliedVeclocity = splineObj.transform.right * addedVelocity.x + splineObj.transform.up * addedVelocity.y + splineObj.transform.forward * addedVelocity.z;
        rb.AddForce(appliedVeclocity * (charge/maxCharge) * chargeBoost, ForceMode.Impulse);
    }
    public int GetID()
    {
        return ID;
    }
    public void Reset()
    {
        constraint.enabled = true;
        player.dashing = false;
        charging = false;
    }
    public void Cast(KeyCode key)
    {
        if (player.dashing | !player.grounded) return;
        player.dashing = true;
        constraint.enabled = false;

        tmpknot = spline.Spline.ToArray()[1];
        tmpknot.Position.y = defDist;
        spline.Spline.SetKnot(1, tmpknot);

        anim.duration = duration;
        anim.container = spline;
        anim.play(self);
        

        rb.linearVelocity = Vector3.zero;
        appliedVeclocity = splineObj.transform.right * addedVelocity.x + splineObj.transform.up * addedVelocity.y + splineObj.transform.forward * addedVelocity.z;
        rb.AddForce(appliedVeclocity, ForceMode.Impulse);
    }

    public void HeavyCast(KeyCode key)
    {
        curKey = key;
        charging = true;
        //player.dashing = true;
        charge = minCharge;
    }

    public void LightCast(KeyCode key)
    {
        rb.linearVelocity = Vector3.zero;
        appliedVeclocity = splineObj.transform.right * addedVelocity.x + splineObj.transform.up * addedVelocity.y + splineObj.transform.forward * addedVelocity.z;
        rb.AddForce(appliedVeclocity, ForceMode.Impulse);
    }
}