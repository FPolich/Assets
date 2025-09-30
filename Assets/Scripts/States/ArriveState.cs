using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArriveState : IState 
{
    private float speedWalk;
    private float distanceToTarget;
    private LayerMask neighborMask;
    private Rigidbody rb;
    private Transform transform;

    public ArriveState(float speedWalk, float distanceToTarget, LayerMask neighborMask, Rigidbody rb, Transform transform)
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
        Collider[] agents = Physics.OverlapSphere(transform.position, distanceToTarget, neighborMask);
        if (agents.Length == 0) return;

        // Tomamos al primer agente como objetivo
        Vector3 targetAgent = agents[0].transform.position;
        Vector3 toTarget = targetAgent - transform.position;
        float distance = toTarget.magnitude;

        float slowingRadius = 3f; // radio para empezar a frenar
        float ramped = speedWalk * (distance / slowingRadius);
        float clamped = Mathf.Min(ramped, speedWalk);

        Vector3 desiredVelocity = toTarget.normalized * clamped;
        Vector3 steering = desiredVelocity - rb.velocity;

        rb.AddForce(steering, ForceMode.Acceleration);
    }
    public void StateExit()
    {
        Debug.Log("Exiting Arrive State");
    }
}
