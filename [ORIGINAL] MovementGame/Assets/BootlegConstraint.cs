using UnityEngine;

public class BootlegConstraint : MonoBehaviour
{
    public Transform source;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (source != null) { 
            transform.position = source.position;
            transform.rotation = source.rotation;
        }
    }
}
