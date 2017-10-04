using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {

    private const float FALL_SPEED = 0.1f;
    private const float SIZE_UP_INCR = 0.5f;
    private const float SIZE_DOWN_INCR = -0.5f;

    public Material plusLifeMat;
    public Material sizeUpMat;
    public Material sizeDownMat;
    public Material magnetMat;

    private int TYPES_NUM = 4;
    public enum PowerUpTypes
    {
        PLUS_LIFE, SIZE_UP, SIZE_DOWN, MAGNET, POWER_BALL
    }

    private PowerUpTypes powerUpType;

    // Use this for initialization
    void Start () {

        // Randomly pick a type
        powerUpType = PickType();

        // Changes texture to match type
        switch (powerUpType)
        {
            case PowerUp.PowerUpTypes.PLUS_LIFE:
                this.GetComponent<Renderer>().material = plusLifeMat;
                break;
            case PowerUp.PowerUpTypes.SIZE_UP:
                this.GetComponent<Renderer>().material = sizeUpMat;
                break;
            case PowerUp.PowerUpTypes.SIZE_DOWN:
                this.GetComponent<Renderer>().material = sizeDownMat;
                break;
            case PowerUp.PowerUpTypes.MAGNET:
                this.GetComponent<Renderer>().material = magnetMat;
                break;
        }
    }
	
	// Floats down towards player
	void Update () {
        Vector3 pos = this.gameObject.transform.position;
        this.gameObject.transform.position = new Vector3(pos.x, pos.y - FALL_SPEED, pos.z);
	}

    // Randomly selects a power up type
    private PowerUpTypes PickType()
    {
        int typeInt = GameManager.instance.RandomInt(0, TYPES_NUM);

        switch (typeInt)
        {
            case 0:
                return PowerUp.PowerUpTypes.PLUS_LIFE;
            case 1:
                return PowerUp.PowerUpTypes.SIZE_UP;
            case 2:
                return PowerUp.PowerUpTypes.SIZE_DOWN;
            case 3:
                return PowerUp.PowerUpTypes.MAGNET;
            default:
                return PowerUp.PowerUpTypes.PLUS_LIFE;
        }
    }

    // Colliding with the paddle grants the effect
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PaddleControl>() != null)
        {
            PaddleControl pc = other.gameObject.GetComponent<PaddleControl>();

            // Activates the power up
            switch (powerUpType)
            {
                case PowerUp.PowerUpTypes.PLUS_LIFE:
                    GameManager.instance.IncrementLife(1);
                    break;
                case PowerUp.PowerUpTypes.SIZE_UP:
                    pc.ScalePaddle(SIZE_UP_INCR);
                    break;
                case PowerUp.PowerUpTypes.SIZE_DOWN:
                    pc.ScalePaddle(SIZE_DOWN_INCR);
                    break;
                case PowerUp.PowerUpTypes.MAGNET:
                    //TODO Magnetise
                    break;
            }

            // Power up disappears
            Destroy(this.gameObject);
        }
    }
}
