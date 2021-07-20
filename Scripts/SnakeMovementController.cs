using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeMovementController : MonoBehaviour
{    public static SnakeMovementController main;
   
    
    
    [SerializeField] List<Material> seqMaterials = new List<Material>();

    [SerializeField] [Range(0, 10)] float speed = 1;
    [SerializeField] [Range(0, 1000)] int startSize = 10;
    [SerializeField] [Range(0, 10)] float smoothmouseTimer = 0.5f;
    [SerializeField] LayerMask ignorLayer;




    public List<Transform> Snakeparts = new List<Transform>();
    public Transform head;
    public GameObject bodyPrefabe;



    Transform currentBody;
    Transform nextBody;

    float currentSpeed;

    [HideInInspector] public bool isDead = false;


    [HideInInspector] public snakedata data;
    private void Awake()
    {
        main = this;
    }




    void Start()
    {
        Time.timeScale = 1;
        main = this;
        Snakeparts.Add(head);
        seqMaterials.Add(head.GetChild(1).GetComponent<Renderer>().material);

        for(int i = 0; i < startSize; i++)
        {
            addBody();
        }
        UIManager.ui.updateLength(Snakeparts.Count);


        applyGlow(0);

        data = new snakedata(PlayerPrefs.GetString("playername"), 0, 24);
    }

    // Update is called once per frame
    void Update()
    {


        
    }

    private void FixedUpdate()
    {
        if(!isDead)
           MoveSnake();
    }

    float snakepartsDist = 0.15f;
    void MoveSnake()
    {
        currentSpeed = speed;
        if (Input.GetMouseButton(0))
        {
            currentSpeed *= 3;
            speedMove();
            applyGlow(1);
        } else {
            applyGlow(0);
        }


        head.Translate(Vector3.forward * currentSpeed * Time.fixedDeltaTime);

        head.LookAt(FlatMousePos());

        for(int i = 1; i < Snakeparts.Count; i++)
        {
            currentBody = Snakeparts[i-1];
            nextBody = Snakeparts[i];

            float distance = Vector3.Distance(currentBody.position, nextBody.position);
            if (distance > snakepartsDist)
            {
                float timer = (1* distance);
                if (timer > 0.99)
                    timer = 0.9f;
                nextBody.position = Vector3.Lerp(nextBody.position, currentBody.position, timer);


            }

        }
    }

    Vector3 smoothpoint = Vector3.zero;
    void addBody() {
        Vector3 newPos = Snakeparts[Snakeparts.Count - 1].position;
        newPos.z -= 0.1f;

        Transform spownedBody = Instantiate(bodyPrefabe, newPos, Snakeparts[Snakeparts.Count - 1].rotation).transform;
        spownedBody.transform.SetParent(this.transform);
        Snakeparts.Add(spownedBody);

        updateColor();
        updateScale();

    }

    void subtractBody()
    {
        // last part
        Transform toDelete = Snakeparts[Snakeparts.Count - 1];

        Snakeparts.Remove(toDelete);
        Destroy(toDelete.gameObject);

    }

    Vector3 FlatMousePos()
    {
        RaycastHit hit;
        Vector3 point = Vector3.zero;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray , out hit , 300 , ignorLayer))
        {
            if(hit.transform.tag == "landscape")
            {
                point = hit.point;
            }
        }

        Vector3 _FlatMousePos = new Vector3(point.x, transform.position.y, point.z);

        smoothpoint = Vector3.Lerp(smoothpoint, _FlatMousePos, smoothmouseTimer * Time.smoothDeltaTime);

        
        return _FlatMousePos;
    }

    void updateColor()
    {
        for(int i = 1; i< Snakeparts.Count; i++)
        {
            Material _tempMat = seqMaterials[(i - 1) % seqMaterials.Count];
            Snakeparts[i].GetComponent<Renderer>().material = _tempMat;
        }
    }

    int fruitXwrawcount = 0;
    int lengthCount = 24;
    int addbodyCount = 0;
    public void xwardniFruit(float size)
    {
        fruitXwrawcount += (int)(size * 10) > 15 ? 15 : (int)(size * 10);

        if (fruitXwrawcount > 10)
        {
            lengthCount++;
            addbodyCount++;
            UIManager.ui.updateLength(lengthCount);
            fruitXwrawcount = 0;

            if(addbodyCount == 6)
            {
                addBody();
                addbodyCount = 0;
            }
        }

        data.setLength(lengthCount);
        UIManager.ui.updaetRanksOnUI();
        updateScale();
    }

    void applyGlow(int value)
    {
        foreach(Material m in seqMaterials)
        {
            if(m.GetInt("isglow") != value)
            {
                m.SetInt("isglow", value);
            }
        }
    }

    int SpeedMoveCounter = 0;
    int subtractBodyCounter = 0 ;
    void speedMove()
    {
        SpeedMoveCounter++;
        if(SpeedMoveCounter > (4000/lengthCount) && lengthCount > 20)
        {

            lengthCount--;
            SpeedMoveCounter = 0;
            subtractBodyCounter++;
            UIManager.ui.updateLength(lengthCount);
            if(subtractBodyCounter == 5)
            {
                subtractBody();
                subtractBodyCounter = 0;
            }
        }


        data.setLength(lengthCount);
        UIManager.ui.updaetRanksOnUI();

    }

    public void die()
    {


        // score
        if (lengthCount > PlayerPrefs.GetInt("highscore"))
        {
            PlayerPrefs.SetInt("highscore", lengthCount);
        }

        //

        isDead = true;

        UIManager.ui.playerdead();
        Time.timeScale = 0;
        deletall();

    }
    void deletall()
    {
        foreach (Transform t in Snakeparts)
        {
            Destroy(t.gameObject);
        }

        Destroy(this.gameObject);
    }

    void updateScale()
    {
        float scaleFactor  = Mathf.Clamp((lengthCount / 24)  , 1f, 1000);

        scaleFactor = 0.65f + (scaleFactor * 0.001f) * 10;
        scaleFactor = Mathf.Clamp(scaleFactor, 0.65f, 3.5f);

        Vector3 headScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
        Snakeparts[0].localScale = headScale;

        for (int i = 1; i < Snakeparts.Count; i++)
        {
            Vector3 newscale = new Vector3(scaleFactor*0.8f, scaleFactor * 0.8f, scaleFactor * 0.8f);
            Snakeparts[i].localScale = newscale;
        }


        snakepartsDist = 0.15f +(scaleFactor *0.001f) * 150;
        snakepartsDist = Mathf.Clamp(snakepartsDist , 0.15f, 2f);

    }

    public int getLengthCount()
    {
        return lengthCount;
    }

}
