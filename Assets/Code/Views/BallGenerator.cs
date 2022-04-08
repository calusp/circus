using Code.ScriptableObjects;
using Code.Views;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallGenerator : MonoBehaviour
{
    [SerializeField] private BallView ballPrefab;
    [SerializeField] private GameConfiguration gameConfiguration;

    private SeesawBallConfiguration seesawBallConfiguration;
    private float coolDownAcc;
    // Start is called before the first frame update
    void Start()
    {
        seesawBallConfiguration = gameConfiguration.SeesawBallConfiguration;
        coolDownAcc = seesawBallConfiguration.maxCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        coolDownAcc += Time.deltaTime;
        if(coolDownAcc >= seesawBallConfiguration.maxCooldown)
        {
            coolDownAcc = 0;
            var ballGo = Instantiate(ballPrefab, transform);   
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(transform.position, 0.2f);
    }
}
