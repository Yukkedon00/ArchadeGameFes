using System;
using Cysharp.Threading.Tasks;
using Photon.Pun;
using R3;
using UnityEngine;

public class AirHockeySequencer : MonoBehaviour
{
    [SerializeField] private AirHockeyController controller;
    [SerializeField] private AirHockeyPhoton photon;
    
    private void Awake()
    {
        photon.IsPreparationObservable.SubscribeAwait(async (isReady, ct) =>
        {
            if (PhotonNetwork.IsMasterClient)
            {
                controller.SetPack(photon.PackObject);
                controller.GameStart();
            }
            
        }).AddTo(this);

        photon.PlayerStatusObservable.Subscribe(status =>
        {
            controller.Initialize(status);
        }).AddTo(this);
        
    }

}
