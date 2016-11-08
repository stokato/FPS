using UnityEngine;
using System.Collections;

public class WanderingAI : MonoBehaviour {

    public const float baseSpeed = 3.0f;

    [SerializeField]
    private GameObject fireballPrefab;
    private GameObject _fireball;

    private bool _alive;

    public float speed = 3.0f;
    public float obstackeRange = 5.0f;
	
    void Awake()
    {
        Messenger<float>.AddListener(GameEvent.SPEED_CHANGED, OnSpeedChanged);
    }

    void OnDestroy()
    {
        Messenger<float>.RemoveListener(GameEvent.SPEED_CHANGED, OnSpeedChanged);
    }

    void Start()
    {
        _alive = true; 
    }

	// Update is called once per frame
	void Update () {
        if (_alive)
        {
            transform.Translate(0, 0, speed * Time.deltaTime); // Непрерывно движимся в каждом кадре

            Ray ray = new Ray(transform.position, transform.forward); // Луч находится в том же положении и нацеливается в том же напраавлении
                                                                      // что и персонаж
            RaycastHit hit;

            if (Physics.SphereCast(ray, 0.75f, out hit)) // Бросаем луч с описанной вокруг него окружностью
            {
                GameObject hitObject = hit.transform.gameObject;

                Debug.Log(hitObject.name);

                if(hitObject.GetComponent<PlayerCharacter>())
                {
                    if(_fireball == null)
                    {
                        _fireball = Instantiate(fireballPrefab) as GameObject;
                        _fireball.transform.position = transform.TransformPoint(Vector3.forward * 1.5f);
                        _fireball.transform.rotation = transform.rotation;
                    }
                }
                else if (hit.distance < obstackeRange)
                {
                    float angle = Random.Range(-110, 110); // Поворот  с наполовину случайным выбором нового направления
                    transform.Rotate(0, angle, 0);
                }
            }
        }
  	}

    public void SetAlive(bool alive)
    {
        _alive = alive;
    }

    private void OnSpeedChanged(float value)
    {
        speed = baseSpeed * value;
    }
}


