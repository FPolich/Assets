using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boids : MonoBehaviour
{
    [Header("Alignment")]
    public LayerMask neighborMask;
    public float alignmentRadius = 10f;
    public float maxSpeed = 5.0f;
    public float maxForce = 10.0f;

    [Header("Separation")]
    public float separationRadius = 5.0f;
    public float separationForce = 10.0f;
    private int separationCount = 0;
    public string groundTag = "Ground";


    [Header("Arrive")]
    public LayerMask foodMask;
    public float distanceToFood = 15f;


    [Header("Evade")]
    public Transform targetNPC;
    public float distanceToEvade = 10f;

    private enum State { Align, Evade, Arrive, Separate, Nothing }

    private State currentState = State.Align;

    private void Update()
    {
        float distanceToNPC = Vector3.Distance(transform.position, targetNPC.position);
        Collider[] neighbors = Physics.OverlapSphere(transform.position, alignmentRadius, neighborMask);
        Collider[] foodies = Physics.OverlapSphere(transform.position, distanceToFood, foodMask);
        Vector3 neighborPosition = Vector3.zero;
        Vector3 separationVector = Vector3.zero;
        Vector3 foodPosition = Vector3.zero;

        foreach (Collider neighbor in neighbors)
        {
            neighborPosition += neighbor.transform.position;
            if (neighbor.gameObject.tag == groundTag)
            {
                Vector3 offset = transform.position - neighbor.transform.position;
                if (offset.magnitude < separationRadius && offset.magnitude != 0)
                {
                    separationVector += offset.normalized / offset.magnitude;
                    separationCount++;
                }
            }
        }

        foreach (Collider food in foodies)
        {
            if (food.gameObject != null && food.gameObject.layer == 4)
            {
                foodPosition += food.transform.position;
            }
        }

        if (distanceToNPC < distanceToEvade)
        {
            currentState = State.Evade;
        }
        else if (Vector3.Distance(transform.position, foodPosition) < distanceToFood && foodPosition.magnitude != 0)
        {
            currentState = State.Arrive;
        }
        else if (Vector3.Distance(transform.position, neighborPosition) < alignmentRadius)
        {
            currentState = State.Align;
        }
        else if (separationCount > 0 && Vector3.Distance(transform.position, separationVector) < separationRadius)
        {
            Debug.Log("separate" + separationVector.magnitude);
            currentState = State.Separate;
        }
        else
        {
            currentState = State.Nothing;
        }

        switch (currentState)
        {
            case State.Align:
                Move((neighborPosition - transform.position).normalized);
                break;

            case State.Evade:
                Vector3 evadeForce = Evade(targetNPC.position);
                Move(evadeForce);
                break;

            case State.Arrive:
                transform.Translate((foodPosition - transform.position).normalized * maxSpeed * Time.deltaTime);
                break;

            case State.Separate:
                Move(separationVector.normalized);
                break;
            case State.Nothing:
                Vector3 actualPosition = transform.position;
                transform.position = actualPosition;
                break;
        }
    }

    void Move(Vector3 force)
    {
        transform.position += force * Time.deltaTime;
    }

    Vector3 Evade(Vector3 targetPosition)
    {
        if (targetNPC == null) return Vector3.zero;
        Vector3 desiredVelocity = (transform.position - targetPosition).normalized * maxSpeed;
        Vector3 steering = desiredVelocity - transform.forward;
        steering = Vector3.ClampMagnitude(steering, maxSpeed);
        return steering;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, separationRadius);
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.green;
        //Gizmos.DrawWireSphere(transform.position, alignmentRadius);

        //Gizmos.color = Color.blue;
        //Gizmos.DrawWireSphere(transform.position, separationRadius);

        //Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(transform.position, distanceToEvade);

        //Gizmos.color = Color.yellow;
        //Gizmos.DrawWireSphere(transform.position, distanceToFood);
    }
}
