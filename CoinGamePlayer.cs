using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using UnityEngine.UI;

public class CoinGamePlayer : NetworkBehaviour
{
    public Rigidbody2D rb;

    public float moveSpeed = 5;
    public float dashDuration, invulnTime;
    public Vector2 movementVec;
    public bool isDashing = false;

    public int currDashCount, maxDashCount;

    public SpriteRenderer spr;
    public Color startColor, dashColor;

    Vector3 spawnPoint;

    //some epic C# Events and Delegates
    public delegate void CoinDestroyDelegate();
    public static event CoinDestroyDelegate CoinDestroyEvent;

    public delegate void PlayerStartDashDelegate();
    public static event PlayerStartDashDelegate playerStartDashEvent;
    public delegate void PlayerEndDashDelegate();
    public static event PlayerEndDashDelegate playerEndDashEvent;
    public static void DestroyEvent()
    {
        CoinDestroyEvent?.Invoke();
    }
    public static void EndDashEvent()
    {
        playerEndDashEvent?.Invoke();
    }
    public static void StartDashEvent()
    {
        playerStartDashEvent?.Invoke();
    }

    private void Start()
    {
        spawnPoint = transform.position;
    }
    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 move = new Vector2(input.x * moveSpeed, input.y * moveSpeed) * Time.deltaTime;
        movementVec = move.normalized;

        if (Input.GetKeyDown(KeyCode.LeftShift) && currDashCount >= maxDashCount)
        {
            isDashing = true;
            StartCoroutine("Dash");
        }
    }
    private void FixedUpdate()
    {
        if (!isDashing)
        {
            rb.velocity = movementVec * moveSpeed;
        }
        if (!isDashing && currDashCount < maxDashCount)
        {
            currDashCount += 1;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Coin")
        {
            NetworkServer.Destroy(collision.gameObject);
            DestroyEvent();
        }
        if (collision.gameObject.tag == "Obstacle")
        {
            transform.position = spawnPoint;
        }
    }
    IEnumerator Dash()
    {
        while (isDashing)
        {
            rb.AddForce(rb.velocity * moveSpeed * 700 * Time.deltaTime);
            StartDashEvent();
            CmdChangeColor();
            yield return new WaitForSeconds(dashDuration);
            currDashCount = 0;
            isDashing = false;
            yield return new WaitForSeconds(invulnTime);
            CmdChangeColor();
            EndDashEvent();
        }
    }
    #region color change
    [Command] //tell the server to notify all clients of this client color change
    void CmdChangeColor()
    {
        RpcChangeColor();// tell clients to update value
    }
    [ClientRpc] // tell client to update their own copy of the players color
    void RpcChangeColor()
    {
        ChangeColor();
    }
    //to change our color locally 
    void ChangeColor()
    {
        // update my player color value
        if (spr.color == startColor)
        {
            spr.color = dashColor;
        }
        else
        {
            spr.color = startColor;
        }
        
    }
    #endregion

}
