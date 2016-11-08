using UnityEngine;
using System.Collections;

public class ScneController : MonoBehaviour {
    [SerializeField]
    private GameObject enemyFrefab; // Сериализуем для связи с префабом
    private GameObject _enemy; // Для слежения за экземпляром врага на сцене

    private float _basicSpeed = 3.0f;
    private float _enemySpeed = 3.0f;

    void Awake()
    {
        Messenger<float>.AddListener(GameEvent.SPEED_CHANGED, OnSpeedChanged);
    }

    void Destroy()
    {
        Messenger<float>.RemoveListener(GameEvent.SPEED_CHANGED, OnSpeedChanged);
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if(_enemy == null)
        {
            _enemy = Instantiate(enemyFrefab) as GameObject; // Копирует префаб
            _enemy.transform.position = new Vector3(0, 1, 0);

            float angle = Random.Range(0, 360);

            _enemy.transform.Rotate(0, angle, 0);

            _enemy.GetComponent<WanderingAI>().speed = _enemySpeed;
        }
	}

    private void OnSpeedChanged(float value)
    {
        _enemySpeed = _basicSpeed * value;
    }
}
