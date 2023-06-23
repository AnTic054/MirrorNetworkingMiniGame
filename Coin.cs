using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Coin : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (CoinGameNetworkManager.playerCount == 1)
        {
            NetworkServer.Destroy(this.gameObject);
            Destroy(this.gameObject);
        }
    }
}
