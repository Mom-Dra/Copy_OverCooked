using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Player
{
    private Rigidbody rigid;
    private void Awake() {
        rigid=GetComponent<Rigidbody>();
    }
    void Update()
    {
        if(Input.GetKey(KeyCode.W))
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward);
            rigid.MovePosition(transform.position + transform.forward * Time.deltaTime * Speed);
        }
        else if(Input.GetKey(KeyCode.S))
        {
            transform.rotation = Quaternion.LookRotation(Vector3.back);
            rigid.MovePosition(transform.position + transform.forward * Time.deltaTime * Speed);
        }
        else if(Input.GetKey(KeyCode.A))
        {
            transform.rotation = Quaternion.LookRotation(Vector3.left);
            rigid.MovePosition(transform.position + transform.forward * Time.deltaTime * Speed);
        }
        else if(Input.GetKey(KeyCode.D))
        {
            transform.rotation = Quaternion.LookRotation(Vector3.right);
            rigid.MovePosition(transform.position + transform.forward * Time.deltaTime * Speed);
        }

        if(Input.GetKeyDown(KeyCode.Space)){
            Debug.DrawRay(transform.position + new Vector3(0,0.2f,0), transform.forward * distance, Color.red, 3.0f);
            RaycastHit hit;
            int layerMask = 1 << LayerMask.NameToLayer("Interactable");
            if(Physics.Raycast(transform.position + new Vector3(0,0.2f,0), transform.forward * distance, out hit, 2, layerMask)){
                Debug.Log("Interact!");
                hit.transform.GetComponent<Interactable>().Interact(this);
            }else{
                Hand(null);
            }
        }
        
    }
    private void FixedUpdate() {
        if(hand != null){
            hand.transform.position = transform.position + (transform.forward * 0.8f);
        }
    }
}
