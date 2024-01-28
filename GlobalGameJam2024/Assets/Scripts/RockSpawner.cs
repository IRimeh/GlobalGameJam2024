using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSpawner : MonoBehaviour
{
    public GameObject Player;
    public Camera ShipCamera;
    public bool IsControllingShip = true;
    public float Radius = 100.0f;
    public float DespawnRadius = 500.0f;
    public float InitialSpawnRadius = 500.0f;

    public List<GameObject> RockPrefabs = new List<GameObject>();
    public Vector3 MinRotation;
    public Vector3 MaxRotation;
    public Vector3 MinScale;
    public Vector3 MaxScale;
    public Vector3 Offset = new Vector3(0, -10, 0);
    public int AmountOfRocks = 20;
    public float RockMoveSpeed = 1.0f;
    public float RotateSpeed = 10.0f;
    public float RotateAcceleration = 1.0f;

    [Header("Ship Swaying")]
    public Transform ShipTransform;
    public Transform SwayTransform;
    public float SwayAmount;
    public float SwaySpeed;

    public Transform Wheel;
    public float WheelRotationSpeed = 10.0f;

    public List<GameObject> Rocks = new List<GameObject>();

    private Vector3 moveDir;
    private Vector3 playerOffset;
    private Quaternion defaultSwayTransformRot;
    private Quaternion defaultCameraRot;
    private bool canStopControllingShip = false;

    private float _currentRotateSpeed = 0.0f;
    private float _targetRotateSpeed = 0.0f;

    [Header("Here ya fukin go daniel, happy now eh?")]
    public List<GameObject> ObjsToMoveWithBoat = new List<GameObject>();

    void Start()
    {
        defaultSwayTransformRot = SwayTransform.rotation;
        defaultCameraRot = ShipCamera.transform.rotation;
        moveDir = transform.forward;
        SpawnInitialRocks();
    }

    private void SpawnInitialRocks()
    {
        for (int i = 0; i < AmountOfRocks; i++)
        {
            float range = Random.Range(InitialSpawnRadius, DespawnRadius);
            Vector2 randDir = Random.insideUnitCircle.normalized;
            GameObject rock = Instantiate(RockPrefabs[Random.Range(0, RockPrefabs.Count)],
                                transform.position + (transform.right * randDir.x * range) + (transform.forward * randDir.y * range) + Offset,// new Vector3(, 0, randDir.y * range),
                                Quaternion.Euler(Random.Range(MinRotation.x, MaxRotation.x), Random.Range(MinRotation.y, MaxRotation.y), Random.Range(MinRotation.z, MaxRotation.z)),
                                transform);
            Rocks.Add(rock);
            rock.GetComponent<Rock>().RockSpawner = this;
        }
    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.F)) && IsControllingShip && canStopControllingShip)
        {
            ControlShip(false);
            canStopControllingShip = false;
        }

        RotateShip();
        MoveRocks();
        MoveOtherObjs();
        ReplaceRocks();
        SwayShip();
        SwayCamera();
    }

    public void ControlShip(bool doControl)
    {
        canStopControllingShip = false;
        IsControllingShip = doControl;

        ShipCamera.gameObject.SetActive(doControl);
        Player.gameObject.SetActive(!doControl);

        if(doControl)
        {
            StartCoroutine(SetCanStopControlling());
            IEnumerator SetCanStopControlling()
            {
                yield return new WaitForSeconds(0.1f);
                canStopControllingShip = true;
            }
        }
    }

    private void SwayShip()
    {
        //SwayTransform.rotation = Quaternion.Euler(defaultSwayTransformRot.eulerAngles.x, defaultSwayTransformRot.eulerAngles.y, Mathf.Sin(Time.time * SwaySpeed) * SwayAmount);
    }

    private void SwayCamera()
    {
        if(IsControllingShip)
        {
            ShipCamera.transform.rotation = Quaternion.Euler(defaultCameraRot.eulerAngles.x, defaultCameraRot.eulerAngles.y, Mathf.Sin(Time.time * SwaySpeed) * SwayAmount);
        }
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

    private void MoveOtherObjs()
    {
        foreach (GameObject obj in ObjsToMoveWithBoat)
        {
            obj.transform.position += moveDir * Time.deltaTime * RockMoveSpeed;
        }
    }

    private void RotateShip()
    {
        transform.Rotate(Vector3.up * Time.deltaTime * _currentRotateSpeed);
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
            _currentRotateSpeed = Mathf.Lerp(_currentRotateSpeed, 0.0f, Time.deltaTime * RotateAcceleration);

        if (!IsControllingShip)
            return;

        _targetRotateSpeed = 0.0f;
        if(Input.GetKey(KeyCode.A))
        {
            Wheel.transform.Rotate(Vector3.right, Time.deltaTime * WheelRotationSpeed);
            _targetRotateSpeed -= RotateSpeed;
        }

        if (Input.GetKey(KeyCode.D))
        {
            Wheel.transform.Rotate(Vector3.right, Time.deltaTime * -WheelRotationSpeed);
            _targetRotateSpeed += RotateSpeed;
        }

        _currentRotateSpeed = Mathf.Lerp(_currentRotateSpeed, _targetRotateSpeed, Time.deltaTime * RotateAcceleration);
    }

    private void ReplaceRocks()
    {
        foreach (GameObject rock in Rocks)
        {
            if(Vector3.Distance(rock.transform.position, transform.position) > DespawnRadius)
            {
                float range = Random.Range(Radius, DespawnRadius);
                Vector2 randDir = Random.insideUnitCircle.normalized;
                rock.transform.position = transform.position + new Vector3(randDir.x * range, 0, randDir.y * range) + Offset;
                rock.transform.rotation = Quaternion.Euler(Random.Range(MinRotation.x, MaxRotation.x), Random.Range(MinRotation.y, MaxRotation.y), Random.Range(MinRotation.z, MaxRotation.z));
            }
        }
    }

    public void ReplaceRock(Rock rock)
    {
        float range = Random.Range(Radius, DespawnRadius);
        Vector2 randDir = Random.insideUnitCircle.normalized;
        rock.transform.position = transform.position + new Vector3(randDir.x * range, 0, randDir.y * range) + Offset;
        rock.transform.rotation = Quaternion.Euler(Random.Range(MinRotation.x, MaxRotation.x), Random.Range(MinRotation.y, MaxRotation.y), Random.Range(MinRotation.z, MaxRotation.z));
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1.0f, 0.1f, 0.6f);
        Gizmos.DrawWireSphere(transform.position, Radius);

        Gizmos.color = new Color(1.0f, 0.1f, 0.1f);
        Gizmos.DrawWireSphere(transform.position, DespawnRadius);
    }
}
