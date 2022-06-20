using Code.ScriptableObjects;
using Code.Views;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class SandBagView : BaseHazardView
{
    private Collider2D _collider;
    private Rigidbody2D _rigidBody;
    private GameObject player;
    [SerializeField] private GameConfiguration gameConfiguration;
    [SerializeField] private DisplayableData distanceData;
    private int _increments = 0;

    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<Collider2D>();
        _rigidBody = GetComponent<Rigidbody2D>();
        _rigidBody.simulated = false;
        player = GameObject.Find("Player(Clone)");
    }

    // Update is called once per frame
    void Update()
    {

        var speed = gameConfiguration.ChunkSpeed;
  
        if (player.transform.position.x >= transform.position.x - (_collider.bounds.size.x + gameConfiguration.BagDropDistance + gameConfiguration.CalculateIncrement(distanceData.Content))  &&
                    player.transform.position.x <= transform.position.x + _collider.bounds.size.x / 2)
            _rigidBody.simulated = true;
        CheckHits(transform.position);
    }

    private void CheckHits(Vector3 position)
    {
        HitSomethingVertically(position);
    }

    private void HitSomethingVertically(Vector3 position)
    {
        List<RaycastHit2D> verticalHits = CreateVerticalRays(position);
        verticalHits.ForEach(hit => Debug.DrawRay(hit.centroid, Vector3.down));
        var hitSomethingVertically = verticalHits.Any(hit => hit.collider && hit.distance < _collider.bounds.size.y * 0.5f);
        if (hitSomethingVertically)
        {
            Debug.Log("Player Hit from top."); 
            player.GetComponent<PlayerView>().DieFromSmash();
        }
    }

    private List<RaycastHit2D> CreateVerticalRays(Vector3 position) => new List<RaycastHit2D>()
        {
            Physics2D.Raycast(new Vector2(position.x - _collider.bounds.size.x*0.33f, position.y), Vector2.down, 1, 1 << 8),
            Physics2D.Raycast(new Vector2(position.x, position.y), Vector2.down, 1, 1 << 8),
            Physics2D.Raycast(new Vector2(position.x + _collider.bounds.size.x*0.33f, position.y), Vector2.down, 1, 1 << 8)
        };

}
