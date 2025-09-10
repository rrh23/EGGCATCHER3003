using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;

public enum EggType
{
    Basic,
    Healing,
    Mine,
    Void
}

public class EggScript : MonoBehaviour
{
    public LogicScript logic;
    public EggType eggType;

    private void Start()
    {
        //not touching with border
        GameObject borders = GameObject.FindWithTag("Borders");
        if (borders != null)
        {
            Collider2D bordersCollider = borders.GetComponent<Collider2D>();
            Collider2D thisCollider = GetComponent<Collider2D>();

            // Ignore collision between this GameObject and Borders
            if (bordersCollider != null && thisCollider != null)
            {
                Physics2D.IgnoreCollision(thisCollider, bordersCollider);
            }
        }


        logic = GameObject.FindWithTag("Logic").GetComponent<LogicScript>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Catcher" && !logic.isGameOver)
        {
            logic.CatchedEgg(eggType);
            //Debug.Log("Egg has collided with Catcher");
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Abyss" && !logic.isGameOver)
        {
            logic.MissedEgg(eggType);
            Destroy(gameObject);
        }
    }

}
