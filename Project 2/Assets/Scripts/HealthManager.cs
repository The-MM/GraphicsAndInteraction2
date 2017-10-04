using UnityEngine;
using System.Collections;

public class HealthManager : MonoBehaviour {

    public const int RECTANGLE_HEALTH = 1;
    public const int TRIANGLE_HEALTH = 2;
    public const int CIRCLE_HEALTH = 3;

    private int startingHealth;
    private int currentHealth;

	// Use this for initialization
	void Start () {

        // Sets starting health based on brick type
        switch (this.gameObject.GetComponent<Brick>().brickType)
        {
            case GameManager.BrickTypes.RECTANGLE:
                startingHealth = RECTANGLE_HEALTH;
                break;
            case GameManager.BrickTypes.TRIANGLE:
                startingHealth = TRIANGLE_HEALTH;
                break;
            case GameManager.BrickTypes.CIRCLE:
                startingHealth = CIRCLE_HEALTH;
                break;
        }

        this.ResetHealthToStarting();
	}

    // Reset health to original starting health
    public void ResetHealthToStarting()
    {
        currentHealth = startingHealth;
    }

    // Reduce the health of the object by a certain amount
    // If health lte zero, destroy the object
    public void ApplyDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            // Tell the GameManager the brick has been destroyed
            GameManager.instance.DestroyedBrick(this.GetComponent<Brick>());

            // Remove the brick and spawn death particles
            Destroy(this.gameObject);
            GameObject obj = Instantiate(this.GetComponent<Brick>().createOnDestroy);
            obj.transform.position = this.transform.position;
        }
    }

    // Get the current health of the object
    public int GetHealth()
    {
        return this.currentHealth;
    }
    
}
