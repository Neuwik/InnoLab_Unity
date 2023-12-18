using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Weapon : GameObjectSpawner
{
    [SerializeField]
    private GameObject Player;
    private MouseTracker Mouse;

    void Start()
    {
        Mouse = Player.GetComponent<MouseTracker>();
    }

    protected override void Spawn(GameObject gameObject, Vector3 position)
    {
        Bullet bullet;
        if ((bullet = gameObject.GetComponent("Bullet") as Bullet) != null)
        {
            bullet.target = Mouse.mouseTarget;
            Instantiate(gameObject, position, Quaternion.identity);
        }
        else if((gameObject.GetComponent("AOE") as AOE) != null)
        {
            Instantiate(gameObject, Mouse.mouseTarget, Quaternion.identity);
        }
        else
        {
            base.Spawn(gameObject, position);
        }
    }
}
