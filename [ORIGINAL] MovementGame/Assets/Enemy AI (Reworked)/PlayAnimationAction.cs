using System;
using System.Collections;
using Unity.AppUI.UI;
using UnityEngine;

[Serializable]
public class PlayAnimationAction : Action
{
    public string animName;
    public float animDuration = 1f;
    public Animator animator;
    //public float durationFraction;

    /*
    public override IEnumerator Execute()
    {
        Debug.Log("Played an animation!");
        if (animator != null)
            animator.Play(animName);

        yield return null;
    }
    */
}
