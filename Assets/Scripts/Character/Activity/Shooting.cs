using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shooting : MonoBehaviour
{
    [SerializeField] private GameObject projectile, gun;
    [SerializeField] private int _maxAmmo;
    private int ammo;
    private bool isReload, readyToShot;
    [SerializeField] private float delayBeforeGivingAmmo, _timeBeforeAttack, _damage;
    [SerializeField] private Text ammoText, reloadText, readyToShootingText;
    private Coroutine _addMoreAmmo;
    private Vector3 _playerDirection; 
    public Vector3 _projectileDirection;

    private void Start()
    {
        isReload = false;
        readyToShot = true;
        ammo = _maxAmmo;

        ammoText.text = ammo.ToString();
        reloadText.text = isReload.ToString();
        readyToShootingText.text = readyToShot.ToString();
    }

    private void Update()
    {
        if(Input.GetMouseButton(0) && isReload == false && readyToShot == true)
        {
            ammo--;
            readyToShot = false;

            RaycastHit clickPoint;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out clickPoint))
            {
                _playerDirection = clickPoint.point - transform.position;
                _playerDirection.y = 0;

                if (Mathf.Abs(clickPoint.point.y - transform.position.y) > 0.2f)
                {
                    Debug.Log(clickPoint.point.y);
                    _projectileDirection = clickPoint.point - transform.position;
                    if((clickPoint.point.y - transform.position.y) > 0)
                        _projectileDirection.y = clickPoint.point.y;
                    else
                    {
                        _projectileDirection.y = clickPoint.point.y - gun.transform.position.y;
                    }
                }
                else
                {
                    _projectileDirection = _playerDirection;
                    _projectileDirection.y = _playerDirection.y;
                }
                transform.rotation = Quaternion.LookRotation(_playerDirection);


                if (ammo == 0)
                {
                    isReload = true;
                }

                ResetCoroutine();

            }

            Invoke(nameof(SetReadyToShot), _timeBeforeAttack);
            Attack();

            ammoText.text = ammo.ToString();
            reloadText.text = isReload.ToString();
            readyToShootingText.text = readyToShot.ToString();
        }
    }

    private void ResetCoroutine()
    {
        if(_addMoreAmmo != null)
        {
            StopCoroutine(_addMoreAmmo);
            _addMoreAmmo = null;
        }

        _addMoreAmmo = StartCoroutine(AddMoreAmmo());
    }

    private IEnumerator AddMoreAmmo()
    {
        while (ammo < _maxAmmo)
        {
            yield return new WaitForSeconds(delayBeforeGivingAmmo);
            ammo++;
            ammoText.text = ammo.ToString();
        }
        isReload = false;

        reloadText.text = isReload.ToString();
        readyToShootingText.text = readyToShot.ToString();
        _addMoreAmmo = null;
    }

    private void SetReadyToShot()
    {
        readyToShot = true;

        ammoText.text = ammo.ToString();
        reloadText.text = isReload.ToString();
        readyToShootingText.text = readyToShot.ToString();
    }

    private void Attack()
    {
        GameObject newProjectile = Instantiate(projectile);
        newProjectile.transform.position = gun.transform.position;
        newProjectile.transform.rotation = Quaternion.LookRotation(_projectileDirection);
        newProjectile.GetComponent<Projectile>().GetDamage = _damage; //тут добавить увеличение урона по характеристикам
    }
}
