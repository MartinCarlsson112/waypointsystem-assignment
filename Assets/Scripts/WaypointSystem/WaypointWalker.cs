using UnityEngine;

public class WaypointWalker : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    WaypointGroup waypoints;

    [SerializeField]
    WaypointWalkerOption options;

    [SerializeField]
    float movementSpeed;

    [SerializeField]
    bool pause = false;

#pragma warning restore 0649

    Vector3 waypoint;
    bool hasWaypoint = false;
    bool reachedEnd = false;
    int waypointIndex = 0;

    private void Update()
    {
        if(pause)
        {
            return;
        }

        if(waypoints == null)
        {
            return;
        }

        if(hasWaypoint)
        {
            Move();
        }
        else
        {
            hasWaypoint = waypoints.Begin(ref waypoint);
        }
    }

    private void Move()
    {
        transform.position += Vector3.Normalize((waypoint - transform.position)) * Time.deltaTime * movementSpeed;
        CheckDistance();
    }

    private void CheckDistance()
    {
        var distance = Vector3.Distance(transform.position, waypoint);
        if (distance < options.distanceToReach)
        {
            if (reachedEnd)
            {
                if(!waypoints.Previous(waypointIndex, out waypointIndex, ref waypoint))
                {
                    reachedEnd = false;
                    waypoints.Next(waypointIndex, out waypointIndex, ref waypoint);
                }
            }
            else
            {
                if (!waypoints.Next(waypointIndex, out waypointIndex, ref waypoint))
                {
                    reachedEnd = true;
                    waypoints.Previous(waypointIndex, out waypointIndex, ref waypoint);
                }
            }
        }
    }
}
