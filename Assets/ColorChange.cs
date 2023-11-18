using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class ColorChange : NetworkBehaviour
{
    public Material objectMaterial;

    private bool isGrabbed = false;

    private void Start()
    {
        if (!IsOwner)
        {
            // Disable script on non-local players
            enabled = false;
        }

        if (objectMaterial == null)
        {
            objectMaterial = GetComponent<Renderer>().material;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsOwner && !isGrabbed && other.CompareTag("ColorCube"))
        {
            MergeWithColorServerRpc(other.GetComponent<Renderer>().material.color);
        }
    }

    [ServerRpc]
    private void MergeWithColorServerRpc(Color newColor)
    {
        // Call this function on the owner (local or remote) to update the color
        objectMaterial.color = newColor;

        // Send the color change to all clients (including the owner) using a RemoteAction
        UpdateColorClientRpc(newColor);
    }

    [ClientRpc]
    private void UpdateColorClientRpc(Color newColor)
    {
        // Update the color on all clients
        objectMaterial.color = newColor;
    }

    private void Update()
    {
        if (!IsOwner)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) && !isGrabbed)
        {
            isGrabbed = true;
            GrabObject();
        }
        else if (Input.GetKeyDown(KeyCode.Space) && isGrabbed)
        {
            isGrabbed = false;
            ReleaseObject();
        }
    }


    private void GrabObject()
    {
        SetParentToHandClientRpc();
    }

    private void ReleaseObject()
    {
        SetParentToNullClientRpc();
    }

    [ClientRpc]
    private void SetParentToHandClientRpc()
    {
        transform.SetParent(IsOwner ? GameObject.FindWithTag("Player").transform : null);
    }

    [ClientRpc]
    private void SetParentToNullClientRpc()
    {
        transform.SetParent(null);
    }
}
