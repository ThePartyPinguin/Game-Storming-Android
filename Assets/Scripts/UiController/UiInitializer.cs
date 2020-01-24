using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiInitializer : MonoBehaviour
{
    public ConnectionMenuUiController ConnectionMenuUiController;
    public RegisterNameMenuUiController RegisterNameMenuUiController;
    public SendIdeaMenuUiController SendIdeaMenuUiController;
    public SendTopicMenuUiController SendTopicMenuUiController;
    public GameObject WaitingOnTopicScreen;
    public GameObject WaitingOnGameStartScreen;
    public GameObject YourAreBuilderScreen;
    public GameObject EndGameScreen;

    void Start()
    {
        OpenConnectionMenu();
    }

    public void OpenConnectionMenu()
    {
        ConnectionMenuUiController.gameObject.SetActive(true);
        RegisterNameMenuUiController.gameObject.SetActive(false);
        SendIdeaMenuUiController.gameObject.SetActive(false);
        SendTopicMenuUiController.gameObject.SetActive(false);
        WaitingOnTopicScreen.SetActive(false);
        WaitingOnGameStartScreen.SetActive(false);
        YourAreBuilderScreen.SetActive(false);
        EndGameScreen.SetActive(false);
    }

    public void OpenTopicMenu()
    {
        ConnectionMenuUiController.gameObject.SetActive(false);
        RegisterNameMenuUiController.gameObject.SetActive(false);
        SendIdeaMenuUiController.gameObject.SetActive(false);
        SendTopicMenuUiController.gameObject.SetActive(true);
        WaitingOnTopicScreen.SetActive(false);
        WaitingOnGameStartScreen.SetActive(false);
        YourAreBuilderScreen.SetActive(false);
        EndGameScreen.SetActive(false);
    }

    public void OpenWaitingOnTopicScreen()
    {
        ConnectionMenuUiController.gameObject.SetActive(false);
        RegisterNameMenuUiController.gameObject.SetActive(false);
        SendIdeaMenuUiController.gameObject.SetActive(false);
        SendTopicMenuUiController.gameObject.SetActive(false);
        WaitingOnTopicScreen.SetActive(true);
        WaitingOnGameStartScreen.SetActive(false);
        YourAreBuilderScreen.SetActive(false);
        EndGameScreen.SetActive(false);
    }

    public void OpenNameMenu()
    {
        ConnectionMenuUiController.gameObject.SetActive(false);
        RegisterNameMenuUiController.gameObject.SetActive(true);
        SendIdeaMenuUiController.gameObject.SetActive(false);
        SendTopicMenuUiController.gameObject.SetActive(false);
        WaitingOnTopicScreen.SetActive(false);
        WaitingOnGameStartScreen.SetActive(false);
        YourAreBuilderScreen.SetActive(false);
        EndGameScreen.SetActive(false);
    }

    public void OpenIdeaMenu()
    {
        ConnectionMenuUiController.gameObject.SetActive(false);
        RegisterNameMenuUiController.gameObject.SetActive(false);
        SendIdeaMenuUiController.gameObject.SetActive(true);
        SendTopicMenuUiController.gameObject.SetActive(false);
        WaitingOnTopicScreen.SetActive(false);
        WaitingOnGameStartScreen.SetActive(false);
        YourAreBuilderScreen.SetActive(false);
        EndGameScreen.SetActive(false);
    }

    public void OpenWaitingOnGameStartScreen()
    {
        ConnectionMenuUiController.gameObject.SetActive(false);
        RegisterNameMenuUiController.gameObject.SetActive(false);
        SendIdeaMenuUiController.gameObject.SetActive(false);
        SendTopicMenuUiController.gameObject.SetActive(false);
        WaitingOnTopicScreen.SetActive(false);
        WaitingOnGameStartScreen.SetActive(true);
        YourAreBuilderScreen.SetActive(false);
        EndGameScreen.SetActive(false);
    }

    public void OpenYourBuilderMenu()
    {
        ConnectionMenuUiController.gameObject.SetActive(false);
        RegisterNameMenuUiController.gameObject.SetActive(false);
        SendIdeaMenuUiController.gameObject.SetActive(false);
        SendTopicMenuUiController.gameObject.SetActive(false);
        WaitingOnTopicScreen.SetActive(false);
        WaitingOnGameStartScreen.SetActive(false);
        YourAreBuilderScreen.SetActive(true);
        EndGameScreen.SetActive(false);
    }

    public void OpenEndGameScreen()
    {
        ConnectionMenuUiController.gameObject.SetActive(false);
        RegisterNameMenuUiController.gameObject.SetActive(false);
        SendIdeaMenuUiController.gameObject.SetActive(false);
        SendTopicMenuUiController.gameObject.SetActive(false);
        WaitingOnTopicScreen.SetActive(false);
        WaitingOnGameStartScreen.SetActive(false);
        YourAreBuilderScreen.SetActive(false);
        EndGameScreen.SetActive(true);
    }
}
