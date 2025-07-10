using UnityEngine;

public class Explosive : MonoBehaviour
{
    public GameObject Explosion;
    public bool onCollision, onStart, destroySelf;
    public float delay;
    public LayerMask layers;

    void Explode() {
        GameObject obj;
        if (Explosion != null)
        {
            obj = Instantiate(Explosion);
            obj.transform.position = transform.position;
        }
        if (destroySelf) Destroy(gameObject);
    }

    void Start()
    {
        if (onStart) Invoke("Explode", delay);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if (onCollision) {
            Invoke("Explode", delay);
        }
    }

}
