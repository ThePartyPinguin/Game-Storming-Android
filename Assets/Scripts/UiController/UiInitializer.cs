using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiInitializer : MonoBehaviour
{
    public ConnectionMenuUiController ConnectionMenuUiController;
    public RegisterNameMenuUiController RegisterNameMenuUiController;
    public SendIdeaMenuUiController SendIdeaMenuUiController;

    void Start()
    {
        ConnectionMenuUiController.gameObject.SetActive(true);
        RegisterNameMenuUiController.gameObject.SetActive(false);
        SendIdeaMenuUiController.gameObject.SetActive(false);
    }
}
