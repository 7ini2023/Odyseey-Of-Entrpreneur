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

    private void Start()
    {
        windZone = GetComponent<WindZone>();
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


