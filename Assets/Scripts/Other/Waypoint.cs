using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Class <c>Waypoint</c> is used by the pathfinder to find the next destination.
/// </summary>
public class Waypoint : MonoBehaviour{

    [SerializeField] List<Waypoint> nextPossibleWaypoints;


    public Waypoint ReturnNextWaypoint() {

        int randomNumber = Random.Range(0, nextPossibleWaypoints.Count);

        return nextPossibleWaypoints[randomNumber];
    }

}
