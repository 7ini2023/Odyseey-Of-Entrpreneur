using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Unity.Netcode;
using UnityEngine.SceneManagement;
public class BackToGameMenuButton : NetworkBehaviour
{
   public GameObject button;
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

    [ServerRpc]
    public void LoadSceneServerRpc()
    {
         if (IsServer)
        {
            SceneTransitionManager.singleton.GoToSceneAsync(1);
        }
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

        LoadSceneServerRpc();
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
}
