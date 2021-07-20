using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class headTrigger : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {

        if (other.transform.tag == "bot" && transform.root.GetComponent<SnakeMovementController>())
        {
            if (!transform.root.GetComponent<SnakeMovementController>().isDead)
            {
                transform.root.GetComponent<SnakeMovementController>().die();
            }
        }

        if (other.transform.tag == "playerSnake" && transform.root.GetComponent<botSnake>())
        {
            if(!transform.root.GetComponent<botSnake>().isDead)
               transform.root.GetComponent<botSnake>().die();
        }

        if (other.transform.tag == "bot" && transform.root.GetComponent<botSnake>() 
            && other.transform.root.GetComponent<botSnake>() != transform.root.GetComponent<botSnake>())
        {
            if (!transform.root.GetComponent<botSnake>().isDead)
            {
                transform.root.GetComponent<botSnake>().die();
                other.transform.root.GetComponent<botSnake>().killedbot();
            }
        }
    }



}
