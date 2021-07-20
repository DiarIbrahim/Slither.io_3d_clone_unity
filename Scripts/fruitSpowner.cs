using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fruitSpowner : MonoBehaviour
{
    [SerializeField] List<Material> frutMat;
    [SerializeField] GameObject frutPref;
    [SerializeField] float ScenefruitsCount = 10;
    [SerializeField] [Range(0.1f, 100)] float delet_in_distancetoplayer = 10;

    List<Transform> fruitPool = new List<Transform>();
    List<Transform> dieSpown_fruitPool = new List<Transform>();

    Transform sarplayer;


    public static fruitSpowner main;
    private void Awake()
    {
        main = this;
    }

    void Start()
    {
        sarplayer = FindObjectOfType<SnakeMovementController>().transform.GetChild(0).transform;
        Createfruitpool();
    }

    // Update is called once per frame
    bool oneCall_lookat = false;
    void Update()
    {

        if (SceneManager.main.is_cameraMode_classic)
        {
            if (!oneCall_lookat)
            {
                foreach (Transform t in fruitPool)
                {
                    t.LookAt(new Vector3(0, 100000, 0));
                }

                foreach (Transform t in dieSpown_fruitPool)
                {
                    if (t)
                        t.LookAt(new Vector3(0 , 100000  ,0));
                }


                oneCall_lookat = true;
            }

        }
        else
        {
            oneCall_lookat = false;
            foreach (Transform t in fruitPool)
            {
                t.LookAt(Camera.main.transform);
            }

            foreach (Transform t in dieSpown_fruitPool)
            {
                if (t)
                    t.LookAt(Camera.main.transform);
            }
        }

    }


     Vector3 RandomPointNearPlayer()
    {
        Bounds pbounds = FindObjectOfType<SnakeMovementController>().transform.GetChild(0).transform.GetChild(0).GetComponent<BoxCollider>().bounds;
        return new Vector3(
            Random.Range(pbounds.min.x , pbounds.max.x),
            2,
            Random.Range(pbounds.min.z , pbounds.max.z)

        );
       
    }

    Vector3 RandomScale()
    {
        float randomV = Random.Range(0.1f, 0.7f);
        return new Vector3(
            randomV - 0.1f,
            randomV, 
            randomV
            );
    }

    public void fruitXwra(Transform t)
    {
        if (SnakeMovementController.main)
        {
            SnakeMovementController.main.xwardniFruit(t.localScale.z);
            t.gameObject.SetActive(false);
            fruitNewPositioner(t);

        }
    }


    public void fruitXwra_lalayan_bot(Transform t , botSnake b)
    {
        if (SnakeMovementController.main)
        {
            b.xwardniFruit(t.localScale.z);
            t.gameObject.SetActive(false);
            if (fruitPool.Contains(t))
            {
                fruitNewPositioner(t);
            }
            else if (dieSpown_fruitPool.Contains(t))
            {
                dieSpown_fruitPool.Remove(t);
                Destroy(t.gameObject);
            }
        }

    }

    void fruitNewPositioner(Transform t)
    {
        t.position = RandomPointNearPlayer();
        t.gameObject.SetActive(true);
        t.LookAt(new Vector3(0, 100000, 0));
    }

    IEnumerator deletFarFruits()
    {
        yield return new WaitForSeconds(1.5f);
        if(SnakeMovementController.main)
           chickTodelet();
    }
    void chickTodelet() {
        for (int i = 0; i<fruitPool.Count; i++)
        {
            if (Vector3.Distance(sarplayer.position, fruitPool[i].position) > delet_in_distancetoplayer)
            {
                fruitPool[i].gameObject.SetActive(false);
                fruitNewPositioner(fruitPool[i]);
            }

           
        }
        StartCoroutine(deletFarFruits());
    }



    int material_indexer = 0;
    void Createfruitpool()
    {
        while (fruitPool.Count < ScenefruitsCount)
        {
        Transform spownedfruit = Instantiate(frutPref, RandomPointNearPlayer(), Quaternion.identity).transform;
         spownedfruit.localScale = RandomScale();
        spownedfruit.GetChild(0).GetComponent<Renderer>().material = frutMat[material_indexer % frutMat.Count];
        spownedfruit.LookAt(Camera.main.transform.position);
        spownedfruit.SetParent(this.transform);
        fruitPool.Add(spownedfruit);
        material_indexer++;

        }

        StartCoroutine(deletFarFruits());
    }

    

    public void SpownfruitByBodies(List<Transform> coliders , int spownsize)
    {
        foreach (Transform t in coliders)
        {
            for(int i =0; i < spownsize; i++)
            {
                Bounds b = t.GetComponent<BoxCollider>().bounds;
                Vector3 Randompos = new Vector3(
                    Random.Range(b.min.x, b.max.x),
                    2,
                    Random.Range(b.max.z, b.max.z)
                    );
                Transform spowned = Instantiate(frutPref, Randompos, Quaternion.identity).transform;
                spowned.GetChild(0).GetComponent<Renderer>().material = frutMat[Random.Range(0, frutMat.Count)];
                spowned.localScale = RandomScale();
                spowned.SetParent(this.transform);
                dieSpown_fruitPool.Add(spowned);

                spowned.LookAt(new Vector3(0, 100000, 0));
            }
        }

        deletFarDieSpownFruits();


    }

   
        void deletFarDieSpownFruits()
        {
        foreach(Transform t in dieSpown_fruitPool)
        {
            if (t && sarplayer && Vector3.Distance(sarplayer.position, t.position) > delet_in_distancetoplayer - 8)
            {
                t.gameObject.SetActive(false);
                Destroy(t.gameObject);
            }
        }
        }

    }
