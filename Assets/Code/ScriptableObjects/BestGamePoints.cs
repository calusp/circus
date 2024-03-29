using Code.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BestGamePoints")]
public class BestGamePoints : ScriptableObject
{
    public int PointsPerTicket = 75;
    public DisplayableData CurrentTicketData;
    public DisplayableData CurrentDistance;

    public void SaveBestGame()
    {
        var bestPoints = DataRepository.ReturnPoints();
        float currentPoints = GetPoints();
        if (currentPoints > bestPoints)
            DataRepository.SaveBestPoints((int)currentPoints);
    }

    public float GetPoints() =>
        CurrentTicketData.Content * PointsPerTicket + CurrentDistance.Content;
}
