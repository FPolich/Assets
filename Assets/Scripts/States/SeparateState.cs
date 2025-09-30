using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeparateState : IState
{
    private float speedWalk;
    private float distanceToTarget;
    private LayerMask neighborMask;
    private Rigidbody rb;
    private Transform transform;
    public SeparateState(float speedWalk, float distanceToTarget, LayerMask neighborMask, Rigidbody rb, Transform transform)
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

        Vector3 separationForce = Vector3.zero;
        foreach (Collider agent in agents)
        {
            Vector3 away = transform.position - agent.transform.position;
            if (away.magnitude > 0)
                separationForce += away.normalized / away.magnitude; // más fuerte si están más cerca
        }

        Vector3 desiredVelocity = separationForce.normalized * speedWalk;
        Vector3 steering = desiredVelocity - rb.velocity;

        rb.AddForce(steering, ForceMode.Acceleration);
    }
    public void StateExit()
    {
        Debug.Log("Exiting Separate State");
    }
}
