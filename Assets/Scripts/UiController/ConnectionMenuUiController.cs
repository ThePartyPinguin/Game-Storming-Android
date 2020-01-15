using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using GameFrame.Networking.Client;
using GameFrame.Networking.Exception;
using GameFrame.Networking.Serialization;
using GameFrame.UnityHelpers.Networking;
using GameFrame.UnityHelpers.Networking.Client;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionMenuUiController : MonoBehaviour
{

    public TMP_InputField IpInputField;
    public TMP_Text ConnectMessageLabel;

    [Space]
    public Button ConnectButton;
    public Button DisconnectButton;

    [Space]
    public RegisterNameMenuUiController RegisterNameMenuUiController;

    private UnityNetworkManager _networkManager;

    private ClientConnectionSettings<NetworkEvent> _connectionSettings;


    void Start()
    {
        _networkManager = UnityNetworkManager.Instance;

        RegisterNameMenuUiController.gameObject.SetActive(false);

        if (PlayerPrefs.HasKey("LastConnectedServer"))
        {
            IpInputField.text = PlayerPrefs.GetString("LastConnectedServer");
        }
        ConnectMessageLabel.text = "Enter server ip to connect";

        _networkManager.OnConnected.AddListener(OnConnected);

        ConnectButton.interactable = true;
        DisconnectButton.interactable = false;
    }
    public void Connect()
    {
        ConnectButton.interactable = false;
        _connectionSettings = new ClientConnectionSettings<NetworkEvent>();

        _connectionSettings.ClientToServerHandshakeEvent = NetworkEvent.CLIENT_TO_SERVER_HANDSHAKE;
        _connectionSettings.ServerToClientHandshakeEvent = NetworkEvent.SERVER_TO_CLIENT_HANDSHAKE;
        _connectionSettings.ClientDisconnectEvent = NetworkEvent.CLIENT_DISCONNECT;
        _connectionSettings.ServerDisconnectEvent = NetworkEvent.SERVER_DISCONNECT;
        _connectionSettings.TcpPort = 5555;
        _connectionSettings.SerializationType = SerializationType.JSON;
        _connectionSettings.UseUdp = false;

        _connectionSettings.ServerIpAddress = ParseIpAddress(IpInputField.text);

        PlayerPrefs.SetString("LastConnectedServer", IpInputField.text);
        PlayerPrefs.Save();

        _networkManager.SetSettings(_connectionSettings);
        _networkManager.Connect();
        StartCoroutine(CheckConnectionFailed());
    }

    private IEnumerator CheckConnectionFailed()
    {
        yield return new WaitForSeconds(10f);

        if (_networkManager.GameClient != null && !_networkManager.GameClient.IsConnected)
        {
            ConnectButton.interactable = true;
        }
    }

    public void Disconnect()
    {
        _networkManager.Disconnect();
    }

    private IPAddress ParseIpAddress(string ipAddress)
    {
        if (ipAddress.Equals("localhost", StringComparison.OrdinalIgnoreCase))
            ipAddress = "127.0.0.1";

        if (IPAddress.TryParse(ipAddress, out var address))
        {
            return address;
        }
        throw new InvalidIPAdressException("The given ipAddress: " + ipAddress + " is not valid");
    }

    public void OnConnected(Guid playerId)
    {
        _networkManager.GameClient.AddAllowedEvent(NetworkEvent.SERVER_REQUEST_TOPIC);
        _networkManager.GameClient.AddAllowedEvent(NetworkEvent.SERVER_REQUEST_NAME);

        Debug.Log("Connected");
        ConnectMessageLabel.text = playerId.ToString();
        DisconnectButton.interactable = true;
        ConnectButton.interactable = false;


        try
        {
            StopCoroutine(CheckConnectionFailed());
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

        //this.gameObject.SetActive(false);
        //RegisterNameMenuUiController.gameObject.SetActive(true);
    }

    public void OnConnectFailed(Guid playerId)
    {
        ConnectButton.interactable = true;
        DisconnectButton.interactable = false;
        ConnectMessageLabel.text = "Connection failed, please try again";

        this.gameObject.SetActive(true);
        RegisterNameMenuUiController.gameObject.SetActive(false);
    }
}
