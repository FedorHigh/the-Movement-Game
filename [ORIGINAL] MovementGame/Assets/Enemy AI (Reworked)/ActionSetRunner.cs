using System.Collections;
using System.Collections.Generic;
using Unity.AppUI.UI;
using UnityEngine;

public class ActionSetRunner : MonoBehaviour
{

    [SerializeReference]
    public List<Action> actions = new List<Action>();

    private Coroutine running;


    private void Start()
    {
        // automatically run the sequence when the scene starts
        if (actions != null && actions.Count > 0)
            StartRun();
    }

    public void StartRun()
    {
        if (running != null) StopCoroutine(running);
        running = StartCoroutine(RunSequence());
    }

    public void StopRun()
    {
        if (running != null)
        {
            StopCoroutine(running);
            running = null;
        }
    }

    private IEnumerator RunSequence()
    {
        foreach (var action in actions)
        {
            if (action == null) continue;
            //yield return StartCoroutine(action.Execute());
            yield return null;

            if (action.postDelay > 0)
                yield return new WaitForSeconds(action.postDelay);
        }
        running = null;
    }
}
