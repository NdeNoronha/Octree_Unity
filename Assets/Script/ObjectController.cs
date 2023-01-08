using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ObjectController : MonoBehaviour
{

    [SerializeField][Range(0, 10)] private int speed = 2;
    private Vector3 direction;
    private SphereCollider sphereCollider;
    public Bounds bound => sphereCollider.bounds;
    public float radius => sphereCollider.radius;
    
    public int ID { get => id; set => id = value; }

    [SerializeField] private int id = -1;

    public float RandomWorlPoint => Random.Range(
        -SimulationManager.instance.WorldSize + bound.size.magnitude,
        SimulationManager.instance.WorldSize - bound.size.magnitude
     );
    public Vector3 RandomWorldPosition => new Vector3(RandomWorlPoint, RandomWorlPoint, RandomWorlPoint);
    private void Awake()
    {
        sphereCollider = gameObject.GetComponent<SphereCollider>();

    }
    private void Start()
    {
        //randomize movement on start
        transform.position = RandomWorldPosition;
        direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
    }
    private void FixedUpdate()
    {
        OutsideBoundary(transform.position);
        transform.position += direction * speed * Time.fixedDeltaTime;
    }
    //restrict object's movement to only world's bounding Box
    private void OutsideBoundary(Vector3 targetPosition)
    {
        Vector3 boundarySimulationVector = Vector3.one * SimulationManager.instance.WorldSize;
        if (targetPosition.x + radius > boundarySimulationVector.x) direction.x *= -1;
        if (targetPosition.y + radius > boundarySimulationVector.y) direction.y *= -1;
        if (targetPosition.z + radius > boundarySimulationVector.z) direction.z *= -1;
        if (targetPosition.x + -radius < -boundarySimulationVector.x) direction.x *= -1;
        if (targetPosition.y + -radius < -boundarySimulationVector.y) direction.y *= -1;
        if (targetPosition.z + -radius < -boundarySimulationVector.z) direction.z *= -1;
    }
    private void SetDirection(Vector3 target)
    {
        direction = target;
    }
    //Inverting object direction of motion on collision
    public void OnCollide(Bounds collisionBounds) {

        Vector3  targetDirection =  -(collisionBounds.center - bound.center);
        SetDirection(targetDirection);
    }


}
