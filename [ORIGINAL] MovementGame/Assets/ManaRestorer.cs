using UnityEngine;

public class ManaRestorer : MonoBehaviour
{
    public Transform parent;
    public float x1, z1, x2, z2, value, valueLittle, cooldown;

    void Reposition() {
        transform.position = new Vector3(parent.position.x + Random.Range(x1, x2), parent.position.y, parent.position.z + Random.Range(z1, z2));
    }
    public void RestoreLittle() {
        GlobalVars.instance.player.hpManager.AddMana(valueLittle);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) { 
            GlobalVars.instance.player.hpManager.AddMana(value);
            InvokeRepeating(nameof(RestoreLittle), 0, cooldown);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CancelInvoke();
            Reposition();
        }
    }
}
