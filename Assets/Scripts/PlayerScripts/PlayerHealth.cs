using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private int health = 100;
    [SerializeField] int armour = 10;

    public void takeDamage(int damage)
    {
        health = health - (damage - armour);
        Debug.Log(("Oof!  - health = " + health));
    }

    public void resetHealth()
    {
        health = 100;
    }
}
