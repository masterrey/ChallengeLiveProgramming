using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public CharacterController characterController;

    public string controlH,controlV,jump;

    public FixedJoystick fixedJoystick;

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
       

        if (Input.GetButtonDown(jump))
        {
            grav = jumpforce;
        }

        if (fixedJoystick!=null && (fixedJoystick.Horizontal != 0 || fixedJoystick.Vertical != 0))
        {
            Vector3 direction = Vector3.forward * fixedJoystick.Vertical + Vector3.right * fixedJoystick.Horizontal;
            direction = Camera.main.transform.TransformDirection(direction);
            direction = new Vector3(direction.x, grav, direction.z);
            characterController.Move(direction * acell * Time.deltaTime);
        }
        else
        {
            Vector3 mov = new Vector3(Input.GetAxis(controlH), 0, Input.GetAxis(controlV));
            mov = Camera.main.transform.TransformDirection(mov);
            mov = new Vector3(mov.x, grav, mov.z);
            characterController.Move(mov * acell * Time.deltaTime);
        }

       if(grav > -8) 
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
                rdb.AddForce((Vector3.up*2+transform.forward)* ballpush+characterController.velocity*0.2f, ForceMode.Impulse);   
            }
        }
    }
}
