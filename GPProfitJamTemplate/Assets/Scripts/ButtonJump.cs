using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonJump : MonoBehaviour
{
    [SerializeField] PlayerMovement playerMovement;

    public void Jump()
    {
        playerMovement.Jump();
    }
}
