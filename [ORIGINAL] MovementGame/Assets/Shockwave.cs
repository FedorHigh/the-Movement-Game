using UnityEngine;

public class Shockwave : MonoBehaviour
{
    public float lifetime, speed;
    public Vector3 growth;
    Vector3 scale, pos;
    void Start()
    {
        scale = transform.localScale;
        pos = transform.position;
        Invoke("End", lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        scale += growth * Time.deltaTime;
        pos += transform.forward * speed * Time.deltaTime;

        transform.localScale = scale;
        transform.position = pos;
    }
    void End() {
        Destroy(gameObject);
    }
}
