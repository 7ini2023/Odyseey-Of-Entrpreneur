using System.Collections;
using System.Collections.Generic;
//using Mirror;
using Unity.Netcode;
using UnityEngine;

public class WindActivation : NetworkBehaviour
{
    public NetworkVariable<bool> windActive = new NetworkVariable<bool>(); // Define windActive as a network variable
    public WindZone windZone;
    public float maxWindStrength = 10f; // Adjust as needed
    public float windIncreaseRate = 1f; // Adjust as needed

    public event System.Action<bool> OnWindActivationChanged; // Event to trigger when windActive changes

    private void Start()
    {
        windZone = GetComponent<WindZone>();
        windActive.OnValueChanged += OnWindActiveValueChanged; // Subscribe to the OnValueChanged event
    }

    private void OnDestroy()
    {
        windActive.OnValueChanged -= OnWindActiveValueChanged; // Unsubscribe from the OnValueChanged event
    }

    private void OnWindActiveValueChanged(bool oldVal, bool newVal)
    {
        // Notify subscribers that wind activation has changed
        if (OnWindActivationChanged != null)
        {
            OnWindActivationChanged(newVal);
        }
    }

    private void Update()
    {
        if (IsOwner)
        {
            if (windActive.Value)
            {
                windZone.windMain = Mathf.Lerp(windZone.windMain, maxWindStrength, Time.deltaTime * windIncreaseRate);
            }
            else
            {
                windZone.windMain = 0f;
            }
        }
    }

    [ServerRpc]
    public void CmdSetWindActiveServerRpc(bool active)
    {
        windActive.Value = active; // Set the network variable's value
    }

     public void ToggleWind()
    {
        windActive.Value = !windActive.Value;
    }

    
}


