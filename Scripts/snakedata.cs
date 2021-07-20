using System.Collections.Generic;
using UnityEngine;
public class snakedata
{
    string name;
    public int ranke;
    public int length;

    public snakedata(string name, int ranke, int length)
    {
        this.name = name;
        this.ranke = ranke;
        this.length = length;
    }

    public void setLength( int length)
    {

        this.length = length;
    }
    public string dataStatement()
    {
        string stm = "#" + ranke + "     " + name + "     " + length;
        if (name == PlayerPrefs.GetString("playername"))
            return "<color=lime>" + stm + "</color>";
        else return stm;
    }


}

public class snakedataCompare : IComparer<snakedata>
{
    public int Compare(snakedata x, snakedata y)
    {
        if (x != null && y != null)
            return y.length - x.length;
        else return 0;
    }
}