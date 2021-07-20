using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class uihome : MonoBehaviour
{
    [SerializeField] Text score;

    private void Start()
    {
        score.text = PlayerPrefs.GetInt("highscore").ToString();
        updaetUIlangusfe();
    }
    public void loadScene(int scene)
    {
        if(scene == 1)
        {
            getPlayerName();
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
    }

    public void loadLink(string link)
    {
        Application.OpenURL(link);
    }


    public void setLang(int langCode)
    {
        PlayerPrefs.SetInt("lang", langCode);

        updaetUIlangusfe();
    }

    [SerializeField] GameObject[] kuLang;
    [SerializeField] GameObject[] engLang;


    void updaetUIlangusfe()
    {
        bool islangKU = PlayerPrefs.GetInt("lang") == 1 ? true : false;
        if (islangKU)
        {
            foreach (GameObject g in kuLang)
            {
                g.SetActive(true);
            }
            foreach (GameObject g in engLang)
            {
                g.SetActive(false);
            }
        }
        else
        {
            foreach (GameObject g in engLang)
            {
                g.SetActive(true);
            }
            foreach (GameObject g in kuLang)
            {
                g.SetActive(false);
            }

        }
    }

    public void pause()
    {
        bool ispaused = Time.timeScale == 0 ? true: false;
        if (ispaused)
        {
            playbtn.SetActive(false);
            pausebtn.SetActive(true);
            
            Time.timeScale = 1;
        }
        else
        {
            playbtn.SetActive(true);
            pausebtn.SetActive(false);

            Time.timeScale = 0;
        }
    }

    [SerializeField]GameObject playbtn, pausebtn;

    public InputField playernamefild;
    void getPlayerName()
    {
        string name =playernamefild.text;

        if(name =="" || name == " ")
        {
            name = "Diar";
        }

        PlayerPrefs.SetString("playername" , name);
    }

    public void Quit()
    {
        Application.Quit();
    }

}
