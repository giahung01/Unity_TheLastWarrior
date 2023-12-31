using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TestScipt : MonoBehaviour
{
    public Transform Target;
    public float firingAngle = 45.0f;
    public float gravity = 9.8f;

    public Transform Projectile;
    private Transform myTransform;
    // Increase speed for projectile
    void Awake()
    {
        myTransform = transform;
    }

    void Start()
    {
        StartCoroutine(SimulateProjectile());
    }


    IEnumerator SimulateProjectile()
    {
        // Short delay added before Projectile is thrown
        yield return new WaitForSeconds(1.5f);

        // Move projectile to the position of throwing object + add some offset if needed.
        Projectile.position = myTransform.position;

        // Calculate distance to target
        float target_Distance = Vector2.Distance(Projectile.position, Target.position);

        // Calculate the velocity needed to throw the object to the target at specified angle.
        float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);

        // Extract the X  Y componenent of the velocity
        float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);

        // Calculate flight time.
        float flightDuration = target_Distance / Vx;

        // Rotate projectile to face the target.
        //Projectile.rotation = Quaternion.LookRotation(Target.position - Projectile.position);
        Vector2 direction = (Target.position - Projectile.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        Projectile.rotation = rotation;


        float elapse_time = 0;

        while (elapse_time < flightDuration)
        {
            // Multiply by -1 if on the player's right, 1 if on the player's left
            Projectile.Translate(Vx * Time.deltaTime, (Vy - (gravity * elapse_time)) * Time.deltaTime , 0);

            elapse_time += Time.deltaTime;

            yield return null;
        }
    }
}
