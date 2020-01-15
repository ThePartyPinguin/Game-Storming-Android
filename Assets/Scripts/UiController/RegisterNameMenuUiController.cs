using System;
using System.Collections;
using System.Collections.Generic;
using GameFrame.UnityHelpers.Networking;
using GameFrame.UnityHelpers.Networking.Client;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RegisterNameMenuUiController : MonoBehaviour
{
    public TMP_InputField NameInputField;
    public Button SubmitButton;
    public SendIdeaMenuUiController SendIdeaMenuUiController;

    private UnityNetworkManager _networkManager;

    // Start is called before the first frame update
    void Start()
    {
        _networkManager = UnityNetworkManager.Instance;
    }

    public void SendName()
    {
        if(string.IsNullOrWhiteSpace(NameInputField.text))
            return;
        SubmitButton.interactable = false;
        _networkManager.GameClient.AddAllowedEvent(NetworkEvent.SERVER_REGISTERED_CLIENT);
        _networkManager.GameClient.AddAllowedEvent(NetworkEvent.SERVER_START_GAME);

        _networkManager.SendSecureMessage(new StringNetworkMessage(NetworkEvent.CLIENT_SEND_NAME, NameInputField.text));
    }

    public void OnServerRegisteredClient(EventOnlyNetworkMessage message, Guid clientId)
    {
        _networkManager.GameClient.RemoveAllowedEvent(NetworkEvent.SERVER_REGISTERED_CLIENT);
        SendIdeaMenuUiController?.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
