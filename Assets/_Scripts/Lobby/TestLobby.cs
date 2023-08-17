using System;
using System.Collections;
using System.Collections.Generic;
using EmreBeratKR.LazyCoroutines;
using QFSW.QC;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class TestLobby : MonoBehaviour
{
    private const int MAX_LİSTED_LOBBY_COUNT = 25;
    
    private Lobby m_HostLobby;
    
    
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
            
             m_HostLobby = lobby;

            Debug.Log("Lobby created with name of " + lobbyName + " with " + maxPlayers + " max players");
            
            SendHearthBeat();
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
            var queryResponse = await Lobbies.Instance.QueryLobbiesAsync(LobbyOptions());

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

    private QueryLobbiesOptions LobbyOptions()
    {
        try
        {
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions
            {
                Count = MAX_LİSTED_LOBBY_COUNT,
                Filters = GetQueryFiltersFilters(),
                Order = GetQueryOrders()
            };

            return queryLobbiesOptions;
        }
        catch (LobbyServiceException exception)
        {
            Debug.Log(exception);
            
            return null;
        }
    }

    private List<QueryFilter> GetQueryFiltersFilters()
    {
        return new List<QueryFilter>
        {
            new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT)
        };
    }

    private List<QueryOrder> GetQueryOrders()
    {
        return new List<QueryOrder>
        {
            new QueryOrder(false, QueryOrder.FieldOptions.Created)
        };
    }

    [Command]
    private async void JoinLobby(string lobbyCode)
    {
        try
        {
            await Lobbies.Instance.JoinLobbyByCodeAsync(lobbyCode);

            Debug.Log("Joined Lobby with code " + lobbyCode);
        }
        catch (LobbyServiceException exception)
        {
            Debug.Log(exception);
        }
    }
    
    [Command]
    private async void QuickJoinLobby()
    {
        try
        {
            await Lobbies.Instance.QuickJoinLobbyAsync();

            Debug.Log("Joined Lobby with quick join ");
        }
        catch (LobbyServiceException exception)
        {
            Debug.Log(exception);
        }
    }
    
    private void SendHearthBeat()
    {
        LazyCoroutines.DoEverySeconds(14, () =>
        {
            LobbyService.Instance.SendHeartbeatPingAsync(m_HostLobby.Id);

            Debug.Log(m_HostLobby.Name + " is still beating <3");
        });
    }
}
