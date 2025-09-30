using UnityEngine;

public enum NPCState
{
    Align,
    Evade,
    Arrive,
    Separate,
    Idle
}

public class FSM : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private NPCState currentStateType;
    private IState currentState;

    [Header("Stats")]
    [SerializeField] private float speedWalk = 5f;
    [SerializeField] private float cooldown = 2f;
    [SerializeField] private float maxEnergy = 100f;
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float distanceToTarget = 5f;

    [Header("Neighbors")]
    [SerializeField] private LayerMask neighborMask;

    private void Start()
    {
        ChangeState(currentStateType);
    }

    private void Update()
    {
        if (currentStateType == NPCState.Idle)
            currentState?.StateUpdate();
    }

    private void FixedUpdate()
    {
        if (currentStateType != NPCState.Idle)
            currentState?.StateUpdate();
    }

    public void ChangeState(NPCState newState)
    {
        currentState?.StateExit();
        currentStateType = newState;

        switch (currentStateType)
        {
            case NPCState.Idle:
                currentState = new IdleState(cooldown, maxEnergy, maxSpeed);
                break;
            case NPCState.Arrive:
                currentState = new ArriveState(speedWalk, distanceToTarget, neighborMask, GetComponent<Rigidbody>(), transform);
                break;
            case NPCState.Align:
                currentState = new AlignState(speedWalk, distanceToTarget, neighborMask, GetComponent<Rigidbody>(), transform);
                break;
            case NPCState.Evade:
                currentState = new EvadeState(speedWalk, distanceToTarget, neighborMask, GetComponent<Rigidbody>(), transform);
                break;
            case NPCState.Separate:
                currentState = new SeparateState(speedWalk, distanceToTarget, neighborMask, GetComponent<Rigidbody>(), transform);
                break;
        }

        currentState?.StateEnter();
    }

}
