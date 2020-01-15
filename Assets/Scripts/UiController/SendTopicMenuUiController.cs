using System;
using System.Collections;
using System.Collections.Generic;
using GameFrame.UnityHelpers.Networking;
using GameFrame.UnityHelpers.Networking.Client;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SendTopicMenuUiController : MonoBehaviour
{
    public TMP_InputField TopicInputField;
    public Button SubmitButton;
    public TMP_Text ButtonText;

    private UnityNetworkManager _networkManager;

    void Start()
    {
        _networkManager = UnityNetworkManager.Instance;
    }

    public void SendTopic()
    {
        StartCoroutine(SendTopicRoutine());
    }

    private IEnumerator SendTopicRoutine()
    {
        if (string.IsNullOrWhiteSpace(TopicInputField.text))
            yield break;

        SubmitButton.interactable = false;
        ButtonText.text = "Sending...";

        try
        {
            _networkManager.SendSecureMessage(new StringNetworkMessage(NetworkEvent.CLIENT_SEND_TOPIC, TopicInputField.text));
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            SubmitButton.interactable = true;
            yield break;
        }

        TopicInputField.text = string.Empty;
    }
}
