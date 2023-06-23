using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using TMPro;

public class CoinGameNetworkManager : NetworkManager
{
    public Transform topLeftSpawn;
    public Transform bottomLeftSpawn;
    public Transform topRightSpawn;
    public Transform bottomRightSpawn;

    [HideInInspector]public Transform startPos;
    public static int playerCount;


    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        switch (numPlayers)
        {
            case 0:
                startPos = topLeftSpawn;
                playerCount = 1;
                break;
            case 1:
                startPos = bottomRightSpawn;
                playerCount = 2;
                break;
            case 2:
                startPos = topRightSpawn;
                playerCount = 3;
                break;
            case 3:
                startPos = bottomLeftSpawn;
                playerCount = 4;
                break;
        }
        GameObject player = Instantiate(playerPrefab, startPos.position, startPos.rotation);
        NetworkServer.AddPlayerForConnection(conn,player);

    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        playerCount -= 1;
        base.OnServerDisconnect(conn);
    }
    
}
