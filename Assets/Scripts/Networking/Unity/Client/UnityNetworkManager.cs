using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Assets.Scripts.Networking.Client;
using GameFrame.Networking.Exception;
using GameFrame.Networking.Messaging.Message;
using GameFrame.Networking.Messaging.MessageHandling;
using GameFrame.Networking.NetworkConnector;
using GameFrame.Networking.Serialization;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(UnityEventOnlyMessageEventDatabase))]
public class UnityNetworkManager : MonoSingleton<UnityNetworkManager>
{
    [SerializeField]
    private string _ipAddress;

    [SerializeField]
    private int _tcpPort;

    [SerializeField]
    private int _udpReceivePort;

    [SerializeField]
    private int _udpRemoteSendPort;

    [SerializeField]
    private SerializationType _serializationType;

    [SerializeField] 
    private bool _useUdp;

    [Header("Connection events")]
    public ClientIdCallback OnConnected;
    public ClientIdCallback OnConnectFailed;
    public ClientIdCallback OnConnectionInterrupted;
    
    private GameClient<NetworkEvent> _gameClient;

    private ClientConnectionSettings<NetworkEvent> _connectionSettings;

    public Action<Guid> OnConnectedAction;

    private UnityEventOnlyMessageEventDatabase _connectionEventDatabase;

    private ClientIdCallback _callbackToCallInCoRoutine;
    private bool _startCoRoutine;

    void Start()
    {
        ClientConnectionSettings<NetworkEvent> settings = new ClientConnectionSettings<NetworkEvent>();

        settings.ClientToServerHandshakeEvent = NetworkEvent.CLIENT_TO_SERVER_HANDSHAKE;
        settings.ServerToClientHandshakeEvent = NetworkEvent.SERVER_TO_CLIENT_HANDSHAKE;
        settings.ClientDisconnectEvent = NetworkEvent.CLIENT_DISCONNECT;
        settings.ServerDisconnectEvent = NetworkEvent.SERVER_DISCONNECT;

        settings.SerializationType = SerializationType.JSON;
        settings.ServerIpAddress = ParseIpAddress();
        settings.TcpPort = _tcpPort;

        if (_useUdp)
        {
            settings.UseUdp = _useUdp;

            settings.UdpRemoteSendPort = _udpRemoteSendPort;
            settings.UdpReceivePort = _udpReceivePort;

        }
    }

    void Update()
    {
        if (_startCoRoutine)
        {
            _startCoRoutine = false;
            StartCoroutine(ConnectionEventHandlerCoRoutine());
        }
    }

    public void SetSettings(ClientConnectionSettings<NetworkEvent> settings)
    {
        _connectionSettings = settings;
    }

    public void Connect()
    {
        if(_gameClient != null && _gameClient.IsConnected)
            return;

        StartCoroutine(ConnectCoRoutine());
    }

    public void Disconnect()
    {
        _gameClient?.SecureSendMessage(new ClientDisconnectMessage<NetworkEvent>(NetworkEvent.CLIENT_DISCONNECT));
    }

    public bool IsConnected()
    {
        if (_gameClient == null)
            return false;

        return _gameClient.IsConnected;
    }

    private IEnumerator ConnectCoRoutine()
    {
        yield return new WaitForSeconds(1f);

        Debug.Log("Setting up connection to server");

        SetupHandshakeEvent();

        if (_gameClient == null)
        {
            _gameClient = new GameClient<NetworkEvent>(_connectionSettings);

            _gameClient.OnConnectionSuccess = (guid) => StartCoRoutineCallback(OnConnected);
            _gameClient.OnConnectionFailed += () => StartCoRoutineCallback(OnConnectFailed);
            _gameClient.OnConnectionLost += () => StartCoRoutineCallback(OnConnectionInterrupted);
        }

        _gameClient.Connect();
    }

    private void SetupHandshakeEvent()
    {
        
    }

    private IPAddress ParseIpAddress()
    {
        string ipAddress = _ipAddress;
        if (_ipAddress.Equals("localhost", StringComparison.OrdinalIgnoreCase))
            ipAddress = "127.0.0.1";

        if (IPAddress.TryParse(ipAddress, out var address))
        {
            return address;
        }
        throw new InvalidIPAdressException("The given ipAddress: " + _ipAddress + " is not valid");
    }

    private void CallOnConnected(Guid clientId)
    {
        Debug.Log("Connected: " + clientId);

       OnConnected?.Invoke(clientId);
    }

    void OnApplicationQuit()
    {
        _gameClient.Stop();
        Debug.Log("quit");
    }
    
    [Serializable]
    public class ClientIdCallback : UnityEvent<Guid>
    {
        
    }

    private void StartCoRoutineCallback(ClientIdCallback callback)
    {
        _startCoRoutine = true;
        _callbackToCallInCoRoutine = callback;
    }

    private IEnumerator ConnectionEventHandlerCoRoutine()
    {
        _callbackToCallInCoRoutine.Invoke(_gameClient.ClientId);
        yield break;
    }

    public void SendSecureMessage(BaseNetworkMessage message)
    {
        _gameClient?.SecureSendMessage(message);
    }
}
