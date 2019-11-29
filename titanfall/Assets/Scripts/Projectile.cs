using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    public Transform TargetObject;
    public float LaunchAngle = 45f;
    private bool touching;

    private Rigidbody rigid;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            Launch();
        }
        if (!touching)
        {
            // update the rotation of the projectile during trajectory motion
            transform.rotation = Quaternion.LookRotation(rigid.velocity);
        }
    }

    // launches the object towards the TargetObject with a given LaunchAngle
    void Launch()
    { 
        touching = false;
        Vector3 projectileXZPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Vector3 targetXZPos = new Vector3(TargetObject.position.x, transform.position.y, TargetObject.position.z);
        
        // rotate the object to face the target
        transform.LookAt(targetXZPos);

        // shorthands for the formula
        float R = Vector3.Distance(projectileXZPos, targetXZPos);
        float G = Physics.gravity.y;
        float tanAlpha = Mathf.Tan(LaunchAngle * Mathf.Deg2Rad);
        float H = TargetObject.position.y - transform.position.y;

        //calculate the local space components of the velocity required to land the projectile on the target object 
        float Vz = Mathf.Sqrt(G * R * R / (2.0f * (H - R * tanAlpha)) );
        float Vy = tanAlpha * Vz;

        //create the velocity vector in local space and get it in global space
        Vector3 localVelocity = new Vector3(0f, Vy, Vz);
        Vector3 globalVelocity = transform.TransformDirection(localVelocity);

        //launch the object by setting its initial velocity and flipping its state
        rigid.velocity = globalVelocity;
        
    }

    void OnCollisionEnter()
    {
        touching = true;
    }

    void OnCollisionExit()
    {
        touching = false;
    }

}