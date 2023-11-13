using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Unity.Netcode;

public class WindButton : NetworkBehaviour
{
   public GameObject button;
    public UnityEvent OnPress;
    public UnityEvent OnRelease;
    public Text windLogText;
    GameObject presser;
    AudioSource sound;
    bool isPressed;
    public WindActivation windActivation; // Reference to the WindActivation script
    void Start()
    {
      sound = GetComponent<AudioSource>();
      isPressed = false;

      if(windActivation != null && IsOwner)
      {
        windActivation.windActive.OnValueChanged += UpdateWindLog;
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

    public void ToggleWind(WindZone other)
    {
        if (other.CompareTag("PlayerHand") || other.CompareTag("PlayerController"))
        {
            // Trigger the wind activation
            windActivation.ToggleWind();
            UpdateWindLog(windActivation.windActive.Value, windActivation.windActive.Value);
        }  
    }

    // Update the wind log text when wind activation changes
    private void UpdateWindLog(bool oldVal, bool newVal)
    {
        if (newVal)
        {
            windLogText.text = "Wind Force: Active";
        }
        else
        {
            windLogText.text = "Wind Force: Inactive";
        }
    }

    // Clean up event subscriptions
    private void OnDestroy()
    {
        if (windActivation != null && IsOwner)
        {
            windActivation.windActive.OnValueChanged -= UpdateWindLog;
        }
    }
}
