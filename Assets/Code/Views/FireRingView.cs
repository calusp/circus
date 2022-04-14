using Code.Presenters;
using Code.Views;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class FireRingView : BaseHazardView
{
    [SerializeField] BoxCollider2D topCollider;
    [SerializeField] BoxCollider2D bottomColliders;
    private PlayerPresenter _playerPresenter;

    // Start is called before the first frame update
    void Start()
    {
 
    }

    // Update is called once per frame
    void Update()
    {
        if (topCollider.IsTouchingLayers(1 << 8) || bottomColliders.IsTouchingLayers(1 << 8))
        {
            _playerPresenter.DieBurnt();
            enabled = false;
        }
           


    }

    public override void Attach(PlayerPresenter playerPresenter)
    {
        _playerPresenter = playerPresenter;
    }
}
