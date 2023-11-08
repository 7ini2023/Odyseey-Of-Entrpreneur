using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CraneControl : NetworkBehaviour
{
    public float rotationSpeed = 50f;
    public float armMovementSpeed = 1f;
    public GameObject hookPrefab;

    private Transform craneArm;
    private GameObject hookObject;
    private bool isHookAttached = false;

    private void Start()
    {
        craneArm = transform.Find("CraneArm"); // Adjust the name accordingly
    }

    private void Update()
    {
        if (IsOwner)
        {
            // Rotate the crane left and right
            float rotationInput = Input.GetAxis("Horizontal");
            transform.Rotate(Vector3.up, rotationInput * rotationSpeed * Time.deltaTime);

            // Move the crane arm up and down
            float armMovementInput = Input.GetAxis("Vertical");
            craneArm.Translate(Vector3.up * armMovementInput * armMovementSpeed * Time.deltaTime);

            // Check for attaching or releasing the hook
            if (Input.GetMouseButtonDown(0))
            {
                if (!isHookAttached)
                {
                    AttachHook();
                }
            }
            else if (Input.GetMouseButtonDown(1))
            {
                if (isHookAttached)
                {
                    ReleaseHook();
                }
            }
        }
    }

    private void AttachHook()
    {
        if (hookPrefab != null)
        {
            // Spawn the hook as a child of the crane
            hookObject = Instantiate(hookPrefab, craneArm.position, Quaternion.identity);
            NetworkObject networkObject = hookObject.GetComponent<NetworkObject>();
            networkObject.SpawnWithOwnership(NetworkManager.Singleton.LocalClientId);
            isHookAttached = true;
        }
    }

    private void ReleaseHook()
    {
        if (hookObject != null)
        {
            NetworkObject networkObject = hookObject.GetComponent<NetworkObject>();
            networkObject.Despawn(true);
            isHookAttached = false;
        }
    }
}
