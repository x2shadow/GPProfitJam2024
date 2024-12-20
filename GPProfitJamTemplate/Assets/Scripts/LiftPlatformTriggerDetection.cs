using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftPlatformTriggerDetection : MonoBehaviour
{
    [SerializeField] LiftPlatform liftPlatform;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            liftPlatform.SetPlayerOnLift(other.GetComponent<CharacterController>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            liftPlatform.UnsetPlayerOnLift();
        }
    }
}

