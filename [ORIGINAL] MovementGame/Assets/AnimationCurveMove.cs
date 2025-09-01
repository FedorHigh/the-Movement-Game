using CustomClasses;
using UnityEngine;
using UnityEngine.Splines;

public class AnimationCurveMove : MonoBehaviour
{
    public AnimationCurve Xcurve = AnimationCurve.Constant(0, 1, 0), Ycurve = AnimationCurve.Constant(0, 1, 0), Zcurve = AnimationCurve.Constant(0, 1, 0);
    public Vector3 targetPosition;
    public float duration = 1, speed = 1;
    public Transform toMove;
    public Rigidbody toMoveRb;
    public string endMethod;
    public Transform directionTransform;

    float time = 0, curveMultiplier = 100;
    bool playing = false;
    Vector3 startPosition;
    StateMachine altCaller;
    IAbility caller;


    public void Start()
    {
        if(toMove == null) toMove = gameObject.transform;
        if(toMoveRb == null) TryGetComponent<Rigidbody>(out toMoveRb);
        TryGetComponent<StateMachine>(out altCaller);
    }

    public void Play(IAbility call) { 

        caller = call;
  
        time = 0;
        playing = true;
        startPosition = transform.position;
    }
    public void Reset()
    {
        playing = false;
        UpdatePosition();

        if (caller != null) caller.Finish();
        if (altCaller != null) altCaller.Invoke(endMethod, 0);
    }
    void UpdatePosition() { 
        float progress = time/duration;
        Vector3 newPosition = startPosition + (targetPosition - startPosition) * progress;
        
        newPosition += directionTransform.right * Xcurve.Evaluate(progress) * curveMultiplier;
        newPosition += directionTransform.up * Ycurve.Evaluate(progress) * curveMultiplier;
        newPosition += directionTransform.forward * Zcurve.Evaluate(progress) * curveMultiplier;

        if(toMoveRb==null)toMove.position = newPosition;
        else toMoveRb.MovePosition(newPosition);
    }
    void Update()
    {
        if (playing)
        {
            time += Time.deltaTime * speed;
            if (time >= duration)
            {
                time = duration;
                Reset();
            }
            else UpdatePosition();
        }
    }
}
