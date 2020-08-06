using System.Data.SqlTypes;
using UnityEngine;

public class WaypointGroup : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    WaypointGroupOption options;

    [SerializeField]
    [HideInInspector]
    Vector3[] waypoints;
#pragma warning restore 0649

    public bool Next(int inputIndex, out int outIndex, ref Vector3 waypoint)
    {
        outIndex = inputIndex;
       
        //if looping waypoints and last index is input
        //wrap outindex
        if(options.loopWaypoints)
        {
            if(inputIndex + 1 == waypoints.Length)
            {
                outIndex = -1;
            }
        }
        
        if(waypoints.Length > outIndex+1)
        {
            outIndex++;
            waypoint = waypoints[outIndex];
            return true;
        }
        return false;
    }

    public bool Previous(int inputIndex, out int outIndex, ref Vector3 waypoint)
    {
        outIndex = inputIndex;

        //if looping waypoints and last index is input
        //wrap outindex
        if (options.loopWaypoints)
        {
            if (inputIndex-1 < 0)
            {
                outIndex = waypoints.Length-1;
            }
        }

        if (outIndex-1 >= 0)
        {
            outIndex--;
            waypoint = waypoints[outIndex];
            return true;
        }
        return false;
    }

    public bool Begin(ref Vector3 waypoint)
    {
        if(waypoints.Length > 0)
        {
            waypoint = waypoints[0];
            return true;
        }
        return false;
    }

    public bool GetPoint(in int inputIndex, ref Vector3 waypoint)
    {
        if(inputIndex < waypoints.Length && inputIndex > 0)
        {
            waypoint = waypoints[inputIndex];
            return true;
        }
        return false;
    }
}
