using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float time;
    float passed = 0;
    public GameObject spawn;
    

    // Update is called once per frame
    void Update()
    {
        passed += Time.deltaTime;
        if (passed >= time) { 
            Instantiate(spawn, transform.position, transform.rotation);
            passed = 0;
        }
    }
}
