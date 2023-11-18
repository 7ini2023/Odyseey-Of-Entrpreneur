using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Unity.Netcode;
public class SpawnObject : NetworkBehaviour
{
    public GameObject button;
    public GameObject objectToSpawnPrefab;
    public float spawnDistance = 1.0f;
    public UnityEvent OnPress;
    public UnityEvent OnRelease;
    GameObject presser;
    AudioSource sound;
    bool isPressed;
    void Start()
    {
        sound = GetComponent<AudioSource>();
        isPressed = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isPressed)
        {
            button.transform.position = new Vector3(button.transform.position.x, button.transform.position.y - 0.1f, button.transform.position.z);
            presser = other.gameObject;
            OnPress.Invoke();
            sound.Play();
            isPressed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == presser)
        {
            button.transform.position = new Vector3(button.transform.position.x, button.transform.position.y + 0.1f, button.transform.position.z);
            OnRelease.Invoke();
            isPressed = false;
        }
    }

    public void Update(){
        if(isPressed){
            SpawnObjectInFrontServerRpc();
        }
    }

    [ServerRpc]
    private void SpawnObjectInFrontServerRpc()
    {
        // Ensure that a prefab is assigned
        if (objectToSpawnPrefab == null)
        {
            Debug.LogError("Object to Spawn is not assigned.");
            return;
        }

        // Spawn the object in front of the button
        Vector3 spawnPosition = button.transform.position + button.transform.forward * spawnDistance;
        Quaternion spawnRotation = button.transform.rotation;

        // Instantiate the object
        GameObject spawnedObject = Instantiate(objectToSpawnPrefab, spawnPosition, spawnRotation);

        NetworkObject networkObject = spawnedObject.GetComponent<NetworkObject>();

        if(networkObject != null){
            networkObject.Spawn();
        }

        // Optionally, you can network the spawned object using Unity.Netcode or other networking solutions
        // Example: NetworkObject networkObject = spawnedObject.GetComponent<NetworkObject>();
        // networkObject.Spawn();
    }
}
