using Code.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class RaysHelper
{
    public static void HitSomethingVertically(List<RaycastHit2D> verticalHits, Collider2D collider, Action onHit)
    {
        verticalHits.ForEach(hit => Debug.DrawRay(hit.centroid, Vector3.down));
        var hitSomethingVertically = verticalHits.Any(hit => hit.collider && hit.distance < collider.bounds.size.y * 0.5f);
        if (hitSomethingVertically)
        {
            Debug.Log("Player Hit from top.");
            onHit();
            verticalHits.First(hit => hit.collider && hit.distance < collider.bounds.size.x * 0.5f).collider.GetComponent<PlayerView>().DieFromSmash();
        }
    }

 
}
