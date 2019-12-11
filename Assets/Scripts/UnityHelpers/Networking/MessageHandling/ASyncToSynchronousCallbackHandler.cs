﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ASyncToSynchronousCallbackHandler : MonoSingleton<ASyncToSynchronousCallbackHandler>
{
    private bool _coRoutineRunning;
    private Queue<Action> _callbacksToHandle;

    void Start()
    {
        _callbacksToHandle = new Queue<Action>();
    }
    public void QueueCallbackToHandle(Action callback)
    {
        if(_callbacksToHandle == null)
            _callbacksToHandle = new Queue<Action>();

        Debug.Log("New callback queued");
        _callbacksToHandle.Enqueue(callback);
    }

    void Update()
    {
        if (!_coRoutineRunning && _callbacksToHandle != null && _callbacksToHandle.Count > 0)
        {
            Debug.Log("Starting coRoutine");
            _coRoutineRunning = true;
            StartCoroutine(HandleCoRoutine());
        }
    }

    private IEnumerator HandleCoRoutine()
    {
        while (_callbacksToHandle.Count > 0)
        {
            if (_callbacksToHandle.Count > 15)
            {
                HandleCallbacks(15);
                yield return new WaitForEndOfFrame();
            }
            else
            {
                HandleCallbacks(_callbacksToHandle.Count);
                yield return new WaitForEndOfFrame();
            }
        }

        _coRoutineRunning = false;
    }

    private void HandleCallbacks(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            var callback = _callbacksToHandle.Dequeue();

            callback.Invoke();
        }

    }
}
