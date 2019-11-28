using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Action { left, right, middle };

public class PlayerController : MonoBehaviour
{
    public LayerMask collisionMask;
    
    public float[] distances = new float[3];

    public float speed = 10;

    public float score = 0;

    public bool alive = true;

    public float viewDistance = 50.0f;

    public NeuralNetwork network;

    public void ChooseMove()
    {
        if (alive)
        {
            float[] decision = network.Compute(distances);

            if (decision[0] < 0.333f)
            {
                MoveLeft();
            }
            else if (decision[0] > 0.666f)
            {
                MoveRight();
            }
        }
    }

    void MoveLeft()
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z - 1);
    }

    void MoveRight()
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z + 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (alive)
        {
            transform.position += new Vector3(-1, 0, 0) * speed * Time.deltaTime;
            UpdateDistances();
            ComputeScore();
        }
    }

    private void UpdateDistances()
    {
        RaycastHit hit;

        Ray checkLeft = new Ray(transform.position + new Vector3(0, 0, 0), new Vector3(0, 0, -1));
        Ray checkRight = new Ray(transform.position + new Vector3(0, 0, 0), new Vector3(0, 0, 1));

        if (Physics.Raycast(checkLeft, out hit, 1.0f, collisionMask))
        {
            distances[0] = 0;
        }
        else
        {
            Ray rayLeft = new Ray(transform.position + new Vector3(0, 0, -1), new Vector3(-1, 0, 0));

            if (Physics.Raycast(rayLeft, out hit, viewDistance, collisionMask))
            {
                distances[0] = hit.distance;
            }
            else
            {
                distances[0] = viewDistance;
            }

            Debug.DrawRay(rayLeft.origin, rayLeft.direction);
        }

        Ray rayMiddle = new Ray(transform.position + new Vector3(0, 0, 0), new Vector3(-1, 0, 0));

        if (Physics.Raycast(rayMiddle, out hit, viewDistance, collisionMask))
        {
            distances[1] = hit.distance;
        }
        else
        {
            distances[1] = viewDistance;
        }

        Debug.DrawRay(rayMiddle.origin, rayMiddle.direction);

        if (Physics.Raycast(checkRight, out hit, 1.0f, collisionMask))
        {
            distances[2] = 0;
        }
        else
        {
            Ray rayRight = new Ray(transform.position + new Vector3(0, 0, 1), new Vector3(-1, 0, 0));

            if (Physics.Raycast(rayRight, out hit, viewDistance, collisionMask))
            {
                distances[2] = hit.distance;
            }
            else
            {
                distances[2] = viewDistance;
            }

            Debug.DrawRay(rayRight.origin, rayRight.direction);
        }

        // distances[0] /= viewDistance;
        // distances[1] /= viewDistance;
        // distances[2] /= viewDistance;


    }

    private void ComputeScore()
    {
        score += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
            alive = false;
    }

    public IEnumerator GameLoop()
    {
        while(alive)
        {
            ChooseMove();
            yield return new WaitForSeconds(0.1f);
        }
    }
}
