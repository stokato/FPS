using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    [SerializeField]
    private Text scoreLabel; // Для свойства text
    [SerializeField]
    private SettingsPopUp settingsPopup;

    private int _score;

    void Awake()
    {
        Messenger.AddListener(GameEvent.ENEMY_HIT, OnEnemyHit);
    }

    void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.ENEMY_HIT, OnEnemyHit);
    }

	// Use this for initialization
	void Start () {
        settingsPopup.Close(); // Закрываем всплывающее окно в момент начала игры
	}
	
	// Update is called once per frame
	void Update () {
        //scoreLabel.text = Time.realtimeSinceStartup.ToString();
	}

    public void OnOpenSettings()
    {
        settingsPopup.Open(); // Заменяем отладочный текст методом всплывающего окна
    }

    public void OnPointerDown()
    {
        Debug.Log("pointer down");
    }

    private void OnEnemyHit()
    {
        _score += 1;
        scoreLabel.text = _score.ToString();
    }
}
