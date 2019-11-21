using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Action { left, right, middle };

public class PlayerController : MonoBehaviour
{
    Action nextAction = Action.middle;

    void MoveLeft()
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z - 1);
    }

    void MoveRight()
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z + 1);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Left"))
            nextAction = Action.left;

        if (Input.GetButton("Right"))
            nextAction = Action.right;
     
        if( nextAction == Action.left)
        {
            MoveLeft();
        }

        if( nextAction == Action.right)
        {
            MoveRight();
        }

        if( nextAction == Action.middle)
        {
           
        }

        nextAction = Action.middle;
    }
}
