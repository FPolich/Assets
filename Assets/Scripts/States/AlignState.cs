using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignState : IState
{
    private float distanceToTarget;
    private LayerMask neighborMask;
    Collider[] agents;
    Vector3 targetAgent = Vector3.zero;
    Vector3 targetDirection;
    Vector3 targetPosition;
    Vector3 desiredVelocity;
    float distance;
    float lookAhead;
    float speedWalk;
    Rigidbody rb;
    Vector3 steeringForce;
    private Transform transform;

    public AlignState(float speedWalk, float distanceToTarget, LayerMask neighborMask, Rigidbody rb, Transform transform)
    {
        this.speedWalk = speedWalk;
        this.distanceToTarget = distanceToTarget;
        this.neighborMask = neighborMask;
        this.rb = rb;
        this.transform = transform;
    }
    public void StateEnter() { }
    public void StateUpdate()
    {
        agents = Physics.OverlapSphere(transform.position, distanceToTarget, neighborMask);
        if (agents.Length == 0) return;

        foreach (Collider agent in agents)
        {
            targetAgent += agent.transform.position;
        }
        targetAgent /= agents.Length;
        targetDirection = targetAgent - transform.position;
        distance = targetDirection.magnitude;
        lookAhead = distance / speedWalk;
        targetPosition = targetAgent + targetDirection.normalized * lookAhead;
        desiredVelocity = (targetPosition - transform.position).normalized * speedWalk;
        steeringForce = desiredVelocity - rb.velocity;
        rb.AddForce(steeringForce, ForceMode.Acceleration);

    }
    public void StateExit()
    {
        Debug.Log("Exiting Align State.");
    }
}
