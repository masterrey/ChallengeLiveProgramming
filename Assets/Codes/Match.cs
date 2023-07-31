using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Match : MonoBehaviour
{
    [SerializeField] float scorep1, scorep2;

    [SerializeField] GameObject Ball;

    public GameObject ballingame;

    [SerializeField] TextMesh textp1, textp2;

    public static Match instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
       
    }

    public void StartMatch()
    {
        Time.timeScale = 1f;
        if (ballingame!=null)
        {
          StartCoroutine(Reset());
           return;
        }   
        Vector3 randposition=new Vector3 (Random.value*8,10, Random.value * 6);
        ballingame= Instantiate(Ball,randposition,Quaternion.identity);
    }
    void EndMatch()
    {
        Time.timeScale = 0.0001f;
    }

    private IEnumerator Reset()
    {
        scorep1 = 0;
        scorep2 = 0;
        textp1.text = scorep1.ToString();
        textp2.text = scorep2.ToString();
        Destroy(ballingame);
        yield return new WaitForSecondsRealtime(2);
        StartMatch();
    }
    private IEnumerator ResetBall()
    {
        ballingame.GetComponent<Rigidbody>().velocity = Vector3.zero;
        ballingame.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;    
        ballingame.GetComponent<Rigidbody>().isKinematic = true;
        Vector3 randposition = new Vector3(Random.value * 8, 10, Random.value * 6);
        yield return new WaitForSecondsRealtime(2);
        
        ballingame.transform.position = randposition;
        yield return new WaitForSecondsRealtime(2);

        ballingame.GetComponent<Rigidbody>().isKinematic = false;



    }

    public void Scorep1()
    {
        scorep1++;
        textp1.text=scorep1.ToString();
        StartCoroutine(ResetBall());
        if (scorep1 >= 7)
        {
            EndMatch();
        }
    }
    public void Scorep2()
    {
        scorep2++;
        textp2.text = scorep2.ToString();
        StartCoroutine(ResetBall());
        if (scorep2 >= 7 )
        {
            EndMatch();
        }

    }

    public static implicit operator Match(MatchOnline v)
    {
        throw new System.NotImplementedException();
    }
}
