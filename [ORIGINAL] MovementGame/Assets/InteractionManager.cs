using CustomClasses;
using TMPro;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public string curText = "";
    public TextMeshProUGUI textMesh;
    public GameObject panel;
    Interactable current = null;
    public KeyCode interactKey = KeyCode.E;
    void Start()
    {
        panel.SetActive(false);
    }

    public void Add(Interactable interactable) { 
        panel.SetActive(true);
        curText = interactable.description;
        textMesh.text = interactable.description;
        current = interactable;
        current.HoverOn();
    }

    public void Remove(Interactable interactable) {
        if (curText == interactable.description)
        {
            current.HoverOff();
            current = null;
            curText = "";
            textMesh.text = "";
            panel.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            if (current != null)
            {
                Interactable interactable = current;
                if(current.oneTime) Remove(current);
                interactable.Activate();
                
            }
        }
    }
}
