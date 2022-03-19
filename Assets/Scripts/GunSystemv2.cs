using UnityEngine;
using UnityEngine.InputSystem;

public class GunSystemv2 : MonoBehaviour
{
    //Gun stats
    [SerializeField] private int damage;
    [SerializeField] private float timeBetweenShooting, spread, range, reloadTime, timeBetweenBFShots; // timeBetweenShots = Burstfire variable. timeBetweenShooting = Automatic variable.
    [SerializeField] private int magazineSize, bulletsPerTap;
    [SerializeField] private bool allowButtonHold;
    private int bulletsLeft, bulletsShot;
    
    //bools
    private bool isShooting, readyToShoot, reloading;

    //Extra Graphics
    [SerializeField] private GameObject bulletHoleGraphic;
    
    //Reference
    [SerializeField] private Camera fpsCam;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private RaycastHit rayHit;

    private void Awake()
    {
        // Instantiate and turn on controls
        NewControlScheme playerInputActions = new NewControlScheme();
        playerInputActions.Enable();
        
        // Setup Controls here
        // assign function to Shoot Control
        playerInputActions.Player.Shoot.performed += gunTrigger;
        playerInputActions.Player.Shoot.canceled += stopGunTrigger;
        // assign function to Reload Control
        playerInputActions.Player.Reload.performed += reloadAction;

        // Gun stats setup
        bulletsLeft = magazineSize; // fill with ammo
        readyToShoot = true;
        reloading = false;
    }

    private void FixedUpdate()
    {
        if (readyToShoot && isShooting && !reloading && bulletsLeft > 0)
        {
            shoot();
        }
    }

    //Shooting script
    private void shoot()
    {
        if (!allowButtonHold) isShooting = false; // Deactivate trigger if weapon semi-automatic
        readyToShoot = false;   // Deactivate chamber for bullet reload
        
        //Shooting Physics here
        for (int i = 0; i < bulletsPerTap; i++)
        {
            //Spread
            float x = Random.Range(-spread, spread);
            float y = Random.Range(-spread, spread);
            // Calculate Direction with Spread
            Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, x);
            //Raycast bullet end position, use spread to calculate
            if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range))
            {
                if (rayHit.collider.CompareTag("Enemy")) // compare tag here, compare layer there.
                {
                    rayHit.collider.GetComponent<EnemyScript>().TakeDamage(damage); // Damage script here
                }
                else
                {
                    Instantiate(bulletHoleGraphic, rayHit.point + rayHit.normal * .001f, Quaternion.LookRotation(rayHit.normal));
                }
            }
        }
        CMCameraController.Instance.cameraShake(0.5f, 7f,  .135f);
        
        // Graphics
        //Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity); // chuck it in later

        bulletsLeft--;
        Invoke("resetShot", timeBetweenShooting);
    }
    
    //Activate or Deactivate trigger from input
    private void gunTrigger(InputAction.CallbackContext context)
    {
        isShooting = true;
    }
    private void stopGunTrigger(InputAction.CallbackContext context)
    {
        isShooting = false;
    }

    //Reload when given the input
    private void reloadAction(InputAction.CallbackContext context)
    {
        if (bulletsLeft < magazineSize && !reloading)
        {
            reloading = true;
            Invoke("reloadGun", reloadTime);
        }
    }

    private void reloadGun()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }
    
    // Reset the chamber, ready to fire another shot
    private void resetShot()
    {
        readyToShoot = true;
        Debug.Log("readyToShoot: " + readyToShoot);
    }
}
