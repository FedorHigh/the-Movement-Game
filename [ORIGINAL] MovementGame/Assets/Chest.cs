using CustomClasses;
using UnityEngine;

public class Chest : Interactable
{
    public GameObject hoverParticles, openParticles;
    public SaveBoolean saver;
    public int score = 0;

    public override void HoverOn() { 
        hoverParticles.SetActive(true);
    }
    public override void HoverOff() { hoverParticles.SetActive(false); }

    public override void Activate() { 
        HoverOff();
        if (score != 0) { 
            GlobalVars.instance.scoreCounter.AddScore(score);
        }
        openParticles.SetActive(true);
        tag = "Untagged";
        saver.value = true;

        gameObject.SetActive(false);
    }
    public void LoadActivated(bool active) {
        gameObject.SetActive(!active);
    }

    public void Awake()
    {
        
        saver.onLoad += LoadActivated;
        saver.key = ID + " opened";
    }
}
