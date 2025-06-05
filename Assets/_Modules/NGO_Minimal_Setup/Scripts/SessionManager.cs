using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Multiplayer;
using Unity.Services.Core;

public class SessionManager : Singleton<SessionManager>
{
    const string PLAYER_NAME_PROPERTY_KEY = "PlayerName";

    public ISession ActiveSession { get; private set; }

    async void Start()
    {
        try 
        {
            await UnityServices.InitializeAsync();
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

            print( $"[Auth] signed in successfully: {AuthenticationService.Instance.PlayerId}" );
        } 
        catch ( Exception e ) 
        {
            Debug.LogException( e );
        }
    }

    async UniTask<Dictionary<string, PlayerProperty>> GetPlayerProperties()
    {
        var pname = await AuthenticationService.Instance.GetPlayerNameAsync();
        var pnameProp = new PlayerProperty( pname, VisibilityPropertyOptions.Member );

        return new() 
        {
            { PLAYER_NAME_PROPERTY_KEY, pnameProp }
        };
    }

    public async void StartSessionAsHost()
    {
        var playerProps = await GetPlayerProperties();

        var opts = new SessionOptions
        {
            Name        = Guid.NewGuid().ToString(),
            MaxPlayers  = 2,
            IsLocked    = false, // @Security please password please
            IsPrivate   = false,
            PlayerProperties = playerProps,
        }.WithRelayNetwork();

        ActiveSession = await MultiplayerService.Instance.CreateSessionAsync( opts );
        print( $" [Session] created {ActiveSession.Id}. join code: {ActiveSession.Code} " );
    }

    public async UniTask JoinSessionByCode( string joinCode ) 
    {
        ActiveSession = await MultiplayerService.Instance.JoinSessionByCodeAsync( joinCode );
        print( $" [Session] joined {ActiveSession.Id} as {ActiveSession.CurrentPlayer.Id} " );
    }
}
