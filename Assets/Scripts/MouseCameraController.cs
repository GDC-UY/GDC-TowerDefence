using UnityEngine;
using System.Collections;

public class MouseCameraController : MonoBehaviour
{
    private Camera cam;

    public float boundX;
    public float boundY;

    [Header("Bordes del mapa")]
    [SerializeField] private float MaxY;
    [SerializeField] private float MinY;
    [SerializeField] private float MaxX;
    [SerializeField] private float MinX;

    [Header("Ajuste de camara")]
    [SerializeField] private float mouseSpeed;
    [SerializeField] private float minZoom;
    [SerializeField] private float maxZoom;

    [Header("Movimiento del mouse")]
    [SerializeField] Vector3 hit_position = Vector3.zero;
    [SerializeField] Vector3 current_position = Vector3.zero;
    [SerializeField] Vector3 camera_position = Vector3.zero;
    
    private void Start()
    {
        getBounds();
        cam = this.GetComponent<Camera>();
    }

    private void getBounds()
    {
        boundX = this.GetComponent<Camera>().orthographicSize * Screen.width / Screen.height;
        boundY = this.GetComponent<Camera>().orthographicSize;
    }

    private void mouseWheelFactor(float num)
    {
        if (num == 100)
        {
            MaxY = 6.5f;
            MinY = -5.5f;
            MaxX = 7;
            MinX = -7;
        }
        else if (num == -100)
        {
            MaxY = 17;
            MinY = -16;
            MaxX = 28f;
            MinX = -28f;
        }
        else if (num < 0)
        {
            MaxY = MaxY - 0.5f;
            MinY = MinY + 0.5f;
            
            MaxX = MaxX - 1;
            MinX = MinX + 1;
        }
        
        else if (num > 0)
        {
            MaxY = MaxY + 0.5f;
            MinY = MinY - 0.5f;
            
            MaxX = MaxX + 1;
            MinX = MinX - 1;
        }
    }
    
    void Update()
    {
        Debug.DrawLine(new Vector2(MinX - boundX, MaxY + boundY), new Vector2(MaxX + boundX, MaxY + boundY), Color.red);
        Debug.DrawLine(new Vector2(MaxX + boundX, MinY - boundY), new Vector2(MinX - boundX, MinY - boundY), Color.red);
        Debug.DrawLine(new Vector2(MinX - boundX, MaxY + boundY), new Vector2(MinX - boundX, MinY - boundY), Color.red);
        Debug.DrawLine(new Vector2(MaxX + boundX, MinY - boundY), new Vector2(MaxX + boundX, MaxY + boundY), Color.red);

        Debug.DrawLine(new Vector2(MinX, MaxY), new Vector2(MaxX, MaxY));
        Debug.DrawLine(new Vector2(MaxX, MinY), new Vector2(MinX, MinY));
        Debug.DrawLine(new Vector2(MinX, MaxY), new Vector2(MinX, MinY));
        Debug.DrawLine(new Vector2(MaxX, MinY), new Vector2(MaxX, MaxY));

        if (Game.CameraShouldMove)
        {
            var wheelValue = Input.GetAxis("Mouse ScrollWheel");
        if (wheelValue < 0)
        {
            if (!(cam.orthographicSize >= maxZoom))
            {
                cam.orthographicSize = cam.orthographicSize + mouseSpeed;
                mouseWheelFactor(-1);
                if (cam.orthographicSize >= maxZoom)
                {
                    cam.orthographicSize = maxZoom;
                    mouseWheelFactor(100);
                }
                getBounds();
            }
        }
        else if(wheelValue > 0)
        {
            if(!(cam.orthographicSize <= minZoom))
            {
                cam.orthographicSize = cam.orthographicSize - mouseSpeed;
                mouseWheelFactor(1);
                if (cam.orthographicSize <= minZoom)
                {
                    cam.orthographicSize = minZoom;
                    mouseWheelFactor(-100);
                }
                getBounds();
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            hit_position = Input.mousePosition;
            camera_position = transform.position;

        }
        if (Input.GetMouseButton(0))
        {
            current_position = Input.mousePosition;
            if (this.transform.position.y < MaxY && this.transform.position.y > MinY && this.transform.position.x > MinX && this.transform.position.x < MaxX)
            {
                LeftMouseDrag();
            }
        }

        //-------------------------------------------------------------------------

        if (this.transform.position.y > MaxY || this.transform.position.y < MinY)
        {
            if (this.transform.position.y > 0)
            {
                this.transform.position = new Vector3(this.transform.position.x, MaxY - 0.1f, -10);
            }
            else
            {
                this.transform.position = new Vector3(this.transform.position.x, MinY + 0.1f, -10);
            }
        }

        if (this.transform.position.x > MaxX || this.transform.position.x < MinX)
        {
            if (this.transform.position.x > 0)
            {
                this.transform.position = new Vector3(MaxX - 0.1f, this.transform.position.y, -10);
            }
            else
            {
                this.transform.position = new Vector3(MinX + 0.1f, this.transform.position.y, -10);
            }
        }
        }
    }

    void LeftMouseDrag()
    {
        current_position.z = hit_position.z = camera_position.y;
        Vector3 direction = Camera.main.ScreenToWorldPoint(current_position) - Camera.main.ScreenToWorldPoint(hit_position);
        direction = direction * -1;
        Vector3 position = camera_position + direction;
        transform.position = position;
    }
}