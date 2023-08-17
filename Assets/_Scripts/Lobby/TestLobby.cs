using System;
using System.Collections;
using System.Collections.Generic;
using QFSW.QC;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class TestLobby : MonoBehaviour
{
    private async void Start()
    {
        await UnityServices.InitializeAsync();
        
        AuthenticationService.Instance.SignedIn += OnSignedIn;

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    private void OnDestroy()
    {
        AuthenticationService.Instance.SignedIn -= OnSignedIn;
    }

    private void OnSignedIn()
    {
        Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
    }
    
    [Command]
    private async void CreateLobby()
    {
        try
        {
            var lobbyName = "Test Lobby";
            var maxPlayers = 4;
        
            var lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers);

            Debug.Log("Lobby created with name of " + lobbyName + " with " + maxPlayers + " max players");
        }
        catch (LobbyServiceException exception)
        {
            Debug.Log(exception);
        }
    }

    [Command]
    private async void ListLobbies()
    {
        try
        {
            var queryResponse = await Lobbies.Instance.QueryLobbiesAsync();

            Debug.Log("Lobbies found: " + queryResponse.Results.Count);

            foreach (var lobby in queryResponse.Results)
            {
                Debug.Log(lobby.Name + " - " + lobby.MaxPlayers);
            }
        }
        catch (LobbyServiceException exception)
        {
            Debug.Log(exception);
        }
    }
}
