using UnityEngine;
using Interfaces;

public class SpawnSpell : MonoBehaviour, IAbility
{
    public BetterController player;
    public GameObject body;
    public GameObject object1;
    public GameObject object2;
    public GameObject object3;
    public float distance;

    public void Reset()
    {
        player = GetComponent<BetterController>();
    }
    public void Cast()
    {
        GameObject tmp = Instantiate(object1);
        tmp.transform.position = Vector3.zero;
        tmp.transform.position = body.transform.position;
        tmp.transform.localRotation = Quaternion.Euler(player.lookDirection);
        tmp.transform.position += tmp.transform.forward * distance;
    }

    public void HeavyCast()
    {
        GameObject tmp = Instantiate(object1);
        tmp.transform.position = Vector3.zero;
        tmp.transform.position = body.transform.position;
        tmp.transform.localRotation = Quaternion.Euler(player.lookDirection);
        tmp.transform.position += tmp.transform.forward * distance;
        tmp.transform.localScale = tmp.transform.localScale * 10;
    }

    public void LightCast()
    {
        GameObject tmp = Instantiate(object1);
        tmp.transform.position = Vector3.zero;
        tmp.transform.position = body.transform.position;
        tmp.transform.localRotation = Quaternion.Euler(player.lookDirection);
        tmp.transform.position += tmp.transform.forward * distance;
        tmp.transform.localScale = tmp.transform.localScale * 0.1f;
    }
}
