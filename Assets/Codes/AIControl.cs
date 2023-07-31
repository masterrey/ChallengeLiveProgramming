using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class AIControl : MonoBehaviour
{
    public NavMeshAgent characterController;

   

    public float acell=0.1f;
    public float ballpush = 10;
    float grav = -8f;
    public float jumpforce =5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    
    void Update()
    {
        Control();
    }

    private void Control()
    {


        characterController.SetDestination(GameObject.FindGameObjectWithTag("Ball").transform.position);



       if (grav > -8) 
        {
            grav -= Time.deltaTime*10;
        }

    }

    void GravityNormalize()
    {
        grav = -8f;

    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.CompareTag("Ball"))
        {
            Rigidbody rdb=coll.gameObject.gameObject.GetComponent<Rigidbody>();
            if (rdb != null) 
            {
                rdb.velocity = Vector3.zero;
                rdb.AddForce((Vector3.up*2-Vector3.right)* ballpush+characterController.velocity*0.2f, ForceMode.Impulse);   
            }
        }
    }
}
