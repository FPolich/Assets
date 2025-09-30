using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvadeState : IState
{
    private float speedWalk;
    private float distanceToTarget;
    private LayerMask neighborMask;
    private Rigidbody rb;
    private Transform transform;
    public EvadeState(float speedWalk, float distanceToTarget, LayerMask neighborMask, Rigidbody rb, Transform transform)
    {
        this.speedWalk = speedWalk;
        this.distanceToTarget = distanceToTarget;
        this.neighborMask = neighborMask;
        this.rb = rb;
        this.transform = transform;
    }

    public void StateEnter() {  }
    public void StateUpdate()
    {
        Collider[] agents = Physics.OverlapSphere(transform.position, distanceToTarget, neighborMask);
        if (agents.Length == 0) return;

        Vector3 threat = agents[0].transform.position;
        Vector3 fromThreat = transform.position - threat;

        Vector3 desiredVelocity = fromThreat.normalized * speedWalk;
        Vector3 steering = desiredVelocity - rb.velocity;

        rb.AddForce(steering, ForceMode.Acceleration);
    }
    public void StateExit()
    {
        Debug.Log("Exiting Evade State");
    }
}
