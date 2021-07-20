using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class botSnake : MonoBehaviour
{
    [Header("settings")]
    public  BoxCollider landscape;
    [SerializeField] Transform raycaster;

    [SerializeField] List<Material> seqMaterials = new List<Material>();

    [SerializeField] [Range(0, 10)] float speed = 1;
    [SerializeField] [Range(0, 1000)] int startSize = 10;





    public List<Transform> Snakeparts = new List<Transform>();
    public Transform head;
    public GameObject bodyPrefabe;



    Transform currentBody;
    Transform nextBody;

    float currentSpeed;

    Vector3 lookPos = Vector3.zero;

    float snakepartsDistf = 0.15f;

    /// <summary>
    [HideInInspector] public snakedata data;
    /// </summary>
    void Start()
    {
       
        currentSpeed = speed;
        
        Snakeparts.Add(head);
        seqMaterials.Add(head.GetChild(0).GetComponent<Renderer>().material);
        startSize = EnemeySpowner.main.currentEnemy_size();
        for (int i = 0; i < startSize; i++)
        {
            addBody();
        }

        StartCoroutine(gotoNext_Pos(0));
        StartCoroutine(randomFast(0));

        // applyGlow(0);


        lengthCount = startSize * 3;
        data = new snakedata(UIManager.ui.nameGenerator(), 0, lengthCount);

    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(head.transform.position , lookPos) < 0.6f)
        {
            lookPos = nextPoint();
        }


        if (isfast && Time.timeScale == 1)
        {
            speedMove();
            applyGlow(1);
        }
        else
        {
            applyGlow(0);
            currentSpeed = speed;

        }

        if (isObjectInFront() && !waitLooking)
        {
            lookPos = nextPoint();
            waitLooking = true;
            StartCoroutine(stopWaiting());
        }
    }
    bool waitLooking = false ;
    private void FixedUpdate()
    {
        if(!isDead)
        MoveSnake();
    }


   

    void MoveSnake()
    {
        head.Translate(Vector3.forward * currentSpeed * Time.fixedDeltaTime);

        head.LookAt(lookPos);

        for (int i = 1; i < Snakeparts.Count; i++)
        {
            currentBody = Snakeparts[i - 1];
            nextBody = Snakeparts[i];

            float distance = Vector3.Distance(currentBody.position, nextBody.position);
            if (distance > snakepartsDistf)
            {
                float timer = (1 * distance);
                if (timer > 0.99)
                    timer = 0.99f;
                nextBody.position = Vector3.Lerp(nextBody.position, currentBody.position, timer);


            }

        }
    }

    bool isfast = true;
    IEnumerator randomFast(float kat)
    {
        yield return new WaitForSeconds(kat);
        if (isfast)
        {
            StartCoroutine(randomFast(Random.Range(10, 20)));

        }
        else
        {
            StartCoroutine(randomFast(Random.Range(2, 12)));

        }

        isfast = !isfast;

    }

    IEnumerator gotoNext_Pos(float kat)
    {
        yield return new WaitForSeconds(kat);
        lookPos = nextPoint();
        StartCoroutine(gotoNext_Pos(Random.Range(1,12)));

    }

    IEnumerator stopWaiting()
    {
        yield return new WaitForSeconds(0.1f);
        waitLooking = false;
    }

    Vector3 nextPoint()
    {
        Bounds bounds = landscape.bounds;
        if (FindObjectOfType<SnakeMovementController>())
        {
            bounds = FindObjectOfType<SnakeMovementController>().transform.GetChild(0).transform.GetChild(0).GetComponent<BoxCollider>().bounds;
        }
        else
        {
            bounds = landscape.bounds;
        }

        return new Vector3(
            Random.Range(bounds.min.x - 10, bounds.max.x +10),
            transform.position.y,
            Random.Range(bounds.min.z - 10, bounds.max.z + 10)
            );
    }

   
    void addBody()
    {
        Vector3 newPos = Snakeparts[Snakeparts.Count - 1].position;
        newPos.z -= 0.1f;

        Transform spownedBody = Instantiate(bodyPrefabe, newPos, Snakeparts[Snakeparts.Count - 1].rotation).transform;
        spownedBody.GetComponent<Renderer>().material = seqMaterials[0];
        spownedBody.transform.SetParent(this.transform);
        Snakeparts.Add(spownedBody);

        updateScale();


    }

    void subtractBody()
    {
        // last part
        Transform toDelete = Snakeparts[Snakeparts.Count - 1];

        Snakeparts.Remove(toDelete);
        Destroy(toDelete.gameObject);

    }


    int fruitXwrawcount = 0;
    int lengthCount = 24;
    public void xwardniFruit(float size)
    {
        fruitXwrawcount += (int)(size * 10) > 10 ? 10 : (int)(size * 10);
        lengthCount+=1;
        if (fruitXwrawcount > 13)
        {
            addBody();
            
            fruitXwrawcount = 0;
        }

        data.setLength(lengthCount);
        UIManager.ui.updaetRanksOnUI();
        updateScale();
    }

    void applyGlow(int value)
    {
        foreach (Material m in seqMaterials)
        {
            if (m.GetInt("isglow") != value)
            {
                m.SetInt("isglow", value);
            }
        }
    }

    int SpeedMoveCounter = 0;
    void speedMove()
    {
        currentSpeed = speed * 3;
        SpeedMoveCounter++;
        if (SpeedMoveCounter > (4000 / lengthCount) && lengthCount > 10)
        {

            lengthCount+=5;
            SpeedMoveCounter = 0;

        }

        data.setLength(lengthCount);
        UIManager.ui.updaetRanksOnUI();

    }

    int SpownSize = 3;
    [HideInInspector]public bool isDead = false;
    public void die()
    {
        isDead = true;

        fruitSpowner.main.SpownfruitByBodies( Snakeparts, SpownSize);
        EnemeySpowner.main.bodDead(this);
        deletall();
    }
    void deletall()
    {
        foreach(Transform t in Snakeparts)
        {
            Destroy(t.gameObject);
        }

        Destroy(this.gameObject);
    }

    public void killedbot()
    {
        for(int i = 0; i < 5; i++)
        {
            addBody();
            addBody();
            addBody();
            lengthCount += 50;
        }
    }

    void updateScale()
    {
        float scaleFactor = Mathf.Clamp((lengthCount / 24), 1f, 1000);

        scaleFactor = 0.65f + (scaleFactor * 0.001f) * 8;

        scaleFactor = Mathf.Clamp(scaleFactor, 0.65f, 3);

        Vector3 headScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
        Snakeparts[0].localScale = headScale;

        for (int i = 1; i < Snakeparts.Count; i++)
        {
            Vector3 newscale = new Vector3(scaleFactor * 0.8f, scaleFactor * 0.8f, scaleFactor * 0.8f);
            Snakeparts[i].localScale = newscale;


        }

        snakepartsDistf = 0.15f + (scaleFactor * 0.001f) * 150;
        snakepartsDistf = Mathf.Clamp(snakepartsDistf, 0.15f, 3f);

    }

    bool isObjectInFront()
    {
        RaycastHit hit;
        if (Physics.Raycast(raycaster.position, transform.forward, out hit, 2))
        {
            if (hit.transform.tag == "playerSnake" || (hit.transform.tag == "bot" && hit.transform != this.transform))
            {
                return true;
            }
            else return false;
        }
        else return false;
    }

}
