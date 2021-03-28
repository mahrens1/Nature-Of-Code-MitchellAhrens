using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment1 : MonoBehaviour
{
    // Declare a mover object
    private Mover1_E mover;

    // Start is called before the first frame update
    void Start()
    {
        // Create a Mover object
        mover = new Mover1_E();
    }

    // Update is called once per frame forever and ever (until you quit).
    void Update()
    {
        StartCoroutine(Wait(5f));
        mover.Update();
        mover.CheckEdges();
    }

    IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);

    }
}

public class Mover1_E
{
    // The basic properties of a mover class
    private Vector2 location, velocity, acceleration;
    private float topSpeed;

    // The window limits
    private Vector2 minimumPos, maximumPos;


    // Gives the class a GameObject to draw on the screen
    private GameObject mover = GameObject.CreatePrimitive(PrimitiveType.Sphere);

    public Mover1_E()
    {
        findWindowLimits();
        location = Vector2.zero; // Vector2.zero is a (0, 0) vector
        velocity = Vector2.zero;
        acceleration = Vector2.zero;
        topSpeed = 3F;

        //We need to create a new material for WebGL
        Renderer r = mover.GetComponent<Renderer>();
        r.material = new Material(Shader.Find("Diffuse"));
    }

    public void Update()
    {
        // Random acceleration but it's not normalized!
        acceleration = new Vector2(Random.Range(-3f, 3f), Random.Range(-2f, 2f));
        // Normilize the acceletation
        acceleration.Normalize();
        // Now we can scale the magnitude as we wish!
        acceleration *= Random.Range(30f, 50f);

        // Speeds up the mover
        velocity += acceleration * Time.deltaTime; // Time.deltaTime is the time passed since the last frame.

        // Limit Velocity to the top speed
        velocity = Vector2.ClampMagnitude(velocity, topSpeed);

        // Moves the mover
        location += velocity * Time.deltaTime;


        // Updates the GameObject of this movement
        mover.transform.position = new Vector2(location.x, location.y);
    }

    public void CheckEdges()
    {
        if (location.x > maximumPos.x || location.x < minimumPos.x)
        {
            velocity.x = -velocity.x;
        }
        if (location.y > maximumPos.y || location.y < minimumPos.y)
        {
            velocity.y = -velocity.y;
        }
    }

    private void findWindowLimits()
    {
        // The code to find the information on the camera as seen in Figure 1.2

        // We want to start by setting the camera's projection to Orthographic mode
        Camera.main.orthographic = true;
        // Next we grab the minimum and maximum position for the screen
        minimumPos = Camera.main.ScreenToWorldPoint(Vector2.zero);
        maximumPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }

}
