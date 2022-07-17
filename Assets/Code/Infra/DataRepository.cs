using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataRepository 
{
   public static void SaveBestPoints(int points)
    {
        PlayerPrefs.SetInt("points", points);
    }

    public static int ReturnPoints() =>
        PlayerPrefs.GetInt("points");
}
