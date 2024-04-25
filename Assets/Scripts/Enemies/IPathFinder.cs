using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Interface <c>IPathFinder</c> can be used for creating new pathfinding types used by enemies.
/// </summary>
public interface IPathFinder{

    public abstract bool MoveTowardsTarget(Vector3 pTargetPosition, float pAgentSpeed);

    public abstract void Stop();
}
