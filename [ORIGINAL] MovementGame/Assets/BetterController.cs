using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Threading;
using UnityEngine.Splines;
using CustomClasses;
using UnityEngine.UIElements;

public class BetterController : MonoBehaviour
{

    public Rigidbody rb;
    public float speed, sensitivity, Y;
    public Vector3 move, lookDirection, dirmove;
    public GameObject cam, head, rtc, abdir, vabdir, pointer;//Rotation Centre, Ability Direction, Vertical ab.dir.
    //public GameObject bbs;
    CinemachineFreeLook cnm;
    //public InputManager input;

    public float playerHeight;
    public LayerMask ground;
    public bool grounded;
    public float groundDrag;
    public float airDrag, gravityScale;
    public float fallMultiplier, dropdownMultiplier, lowJumpMultiplier, slowFallMultiplier = 2;

    public float jumpHeight, jumpCooldown, airMultiplier;
    public bool canJump, timedJump, dashing, lockedOn, queued, slowFall;

    public float groundGap;
    public float tmp, timer;

    public SplineAnimate anim;
    public CastInfo currentAbility;

    public UIDocument doc;

    Vector3 rtcrotat;
    public CastInfo queuedCast;
    public float queueWindow;
    public float queueTimer;
    public bool lastDashing;

    public Ability[] abilities;

    public AbilityInputManager inputManager;
    public PlayerHpManager hpManager;

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
    void Start()
    {
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
    void FixedUpdate()
    {

        if (dashing != lastDashing) {
            Debug.Log("changed dashing to " + dashing);
        }
        lastDashing = dashing;
        //Debug.Log(rb.linearVelocity);
        

        //Ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + groundGap, ground);
        if (grounded)
        {
            rb.linearDamping = groundDrag;
            slowFall = false;
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
                canJump = false;
                Invoke("resetJump", jumpCooldown);
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
        if (rb.linearVelocity.y > 0 && !dashing && !slowFall) { 
            //rb.AddForce(rtc.transform.up * Time.deltaTime * 100 * gravityScale * (slowFallMultiplier - 1), ForceMode.Acceleration);
        }

        //Movement
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        move = rtc.transform.forward * vertical + rtc.transform.right * horizontal;

        if ((horizontal != 0.0f || vertical != 0.0f) & !dashing)
        {
            transform.localRotation = Quaternion.Euler(0.0f, 90 - (Mathf.Atan2(move.z, move.x) * Mathf.Rad2Deg), 0.0f);
            head.transform.localRotation = transform.localRotation;
        }
        //move = move * speed;
        if(!dashing)rb.AddForce(move.normalized * speed * Time.deltaTime * 100, ForceMode.Force);
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

    public void AbortDash() {
        if (currentAbility == null) return;
        Debug.Log(currentAbility.ID.ToString() + " reset");
        inputManager.abilities[currentAbility.ID].Reset();
    }
}
