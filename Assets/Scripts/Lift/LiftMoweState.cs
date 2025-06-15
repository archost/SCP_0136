using UnityEngine;

public class LiftMoweState : LiftState
{
    protected Transform liftTransform;
    protected Transform pointTransform;
    protected Lift lift;
    protected float speed;

    public LiftMoweState(Lift lift, Transform liftTransform, Transform pointTransform, float speed)
    {
        this.lift = lift;
        this.liftTransform = liftTransform;
        this.pointTransform = pointTransform;
        this.speed = speed;
    }

    public override void StateExecute()
    {
        liftTransform.position = Vector3.MoveTowards(liftTransform.position, pointTransform.position, speed * Time.deltaTime);

        if (Vector3.Distance(liftTransform.position, pointTransform.position) < 0.01f)
        {
            lift.LiftAfkStateApplication();
            lift.OnOrOffPlayerSkripts(true);
        }
    }
}
