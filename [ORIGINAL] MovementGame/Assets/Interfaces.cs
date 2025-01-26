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
        void Finish();

    }



    public class DashSpline {
        public GameObject splineObj;
        public SplineContainer spline;
        public ParentConstraint constraint;
        public float duration;
        public TrailRenderer trail;
        public ParticleHandle prtStart, prtEnd, prtMid;
        public Vector3 addedVelocity;

        public DashSpline(GameObject s)
        {
            splineObj = s;
            AssociatedVarsSpline ass = s.GetComponent<AssociatedVarsSpline>();
            spline = s.GetComponent<SplineContainer>();
            constraint = s.GetComponent<ParentConstraint>();
            duration = ass.duration;
            trail = ass.trail;
            addedVelocity = ass.AppliedVeclocity;
            prtStart = ass.prtStart;
            prtEnd = ass.prtEnd;
            prtMid = ass.prtMid;

            
        }
            
        
    }
    



    public class Ability : MonoBehaviour, IAbility{
        //declaring and returning vars
         public int ID;
        public VisualElement icon;
        //public DashSpline spl;
        [Space(30)]

        [Header("Spline settings")]
        public GameObject[] splineObjs;
        [Space(30)]

        [Header("Floats")]
        public float BaseCD;
        public float LightCD, HeavyCD;
        public float minCharge, maxCharge, chargeBoost, lightBoost;
        [Space(100)]

        [Header ("Debug Vars")]
        public BetterController player;
        public IAbility self;
        public Vector3 savedVelocity;
        public Vector3 appliedVeclocity;
        public Rigidbody rb;
        public SmoothAnimate anim;
        public bool active;
        public float CDleft, CDset, charge;
        public bool ready, charging;
        public KeyCode curKey;
        public DashSpline[] splines;
        public int curSpline;


        public bool IsReady() { return ready; }
        public VisualElement GetIcon() { return icon; }
        public int GetID() { return ID; }
        public float GetCDleft() { return CDleft; }
        public float GetCDset() { return CDset; }


        //setting and reseting vars
        public virtual void Start()
        {
            rb = GetComponent<Rigidbody>();
            anim = GetComponent<SmoothAnimate>();
            player = GetComponent<BetterController>();
            self = this;
            //icon = player.doc.rootVisualElement.Q<VisualElement>("cooldownBar");
            splines = new DashSpline[splineObjs.Length];
            for(int i = 0;i<splines.Length;i++) {
                splines[i] = new DashSpline(splineObjs[i]);
            }


        }
        public virtual void Reset()
        {
            //Debug.Log("RESET TRAIL " + curSpline.ToString());
            ResetVars();
        }
        public virtual void ResetVars()
        {
            //DashSpline s = splines[curSpline];
            //s.constraint.enabled = true;
            //Debug.Log(s.constraint.enabled.ToString());
            player.dashing = false;
            player.currentAbility = null;
            //s.trail.emitting = false;
            foreach(DashSpline s in splines){
                s.constraint.enabled = true;
                s.trail.emitting = false;
            }

        }


        //handling Update(): cooldowns and charging
        public virtual void ReleaseCharge() {
            Debug.Log("not implemented");
        }
        public virtual void CancelCharge()
        {
            charge = 0;
            charging = false;
        }
        public virtual void DoCooldowns() {
            if (!ready)
            {
                CDleft -= Time.deltaTime;
                if (CDleft <= 0)
                {
                    CDleft = 0;
                    ready = true;
                }
            }
        }
        public virtual void DoCharging() {
            if (charging)
            {
                if (!Input.GetKey(curKey))
                {
                    if (charge < minCharge)
                    {
                        CancelCharge();
                    }
                    else
                    {
                        charging = false;
                        ReleaseCharge();
                    }
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
        public virtual void Update()
        {
            DoCharging();
            DoCooldowns();
        }
        public virtual void Dash(KeyCode key, float CD, int splineIndex) {


            if (player.dashing) return;

            

            curSpline = splineIndex;
            DashSpline s = splines[splineIndex];
            curKey = key;

            ready = false;
            CDleft = CD;
            CDset = CDleft;

            player.dashing = true;
            s.constraint.enabled = false;
            player.currentAbility = self;
            s.trail.time = s.duration;
            s.trail.emitting = true;
            if (s.prtMid != null) {
                s.prtMid.Play();
            }

            if (s.prtStart != null)
            {
                s.prtStart.Play();
            }

            anim.duration = s.duration;
            anim.container = s.spline;
            anim.play(self);

            rb.linearVelocity = Vector3.zero;
            appliedVeclocity = s.splineObj.transform.right * s.addedVelocity.x + s.splineObj.transform.up * s.addedVelocity.y + s.splineObj.transform.forward * s.addedVelocity.z;
            rb.AddForce(appliedVeclocity, ForceMode.Impulse);
        }
        public virtual void Finish() {
            DashSpline s = splines[curSpline];
            if (s.prtEnd != null) s.prtEnd.Play();
            Reset();
        }

        public virtual void LightCast(KeyCode key)
        {
            throw new System.NotImplementedException();
        }

        public virtual void Cast(KeyCode key)
        {
            throw new System.NotImplementedException();
        }

        public virtual void HeavyCast(KeyCode key)
        {
            throw new System.NotImplementedException();
        }
    }
}
