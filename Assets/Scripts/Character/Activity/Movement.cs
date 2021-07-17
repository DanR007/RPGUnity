using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private CharacterController controller;
    private Animator characterAnimator;
    private PlayerHealth _invulnerability;
    [SerializeField] Transform checkGroundPoint;
    [SerializeField] private Vector3 moveVector;
    [SerializeField] private float _speedRunning, _defaultSpeed, _jumpForce, _dashForce;
    private float _speed, _gravityForce;

    public float SetSprintDuration
    {
        set
        {
            _speed = _speedRunning;
            Invoke(nameof(InactiveSprint), value);
        }
    }


    private void Start()
    {
        characterAnimator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        _invulnerability = GetComponent<PlayerHealth>();
        _speed = _defaultSpeed;
    }

    private void Update()
    {
        Move();
    }
    private void Move()
    {
        moveVector = Vector3.zero;
        moveVector.x = Input.GetAxis("Horizontal") * _speed;
        moveVector.z = Input.GetAxis("Vertical") * _speed;

        if(moveVector != Vector3.zero)
        {
            characterAnimator.SetBool("Walking", true);
        }
        else
        {
            characterAnimator.SetBool("Walking", false);
        }

        Rotate();

        if (OnTheGround())
        {
            characterAnimator.SetBool("Falling", false);
            _gravityForce = 0;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                characterAnimator.SetBool("Jumping", true);
                moveVector.y = _jumpForce;//прыжок
            }
            else
            {
                characterAnimator.SetBool("Jumping", false);
            }
            //if(Input.GetKeyDown(KeyCode.LeftShift))
            //{
            //    Straddles();
            //}
        }
        else
        {
            characterAnimator.SetBool("Falling", true);
            Gravity();
        }

        controller.Move(moveVector * Time.deltaTime);

    }

    private void Rotate()
    {
        if (Vector3.Angle(Vector3.forward, moveVector) > 1f || Vector3.Angle(Vector3.forward, moveVector) == 0)
        {
            if (Mathf.Abs(moveVector.x) > 1.75f || Mathf.Abs(moveVector.z) > 1.75f)
            {
                Vector3 direct = Vector3.RotateTowards(transform.forward, moveVector, _speed, 0f);
                transform.rotation = Quaternion.LookRotation(direct);
            }
        }
    }
    private void InactiveSprint()
    {
        _speed = _defaultSpeed;
    }

    private void Gravity()
    {
        _gravityForce -= 10f * Time.deltaTime;
        moveVector.y = _gravityForce;
    }

    //private void Jump()
    //{
    //    moveVector.y = _jumpForce;
    //}

    private bool OnTheGround()
    {
        Ray ray = new Ray(checkGroundPoint.position, Vector3.down);
        if(Physics.Raycast(ray,out RaycastHit pointUnderCharacter ,0.1f))
        {
            return pointUnderCharacter.collider.TryGetComponent<WallsAndLand>(out _);
        }
        return false;
    }

    internal void Dash()
    {
        _invulnerability.SetInvulnerabilityDuration = 1f;//поставить время неуязвимости во время переката
        controller.Move(transform.forward * _dashForce);
    }


}
