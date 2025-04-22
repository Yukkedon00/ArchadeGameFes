using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class SmasherManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Rigidbody myRb;

    private float defaultPosY = 0f;
    private float cameraDistance = 0f;
    
    public void Initialise(InputManager inputManger)
    {
        if (photonView.IsMine)
        {
            cameraDistance = Camera.main.transform.position.z - this.transform.position.z;
            defaultPosY = this.transform.position.y;
            SetInput(inputManger.GetPlayerInput);
        }
    }

    private void SetInput(PlayerInput input)
    {
        //var inputGame = input.actions.FindActionMap("AirHockey");

        if (photonView.IsMine)
        {
            input.SwitchCurrentActionMap("AirHockey");
            input.actions["Move"].performed += OnMove;
            input.actions["SmasherDown"].started += OnSmasherDown;
        }
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        var moveVal = context.ReadValue<Vector2>();
        var ray = Camera.main.ScreenPointToRay(moveVal);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            // NOTE:当たり判定ではない部分で消してしまったら
            //      処理しない部分でつっかえが起こってしまう為要修正
            if (hit.collider.tag != "moveSeigen")
            {
                return;
            }
        }
        
        var t = (defaultPosY - ray.origin.y) / ray.direction.y;
        var hitPoint = ray.origin + ray.direction * t;

        // myRb.MovePosition(hitPoint);
        //myRb.linearVelocity = hitPoint;
        // 速度ベクトルが移動していたらその方向に向かっていくのは当り前じゃanaika
        myRb.MovePosition(hitPoint);
    }

    private void OnSmasherDown(InputAction.CallbackContext context)
    {
        Debug.Log("Down");
    }

    private void OnCollisionEnter(Collision other)
    {
        
    }
}
