using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemeySpowner : MonoBehaviour
{
    [SerializeField] Material[] matset;
    [SerializeField] GameObject botPre;
    [SerializeField] BoxCollider landscape;
    [SerializeField] int BotsCountInTheScene = 35;
    [SerializeField] float smallbounder = 35;
    [HideInInspector] public List<botSnake> bots = new List<botSnake>();
    int min_size, max_size;
    
    public static EnemeySpowner main;
    private void Awake()
    {
        main = this;
        
    }
    void Start()
    {
        min_size = 24;
        max_size = 50;
        craeteBotPool();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public int currentEnemy_size()
    {
        return Random.Range(min_size , max_size);
    }

    Vector3 randomPoint()
    {
        Bounds b;
        if (FindObjectOfType<SnakeMovementController>())
        {
            b = FindObjectOfType<SnakeMovementController>().transform.GetChild(0).transform.GetChild(0).GetComponent<BoxCollider>().bounds;
        }
        else b = landscape.bounds;
            
        Vector3 randPos =  new Vector3(
            Random.Range(b.min.x- smallbounder, b.max.x +smallbounder) ,
            1.8f ,
            Random.Range(b.min.z- smallbounder, b.max.z + smallbounder)
            );

        if (Vector3.Distance(randPos, SnakeMovementController.main.transform.GetChild(0).position) > 5)
        {
            return randPos;
        }
        else return randomPoint();
    }

    void craeteBotPool()
    {
        for(int i = 0; i < BotsCountInTheScene; i++)
        {
            createOneBot();
        }
    }

    public void bodDead(botSnake b)
    {
        bots.Remove(b);
        createOneBot();
    }

    void createOneBot()
    {
        botSnake spownedSnake = Instantiate(botPre, randomPoint(), Quaternion.identity).GetComponent<botSnake>();
        spownedSnake.landscape = landscape;
        spownedSnake.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material = matset[Random.Range(0, matset.Length)];
       // spownedSnake.transform.SetParent(this.transform);
        bots.Add(spownedSnake);
    }
}
 