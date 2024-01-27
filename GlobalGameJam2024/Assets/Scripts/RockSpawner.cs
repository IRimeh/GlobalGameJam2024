using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSpawner : MonoBehaviour
{
    public float Radius = 100.0f;
    public float DespawnRadius = 500.0f;

    public List<GameObject> RockPrefabs = new List<GameObject>();
    public Vector3 MinRotation;
    public Vector3 MaxRotation;
    public Vector3 MinScale;
    public Vector3 MaxScale;
    public int AmountOfRocks = 20;
    public float RockMoveSpeed = 1.0f;
    public float RotateSpeed = 10.0f;

    public List<GameObject> Rocks = new List<GameObject>();

    private Vector3 moveDir;
    private Vector3 playerOffset;

    void Start()
    {
        moveDir = transform.forward;
        SpawnInitialRocks();
    }

    private void SpawnInitialRocks()
    {
        for (int i = 0; i < AmountOfRocks; i++)
        {
            float range = Random.Range(Radius, DespawnRadius);
            Vector2 randDir = Random.insideUnitCircle.normalized;
            GameObject rock = Instantiate(RockPrefabs[Random.Range(0, RockPrefabs.Count)],
                                transform.position + new Vector3(randDir.x * range, 0, randDir.y * range),
                                Quaternion.Euler(Random.Range(MinRotation.x, MaxRotation.x), Random.Range(MinRotation.y, MaxRotation.y), Random.Range(MinRotation.z, MaxRotation.z)),
                                transform);
            Rocks.Add(rock);
        }
    }

    void Update()
    {
        RotateShip();
        MoveRocks();
        ReplaceRocks();
    }

    private void MoveRocks()
    {
        playerOffset += transform.InverseTransformDirection(moveDir) * Time.deltaTime * RockMoveSpeed;
        foreach (GameObject rock in Rocks)
        {
            rock.transform.position += moveDir * Time.deltaTime * RockMoveSpeed;
        }
        Shader.SetGlobalVector("_PlayerOffset", playerOffset);
        Shader.SetGlobalVector("_MoveDir", moveDir);
    }

    private void RotateShip()
    {
        if(Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.up * Time.deltaTime * RotateSpeed);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up * Time.deltaTime * -RotateSpeed);
        }
    }

    private void ReplaceRocks()
    {
        foreach (GameObject rock in Rocks)
        {
            if(Vector3.Distance(rock.transform.position, transform.position) > DespawnRadius)
            {
                float range = Random.Range(Radius, DespawnRadius);
                Vector2 randDir = Random.insideUnitCircle.normalized;
                rock.transform.position = transform.position + new Vector3(randDir.x * range, 0, randDir.y * range);
                rock.transform.rotation = Quaternion.Euler(Random.Range(MinRotation.x, MaxRotation.x), Random.Range(MinRotation.y, MaxRotation.y), Random.Range(MinRotation.z, MaxRotation.z));
            }
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1.0f, 0.1f, 0.6f);
        Gizmos.DrawWireSphere(transform.position, Radius);

        Gizmos.color = new Color(1.0f, 0.1f, 0.1f);
        Gizmos.DrawWireSphere(transform.position, DespawnRadius);
    }
}