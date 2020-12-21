using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Mouse : MonoBehaviour
{
    static Mouse _instance;
    public static Mouse Instance
    {
        get
        {
            return _instance;
        }
    }

    public Texture2D dog, wall, bone,def;
    public Material[] mat;//0.dog,1.wall,2.bone,3.def 
    public GameObject DogObject = null;
    public GameObject BoneObject = null;
    private GameObject[] WallsObject=new GameObject[20];
    public Kind kind=Kind.Nothing;

    public Button[] buttons;
   void Awake()
    {
        _instance = this;
    }


    private void SetCursorDog()
    {
        Cursor.SetCursor(dog, Vector2.zero, CursorMode.Auto);
    }


    private void SetCursorWall()
    {
        Cursor.SetCursor(wall, Vector2.zero, CursorMode.Auto);
    }


    private void SetCursorBone()
    {
        Cursor.SetCursor(bone, Vector2.zero, CursorMode.Auto);
    }


    private void SetCursorDef()
    {
        Cursor.SetCursor(def, Vector2.zero, CursorMode.Auto);
    }

    public void setdog()
    {
        if (DogObject != null)
        {
            setTexture(DogObject, Kind.Nothing);
        }
        SetCursorDog();
        kind = Kind.dog;
    }
    public void setBone()
    {
        if (BoneObject != null)
        {
            setTexture(BoneObject, Kind.Nothing);
        }
        SetCursorBone();
        kind = Kind.bone;
    }
    public void setWall()
    {
        if (kind == Kind.wall)
        {
            SetCursorDef();
            kind = Kind.Nothing;
            return;
        }
        SetCursorWall();
        kind = Kind.wall;
    }

    private void setTexture(GameObject gameObject,Kind kind)
    {
        //if (gameObject.tag != "Cube")
        //{ print("cube不对劲");return; }
        if (DogObject == gameObject)
        {
            DogObject = null;
        }else if (BoneObject == gameObject)
        {
            BoneObject = null;
        }
        gameObject.GetComponent<label1>().kind = kind;
        
        switch (kind)
        {
            case Kind.dog:
                gameObject.GetComponent<MeshRenderer>().material = mat[0];
                DogObject = gameObject;
                break;
            case Kind.wall:
                gameObject.GetComponent<MeshRenderer>().material = mat[1];
                
                break;
            case Kind.bone:
                gameObject.GetComponent<MeshRenderer>().material=mat[2];
                BoneObject = gameObject;
                break;
            case Kind.Nothing:
                gameObject.GetComponent<MeshRenderer>().material = mat[3];
                break;
            default:print("错误?");
                break;
        }
    }

    public void ClearAll()
    {
        DogObject = null;
        WallsObject = null;
        BoneObject = null;
        foreach(var i in Manager.Cubes)
        {
            setTexture(i, Kind.Nothing);
        }
        SetCursorDef();

        if (!this.GetComponent<A_Star>().isReadly)
        {
            this.GetComponent<A_Star>().onClearButtonCilck();
        }
    }    

    private void Update()
    {//点击放置图标
        if (Input.GetMouseButtonDown(0)&&kind!=Kind.Nothing)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction, Color.red);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("检测到物体");
                if (hit.collider.tag == "Cube")
                {
                    setTexture(hit.collider.gameObject, kind);
                    if (kind != Kind.wall)
                    {
                        SetCursorDef();
                        kind = Kind.Nothing;
                    }
                }
            }
        }
        //删除标记点
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction, Color.red);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("检测到物体");
                if (hit.collider.tag == "Cube")
                {
                    Kind temp = hit.collider.GetComponent<label1>().kind;
                    if (temp == Kind.bone)
                    {
                        BoneObject = null;
                    }
                    else if (temp == Kind.dog)
                    {
                        DogObject = null;
                    }
                    setTexture(hit.collider.gameObject, Kind.Nothing);
                    SetCursorDef();
                    kind = Kind.Nothing;
                }
            }
        }
    }
}
