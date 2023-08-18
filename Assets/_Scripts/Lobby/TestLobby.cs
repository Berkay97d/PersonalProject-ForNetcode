using System;
using System.Collections.Generic;
using EmreBeratKR.LazyCoroutines;
using QFSW.QC;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using Random = UnityEngine.Random;


public class TestLobby : MonoBehaviour
{
    private const int MAX_LİSTED_LOBBY_COUNT = 25;
    
    private Lobby m_HostLobby;
    private string m_PlayerName;


    private void Awake()
    {
        m_PlayerName = "Berkay" + Random.Range(0, 100);
    }

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
        Debug.Log(m_PlayerName + "Signed in " + AuthenticationService.Instance.PlayerId);
    }
    
    [Command]
    private async void CreateLobby(string lobbyName, bool isPrivate)
    {
        try
        {
            var maxPlayers = 4;
            
            var lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, GetCreateLobbyOptions(isPrivate));
            
            m_HostLobby = lobby;
             
            PrintPlayers(m_HostLobby);
             
            Debug.Log(" Lobby created with name of " + lobbyName + " with " + maxPlayers + " max players and lobby code: " + lobby.LobbyCode);
            
            SendHearthBeat();
        }
        catch (LobbyServiceException exception)
        {
            Debug.Log(exception);
        }
    }

    private CreateLobbyOptions GetCreateLobbyOptions(bool isPrivate)
    {
        try
        {
            CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions
            {
               IsPrivate = isPrivate,
               Player = GetPlayer(),
            };

            return createLobbyOptions;
        }
        catch (LobbyServiceException exception)
        {
            Debug.Log(exception);
            return null;
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
    private async void JoinLobbyByCode(string lobbyCode)
    {
        try
        {
            JoinLobbyByCodeOptions joinLobbyByCodeOptions = new JoinLobbyByCodeOptions
            {
                Player = GetPlayer(),
            };
            
            var joinedLobby = await Lobbies.Instance.JoinLobbyByCodeAsync(lobbyCode, joinLobbyByCodeOptions);

            Debug.Log("Joined Lobby with code " + lobbyCode);
            
            PrintPlayers(joinedLobby);
        }
        catch (LobbyServiceException exception)
        {
            Debug.Log(exception);
        }
    }
    
    [Command]
    private async void JoinLobbyQuick()
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

    private Player GetPlayer()
    {
        return new Player
        {
            Data = new Dictionary<string, PlayerDataObject>
            {
                {"PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, m_PlayerName)},
            }
        };
    }

    private void SendHearthBeat()
    {
        LazyCoroutines.DoEverySeconds(14, () =>
        {
            LobbyService.Instance.SendHeartbeatPingAsync(m_HostLobby.Id);

            Debug.Log(m_HostLobby.Name + " is still beating <3");
        });
    }

    private void PrintPlayers(Lobby lobby)
    {
        Debug.Log("PLayers in lobby " + lobby.Name);

        foreach (var player in lobby.Players)
        {
            if (player.Data.TryGetValue("PlayerName", out PlayerDataObject dataObject))
            {
                Debug.Log(dataObject.Value);    
            }
        }
    }
    
}
