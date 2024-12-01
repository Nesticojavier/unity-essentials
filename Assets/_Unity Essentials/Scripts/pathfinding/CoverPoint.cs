using UnityEngine;

public class CoverPoint : MonoBehaviour
{
    public bool hiddenForPatrol1; 
    public bool hiddenForPatrol2;

        public override string ToString()
    {
        return $"[hiddenForPatrol1={hiddenForPatrol1},\n hiddenForPatrol2={hiddenForPatrol2}]";
    }
}