using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _enemyMaxHealth;
    private float _health;
    public float _timeBetweenAttack;
    public float _speed;
    public float _triggerDistance;
    private PlayerHealth _player;
    private EnemyManager _manager;
    public NavMeshAgent _nav;
    public Vector3 _target, pointNearPlayer;
    public Camera cam;
    public GameObject _gun;
    private Coroutine _stanLock;
    public float timeBetweenChangePosition = 5, _timeStanLock;
    public bool _notChangePosition, _notFrozen, _notCoolDown, _notStanLock;
    public int index;

    internal bool CheckPossibilityToFollowPlayer
    {
        get
        {
            return _notStanLock && _notFrozen && _notChangePosition;
        }
    }
    internal void SetDefaultSettings()
    {
        _health = _enemyMaxHealth;
        _nav = GetComponent<NavMeshAgent>();
        _notChangePosition = true;
        _notCoolDown = true;
        _notFrozen = true;
        _notStanLock = true;
        index = -1;
        pointNearPlayer = Vector3.zero;
        StartCoroutine(ChangePosition());
    }
    internal void Damage(float damage)
    {
        if (_health - damage > 0)
            _health -= damage;
        else
        {
            if (index != -1)
                    GetEnemyManager.pointMeleeValue[index] = -1;

            Destroy(gameObject);
        }
    }

    internal float FreezingTime
    {
        set
        {
            _notFrozen = false;
            Invoke(nameof(Unfreeze), value);
        }
    }
    

    internal void Unfreeze()
    {
        _notFrozen = true;
    }

    internal float StanLockDuration
    {
        set
        {
            _notStanLock = false;
            if (_timeStanLock < value)
            {
                _timeStanLock = value;
                _stanLock = StartCoroutine(UnStanLock());
            }
        }
    }

    internal IEnumerator UnStanLock()
    {
        while(_timeStanLock > 0)
        {
            yield return new WaitForSeconds(0.01f);
            _timeStanLock -= 0.01f;
        }
        _notStanLock = true;
        StopCoroutine(_stanLock);
    }

    internal float CoolDownDuration
    {
        set
        {
            _notCoolDown = false;
            Invoke(nameof(SetReadyToAttack), value);
        }
    }

    internal void SetReadyToAttack()
    {
        _notCoolDown = true;
    }

    internal PlayerHealth GetPlayer
    {
        get
        {
            if (_player != null)
            {
                return _player;
            }
            else
            {
                _player = FindObjectOfType<PlayerHealth>();
                return _player;
            }
        }
    }

    internal Vector3 GetPlayerPosition
    {
        get
        {
            if (_player != null)
            {
                return _player.transform.position;
            }
            else
            {
                _player = FindObjectOfType<PlayerHealth>();
                return _player.transform.position;
            }
        }
    }
    internal EnemyManager GetEnemyManager
    {
        get
        {
            if(_manager != null)
            {
                return _manager;
            }
            else
            {
                _manager = FindObjectOfType<EnemyManager>();
                return _manager;
            }
        }
    }

    internal float GetDistance
    {
        get
        {
            return Vector3.Distance(transform.position, GetPlayer.transform.position);
        }
    }
    internal void LookAtPlayer()
    {
        Vector3 direction = GetPlayer.transform.position - transform.position;
        direction.y = 0;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    public Vector3 GetTargetPosition
    {
        get
        {
            Vector3 vect = new Vector3(Random.Range(1, 10) - 5, 0, Random.Range(1, 10) - 5);
            return transform.position + vect;
        }
    }
    internal IEnumerator ChangePosition()
    {
        yield return new WaitForSeconds(timeBetweenChangePosition);
        timeBetweenChangePosition = Random.Range(6, 10);
        _target = GetTargetPosition;
        if (_notFrozen && _notStanLock)
        {
            Move(_target);
            _notChangePosition = false;
            StartCoroutine(ChangePosition());
        }

    }

    internal void RangeAttack(GameObject projectile)
    {
        if (GetEnemyManager.GetCountAttack < GetEnemyManager._maxCountAttacks)
        {
            GetEnemyManager.AnAttackPlayer(); ;
            GameObject newProjectile = Instantiate(projectile);
            newProjectile.transform.position = _gun.transform.position;
            newProjectile.transform.rotation = Quaternion.LookRotation(GetPlayerPosition - transform.position);
            CoolDownDuration = _timeBetweenAttack;
        }
    }

    internal void AttackMelee(float damage)
    {
        if (GetEnemyManager.GetCountAttack < GetEnemyManager._maxCountAttacks)
        {
            GetEnemyManager.AnAttackPlayer();
            GetPlayer.Damage(damage);
            CoolDownDuration = _timeBetweenAttack;
        }
    }

    internal void Stop()
    {
        if (_notChangePosition == true)
        {
            _nav.speed = 0;
            _nav.destination = transform.position;
        }
    }

    internal void Move(Vector3 target)
    {
        _nav.speed = _speed;

        _nav.destination = target;
    }

    internal void LongFightDistance(GameObject defProjectile)
    {
        if(_notCoolDown && CheckPossibilityToFollowPlayer)
        {
            Stop();
            Ray rayToPlayer = new Ray(transform.position, GetPlayer.transform.position - transform.position);
            if(Physics.Raycast(rayToPlayer, out RaycastHit hit))
            {
                if (hit.collider.TryGetComponent<PlayerHealth>(out _))
                {
                    RangeAttack(defProjectile);
                }
            }
            
        }

        if (index != -1)
            if (GetEnemyManager.pointMeleeValue[index] != -1)
            {
                GetEnemyManager.pointMeleeValue[index] = -1;
                index = -1;
            }
    }

    internal virtual void CloseDistance(float damage, float meleeFightDistance)
    {

        if (CheckPossibilityToFollowPlayer)
        {
            if (meleeFightDistance <= GetDistance)
            {

                if (index == -1)
                {
                    if (gameObject.name == "Melee")
                    {
                        Debug.Log(GetDistance);
                    }
                    for (int i = 0; i < GetEnemyManager.pointMeleeValue.Count; i++)
                    {
                        if (GetEnemyManager.pointMeleeValue[i] == -1 && pointNearPlayer == Vector3.zero)
                        {
                            pointNearPlayer = GetEnemyManager.pointsMelee[i].transform.position;
                            
                            GetEnemyManager.pointMeleeValue[i] = i;

                            index = i;
                        }
                    }
                }

                if (pointNearPlayer != Vector3.zero)
                    Move(pointNearPlayer);


            }
            else
            {
                Stop();
                if (_notCoolDown && pointNearPlayer != Vector3.zero)
                {
                    AttackMelee(damage);
                }
            }
        }
        else
        {
            Stop();
        }
    }


}
