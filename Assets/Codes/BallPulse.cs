using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPulse : MonoBehaviour
{
 
    public Rigidbody ballRigidbody;
    public float simulationTime = 5f;
    public float simulationStep = 0.1f;

    void Update()
    {
        if (ballRigidbody != null)
        {
            

            Vector3 landingSpot = PredictLandingSpot();
            transform.position = Vector3.Lerp(transform.position,landingSpot,Time.deltaTime);
            transform.localScale = Vector3.one*Mathf.Sin(Time.time*10f)*0.1f+Vector3.one*0.5f;
        }else
        {

            GameObject ball = GameObject.FindGameObjectWithTag("Ball");
            if(ball != null)
                ballRigidbody = GameObject.FindGameObjectWithTag("Ball").GetComponent<Rigidbody>();
            
        }
       
    }

    Vector3 PredictLandingSpot()
    {
        Vector3 currentPosition = ballRigidbody.transform.position;
        Vector3 currentVelocity = ballRigidbody.velocity;

        for (float t = 0; t < simulationTime; t += simulationStep)
        {
            
            currentPosition += currentVelocity * simulationStep;
            currentVelocity += Physics.gravity * simulationStep;

          
            if (currentPosition.y <= 0.1f)
            {
                break;
            }
        }
        currentPosition=new Vector3(currentPosition.x,transform.position.y,currentPosition.z);
        return currentPosition;
    }
}


