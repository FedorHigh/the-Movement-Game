using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Threading;
using UnityEngine.Splines;
using CustomClasses;
using UnityEngine.UIElements;
using System;

public class BetterController : MonoBehaviour, ISaveable
{

    public Rigidbody rb;
    public float speed, sensitivity, Y;
    public Vector3 move, lookDirection, dirmove;
    public GameObject cam, head, rtc, abdir, vabdir, pointer;//Rotation Center, Ability Direction, Vertical ab.dir.
    //public GameObject bbs;
    CinemachineFreeLook cnm;
    //public InputManager input;

    public float playerHeight;
    public LayerMask ground;
    public bool grounded;
    public bool sprinting, forbidSprinting;
    public float groundDrag;
    public float airDrag, gravityScale;
    public float fallMultiplier, dropdownMultiplier, lowJumpMultiplier, slowFallMultiplier = 2;

    public float jumpHeight, jumpCooldown, airMultiplier, coyote = 0.5f;
    public bool canJump, timedJump, dashing, lockedOn, queued, slowFall, justJumped;

    public float groundGap;
    public float tmp, timer;

    public SplineAnimate anim;
    public CastInfo currentAbility;

    public UIDocument doc;

    public Vector3 rtcrotat, safespot;
    public CastInfo queuedCast;
    public float queueWindow;
    public float queueTimer;
    public float sprintSpeed = 150, threshold = 0.2f;
    public bool lastDashing;

    public Ability[] abilities;

    public AbilityInputManager inputManager;
    public PlayerHpManager hpManager;

    public GameObject slowfallParticles;

    

    public void ResetQueue()
    {
        if (queued)
        {
            Debug.Log("qd " + queuedCast.ID + " " + currentAbility.ID.ToString() + " " + currentAbility.cast + " " + queuedCast.cast);
            abilities[queuedCast.ID].ResolveQueue(currentAbility, queuedCast.cast);
        }
      
        
        queued = false;
        queuedCast = null;
        //Debug.Log("reset!");
    }
    public void SetQueue(CastInfo s)
    {
        queued = true;
        queuedCast = s;
        queueTimer = queueWindow;
        //Debug.Log("set!");
    }
    public void UpdateSafeSpot() {
        if (!grounded) return;
        RaycastHit spot;
        Ray ray = new Ray(transform.position, Vector3.down);
        Physics.Raycast(ray, out spot, playerHeight * 0.5f + groundGap, ground);
        if (!spot.Equals(null) && spot.collider.CompareTag("Safe")) { 
            safespot = spot.point;
            safespot.y += 2;
        }
    }
    void Start()
    {
        safespot = transform.position;
        //currentAbility = new CastInfo(1, 0);
        InvokeRepeating("UpdateSafeSpot", 0, 5);
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        anim = GetComponent<SplineAnimate>();
        inputManager = GetComponent<AbilityInputManager>();
        hpManager = GetComponent<PlayerHpManager>();

        cnm = cam.GetComponent<CinemachineFreeLook>();
    }

    void resetJump() {
        canJump = true;
    }
    private void Update()
    {
        if (queued) {
            queueTimer -= Time.deltaTime;
            if (queueTimer <= 0) { 
                queueTimer = 0;
                ResetQueue();
            }
        }
    }

    private void leaveGround() {
        grounded = false;
    }
    public void ForceLeaveGround() {
        CancelInvoke("ResetJustJumped");
        justJumped = true;
        Invoke("ResetJustJumped", coyote);
        leaveGround();
    }
    public void Slowfall() {
        slowFall = true;
        slowfallParticles.SetActive(true);
    }
    public void resetSlowfall()
    {
        slowFall = false;
        slowfallParticles.SetActive(false);
    }
    public void ResetJustJumped() {
        justJumped = false;
    }
    void FixedUpdate()
    {

        if (dashing != lastDashing) {
            Debug.Log("changed dashing to " + dashing);
        }
        lastDashing = dashing;
        //Debug.Log(rb.linearVelocity);


        //Ground check
        if (!justJumped && Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + groundGap, ground))
        {
            
            grounded = true;
            CancelInvoke("leaveGround");
            resetSlowfall();
        }
        else {
            if (grounded) { 
                Invoke("leaveGround", coyote);
            }
        }
        
        if (grounded)
        {
            rb.linearDamping = groundDrag;
            if (!Input.GetKey(KeyCode.Space))
            {
                timedJump = true;
            }
        }
        else
        {
            rb.linearDamping = airDrag;
            if(!dashing)rb.AddForce(rtc.transform.up * Time.deltaTime * -100 * gravityScale, ForceMode.Acceleration);
        }

        //Rotation
        const float MIN_Y = -60.0f;
        const float MAX_Y = 70.0f;
        
        Y += Input.GetAxis("Mouse Y") * (sensitivity * Time.deltaTime);

        if (Y < MIN_Y)
            Y = MIN_Y;
        else if (Y > MAX_Y)
            Y = MAX_Y;

        
        lookDirection = new Vector3(cnm.m_XAxis.Value, cnm.m_YAxis.Value, 0.0f);

