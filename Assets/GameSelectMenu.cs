using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class GameSelectMenu : NetworkBehaviour
{
    [Header("Game Select Menu")]
    public GameObject SelectMenu;
    //public GameObject City;
    //public GameObject X;

    [Header("Game Select Buttons")]
    public Button TTButton;
    public Button CityButton;
    public Button XButton;
    public Button backButton;

    public List<Button> returnButtons;

    // Start is called before the first frame update
    void Start()
    {
        EnableGameMenu();

        //Hook events
        TTButton.onClick.AddListener(StartTTServerRpc);
        CityButton.onClick.AddListener(StartCityServerRpc);
        XButton.onClick.AddListener(StartXServerRpc);
        backButton.onClick.AddListener(BackToMainMenuServerRpc);

        foreach (var item in returnButtons)
        {
            item.onClick.AddListener(EnableGameMenu);
        }
    }

    [ServerRpc]
    public void BackToMainMenuServerRpc()
    {
        if(IsServer)
        {
            HideAll();
            SceneTransitionManager.singleton.GoToSceneAsync(0);
            DisableGameMenuClientRpc();
        }
        
    }

    [ServerRpc]
    public void StartTTServerRpc()
    {
        if(IsServer)
        {
            HideAll();
            SceneTransitionManager.singleton.GoToSceneAsync(2);
            DisableGameMenuClientRpc();
        }
    }

    [ServerRpc]
    public void StartCityServerRpc()
    {
        if(IsServer)
        {
            HideAll();
            SceneTransitionManager.singleton.GoToSceneAsync(3);
            DisableGameMenuClientRpc();
        }
    }
    
    [ServerRpc]
    public void StartXServerRpc()
    {
        if(IsServer)
        {
            HideAll();
            SceneTransitionManager.singleton.GoToSceneAsync(4);
            DisableGameMenuClientRpc();
        }
    }
    
    [ClientRpc]
    void DisableGameMenuClientRpc()
    {
        SelectMenu.SetActive(false);
    }
    public void HideAll()
    {
        SelectMenu.SetActive(false);
        //City.SetActive(false);
        //X.SetActive(false);
    }

    public void EnableGameMenu()
    {
        SelectMenu.SetActive(true);
        //City.SetActive(false);
        //X.SetActive(false);
    }
 
    
}

