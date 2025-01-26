using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    public CharacterController cc;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerHead;
    [SerializeField] private GameObject rotationCenter;
    [SerializeField] private GameObject directionCamera;
    [SerializeField] private Camera cam;
    [SerializeField] private float Sensitivity;
    [SerializeField] private Rigidbody rb;

    [SerializeField]
    public float speed, walk, run, crouch, gravityScale;

    private Vector3 crouchScale, normalScale;

    public bool isMoving, isCrouching, isRunning;

    public float X, Y;

    public float horizontal, vertical;

    public Vector3 forward, right, lookDirection, move;



    private void Start()
    {
        speed = walk;
        crouchScale = new Vector3(1, .75f, 1);
        normalScale = new Vector3(1, 1, 1);
        cc = GetComponent<CharacterController>();
        cc.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        #region Camera Limitation Calculator
        //Camera limitation variables
        const float MIN_Y = -60.0f;
        const float MAX_Y = 70.0f;

        X -= Input.GetAxis("Mouse X") * (Sensitivity * Time.deltaTime);
        Y += Input.GetAxis("Mouse Y") * (Sensitivity * Time.deltaTime);
        //Debug.Log(Input.GetAxis("Mouse X"));
        if (Y < MIN_Y)
            Y = MIN_Y;
        else if (Y > MAX_Y)
            Y = MAX_Y;
        #endregion
        CinemachineFreeLook cnm = directionCamera.GetComponent<CinemachineFreeLook>();
        lookDirection = new Vector3(cnm.m_XAxis.Value, cnm.m_YAxis.Value, 0.0f);
        //rotationCenter.transform.localRotation = Quaternion.Euler(0.0f, X, 0.0f);
        rotationCenter.transform.localRotation = Quaternion.Euler(0.0f, 90+lookDirection.x, 0.0f);
        playerHead.transform.localRotation = Quaternion.Euler(0-Y, 90 + lookDirection.x, 0.0f);
        //player.transform.localRotation = Quaternion.Euler(0.0f, player.transform.localRotation.y, 0.0f);

        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        forward = rotationCenter.transform.forward * vertical;
        right = rotationCenter.transform.right * horizontal;
        move = (forward + right);

        if (horizontal != 0.0f || vertical != 0.0f) { //can be optimised
            //Debug.Log("changed");
            //player.transform.localRotation = rotationCenter.transform.localRotation;
            player.transform.localRotation = Quaternion.Euler(0.0f, 90-(Mathf.Atan2(move.z, move.x) * Mathf.Rad2Deg), 0.0f);
            //cc.SimpleMove((forward + right) * speed);
            //playerHead.transform.localRotation = Quaternion.Euler(Y, X, 0.0f);
        }
        move = move * speed;
        

        //
        //
        

        if (Input.GetKey(KeyCode.Space)) {
            //move.y += new Vector3(rb.velocity.x, rb.velocity.y+10, rb.velocity.z);
            move.y = 10;
        }
        //move.y -= gravityScale;
        cc.Move(move * speed);
        //rb.velocity = move*Time.deltaTime*1000;
        //rb.velocity += new Vector3(100, 100, 100);

        // Determines if the speed = run or walk
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = run;
            isRunning = true;
        }
        //Crouch
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            isCrouching = true;
            isRunning = false;
            speed = crouch;
            player.transform.localScale = crouchScale;
        }
        else
        {
            isRunning = false;
            isCrouching = false;
            speed = walk;
            player.transform.localScale = normalScale;
        }
        // Detects if the player is moving.
        // Useful if you want footstep sounds and or other features in your game.
        isMoving = cc.velocity.sqrMagnitude > 0.0f ? true : false;
    }
}