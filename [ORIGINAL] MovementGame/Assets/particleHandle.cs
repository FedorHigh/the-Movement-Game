using UnityEngine;
using UnityEngine.Animations;

public class ParticleHandle : MonoBehaviour
{
    public GameObject parent;
    public ParticleSystem prt;
    private void Start()
    {
        prt = GetComponent<ParticleSystem>();
    }
    public void Play() {
        if (parent != null)
        {
            transform.position = parent.transform.position;
            transform.rotation = parent.transform.rotation;
        }
        prt.Play();
    }
}
