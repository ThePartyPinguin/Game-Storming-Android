using System;
using System.Collections;
using GameFrame.UnityHelpers.Networking;
using GameFrame.UnityHelpers.Networking.Client;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SendIdeaMenuUiController : MonoBehaviour
{
    public TMP_InputField IdeaInputField;
    public Button SubmitButton;
    public SwipeController SwipeController;
    public float AnimationSpeed;

    private TMP_Text _buttonText;
    private UnityNetworkManager _networkManager;
    private Animator _animator;

    void Start()
    {
        _networkManager = UnityNetworkManager.Instance;
        _buttonText = SubmitButton.GetComponentInChildren<TMP_Text>();
        _animator = GetComponent<Animator>();
        SwipeController.OnSwiped += OnSwiped;
    }

    private void OnSwiped()
    {
        Debug.Log("Swipe callback invoked");
        SendIdea();
    }

    public void SendIdea()
    {
        if(IdeaInputField == null || string.IsNullOrWhiteSpace(IdeaInputField.text))
            return;
        _animator.speed = AnimationSpeed;
        _animator.SetTrigger("SendTrigger");

        StartCoroutine(SendIdeaRoutine());
    }

    private IEnumerator SendIdeaRoutine()
    {
        if(string.IsNullOrWhiteSpace(IdeaInputField.text))
            yield break;

        SubmitButton.interactable = false;
        _buttonText.text = "Sending...";

        try
        {
            _networkManager.SendSecureMessage(new StringNetworkMessage(NetworkEvent.CLIENT_SEND_IDEA, IdeaInputField.text));
        }
        catch(Exception e)
        {
            Debug.LogError(e);
            SubmitButton.interactable = true;
            yield break;
        }

        IdeaInputField.text = string.Empty;
        yield return new WaitForSeconds(1.2f);

        SubmitButton.interactable = true;
        _buttonText.text = "Send";
    }
}
