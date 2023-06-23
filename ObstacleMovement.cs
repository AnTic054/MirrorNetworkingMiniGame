using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ObstacleMovement : NetworkBehaviour
{
    public GameObject[] movePoints;
    public float speed = 1;
    int targetPoint;
    public Collider2D coll;


    private void OnEnable()
    {
        coll = GetComponent<Collider2D>();
        CoinGamePlayer.playerStartDashEvent += DisableCollider;
        CoinGamePlayer.playerEndDashEvent += EnableCollider;
    }
    private void OnDisable()
    {
        CoinGamePlayer.playerStartDashEvent -= DisableCollider;
        CoinGamePlayer.playerEndDashEvent -= EnableCollider;
    }
    // Start is called before the first frame update
    void Start()
    {
        //currPoint = startPoint;
        for (int i = 0; i < movePoints.Length; i++)
        {
            if (transform.position == movePoints[i].transform.position)
            {
                targetPoint = i;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position == movePoints[targetPoint].transform.position)
        {
            SetTargetInt();
        }
        transform.position = Vector3.MoveTowards(transform.position, movePoints[targetPoint].transform.position, speed * Time.deltaTime);
    }

    void SetTargetInt()
    {
            targetPoint++;
        if (targetPoint >= movePoints.Length)
        {
            targetPoint = 0;
        }
        
    }
    void EnableCollider()
    {
        coll.enabled = true;
    }
    void DisableCollider()
    {
        coll.enabled = false;
    }
}
