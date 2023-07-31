using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorePointOnline : MonoBehaviour
{
    public MatchOnline match;
    public string point;
    public int playercheck;
    private bool hasCollided;

    private void OnCollisionEnter(Collision collision)
    {
        if(PhotonNetwork.LocalPlayer.ActorNumber== playercheck)
        {
            if (!hasCollided && collision.collider.gameObject.CompareTag("Ball"))
            {
                match.Invoke(point,0);
                hasCollided = true;
                Invoke("Reset", 2f);
            }
        }
       

    }

    private void Reset()
    {
        hasCollided = false;
    }
}