        if (!lockedOn) rtcrotat = new Vector3(0.0f, 90 + lookDirection.x, 0.0f);
        else
        {
            cnm.m_XAxis.Value = rtcrotat.y-90;
            cnm.m_YAxis.Value = 0.5f;
            //cnm.m_YAxis.m_InputAxisValue = 0;
            //cnm.m_XAxis.m_InputAxisValue = 0;
        }
            //rtc.transform.localRotation = ;
            //else rtc.transform.localRotation;
            //else //cnm.m_XAxis.Value = rtc.transform.localRotation.y;
            //bbs.transform.localRotation = Quaternion.Euler(0.0f, 270 + lookDirection.x, 0.0f);
            //head.transform.localRotation = Quaternion.Euler(0 - Y, 90 + lookDirection.x, 0.0f);

            //Jumping
            if (Input.GetKey(KeyCode.Space) && grounded && timedJump & !dashing)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            if (canJump)
            {
                rb.AddForce(transform.up * jumpHeight, ForceMode.Impulse);
                ForceLeaveGround();
                //canJump = false;
                //Invoke("resetJump", jumpCooldown);
            }
            else rb.AddForce(transform.up * jumpHeight * lowJumpMultiplier, ForceMode.Impulse);
            timedJump = false;
            
        }
        else if (!Input.GetKey(KeyCode.Space) && rb.linearVelocity.y > 0 && !dashing && !slowFall) {
            rb.AddForce(rtc.transform.up * Time.deltaTime * -100 * gravityScale * (dropdownMultiplier - 1), ForceMode.Acceleration);
        }
        if (rb.linearVelocity.y < 0 && !grounded & !dashing) {
            rb.AddForce(rtc.transform.up * Time.deltaTime * -100 * gravityScale * (fallMultiplier - 1), ForceMode.Acceleration);
        }
        if (rb.linearVelocity.y < 0 && !dashing && slowFall) { 
            rb.AddForce(rtc.transform.up * Time.deltaTime * 100 * gravityScale * (slowFallMultiplier - 1), ForceMode.Acceleration);
        }

        //Movement
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        

        if ((Mathf.Abs(horizontal) >= threshold || Mathf.Abs(vertical) >= threshold) & !dashing)
        {
            move = rtc.transform.forward * vertical + rtc.transform.right * horizontal;
            transform.localRotation = Quaternion.Euler(0.0f, 90 - (Mathf.Atan2(move.z, move.x) * Mathf.Rad2Deg), 0.0f);
            head.transform.localRotation = transform.localRotation;
        }
        else move = Vector3.zero;
        //move = move * speed;



        if (!dashing)
        {   

            if(sprinting && !forbidSprinting) rb.AddForce(move.normalized * sprintSpeed * Time.deltaTime * 100, ForceMode.VelocityChange);
            else rb.AddForce(move.normalized * speed * Time.deltaTime * 100, ForceMode.VelocityChange);
        }
        //Debug.Log(horizontal);

        dirmove = new Vector3(0, 0, 0);
        //FIX LATER
        if (Input.GetKey(KeyCode.A)) {
            dirmove.x -= 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            dirmove.x += 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            dirmove.z -= 1;
        }
        if (Input.GetKey(KeyCode.W))
        {
            dirmove.z += 1;
        }
        if (dirmove == Vector3.zero) dirmove.z = 1;
        
        dirmove = dirmove.normalized;

        if(!lockedOn)rtc.transform.localRotation = Quaternion.Euler(rtcrotat);
        else rtcrotat = rtc.transform.localRotation.eulerAngles;
        //90 - (Mathf.Atan2(dirmove.z, dirmove.x) * Mathf.Rad2Deg)
        abdir.transform.localRotation = Quaternion.Euler(new Vector3(0.0f, 90-(Mathf.Atan2(dirmove.z, dirmove.x) * Mathf.Rad2Deg), 0.0f) + rtcrotat) ;

        if (dirmove.z < 0) vabdir.transform.localRotation = Quaternion.Euler(new Vector3(0.0f, 180, (Mathf.Atan2(dirmove.z, dirmove.x) * Mathf.Rad2Deg) - 270) + rtcrotat);
        else vabdir.transform.localRotation = Quaternion.Euler(new Vector3(0.0f, 0, (Mathf.Atan2(dirmove.z, dirmove.x) * Mathf.Rad2Deg) - 90) + rtcrotat);
        
        //Debug.Log(rb.linearVelocity);
        //if (dashing) timer += Time.deltaTime;
        //else if (timer != 0.0f)
        //{
        //    Debug.Log(timer);
        //    timer = 0;
        //}
        //Debug.Log(anim.IsPlaying);
    }
    public void ToSafeSpot(Collider collision) {
        rb.MovePosition(safespot);
        damager dmg = new damager();
        dmg.push = false;
        dmg.dmg = 50;
        hpManager.Damage(dmg, collision);
        if (currentAbility.ID!=0) abilities[currentAbility.ID].Reset();
        
    }
    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Respawn")){ 
            ToSafeSpot(collision);
        }
    }
    public void AbortDash() {
        Debug.Log("clipping detected! aborting ability");
        if (currentAbility == null) return;
        Debug.Log(currentAbility.ID.ToString() + " reset");
        inputManager.abilities[currentAbility.ID].Abort();
    }

    public void SaveData(ref GameData data)
    {
        
    }

    public void LoadData(GameData data)
    {
        Debug.Log("Loaded: " + data.respawnPoint);
        rb = GetComponent<Rigidbody>();
        if (!data.doTransport) rb.MovePosition(new Vector3(data.respawnPoint[0], data.respawnPoint[1], data.respawnPoint[2]));
        else
        {
            rb.MovePosition(new Vector3(data.transportTo[0], data.transportTo[1], data.transportTo[2]));
            data.doTransport = false;
        }
    }
}
