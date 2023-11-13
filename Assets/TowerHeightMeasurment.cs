using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Events;

public class TowerHeightMeasurment : NetworkBehaviour
{
     public Transform towerBase; // Reference to the empty GameObject at the base of the tower
     public UnityEvent<float> OnMeasureHeight; // Event to trigger when the tower height is measured

    public void MeasureHeight()
    {
        if (IsOwner)
        {
            // Get the tower's height by measuring the distance between its top and the base
            float towerHeight = towerBase.position.y - transform.position.y;
            
            // Print the height to the console or display it in VR
            Debug.Log("Tower Height: " + towerHeight);
            OnMeasureHeight.Invoke(towerHeight);
        }
    }
}
