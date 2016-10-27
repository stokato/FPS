using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class RayShooter : MonoBehaviour {

    private Camera _camera;

	// Use this for initialization
	void Start () {
        _camera = GetComponent<Camera>();

        //Cursor.lockState = CursorLockMode.Locked; // Скрываем указатель мыши
        //Cursor.visible = false;
	}

    void OnGUI()
    {
        int size = 12;

        float posX = _camera.pixelWidth / 2 - size / 4;
        float posY = _camera.pixelHeight * 0.5f - size * 0.5f;

        GUI.Label(new Rect(posX, posY, size, size), "*");
    }
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject()) // Проверка - GUI не используется
        {
            Vector3 point = new Vector3(_camera.pixelWidth * 0.5f, _camera.pixelHeight * 0.5f, 0);

            Ray ray = _camera.ScreenPointToRay(point); ;
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit))
            {
                GameObject hitObject = hit.transform.gameObject; // Объект, в который попал луч
                ReactiveTarget target = hitObject.GetComponent<ReactiveTarget>();

                if(target != null)
                {
                    target.ReactToHit();
                } else
                {
                    StartCoroutine(SphereIndicator(hit.point));  
                }
            }
        }
	}

    private IEnumerator SphereIndicator(Vector3 pos)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        sphere.transform.position = pos;

        yield return new WaitForSeconds(1);

        Destroy(sphere);
    }
}
