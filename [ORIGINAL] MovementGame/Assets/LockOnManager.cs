using Cinemachine;
using UnityEngine;
using UnityEngine.Animations;


public class LockOnManager : MonoBehaviour
{
    public BetterController player;
    public GameObject rtc, target, camerapoint, pointer, protector;
    public float radius, fallOffRadius;
    public LayerMask layer;
    public CinemachineFreeLook lookaround;
    public CinemachineVirtualCamera locked;
    public float cameraDistance;

    public string savedXaxis;
    public string savedYaxis;

    ConstraintSource tmpsource;
    public void ResetLockOn()
    {
        target = null;
        player.lockedOn = false;
        protector.SetActive(false);
        lookaround.Priority = 10;
        locked.Priority = 1;
    }
    void TryLockOn() {
        //target = null;
        Collider[] targets = Physics.OverlapSphere(transform.position + rtc.transform.forward*radius, radius, layer);
        Vector3 dir;
        float min = 360;
        GameObject tmptrg = null;
        for (int i = 0; i < targets.Length; i++) {
            dir = targets[i].transform.position - transform.position;
            float tmp = Vector3.Angle(rtc.transform.forward, dir);
            if (tmp < min)
            {
                min = tmp;
                tmptrg = targets[i].gameObject;
            }
        }


        if (tmptrg == target | tmptrg == null){
            ResetLockOn();

            //lookaround.m_YAxis.m_InputAxisName = savedYaxis;
            //lookaround.m_XAxis.m_InputAxisName = savedXaxis;
        }
        else
        {
            target = tmptrg;
            player.lockedOn = true;
            protector.SetActive(true);
            //tmpsource.sourceTransform = target.transform;
            //protector.GetComponent<ParentConstraint>().SetSource(0, tmpsource);

            lookaround.Priority = 1;
            locked.Priority = 10;

            savedXaxis = lookaround.m_XAxis.m_InputAxisName;
            savedYaxis = lookaround.m_YAxis.m_InputAxisName;

            //lookaround.m_YAxis.m_InputAxisName = "";
            //lookaround.m_XAxis.m_InputAxisName = "";

        }
    }
    void Start()
    {
        player = GetComponent<BetterController>();
        rtc = player.rtc;
        //tmpsource.weight = 1;
    }

    void Update()
    {
        cameraDistance += Input.GetAxis("Mouse ScrollWheel");
        cameraDistance = Mathf.Clamp(cameraDistance, 0.0f, 2.0f);
        if (Input.GetKeyDown(KeyCode.Q)) {
            TryLockOn();
        }
        if (player.lockedOn) {
            //if(target == GameObject.Missing)player.lockedOn = false;
            protector.transform.position = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
            rtc.transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
            camerapoint.transform.position = transform.position + (( (target.transform.position - transform.position) / 2 ) * cameraDistance);
        }
    }
}
