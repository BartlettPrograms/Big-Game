using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class GunSystem : MonoBehaviour
{
    private PlayerInput playerInput;
    
    //Gun stats
    public int damage = 9;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    private int bulletsLeft, bulletsShot;
    
    //Bools
    private bool shooting, readyToShoot, reloading;
    
    //Reference
    [SerializeField] private Camera fpsCam;
    [SerializeField] private Transform attackPoint;
    private RaycastHit rayHit;
    [SerializeField] private LayerMask whatIsEnemy;
    
    //Fancy Extras
    [SerializeField] private GameObject muzzleFlash;
    [SerializeField] private GameObject bulletHoleGraphic;
    //[SerializeField] private GameObject bullet; // Add later... more scripting to do to implement
    
    // GUI
    [SerializeField] private TextMeshProUGUI text;
    

    private void Awake()
    {
        NewControlScheme playerInputActions = new NewControlScheme();
        playerInputActions.Enable();
        playerInputActions.Player.Shoot.performed += GunTrigger;
        playerInputActions.Player.Reload.performed += Reload;

        // Gun stats setup
        bulletsLeft = magazineSize; // fill with ammo
        readyToShoot = true;
    }

    private void Update()
    {
        // Set GUI bullet counter
        text.SetText(bulletsLeft + " / " + magazineSize);
    }
    
    private void GunTrigger(InputAction.CallbackContext context)
    {
        if (context.performed && readyToShoot && !reloading && bulletsLeft > 0)
        {
            //Debug.Log("TriggerPulled");
            bulletsShot = bulletsPerTap;
            Shoot();
            readyToShoot = false;
        }
    }
    
    private void Shoot()
    {
        //Spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);
        CMCameraController.Instance.cameraShake(0.5f, 7f,  .135f);
        // Calculate Direction with Spread
        Vector3 direction = fpsCam.transform.forward + new Vector3(0, x, y);
        //Shooting Physics here
        if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range))
        {
            //Debug.Log(rayHit.collider.name);
            if (rayHit.collider.CompareTag("Enemy")) // compare tag here, compare layer there.
            {
                rayHit.collider.GetComponent<EnemyScript>().TakeDamage(damage); // Damage script here
            }
            else
            {
                Instantiate(bulletHoleGraphic, rayHit.point + rayHit.normal * .001f, Quaternion.LookRotation(rayHit.normal));
            }
        }
        
        // Graphics
        
        Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);

        bulletsLeft--;
        bulletsShot--;
        Invoke("ResetShot", timeBetweenShooting);

        if (bulletsShot > 0 && bulletsLeft > 0)
        {
            Invoke("Shoot", timeBetweenShots);
        }
    }

    private void ResetShot()
    {
        // Debug.Log("Shooting Reset");
        readyToShoot = true;
    }
    
    private void Reload(InputAction.CallbackContext context)
    {
        if (context.performed && bulletsLeft < magazineSize && !reloading)
        {
            //Debug.Log("Reloading");
            // Reload here
            reloading = true;
            Invoke("ReloadFinished", reloadTime);
        }
    }

    private void ReloadFinished()
    {
        //Debug.Log("Reloading Finished");
        bulletsLeft = magazineSize;
        reloading = false;
    }
}
