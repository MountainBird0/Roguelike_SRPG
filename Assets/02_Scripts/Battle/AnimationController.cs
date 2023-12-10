using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void StartMove()
    {
        animator.SetBool("IsMove", true);
    }
    public void StopMove()
    {
        animator.SetBool("IsMove", false);
    }

    public void PhysicsAttack()
    {
        animator.SetTrigger("Attack");
    }

    public void GotHit()
    {
        animator.SetTrigger("GotHit");
    }
    public void Death()
    {
        animator.SetBool("IsDeath", true);
    }



    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log($"{GetType()} - �����̱�");
            StartMove();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log($"{GetType()} - ���߱�");
            StopMove();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log($"{GetType()} - ������");
            PhysicsAttack();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Debug.Log($"{GetType()} - �±�");
            GotHit();
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Debug.Log($"{GetType()} - �ױ�");
            Death();
        }
    }
}
