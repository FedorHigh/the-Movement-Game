using UnityEngine;
using Interfaces;
using UnityEngine.Splines;
using UnityEngine.Animations;
using Cinemachine;

public class LeapSpell : MonoBehaviour, IAbility
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
    public float duration, speed;
    public bool active;
    public int ID;
    //public ParticleSystem particles;
    public TrailRenderer trail;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<SmoothAnimate>();
        player = GetComponent<BetterController>();
        spline = splineObj.GetComponent<SplineContainer>();
        constraint = splineObj.GetComponent<ParentConstraint>();
        self = GetComponent<LeapSpell>();
        trail.time = duration;
    
    }
    public int GetID() { 
        return ID;
    }
    public void Reset()
    {
        constraint.enabled = true;
        player.dashing = false;
        player.currentAbility = null;
        trail.emitting = false;
    }
    public void Cast(KeyCode key)
    {
        if (player.dashing) return;
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
        throw new System.NotImplementedException();
    }

    public void LightCast(KeyCode key)
    {
        throw new System.NotImplementedException();
    }
}
