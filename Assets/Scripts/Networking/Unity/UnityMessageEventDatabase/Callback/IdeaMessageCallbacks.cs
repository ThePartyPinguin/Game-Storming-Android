using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdeaMessageCallbacks : MonoBehaviour
{
    public void ClientSendIdea(IdeaNetworkMessage message, Guid clientId)
    {
        Debug.Log(clientId);
        Debug.Log(message.Idea);
    }
}
