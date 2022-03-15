using UnityEngine;
using UnityEngine.InputSystem;

public class GunSystem : MonoBehaviour
{
    private PlayerInput playerInput;
    
    //Gun stats
    public int damage;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    private int bulletsLeft, bulletsShot;
    
    //Bools
    private bool shooting, readyToShoot, reloading;
    
    //Reference
    [SerializeField] private Camera fpsCam;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private RaycastHit rayHit;
    [SerializeField] private LayerMask whatIsEnemy;

    private void Awake()
    {
        NewControlScheme playerInputActions = new NewControlScheme();
        playerInputActions.Enable();
        //playerInputActions.Player.Shoot.performed += Shoot(); // just fuck it rn
    }
}
