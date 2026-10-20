using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    [SerializeField] RhythmManager rhythm;
    public FlowMeter meter;
    public HealthManager health;
    public GridPosition position;

    int lastHitSubdivision;
    int invincibility = 12;

    void Start()
    {

    }

    void Update()
    {
        
    }
}
