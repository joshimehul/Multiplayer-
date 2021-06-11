using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;

    //need a refrence from where we need to cast a ray
    public Camera fpsCam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void shoot()
    {
        //this is the variable that is used to store some info  about what we hit with a ray  
        RaycastHit hit;
        //to shoot out a ray 
        if(Physics.Raycast(/*starting from a position of a camera*/fpsCam.transform.position,/* To shoot it in the direction of facing*/
           fpsCam.transform.forward,/*gather the information and put it in variable*/ out hit, range))
        {
            Debug.Log(hit.transform.name);
        }

    }
}
