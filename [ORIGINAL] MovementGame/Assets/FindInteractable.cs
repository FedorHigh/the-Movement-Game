using CustomClasses;
using UnityEngine;

public class FindInteractable : MonoBehaviour
{
    public InteractionManager manager;

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("gottem");
        if (other.CompareTag("Interactable"))
        {
            //Debug.Log("gottem!!!");
            manager.Add(other.GetComponent<Interactable>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            manager.Remove(other.GetComponent<Interactable>());
        }
    }

    
}
