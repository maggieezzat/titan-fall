using UnityEngine;

public class Projectile : MonoBehaviour
{
    public bool trajectoryPath;
    public Transform TargetObject;
    public float LaunchAngle = 45f;
    public bool launched = false;
    private bool touching;

    private Rigidbody rigid;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        rigid.useGravity = false;
        rigid.isKinematic = true;
        rigid.detectCollisions = false;

        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
    }

    void Update()
    {   
        if (!touching && launched)
        {
            // update the rotation of the projectile during trajectory motion
            transform.rotation = Quaternion.LookRotation(rigid.velocity);
        }  
    }

    public void launch(Transform target)
    {
        TargetObject = target;

        rigid.isKinematic = false;
        rigid.detectCollisions = true;
        launched = true;

        if(trajectoryPath)
            grenadeLaunch(target);
        else
            rocketLaunch(target);
    }

    // launches the object towards the TargetObject with a given LaunchAngle
    void grenadeLaunch(Transform target)
    { 
        
        rigid.useGravity = true;
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

    void rocketLaunch(Transform target)
    {
        rigid.useGravity = false;

        transform.LookAt(TargetObject.position);
        rigid.velocity = transform.forward * 10;
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag != "HeavyWeapon")
        {
            touching = true;
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;

            rigid.isKinematic = true;
            rigid.detectCollisions = false;

            WeaponScript.Instance.setInactive(this.gameObject, initialPosition, initialRotation);

        }
        
    }

    void OnCollisionExit()
    {
        touching = false;
    }

}