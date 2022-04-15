using Code.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DataDisplayer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI label;
    [SerializeField] DisplayableData displayableData;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        label.text =  displayableData.DisplayContent();
    }
}
