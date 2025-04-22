using System;
using Photon.Pun;
using R3;
using UnityEngine;

public enum eHitType
{
    None,
    Wall,
    Smasher,
    Goal,
}

public class Pack : MonoBehaviourPunCallbacks
{
    [SerializeField] private Rigidbody myRb;
    
    public void InactivePack()
    {
        this.gameObject.SetActive(false);
    }

    public void ResetPosition(Vector3 position)
    {
        this.transform.position = position;
        myRb.linearVelocity = Vector3.zero;
        photonView.RPC("ActivePack", RpcTarget.All, position);
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Untagged"))
        {
            return;
        }
        
        if (other.gameObject.CompareTag("wall"))
        {
            SeManager.Instance.PlayPositionSe(this.transform.position, 0);
        }
        else if (other.gameObject.CompareTag("smasher"))
        {
            if (!photonView.IsMine)
            {
                photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
                Debug.Log("RequestOwnershipしたよ！");
                return;
            }
            Debug.Log("私オーナー！");
            SeManager.Instance.PlayPositionSe(this.transform.position, 1);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("goal"))
        {
            SeManager.Instance.PlayPositionSe(this.transform.position, 2);
        }
    }

    [PunRPC]
    private void ActivePack(Vector3 position)
    {
        gameObject.transform.position = position;
        myRb.linearVelocity = Vector3.zero;
        this.gameObject.SetActive(true);
    }
}
