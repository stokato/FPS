using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
[AddComponentMenu("Control Scripts/FPS Input")]
public class FPSInput : MonoBehaviour {

    private CharacterController _characterController;

    public const float baseSpeed = 6.0f;

    void Awake()
    {
        Messenger<float>.AddListener(GameEvent.SPEED_CHANGED, OnSpeedChanged);
    }

    void OnDestroy()
    {
        Messenger<float>.RemoveListener(GameEvent.SPEED_CHANGED, OnSpeedChanged);
    }

    public float speed = 6.0f;
    public float gravity = -9.8f;

    // Use this for initialization
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float deltaX = Input.GetAxis("Horizontal") * speed;
        float deltaZ = Input.GetAxis("Vertical") * speed;

        Vector3 movement = new Vector3(deltaX, 0, deltaZ);

        movement = Vector3.ClampMagnitude(movement, speed); // Ограничим движение по диагонали той же скоростью, что и движени по осям
        movement.y = gravity;

        movement *= Time.deltaTime;
        movement = transform.TransformDirection(movement); // Преобразуем вектор к глобальным переменным

        _characterController.Move(movement);

        //transform.Translate(deltaX * Time.deltaTime, 0, deltaZ * Time.deltaTime);
    }

    private void OnSpeedChanged(float value)
    {
        speed = baseSpeed * value;
    }
}
