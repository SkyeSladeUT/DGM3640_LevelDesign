using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Follow_Player_Movement : MonoBehaviour
{
    public float WalkSpeed, CrouchSpeed, Radius;
    public float StandTime, CrouchTime, CrouchStandTime;
    public Transform Player;
    public Animator anim;
    private PlayerMovement _playerMovement;
    private NavMeshAgent _agent;
    private bool following, crouched;
    private Vector3 dest;
    private WaitForSeconds crouchTimeWait, uncrouchTimeWait, standTimeWait;
    private WaitForFixedUpdate fixedUpdateWait;

    private void Start()
    {
        crouchTimeWait = new WaitForSeconds(CrouchTime);
        uncrouchTimeWait = new WaitForSeconds(CrouchStandTime);
        standTimeWait = new WaitForSeconds(StandTime);
        fixedUpdateWait = new WaitForFixedUpdate();
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = 0;
        _playerMovement = Player.GetComponent<PlayerMovement>();
    }

    public void Standup()
    {
        anim.SetTrigger("Stand");
        StartCoroutine(WaitStart());
    }

    IEnumerator WaitStart()
    {
        yield return standTimeWait;
        StartFollow();
    }
    
    private void StartFollow()
    {
        following = true;
        crouched = false;
        _agent.speed = WalkSpeed;
        StartCoroutine(FollowPlayer());
    }

    IEnumerator FollowPlayer()
    {
        while (following)
        {
            dest = Player.position;
            dest.y = transform.position.y;
            _agent.destination = dest;
            if (_playerMovement.isCrouched && !crouched)
            {
                anim.ResetTrigger("Stand");
                anim.ResetTrigger("Sneak_Stop");
                anim.ResetTrigger("Walk_Start");
                anim.ResetTrigger("Walk_Stop");
                anim.ResetTrigger("Sneak_Start");
                anim.SetTrigger("Crouch");
                _agent.speed = 0;
                yield return crouchTimeWait;
                crouched = true;
            }
            else if (!_playerMovement.isCrouched && crouched)
            {
                anim.ResetTrigger("Crouch");
                anim.ResetTrigger("Sneak_Stop");
                anim.ResetTrigger("Walk_Start");
                anim.ResetTrigger("Walk_Stop");
                anim.ResetTrigger("Sneak_Start");
                anim.SetTrigger("Stand");
                _agent.speed = 0;
                yield return uncrouchTimeWait;
                crouched = false;
            }

            if (crouched)
            {
                if (CheckDist())
                {
                    anim.ResetTrigger("Stand");
                    anim.ResetTrigger("Crouch");
                    anim.ResetTrigger("Walk_Start");
                    anim.ResetTrigger("Walk_Stop");
                    anim.ResetTrigger("Sneak_Start");
                    anim.SetTrigger("Sneak_Stop");
                    _agent.speed = 0;
                }
                else
                {
                    anim.ResetTrigger("Stand");
                    anim.ResetTrigger("Crouch");
                    anim.ResetTrigger("Walk_Start");
                    anim.ResetTrigger("Walk_Stop");
                    anim.ResetTrigger("Sneak_Stop");
                    anim.SetTrigger("Sneak_Start");
                    _agent.speed = CrouchSpeed;
                }
            }
            else
            {
                if (CheckDist())
                {
                    anim.ResetTrigger("Stand");
                    anim.ResetTrigger("Crouch");
                    anim.ResetTrigger("Sneak_Start");
                    anim.ResetTrigger("Sneak_Stop");
                    anim.ResetTrigger("Walk_Start");
                    anim.SetTrigger("Walk_Stop");
                    _agent.speed = 0;
                }
                else
                {
                    anim.ResetTrigger("Stand");
                    anim.ResetTrigger("Crouch");
                    anim.ResetTrigger("Sneak_Start");
                    anim.ResetTrigger("Sneak_Stop");
                    anim.ResetTrigger("Walk_Stop");
                    anim.SetTrigger("Walk_Start");
                    _agent.speed = WalkSpeed;
                }
            }

            yield return fixedUpdateWait;
        }
    }

    private bool CheckDist()
    {
        if (((transform.position.x >= dest.x - Radius) && (transform.position.x <= dest.x + Radius))
            && (transform.position.z >= dest.z - Radius) && (transform.position.z <= dest.z + Radius))
        {
            return true;
        }
        return false;
    }
}
