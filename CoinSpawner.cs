using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CoinSpawner : NetworkBehaviour
{
    public GameObject coin;
    [SyncVar]
    public int coinCounter = 0;
    public int coinLimit = 10;
    public float coinSpawnDelay;
    public Vector2 spawnBounds;

    public delegate void CoinCounterDelegate();
    public static event CoinCounterDelegate CoinCounterEvent;

    private void OnEnable()
    {
        CoinGamePlayer.CoinDestroyEvent += SubCoinCounter;
        StartCoroutine("Spawn");
    }
    private void OnDisable()
    {
        CoinGamePlayer.CoinDestroyEvent -= SubCoinCounter;
    }
    private void Update()
    {
        if (CoinGameNetworkManager.playerCount <= 1)
        {
            coinCounter = 0;
        }
    }
    void SpawnCoin()
    {
        GameObject coinClone = Instantiate(coin, RandomSpawn(spawnBounds),Quaternion.identity);
        NetworkServer.Spawn(coinClone);
        coinCounter += 1;
        
    }
    public void SubCoinCounter()
    {
        coinCounter -= 1;
    }
    Vector3 RandomSpawn(Vector2 bounds)
    {
        float randX;
        float randY;
        randX = Random.Range(-bounds.x, bounds.x);
        randY = Random.Range(-bounds.y, bounds.y);
        return new Vector3(randX,randY,0);
    }
    IEnumerator Spawn()
    {
        while (NetworkServer.active)
        {
            yield return new WaitForSeconds(coinSpawnDelay);
            if (coinCounter < coinLimit && CoinGameNetworkManager.playerCount >= 2)
            {
                SpawnCoin();
            }
        }
    }
}
