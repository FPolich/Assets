using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    public void StateEnter() { }
    public void StateUpdate() { }
    public void StateExit() { }
}
