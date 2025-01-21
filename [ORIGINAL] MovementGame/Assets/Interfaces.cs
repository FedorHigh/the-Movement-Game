using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Splines;
using UnityEngine.UIElements;

namespace Interfaces
{
    public interface IAbility
    {
        public float GetCDleft();
        public float GetCDset();
        bool IsReady();
        public VisualElement GetIcon();
        int GetID();
        void LightCast(KeyCode key);
        void Cast(KeyCode key);
        void HeavyCast(KeyCode key);
        void Reset();

    }
    public class AAbility{
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
        public void BasicCast(KeyCode key)
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
    }
}
