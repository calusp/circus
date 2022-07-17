using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BestPointsView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI pointsText;
    private void OnEnable()
    {
        pointsText.text = DataRepository.ReturnPoints().ToString();
    }
}
