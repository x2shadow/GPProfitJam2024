using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeMute : MonoBehaviour
{
    [SerializeField] VolumeManager volumeManager;

    public void ToggleMute()
    {
        volumeManager.ToggleMute();
    }
}
