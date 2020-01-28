using System;
using System.Collections;
using Boo.Lang;
using GameFrame.UnityHelpers.Networking;
using GameFrame.UnityHelpers.Networking.Client;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SendIdeaMenuUiController : MonoBehaviour
{
    public TMP_InputField IdeaInputField;
    public TMP_Text AlreadySendIdeaWarningLabel;
    public Button SubmitButton;
    public SwipeController SwipeController;
    public float AnimationSpeed;

    private TMP_Text _buttonText;
    private UnityNetworkManager _networkManager;
    private Animator _animator;

    private List<string> _sendIdeas;

    void Start()
    {
        _networkManager = UnityNetworkManager.Instance;
        _buttonText = SubmitButton.GetComponentInChildren<TMP_Text>();
        _animator = GetComponent<Animator>();
        _sendIdeas = new List<string>();
        SwipeController.OnSwiped += OnSwiped;
        AlreadySendIdeaWarningLabel.text = string.Empty;
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

        if (_sendIdeas.Contains(s => s.Equals(IdeaInputField.text, StringComparison.InvariantCultureIgnoreCase)))
        {
            AlreadySendIdeaWarningLabel.text = "You already send this idea, please send a new idea";
            return;
        }
        else
        {
            AlreadySendIdeaWarningLabel.text = string.Empty;
            _sendIdeas.Add(IdeaInputField.text);
        }

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
