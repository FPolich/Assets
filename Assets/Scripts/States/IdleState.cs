using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : MonoBehaviour, IState
{
    private float currentTime;
    private float cooldown;
    private float speedWalk;
    private float currentEnergy;
    private float maxEnergy;
    private float maxSpeed;

    public IdleState (float cooldown, float maxEnergy, float maxSpeed)
    {
        this.cooldown = cooldown;
        this.maxEnergy = maxEnergy;
        this.maxSpeed = maxSpeed;
    }
    public void StateEnter()
    {
        currentEnergy = maxEnergy;
        currentTime = cooldown;
    }

    public void StateUpdate()
    {
        currentTime -= Time.deltaTime;
        if (currentTime <= 0 && currentEnergy < maxEnergy)
        {
            speedWalk = 0;
            transform.rotation = Quaternion.identity;
            currentEnergy = maxEnergy;
            currentTime = cooldown;
        }
        else
        {
            speedWalk = maxSpeed;
        }
    }

    public void StateExit()
    {
        Debug.Log("Exiting Idle State");
    }

    //Getters
    public float GetSpeedWalk() => speedWalk;
    public float GetEnergy() => currentEnergy;
}
