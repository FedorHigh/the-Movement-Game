using System;
using System.Collections;
using System.Collections.Generic;

//using System.Linq;
//using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;
using UnityEngine.Events;
using UnityEngine.Splines;
using UnityEngine.UIElements;

namespace CustomClasses
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
        void Abort();
        void Finish();
        public void ResolveQueue(CastInfo curAbility, int cast);

    }

    public interface IAction {
        public double GetCDleft();

        public bool GetReady();

        public void StartAction();

        public void Cancel();


    }

    public class DashInfo {
        public GameObject splineObj;
        public SplineContainer spline;
        public ParentConstraint constraint;
        public float duration;
        public TrailRenderer trail;
        public ParticleHandle prtStart, prtEnd, prtMid;
        public Vector3 addedVelocity;
        public Vector3 targetPosition;
        public AssociatedVarsSpline ass;

        public DashInfo(GameObject s)
        {
            splineObj = s;
            ass = s.GetComponent<AssociatedVarsSpline>();
            spline = s.GetComponent<SplineContainer>();
            constraint = s.GetComponent<ParentConstraint>();
            duration = ass.duration;
            trail = ass.trail;
            addedVelocity = ass.AppliedVeclocity;
            prtStart = ass.prtStart;
            prtEnd = ass.prtEnd;
            prtMid = ass.prtMid;
            targetPosition = ass.targetPosition;

        }
            
        
    }

    public class CastInfo { 
        public int ID;
        public int cast;
        public CastInfo(int ID, int cast)
        {
            this.ID = ID;
            this.cast = cast;
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
        public float defaultMana, defaultStamina, heavyMana, heavyStamina;
        [Space(100)]

        [Header ("Debug Vars")]
        public BetterController player;
        public IAbility self;
        public Vector3 savedVelocity;
        public Vector3 appliedVeclocity;
        public Rigidbody rb;
        public SmoothAnimate anim;
        [Obsolete] public AnimationCurveMove animator;
        public bool active;
        public float CDleft, CDset, charge;
        public bool ready, charging, ignoreDashing = false;
        public KeyCode curKey;
        public DashInfo[] dashInfo;
        public int currendDashIndex;
        public CastInfo[] combos;
        public ParticleSystem chargingParticles;

        public UnityEvent onAbortionUnityEvent;//done
        public UnityEvent onReleaseChargeUnityEvent;//done
        public UnityEvent onCancelChargeUnityEvent;//done
        public UnityEvent onBeginChargingUnityEvent;//done
        public UnityEvent onDashFinishUnityEvent;//done
        public UnityEvent onSuccessfulHitUnityEvent;
        public UnityEvent onCastUnityEvent;
        public UnityEvent onHeavyCastUnityEvent;


        public virtual bool IsReady() {
            return ready && (ignoreDashing || !player.dashing); 
        }
        public virtual bool ProcessInput(int cast) {
            if (!IsReady())
            {
                QueueCast(cast);
                return false;
            }

            if (cast == 0)
            {
                if (!player.hpManager.UseManaAndStamina(defaultMana, defaultStamina)) return false;

                Cast();
            }
            if (cast == 1)
            {
                if (!player.hpManager.UseManaAndStamina(heavyMana, heavyStamina)) return false;

                HeavyCast();
            }


            return true;
        }
        public VisualElement GetIcon() { return icon; }
        public int GetID() { return ID; }
        public float GetCDleft() { return CDleft; }
        public float GetCDset() { return CDset; }


        //setting and reseting vars
        public virtual void Start()
        {
            
            rb = GetComponent<Rigidbody>();
            anim = GetComponent<SmoothAnimate>();
            //animator = GetComponent<AnimationCurveMove>();
            player = GetComponent<BetterController>();
            self = this;
            //icon = player.doc.rootVisualElement.Q<VisualElement>("cooldownBar");
            dashInfo = new DashInfo[splineObjs.Length];
            for(int i = 0;i<dashInfo.Length;i++) {
                dashInfo[i] = new DashInfo(splineObjs[i]);
            }

            if(chargingParticles==null)chargingParticles = player.chargingParticles;
            chargingParticles.Stop();
        }
        public virtual void Reset()
        {
            //Debug.Log("RESET TRAIL " + curSpline.ToString());
            ResetVars();
        }
        public virtual void Abort() {
            Debug.Log("Ability aborted!");
            onAbortionUnityEvent.Invoke();
            Reset();
        }
        public virtual void ResetVars()
        {
            //DashSpline s = splines[curSpline];
            //s.constraint.enabled = true;
            //Debug.Log(s.constraint.enabled.ToString());
            
            player.dashing = false;
            //player.ResetQueue();
            player.currentAbility = null;
            anim.playing = false;
            //s.trail.emitting = false;
            foreach(DashInfo s in dashInfo){
                s.constraint.enabled = true;
                s.trail.emitting = false;
            }

        }

        public void BeginCharging() {
            charging = true;
            charge = 0;
            onBeginChargingUnityEvent.Invoke();
        }
        //handling Update(): cooldowns and charging
        public virtual void ReleaseCharge() {
            //Debug.Log("not implemented");
            onReleaseChargeUnityEvent.Invoke();
        }
        public virtual void CancelCharge()
        {
            charge = 0;
            charging = false;
            onCancelChargeUnityEvent.Invoke();
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
                if (!chargingParticles.isPlaying)
                {
                    //chargingParticles.set = minCharge;
                    chargingParticles.Play();
                }
                if (!Input.GetKey(curKey))
                {
                    if (charge < minCharge)
                    {
                        CancelCharge();
                    }
                    else
                    {
                        charging = false;
                        //chargingParticles.Stop();
                        ReleaseCharge();
                    }
                }

                charge += Time.deltaTime;
                if (charge >= maxCharge)
                {
                    charge = maxCharge;
                    chargingParticles.Stop();
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

            
            /*
            if (player.dashing) {
                QueueCast(splineIndex);
                return;
            }
            */

            currendDashIndex = splineIndex;
            DashInfo s = dashInfo[splineIndex];
            

            ready = false;
            CDleft = CD;
            CDset = CDleft;

            player.dashing = true;
            Debug.Log("dashing");

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
            //rb.AddForce(appliedVeclocity, ForceMode.VelocityChange);
            rb.AddForce(appliedVeclocity, ForceMode.Impulse);
            //rb.linearVelocity = appliedVeclocity;
        }

        [Obsolete]
        public virtual void AnimationCurveDash(float CD, int dashIndex)
        {
            currendDashIndex = dashIndex;
            DashInfo s = dashInfo[dashIndex];

            ready = false;
            CDleft = CD;
            CDset = CDleft;

            player.dashing = true;
            Debug.Log("dashing");

            player.currentAbility = new CastInfo(ID, dashIndex);
            s.trail.time = s.duration;
            s.trail.emitting = true;
            if (s.prtMid != null)
            {
                s.prtMid.Play();
            }

            if (s.prtStart != null)
            {
                s.prtStart.Play();
            }

            animator.duration = s.duration;
            animator.targetPosition = transform.position + (s.splineObj.transform.rotation * s.targetPosition);
            animator.Xcurve = s.ass.Xcurve;
            animator.Ycurve = s.ass.Ycurve;
            animator.Zcurve = s.ass.Zcurve;
            animator.directionTransform = s.splineObj.transform;
            animator.Play(self);

            rb.linearVelocity = Vector3.zero;
            appliedVeclocity = s.splineObj.transform.right * s.addedVelocity.x + s.splineObj.transform.up * s.addedVelocity.y + s.splineObj.transform.forward * s.addedVelocity.z;
            //rb.AddForce(appliedVeclocity, ForceMode.VelocityChange);
            rb.AddForce(appliedVeclocity, ForceMode.Impulse);
            //rb.linearVelocity = appliedVeclocity;
        }
        public virtual void Finish() {
            onDashFinishUnityEvent.Invoke();
            if (currendDashIndex < dashInfo.Length)
            {
                DashInfo s = dashInfo[currendDashIndex];
                if (s.prtEnd != null) s.prtEnd.Play();
            }
                //rb.AddForce(appliedVeclocity, ForceMode.Impulse);

                //rb.linearVelocity = appliedVeclocity;

           Reset();
        }

        public virtual void LightCast()
        {
            throw new System.NotImplementedException();
        }

        public virtual void Cast()
        {
            onCastUnityEvent.Invoke();
        }

        public virtual void HeavyCast()
        {
            onHeavyCastUnityEvent.Invoke();
        }

        public void QueueCast(int cast) {
            //Debug.Log("queued " + this.GetID().ToString());
            //player.SetQueue(new CastInfo(ID, cast));

            //too much trouble for what it's worth
        }
        public virtual void ResolveQueue(CastInfo curAbility, int cast)
        {
            //Debug.Log("resolved " + this.GetID().ToString());
            ProcessInput(cast);
            //else LightCast();
            //Dash(curKey, CD[cast], cast);
        }

        public void SetKey(KeyCode key)
        {
            curKey = key;
        }



        public virtual void doAttack(int id, GameObject target, bool follow = false) {
            GameObject box = Instantiate(attackBoxes[id], target.transform);
            box.transform.position = target.transform.position;
            box.transform.rotation = target.transform.rotation;
            Debug.Log("did attack");
        }

        public virtual void OnSuccessfulHit(float damage) {
            onSuccessfulHitUnityEvent.Invoke();
        }
    }

    public class Resistance : MonoBehaviour {
        public GameObject source;
        public float duration;
        public Entity host;
        public void Init(GameObject src, float dur, Entity hst)
        {
            source = src;
            duration = dur;
            host = hst;
            Invoke("Remove", dur);
        }
        public void Remove()
        {
            host.RemoveResistance(source);
            Destroy(this);
        }
    }
    public class Action : MonoBehaviour
    {
        public float CDset, duration;
        float CDleft;
        public bool ready = true, waiting = false, enableWait = true, enableChaining = false;
        public Entity host;
        public string endMethod = "", readyMethod = "";
        public Action chainAction;
        public float chainDelay;
        public bool chainActive = true, chainOnStart = false;
        public Animator anim;
        public string animTrigger;

        public virtual void Chain() {
            if (chainActive) {
                chainAction.Invoke("StartAction", chainDelay);
            }
        }
        public virtual void Start()
        {
            if(host==null) host = GetComponent<Entity>();
            ready = true;
            waiting = false;
        }
        public virtual void Update() {
            //DoCooldown();
        }
        public virtual void ResetReady()
        {
            ready = true;
            if (readyMethod != "")
            {
                host.Invoke(readyMethod, 0f);
            }
            if (waiting && enableWait) {
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
            ready = false;
            Invoke("ResetReady", (float)CDset);
        }
        public virtual void Cancel()
        {
            Debug.Log("action cancelled");
            CancelInvoke("StartActionRepeating");
            CancelInvoke("StartAction");
            //CancelInvoke();
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

        public virtual bool PrepareToStart()
        {
            if (!ready)
            {
                if (enableWait) waiting = true;
                //Debug.Log("Action not ready");
                return false;
            }
            ready = false;
            StartCooldown();
            Invoke("EndAction", (float)duration);
            if (chainOnStart) Chain();
            //else if (enableChaining) Chain();
            return true;
        }
        public virtual void StartAction()
        {
            if (anim != null) { 
                anim.ResetTrigger(animTrigger);
                anim.SetTrigger(animTrigger);
            }
        }

        public virtual void StartActionRepeating() {
            //if (period == -1) period = CDset;
            Debug.Log("repeated action");
            //Cancel();
            StartAction();
            Invoke("StartActionRepeating", CDset);
        }
        public virtual void ScheduleEnd() {
            //
        }
        public virtual void EndAction() {
            if(enableChaining && !chainOnStart) Chain();
            if (endMethod != "") {
                host.Invoke(endMethod, 0f);
            }
            //StartCooldown();
        }
    }
    public class Entity : MonoBehaviour, ISaveable{
        
        public float maxHp, moveSpeed;
        public float hp, rallyHp, rallyDepletion;
        public bool followTarget, lookAtTarget, moveForward, saveDeath = false;
        public Rigidbody rb;
        public GameObject TargetObj, head, lockOnPoint;
        public Vector3 move;
        public Dictionary<GameObject, Resistance> resistances;
        public LayerMask targetLayer;
        public NavMeshAgent agent;
        public GameObject detector;
        public WanderAround wander;

        public UnityAction<HitInfo> onHitEvent = null;
        public UnityAction<GameObject> onDetectionEvent = null;
        public UnityAction<GameObject> onDeathEvent = null;
        //
        public UnityEvent<HitInfo> onHitUnityEvent = null;
        public UnityEvent<GameObject> onDetectionUnityEvent = null;
        public UnityEvent<GameObject> onDeathUnityEvent = null;

        public GameObject healthbarObj;
        public EnemyHealthBar healthbar;
        
        //public Bet TargetHp;
        public float tmp, lastDamage;
        public string ID = "";

        void OnHitEvent(HitInfo pass) { 
            onHitUnityEvent.Invoke(pass);
        }
        void OnDetectionEvent(GameObject pass)
        {
            onDetectionUnityEvent.Invoke(pass);
        }
        void OnDeathEvent(GameObject pass)
        {
            onDeathUnityEvent.Invoke(pass);
        }
        public virtual void Start()
        {
            onHitEvent += OnHitEvent;
            onDetectionEvent += OnDetectionEvent;
            onDeathEvent += OnDeathEvent;

            healthbarObj = Instantiate(GlobalVars.instance.EnemyHealthbarPrefab, transform);
            healthbar = healthbarObj.GetComponent<EnemyHealthBar>();
            rallyHp = hp;
            rallyDepletion = maxHp * GlobalVars.instance.EnemyRallyDepletionPercent * 0.01f;
            rb = GetComponent<Rigidbody>();
            resistances = new Dictionary<GameObject, Resistance>();
            //Debug.LogError(resistances);
            agent = GetComponent<NavMeshAgent>();
            TryGetComponent<WanderAround>(out wander);

        }

        public void RemoveResistance(GameObject source)
        {
            Resistance tmp;
            if (resistances.TryGetValue(source, out tmp))
            {
                resistances.Remove(source);
                //Debug.Log("removed resistance from " + source.name);
            }
        }
        public virtual void locateAnyTarget(float radius) {
            Collider[] targets = Physics.OverlapSphere(transform.position, radius, targetLayer);
            TargetObj = targets[0].gameObject;
            LocateTarget(TargetObj);
        }
        public virtual void LocateTarget(GameObject target) {
            TargetObj = target;
            detector.SetActive(false);
            if(onDetectionEvent != null)onDetectionEvent.Invoke(target);
            //agent.speed = moveSpeed;
            //WanderAround tmp;
            //if (TryGetComponent<WanderAround>(out tmp)) tmp.enabled = false;
            //wander.Cancel();
        }

        
        
        

        public virtual void DoFollowTarget() {
            //if (agent.enabled) agent.destination = trg.transform.position;
            //move = TargetObj.transform.position - transform.position;
            //move.y = 0;
            //rb.AddForce(move.normalized * moveSpeed * Time.deltaTime, ForceMode.VelocityChange);
            //if(agent.enabled)agent.SetDestination(TargetObj.transform.position);
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
            //if (followTarget) DoFollowTarget();
            //if (lookAtTarget) DoLookAtTarget();
            //if (moveForward) DoMoveForward();
            //UpdResistances();
            if (rallyHp > hp) { 
                rallyHp -= rallyDepletion*Time.deltaTime;
                if(rallyHp < hp) rallyHp = hp;
                healthbar.UpdateRally(rallyHp / maxHp);
            }
        }
        public virtual void Damage(HitInfo hit) {
            //Debug.LogError(hit.value);
            //
            hp -= hit.value;
            
            
            if(onHitEvent != null)onHitEvent.Invoke(hit);
            healthbar.UpdateHp(hp / maxHp);

            CheckIsAlive();
            GetComponent<DamageIndicator>().FlashRed();
        }
        public virtual void OnDeath() {
            if(onDeathEvent!=null)onDeathEvent.Invoke(gameObject);
            LockOnManager tmpComp;
            if (TargetObj.TryGetComponent<LockOnManager>(out tmpComp))
            {
                if (tmpComp.target == lockOnPoint) tmpComp.ResetLockOn();
            }
            GlobalVars.instance.player.hpManager.AddManaFromKill(rallyHp);
            //Destroy(gameObject);
            
            gameObject.SetActive(false);
        }
        public virtual void CheckIsAlive() {
            if (hp <= 0) OnDeath();
        }
        public virtual void OnTriggerStay(Collider other) {
            if (other.gameObject.CompareTag("HurtEntity")) {
                //Debug.Log("ow");
                Resistance tmp;
                //Debug.LogError(resistances);
                if (resistances.TryGetValue(other.gameObject, out tmp)) return;
                damager dmg = other.gameObject.GetComponent<damager>();
                Resistance tmpres = gameObject.AddComponent<Resistance>();
                Debug.Log("new resistance: " + tmpres.ToString());
                tmpres.Init(other.gameObject, dmg.cooldown, this);
                resistances.Add(other.gameObject, tmpres);

                PlayerHpManager playerHp;
                lastDamage = dmg.dmg;
                if (TargetObj.TryGetComponent<PlayerHpManager>(out playerHp))
                {
                    DamageTrigger damageTrigger;
                    if(other.gameObject.TryGetComponent<DamageTrigger>(out damageTrigger)) playerHp.OnSuccesfulHit(dmg.dmg, damageTrigger);
                    else playerHp.OnSuccesfulHit(dmg.dmg);
                }

                Damage(new HitInfo(other.gameObject, gameObject));
                
            }
        }
        public virtual IEnumerator OnWeakpointHit(float dmg) { 
            yield return null;
            Debug.Log("Weakpoint hit for " + dmg.ToString() + " damage");
        }

        public void SaveData(ref GameData data)
        {
            if (!saveDeath) return;
            data.booleans[ID + " alive"] = gameObject.activeSelf;
            //data.SetPoint(positionMarker.transform.position);
        }

        public void LoadData(GameData data)
        {
            if (!saveDeath) return;
            if (!data.booleans.ContainsKey(ID + " alive"))gameObject.SetActive(true);
            else gameObject.SetActive(data.booleans[ID + " alive"]);
        }
    }

    public class State : MonoBehaviour {
        public int id = 0;
        public StateMachine parent;
        public bool active;
        public State connected;
        public Animator animator;
        public string trigger;
        public virtual void Start() {
            if (parent == null) parent = GetComponent<StateMachine>();
        }
        public virtual void Enter(string info = "") { 
            active = true;
            
            //Debug.Log("entered");
            if (connected != null)
            {
                
                connected.Enter();
                Debug.Log("connected");
            }

            if (animator != null) { 
                animator.ResetTrigger(trigger);
                animator.SetTrigger(trigger);
            }
        }
        public virtual void Exit(string info = "") { 
            active = false;
            //Debug.Log("exited");
            if (connected != null)
            {
                
                connected.Exit();
                //Debug.Log("connected");
            }
        }
    }

    public class StatesTable
    {
        List<State> states;
        List<List<int>> table;
        List<string> triggers;
        

        public virtual void Init() { 
            int n = states.Count;
            int t = triggers.Count;
            table = new List<List<int>>(n);
            for (int i = 0; i < n; i++) {
                List<int> l = new List<int>(t);
                for (int j = 0; j < t; j++)
                {
                    l[j] = i;
                }
                table[i] = l;
            }
        }
    }
    public class HitInfo
    {
        public GameObject hitBox, hurtBox;
        public float value;
        public HitInfo(GameObject hitBox_, GameObject hurtBox_) {
            hitBox = hitBox_;
            hurtBox = hurtBox_;
            value = hitBox.GetComponent<damager>().dmg;
        }
    }
    public class StateMachine : MonoBehaviour {
        public StatesTable statesTable;
        public int[][] table;
        public State[] states;
        public string[] triggers;
        public int curState = 0;
        public string[] inputTable;
        public Entity entity;
        public Queue<int> queued;
        public string label = "bro 1";
        

        public virtual void Start()
        {
            //table = statesTable.table;
            //states = statesTable.states;
            entity = GetComponent<Entity>();
            int n = states.Length;
            int t = triggers.Length;
            queued = new Queue<int>();
            table = new int[10][];

            for (int i = 0; i < n; i++)
            {
                int[] l = new int[t];
                Debug.Log(l.Length);
                for (int j = 0; j < t; j++)
                {
                    Debug.Log(j);
                    l[j] = i;
                }
                table[i] = l;
                states[i].id = i;
            }

            for (int i = 0; i < n; i++)
            {
                //List<int> l = new List<int>(t);
                for (int j = 0; j < t; j++)
                {
                    if (inputTable[i][j] == 'q') table[i][j] = -1;
                    else if (inputTable[i][j] != '-') table[i][j] = inputTable[i][j]-'0';
                }
                //table[i] = l;
            }

            for (int i = 0; i < n; i++)
            {
                //List<int> l = new List<int>(t);
                string s = "";
                for (int j = 0; j < t; j++)
                {
                    s += (table[i][j]).ToString();
                }
                Debug.Log(s);
                //table[i] = l;
            }
            states[0].Enter();
        }
        public virtual void Trigger(int id)
        {
            Debug.Log(label+ " TRIGGERED on state " + curState.ToString() + " trigger " + id.ToString());
            //if (id == 3) return;
            if (table[curState][id] == -1) queued.Enqueue(id);
            else if (table[curState][id]!=curState) Switch(table[curState][id]);
        }
        public virtual void Switch(int target){
            Debug.Log(label + " switched state from " + curState.ToString()+ " to " + target.ToString());
            
            states[curState].Exit();
            //if (curState == 2) return;
            states[target].Enter();
            curState = target;
            ResolveQueue();
        }
        public virtual void ResolveQueue() {
            for(int i = 0;i<queued.Count;i++) {
                int tmp = queued.Peek();
                queued.Dequeue();
                Trigger(tmp);
            }
        }
    }

    public class StateEntity : Entity {
        public StateMachine machine;
        public int damageTrigger = -1;
        public void SetState(int s) {
            //state = s;
            //stateStr = states[state];
        }
        public override void Start()
        {
            machine = GetComponent<StateMachine>();
            base.Start();
            //SetState(state);
        }
        public override void Update()
        {
            base.Update();
            //if(stateStr!="") Invoke(stateStr, 0);
        }
        public override void Damage(HitInfo hit)
        {
            base.Damage(hit);
            if (damageTrigger != -1)
            {
                machine.Trigger(damageTrigger);
                Debug.Log("triggered from damage");
            }
        }
    }

    public class Interactable : MonoBehaviour {
        public string description = "interact with object";
        public bool oneTime = true;
        public string ID = "";
        public GameObject parent;
        public UnityEvent onActivationUnityEvent;
        public virtual void Start() {
            //ID = gameObject.name + transform.position.ToString() + transform.rotation.ToString();
            if (parent == null) parent = gameObject;
            //if (ID == "") ID = parent.name;
        }
        public virtual void HoverOn() { 
            
        }
        public virtual void HoverOff() { 
        
        }
        public virtual void Activate() { 
            onActivationUnityEvent.Invoke();
        }
    }
}
