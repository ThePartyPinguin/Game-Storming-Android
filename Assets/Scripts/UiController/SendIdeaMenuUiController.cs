using GameFrame.UnityHelpers.Networking;
using GameFrame.UnityHelpers.Networking.Client;
using TMPro;
using UnityEngine;

public class SendIdeaMenuUiController : MonoBehaviour
{
    public TMP_InputField IdeaInputField;

    private UnityNetworkManager _networkManager;

    void Start()
    {
        _networkManager = UnityNetworkManager.Instance;
    }

    public void SendIdea()
    {
        if(IdeaInputField == null || string.IsNullOrWhiteSpace(IdeaInputField.text))
            return;


        _networkManager.SendSecureMessage(new StringNetworkMessage(NetworkEvent.CLIENT_SEND_IDEA, IdeaInputField.text));
    }
}
