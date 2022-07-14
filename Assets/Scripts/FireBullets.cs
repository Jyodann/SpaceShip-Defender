using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class FireBullets : MonoBehaviour
{
    /// <summary>
    ///     This class is mainly for handling bullet firing from the playerObject
    /// </summary>
    private InputMaster controls;
    //Takes in a bullet prefab as the bullet to be shot:
    [SerializeField] private GameObject bulletObject;

    //All these transforms are empty game objects childed to the ship in their respective locations
    //These serialized variables are not public as they do not need to be referenced anywhere else,
    //but need to have a reference from the editor.

    [SerializeField] private Transform mainCannon;
    [SerializeField] private Transform leftCannon;
    [SerializeField] private Transform rightCannon;
    [SerializeField] private Transform extremeLeftCannon;
    [SerializeField] private Transform extremeRightCannon;

    //AudioClip that can be changed from inspector if required. This sound plays when a bullet is fired:
    [SerializeField] private AudioClip lazerShot;

    //Bullet speed can be changed from inspector, affects how fast the bullet flies from the ship.
    public float bulletSpeed = 100f;

    //FireRate float will affect the number of bullets shot per second.
    public float fireRate = 0.5f;

    //Affects number of cannons currently firing based on upgrade:
    [Range(1, 5)] public int cannonCount = 1;

    //Affects amount of damage dealt per bullet based on upgrade:
    public int damageDealt = 10;

    //Stores an audioSource as a global variable so it can play the LazerShot sound


    private ButtonControl btn;
    private bool isShootHeld;
    private ObjectPooler objectPooler; 
    private void Awake()
    {
        controls = new InputMaster();
        controls.Player.Shoot.performed += ShootOnperformed;
        controls.Player.Shoot.canceled += ShootOncanceled;
        //objectPooler = ObjectPooler.Instance;
    }

    private void ShootOncanceled(InputAction.CallbackContext obj)
    {
        isShootHeld = false;
    }

    private void ShootOnperformed(InputAction.CallbackContext obj)
    {
        isShootHeld = true;
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Start()
    {
        //Starts the fire() Coroutine so that the player can start Firing Bullets:
        StartCoroutine(Fire());
    }

    private void Update()
    {
        
    }

    /// <summary>
    ///     Fire() coroutine is responsible for regulating the bullet fire rate.
    ///     Coroutine used here as the rate of fire is easier to manage with yeild return waitForSeconds
    ///     compared to using Update()
    /// </summary>
    /// <returns></returns>
    private IEnumerator Fire()
    {
        while (true)
        {
            //If game is paused, disallows any other action to occur:
            while (GameManager.Instance.isPaused) yield return null;
            //If Fire1 (MouseLeft) is held, then it starts to fire:
            if (isShootHeld)
            {
                AudioManager.Instance.PlaySound(AudioManager.AudioName.SFX_Lazer);
                
                
                //Fires from respective cannons depending on how many there are in the current upgrade:
                switch (cannonCount)
                {
                    case 1:
                        var bullet = ObjectPooler.Pool.Get();
                        bullet.OnObjectSpawn(mainCannon.position, mainCannon.rotation);
                        //objectPooler.SpawnFromPool("Bullet", mainCannon.position, mainCannon.rotation);
                        break;

                    case 2:
                        //objectPooler.SpawnFromPool("Bullet", leftCannon.position, leftCannon.rotation);
                        //objectPooler.SpawnFromPool("Bullet", rightCannon.position, rightCannon.rotation);
                        break;

                    case 3:
                       // objectPooler.SpawnFromPool("Bullet", mainCannon.position, mainCannon.rotation);
                        //objectPooler.SpawnFromPool("Bullet", leftCannon.position, leftCannon.rotation);
                        //objectPooler.SpawnFromPool("Bullet", rightCannon.position, rightCannon.rotation);
                        break;

                    case 4:
                        //objectPooler.SpawnFromPool("Bullet", extremeLeftCannon.position, extremeLeftCannon.rotation);
                        //objectPooler.SpawnFromPool("Bullet", leftCannon.position, leftCannon.rotation);
                        //objectPooler.SpawnFromPool("Bullet", rightCannon.position, rightCannon.rotation);
                        //objectPooler.SpawnFromPool("Bullet", extremeRightCannon.position, extremeRightCannon.rotation);
                        break;

                    case 5:
                        //objectPooler.SpawnFromPool("Bullet", extremeLeftCannon.position, extremeLeftCannon.rotation);
                        //objectPooler.SpawnFromPool("Bullet", leftCannon.position, leftCannon.rotation);
                        //objectPooler.SpawnFromPool("Bullet", rightCannon.position, rightCannon.rotation);
                        //objectPooler.SpawnFromPool("Bullet", extremeRightCannon.position, extremeRightCannon.rotation);
                        //objectPooler.SpawnFromPool("Bullet", mainCannon.position, mainCannon.rotation);
                        break;
                }
                
            }

            //returns waitForSecondsRealTime as fireRate is counted in realSeconds, and this method will wait for the seconds to end
            //before allowing another call:
            yield return new WaitForSecondsRealtime(fireRate);
        }
    }

    //Fire cannon Method accepts a cannon location, and instantiates a bullet prefab in that location
    private void FireCannon(Transform cannonLocation)
    {
        //TransformDirection translates the current ship's facing direction (local) to a
        //world vector, which allows bullets to fly from where the ship is facing:

        Instantiate(bulletObject, cannonLocation.position, transform.rotation);
    }
}