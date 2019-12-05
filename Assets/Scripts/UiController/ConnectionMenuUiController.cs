using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using GameFrame.Networking.Exception;
using GameFrame.Networking.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionMenuUiController : MonoBehaviour
{
    public TMP_InputField IpInputField;
    public TMP_Text ConnectMessageLabel;

    public Button ConnectButton;
    public Button DisconnectButton;

    public UnityNetworkManager NetworkManager;

    private ClientConnectionSettings<NetworkEvent> _connectionSettings;

    void Start()
    {
        ConnectMessageLabel.text = "Enter server ip to connect";
        NetworkManager.OnConnectedAction += OnConnected;

        CheckInteractableState();

        ConnectButton.onClick.AddListener(CheckInteractableState);
        DisconnectButton.onClick.AddListener(CheckInteractableState);


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

        NetworkManager.SetSettings(_connectionSettings);

        NetworkManager.Connect();
    }

    public void Disconnect()
    {
        NetworkManager.Disconnect();
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
        Debug.Log("Connected");
        ConnectMessageLabel.text = playerId.ToString();
        DisconnectButton.interactable = true;
    }

    public void OnConnectFailed(Guid playerId)
    {
        ConnectButton.interactable = true;
        ConnectMessageLabel.text = "Connection failed, please try again";
    }

    private void CheckInteractableState()
    {
        ConnectButton.interactable = !NetworkManager.IsConnected();
        DisconnectButton.interactable = NetworkManager.IsConnected();
    }
}
