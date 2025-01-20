using UnityEngine;
using Cinemachine;

public class aimDirector : MonoBehaviour
{
    public Transform cnm, lcnm;
    public GameObject marker, Aabdir;
    public Transform cameraPoint, targetPoint;
    public LayerMask layers;
    public Ray ray;
    public RaycastHit hitData;
    public bool hit;
    public BetterController player;
    public LockOnManager lockOnManager;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GetComponent<BetterController>();
        lockOnManager = GetComponent<LockOnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.lockedOn)
        {
            targetPoint = lockOnManager.target.transform;
            ray = new Ray(lcnm.position, targetPoint.position - lcnm.position);
        }
        else {
            ray = new Ray(cnm.position, cameraPoint.position - cnm.position);
        }
        hit = Physics.Raycast(ray, out hitData, 10000f, layers);
        if (hit) marker.transform.position = hitData.point;
        else {
            if (player.lockedOn)
            {
                marker.transform.position = targetPoint.position;
            }
            else
            {
                marker.transform.position = cameraPoint.position;
            }
        } 

        Aabdir.transform.LookAt(marker.transform.position);

    }
}
