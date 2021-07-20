using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public static UIManager ui;
        





    [SerializeField] Text lengthText;
    [SerializeField] Text ranksTxt;
    [SerializeField] Text score;




    private void Awake()
    {
        ui = this;
    }

    void Start()
    {
        deadUi.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
   
    }
    

    public void updateLength(int length)
    {
        lengthText.text = "Your Length : " + length;

    }


    public List<string> top10()
    {
        List<snakedata> _list= new List<snakedata>();
        foreach(botSnake s in EnemeySpowner.main.bots)
        {
            if(s)
              _list.Add(s.data);
        }

        _list.Add(SnakeMovementController.main.data);

        _list.Sort(new snakedataCompare());
        List<string> rankedlist = new List<string>();
        int indexer = 0;
        foreach(snakedata s in _list)
        {

            indexer++;
            if (indexer <= 10 && SnakeMovementController.main )
            {
                s.ranke = _list.IndexOf(s) + 1;
                rankedlist.Add(s.dataStatement());
            }
            else break;
        }

        return rankedlist;

    }

    public string nameGenerator()
    {
        int range = (int)Random.Range(3 , 7);
        char[] alphabet = "abcdefghlmnopqrstuvwxyz".ToCharArray();
        string _name = "";
        for(int i = 0; i < range; i++)
        {
            int randomIndex = (int)Random.Range(0 , alphabet.Length);
            _name += alphabet[randomIndex];

        }
        if(_name == "diar")
        {
            _name = "sa" + _name;
        }
        return _name;
    }

    public void updaetRanksOnUI()
    {
        ranksTxt.text = "<color=grey>----</color><color=white>Leaderbord</color><color=grey>----</color>\n";
        foreach (string s in top10())
        {
            ranksTxt.text += s + "\n";
        }
    }
    [SerializeField] GameObject deadUi;


    public void playerdead()
    {
        deadUi.SetActive(true);
        score.text = SnakeMovementController.main.getLengthCount().ToString();
    }

    public void loadScene(int scene)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene);    
    }


}
