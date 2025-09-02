using UnityEngine;

public class NPC : MonoBehaviour
{
    [Header("Waypoint System")]
    public Transform[] waypoints;
    private int waypointIndex;
    [SerializeField] private float offset;
    [SerializeField] private int directionArray = 1;

    [SerializeField] private float energy;
    [SerializeField] private float currentEnergy;
    [SerializeField] private float speedToWalk;
    [SerializeField] private float cooldown;
    bool canIdle = true;
    [SerializeField] float currentTime;

    [Header("Chase")]

    public float distanceToTarget;
    public LayerMask neighborMask;
    void Start()
    {
        currentEnergy = energy;
        currentTime = cooldown;
    }

    void MoveByWaypoints()
    {
        transform.position = Vector3.MoveTowards(transform.position, waypoints[waypointIndex].position, speedToWalk * Time.deltaTime);
        if (Vector3.Distance(transform.position, waypoints[waypointIndex].position) < offset)
        {
            waypointIndex += directionArray;
        }
        if (waypointIndex >= waypoints.Length)
        {
            waypointIndex = 0;
        }
    }

    void Chase()
    {
        Collider[] agents = Physics.OverlapSphere(transform.position, distanceToTarget, neighborMask);
        Vector3 targetAgent = Vector3.zero;

        foreach (Collider agent in agents)
        {
            targetAgent += agent.transform.position;
        }

        if (agents.Length > 0)
        {
            Vector3 targetDirection = targetAgent - transform.position;
            float distance = targetDirection.magnitude;
            float lookAhead = distance / speedToWalk;
            Vector3 targetPosition = targetAgent * lookAhead;
            Vector3 desiredVelocity = (targetPosition - transform.position).normalized * speedToWalk;
            Vector3 steeringForce = desiredVelocity - GetComponent<Rigidbody>().velocity;
            GetComponent<Rigidbody>().AddForce(steeringForce);

            transform.Translate(targetAgent.normalized * speedToWalk * Time.deltaTime);
        }
    }

    void Idle()
    {
        currentTime -= Time.deltaTime;
        if (currentEnergy < energy && currentTime <= 0)
        {
            speedToWalk = 0;
            transform.rotation = Quaternion.identity;
            currentEnergy = energy;
            canIdle = false;
            currentTime = cooldown;
        }
        else
        {
            speedToWalk = 10;
            canIdle = true;
        }
    }

    void FSM()
    {
        Collider[] agents = Physics.OverlapSphere(transform.position, distanceToTarget, neighborMask);
        Vector3 targetAgent = Vector3.zero;

        foreach (Collider agent in agents)
        {
            targetAgent += agent.transform.position;
        }

        if (Vector3.Distance(transform.position, targetAgent) > distanceToTarget && currentEnergy >= 5 && canIdle)
        {
            MoveByWaypoints();
            if (Vector3.Distance(transform.position, waypoints[waypointIndex].position) < (offset + 0.1f) && waypointIndex == 0)
            {
                currentEnergy -= 5;
            }
        }
        else if (Vector3.Distance(transform.position, targetAgent) <= distanceToTarget && currentEnergy >= 1 && canIdle)
        {
            Chase();
            currentEnergy -= 0.5f;
        }
        else
        {
            Idle();
        }

    }

    private void Update()
    {
        FSM();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanceToTarget);
    }
}
