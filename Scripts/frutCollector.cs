using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class frutCollector : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
 
        if ( other.transform.tag == "fruit" && transform.root.GetComponent<SnakeMovementController>())
        {
            
            FindObjectOfType<fruitSpowner>().fruitXwra(other.transform);
        }

        if (other.transform.tag == "fruit" && transform.root.GetComponent<botSnake>())
        { 
            FindObjectOfType<fruitSpowner>().fruitXwra_lalayan_bot(other.transform , transform.root.GetComponent<botSnake>());
        }

    }


}
