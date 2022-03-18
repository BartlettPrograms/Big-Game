using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSwitching : MonoBehaviour
{
    private PlayerInput playerInput;
    
    public int selectedWeapon = 0;

    private void Awake()
    {
        NewControlScheme playerInputActions = new NewControlScheme();
        playerInputActions.Player.SelectWeapon1.performed += selectWeapon1;
        playerInputActions.Player.SelectWeapon2.performed += selectWeapon2;
        playerInputActions.Player.SelectWeapon3.performed += selectWeapon3;
    }

    void Start()
    {
        selectWeapon();
    }
    
    void Update()
    {
        
    }

    void selectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
                weapon.gameObject.SetActive(true);
            else
            {
                weapon.gameObject.SetActive(false);
            }
            i++;
        }
    }

    private void selectWeapon1(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            selectedWeapon = 1;
        }
    }
    private void selectWeapon2(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            selectedWeapon = 2;
        }
    }
    private void selectWeapon3(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            selectedWeapon = 3;
        }
    }
}
