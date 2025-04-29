using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;
using UnityEngine.Splines;
using UnityEngine.UIElements;

namespace Interfaces
{
    public interface IAbility
    {
        public void SetKey(KeyCode key);
        public float GetCDleft();
        public float GetCDset();
        bool IsReady();
        public VisualElement GetIcon();
        int GetID();
        void LightCast();
        void Cast();
        void HeavyCast();
        void Reset();
        void Finish();
        public void ResolveQueue(CastInfo curAbility, int cast);

    }

    public interface IAction {
        public double GetCDleft();

        public bool GetReady();

        public void StartAction();

        public void Cancel();


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

    public class CastInfo { 
        public int ID;
        public int cast;
        public CastInfo(int a, int c)
        {
            ID = a;
            cast = c;
        }
        public bool Equals(CastInfo other, bool loose = false) {
            return (ID == other.ID & (loose | cast == other.cast));
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
        public GameObject[] attackBoxes;
        [Space(30)]

        [Header("Floats")]
        public float[] CD;
        //public float BaseCD, HeavyCD, LightCD;
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
        public CastInfo[] combos;
        


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
            player.ResetQueue();
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
        public virtual void Dash(float CD, int splineIndex) {

            

            if (player.dashing) {
                QueueCast(splineIndex);
                return;
            }

            curSpline = splineIndex;
            DashSpline s = splines[splineIndex];
            

            ready = false;
            CDleft = CD;
            CDset = CDleft;

            player.dashing = true;
            s.constraint.enabled = false;
            player.currentAbility = new CastInfo(ID, splineIndex);
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

        public virtual void LightCast()
        {
            throw new System.NotImplementedException();
        }

        public virtual void Cast()
        {
            throw new System.NotImplementedException();
        }

        public virtual void HeavyCast()
        {
            throw new System.NotImplementedException();
        }

        public virtual void QueueCast(int cast) { 
            player.SetQueue(new CastInfo(ID, cast));
        }
        public virtual void ResolveQueue(CastInfo curAbility, int cast)
        {
            //Debug.Log("resolved " + this.GetID().ToString());
            if (cast == 0) Cast();
            else if (cast == 1) HeavyCast();
            else LightCast();
            //Dash(curKey, CD[cast], cast);
        }

        void IAbility.SetKey(KeyCode key)
        {
            curKey = key;
        }



        public virtual void doAttack(int id, GameObject target, bool follow = false) {
            GameObject box = Instantiate(attackBoxes[id], target.transform);
            box.transform.position = target.transform.position;
            box.transform.rotation = target.transform.rotation;
            Debug.Log("did attack");
        }
    }

    public class Action : MonoBehaviour
    {
        public float CDset, duration;
        float CDleft;
        public bool ready, waiting, enableWait = true;
        public Entity host;
        public string endMethod = "", readyMethod = "";
        public virtual void Start()
        {
            host = GetComponent<Entity>();
            ready = true;
            waiting = false;
        }
        public virtual void Update() {
            //DoCooldown();
        }
        public virtual void ResetReady()
        {
            if (readyMethod != "")
            {
                host.Invoke(readyMethod, 0f);
            }
            ready = true;
            if (waiting) {
                waiting = false;
                StartAction();
            }
        }
        public virtual void DoCooldown() {
            if (!ready)
            {
                CDleft -= Time.deltaTime;
                if (CDleft <= 0)
                {
                    CDleft = CDset;
                    ready = true;
                }
            }
        }
        public virtual void StartCooldown() {
            /*
            if (!ready)
            {
                CDleft -= Time.deltaTime;
                if (CDleft <= 0) {
                    CDleft = CDset;
                    ready = true;
                }
            }
            */
            ready = false;
            Invoke("ResetReady", (float)CDset);
        }
        public virtual void Cancel()
        {
            CancelInvoke("StartActionRepeating");
            CancelInvoke("StartAction");
            waiting = false;
        }

        public virtual double GetCDset()
        {
            return CDset;
        }

        public virtual bool GetReady()
        {
            return ready;
        }

        public virtual bool StartAction()
        {
            if (!ready) {
                if(enableWait) waiting = true;
                Debug.Log("cancelled");
                return false;
            }
            //Debug.Log("performed" + ready.ToString());
            StartCooldown();
            if(duration >= 0)ScheduleEnd();
            return true;
        }

        public virtual void StartActionRepeating() {
            StartAction();
            Invoke("StartActionRepeating", (float)(duration+CDset));
        }
        public virtual void ScheduleEnd() {
            Invoke("EndAction", (float) duration);
        }
        public virtual void EndAction() {
            if (endMethod != "") {
                host.Invoke(endMethod, 0f);
            }
            StartCooldown();
        }
    }
    public class Entity : MonoBehaviour{
        public float maxHp, moveSpeed;
        public float hp;
        public bool followTarget, lookAtTarget, moveForward;
        public Rigidbody rb;
        public GameObject TargetObj, head, lockOnPoint;
        public Vector3 move;
        public Dictionary<GameObject, float> resistances;
        public LayerMask targetLayer;
        public NavMeshAgent agent;
        public GameObject detector;
        public WanderAround wander;
        //public Bet TargetHp;
        float tmp;
        public virtual void locateAnyTarget(float radius) {
            Collider[] targets = Physics.OverlapSphere(transform.position, radius, targetLayer);
            TargetObj = targets[0].gameObject;
            OnLocateTarget(TargetObj);
        }
        public virtual void OnLocateTarget(GameObject target) {
            TargetObj = target;
            detector.SetActive(false);
            agent.speed = moveSpeed;
            //WanderAround tmp;
            //if (TryGetComponent<WanderAround>(out tmp)) tmp.enabled = false;
            wander.Cancel();
        }

        public virtual void Start() {
            rb = GetComponent<Rigidbody>();
            resistances = new Dictionary<GameObject, float>();
            agent = GetComponent<NavMeshAgent>();
            TryGetComponent<WanderAround>(out wander);
        }
        public virtual void UpdResistances() {
            //Debug.Log(resistances.ToString());
            foreach(GameObject a in resistances.Keys) {
                //Debug.Log(a.name);
                resistances[a] -= Time.deltaTime;
                if (resistances[a] <= 0)resistances.Remove(a);
            }
        }
        
        public virtual void DoFollowTarget() {
            //if (agent.enabled) agent.destination = trg.transform.position;
            //move = TargetObj.transform.position - transform.position;
            //move.y = 0;
            //rb.AddForce(move.normalized * moveSpeed * Time.deltaTime, ForceMode.VelocityChange);
            if(agent.enabled)agent.SetDestination(TargetObj.transform.position);
        }
        
        public virtual void DoMoveForward() {
            rb.AddForce(transform.forward * moveSpeed * Time.deltaTime, ForceMode.VelocityChange);
        }
        public virtual void DoLookAtTarget() {
            if (head != null) head.transform.LookAt(TargetObj.transform);
            Vector3 tmp = TargetObj.transform.position;
            tmp.y = transform.position.y;
            transform.LookAt(tmp);
        }
        
        public virtual void Update() {
            if (followTarget) DoFollowTarget();
            if (lookAtTarget) DoLookAtTarget();
            if (moveForward) DoMoveForward();
            UpdResistances();
        }
        public virtual void Damage(float damage) {
            hp -= damage;
            CheckIsAlive();
        }
        public virtual void OnDeath() {
            LockOnManager tmpComp;
            if (TargetObj.TryGetComponent<LockOnManager>(out tmpComp))
            {
                if (tmpComp.target == lockOnPoint) tmpComp.ResetLockOn();
            }
            Destroy(gameObject);
        }
        public virtual void CheckIsAlive() {
            if (hp <= 0) OnDeath();
        }
        public virtual void OnTriggerStay(Collider other) {
            if (other.gameObject.CompareTag("HurtEntity")) {
                if (resistances.TryGetValue(other.gameObject, out tmp)) return;
                damager dmg = other.gameObject.GetComponent<damager>();
                Damage(dmg.dmg);
                resistances.Add(other.gameObject, dmg.cooldown);
            }
        }

    }

    public class StateEntity : Entity {
        public string[] states;
        public int state;
        public string stateStr;
        public void SetState(int s) {
            state = s;
            stateStr = states[state];
        }
        public override void Start()
        {
            base.Start();
            SetState(state);
        }
        public override void Update()
        {
            base.Update();
            if(stateStr!="") Invoke(stateStr, 0);
        }
    }
}
