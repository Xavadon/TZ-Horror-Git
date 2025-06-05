using UnityEngine;

public class PlayerCheckerCollider : MonoBehaviour
{
    public bool IsPlayerHere { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController player))
        {
            IsPlayerHere = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerController player))
        {
            IsPlayerHere = false;
        }
    }
}
