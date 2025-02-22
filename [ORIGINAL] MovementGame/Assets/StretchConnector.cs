using UnityEngine;

public class StretchConnector : MonoBehaviour
{
    public GameObject obj;
    public Transform other;
    public float offset;
    public float multiplier;
    public float curDist;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        obj.transform.position = (transform.position + other.transform.position)/2;
        obj.transform.LookAt(other.position);
        curDist = (other.transform.position - transform.position).magnitude;

        obj.transform.localScale = new Vector3(1, 1, (curDist - offset*2)*multiplier);


    }
}
