using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

public class MouseCameraController : MonoBehaviour
{
    public Camera cam;
    [SerializeField] private GridManager gm;

    [Header("Mapa objeto")]
    Vector2[] vertices;

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

    [FormerlySerializedAs("hit_position")]
    [Header("Movimiento del mouse")]
    [SerializeField] Vector3 hitPosition = Vector3.zero;
    [SerializeField] Vector3 currentPosition = Vector3.zero;
    [SerializeField] Vector3 cameraPosition = Vector3.zero;
    
    private void Start()
    {
        GetBounds();
    }

    private void GetBounds()
    {
        boundY = cam.orthographicSize;
        boundX = boundY * Screen.width / Screen.height;
        
        MaxY = (gm.Width/2) - boundY;
        MinY = -(gm.Width/2) + boundY;
        MaxX = gm.Height/2 - boundX;
        MinX = -(gm.Width/2) + boundX;
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

        var wheelValue = Input.GetAxis("Mouse ScrollWheel");
        if (wheelValue < 0)
        {
            if (!(cam.orthographicSize >= maxZoom))
            {
                cam.orthographicSize = cam.orthographicSize + mouseSpeed;
                if (cam.orthographicSize >= maxZoom)
                {
                    cam.orthographicSize = maxZoom;
                }
                GetBounds();
            }
        }
        else if(wheelValue > 0)
        {
            if(!(cam.orthographicSize <= minZoom))
            {
                cam.orthographicSize = cam.orthographicSize - mouseSpeed;
                if (cam.orthographicSize <= minZoom)
                {
                    cam.orthographicSize = minZoom;
                }
                GetBounds();
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            hitPosition = Input.mousePosition;
            cameraPosition = transform.position;

        }
        if (Input.GetMouseButton(0))
        {
            currentPosition = Input.mousePosition;
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

    void LeftMouseDrag()
    {
        currentPosition.z = hitPosition.z = cameraPosition.y;
        Vector3 direction = Camera.main.ScreenToWorldPoint(currentPosition) - Camera.main.ScreenToWorldPoint(hitPosition);
        direction = direction * -1;
        Vector3 position = cameraPosition + direction;
        transform.position = position;
    }
}