using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class A_Star : MonoBehaviour
{
    private readonly GameObject[,] Map=new GameObject[13,13];
    private Mouse mouse ;
    private Point Player;
    private Point Target;

    public bool isReadly = true;
    private List<Point> Open_List=new List<Point>(); //
    private List<Point> Close_List=new List<Point>();  //

    private void Awake()
    {
        mouse = this.GetComponent<Mouse>();
        最短路径 = new Point[100];
        Point temp1 = new Point(0, 0, Kind.Nothing, new GameObject());
        for (int i = 0; i < 100; i++)
        {
            最短路径[i] = temp1;
        }

    }
    public void OnStartClick()
    {//初始化把所有方块加入map中，取出小狗和目标的坐标
        if (mouse.DogObject != null && mouse.BoneObject != null)
        {
            foreach (var i in Manager.Cubes)
            {
                label1 temp = i.GetComponent<label1>();
                Map[temp.X-1, temp.Y-1] = i;
            }
            Player = new Point(mouse.DogObject.GetComponent<label1>().X, mouse.DogObject.GetComponent<label1>().Y, mouse.kind, mouse.gameObject);
            Target = new Point(mouse.BoneObject.GetComponent<label1>().X, mouse.BoneObject.GetComponent<label1>().Y, mouse.kind, mouse.gameObject);
            print("玩家坐标X"+ Player.X + ",Y"+ Player.Y + ";目标坐标X"+ Target.X + ",Y"+ Target.Y + "");


            StartCoroutine(A星算法核心());
            isReadly = false;
        }
        else
            return;

    }
    public void onClearButtonCilck()
    {
        Player = null;
        Target = null;
        Open_List = new List<Point>();
        Close_List = new List<Point>();
        isReadly = true;
    }
    private Point[] 最短路径;
    private int index;//最短队列索引
    private void 检查附近点(Point point)
    {
       
    }

    private int 计算H(Point my,Point target)
    {
        return Mathf.Abs(my.X - target.X) + Mathf.Abs(my.Y - target.Y);
    }
    private int 计算G(Point my)
    {
        return my.father == null ? my.G + 1 : 0;
    }
    private Point 从开表中取得F最小的点()
    {
        int min_F = Open_List[0].F;
        Point min_point= Open_List[0];
        foreach(var i in Open_List)
        {
            if (i.F < min_F)
            {
                min_point = i;
                min_F = i.F;//这一条忘了加上然后变成了转圈圈了。
            }
        }
        return min_point;
    }
    private bool 某点存在列表(int X,int Y,List<Point> points)
    {
        foreach(var i in points)
        {
            if (i.X == X && i.Y == Y)
            {
                return true;
            }
        }
     //   print("该点不存在");
        return false;
    }
    private Point 得到某点在列表(int X, int Y, List<Point> points)
    {
        foreach (var i in points)
        {
            if (i.X == X && i.Y == Y)
            {
                return i;
            }
        }
        print("该点不存在");
        return null;
    }
    private bool IsInMap(int x,int y)//该点是否越界
    {
        if(x>13|| x< 1)
        {
            return false;
        }
        if (y > 13 || y < 1)
        {
            return false;
        }
        return true;
    }
    private bool 对比(Point x,Point y)
    {
        if (x.X == y.X && x.Y == y.Y)
        {
            return true;
        }
        return false;
    }
    IEnumerator  A星算法核心()
    {
        bool find = false;
        Open_List.Add(Player);
        Point EndPoint=new Point();
        while (Open_List.Count > 0)
        {
            Point min_point=从开表中取得F最小的点();
            Open_List.Remove(min_point);
            Close_List.Add(min_point);
            Point point = min_point;
            for (int a = -1; a <= 1; a++)
            {
                for (int b = -1; b <= 1; b++)
                {
                    if (IsInMap(point.X + a, point.Y + b) && !(a == 0 && b == 0))//检查是否越界，或者是中心点
                    {
                        Point temp = new Point(point.X + a , point.Y + b, Map[point.X + a-1, point.Y + b-1].GetComponent<label1>().kind, Map[point.X + a-1, point.Y + b-1]);//初始化该点数据
                        if (temp.K == Kind.wall || 某点存在列表(temp.X, temp.Y, Close_List))//当该店为障碍物则或者在close表中跳过
                            continue;
                        if (!某点存在列表(temp.X, temp.Y, Open_List))
                        {
                            temp.father = point;
                            temp.G = 计算G(point);
                            temp.H = 计算H(temp, Target);
                            temp.F = temp.G + temp.H;
                            Open_List.Add(temp);
                        }
                        else
                        {
                            Point x = 得到某点在列表(temp.X, temp.Y, Open_List);
                            if (point.G < x.G)//如果从当前处理顶点到该顶点使得G更小
                            {
                                x.father = point;
                                x.G = point.G + 1;
                                x.F = x.G + x.H;
                            }
                        }
                        高亮选中方块(Open_List, Color.blue);
                        高亮选中方块(Close_List, Color.red);
                        yield return new WaitForSeconds(0.25f);
                    }
                }
            }
            if (对比(min_point,Target))
            {
                print("找到了目标！");
                find = true;
                EndPoint = min_point;
                break;
            }
        }
        最短路径[0] = EndPoint;
        for(int x = 0; x < 1000; x++)
        {
            if (最短路径[x].father != null)
            {
                最短路径[x+1] = 最短路径[x].father;
            }
            else
            {
                break;
            }
        }
        高亮选中方块(最短路径, Color.green);
        for(int num = 0; num < 最短路径.Length; num++)
        {
            Console.WriteLine("第"+ num + "个点X"+ 最短路径[num].X + ",Y"+ 最短路径[num].Y);
        }
    }

    private void 高亮选中方块(List<Point> points, Color color)//openlist中为蓝色,closelist为红色,最终结果为绿色
    {
        foreach(var i in points)
        {
            if(i.GO.tag=="Cube")
            i.GO.GetComponent<MeshRenderer>().material.color = color;
        }
    }

    private void 高亮选中方块(Point[] points, Color color)//openlist中为蓝色,closelist为红色,最终结果为绿色
    {
        foreach (var i in points)
        {
            if (i.GO.tag == "Cube")
                i.GO.GetComponent<MeshRenderer>().material.color = color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}

public class Point
{
    public int X;
    public int Y;
    public int G;
    public int H;
    public int F;
    public Kind K;
    public GameObject GO;
    public Point father;//该点的父亲节点
    public Point(int x,int y,int g,int h,Kind k,GameObject go)
    {
        X = x;Y = y;G = g;H = h;K = k;GO = go;father = null;
    }
    public Point() {}
    public Point(int x,int y,Kind k,GameObject go) { X = x; Y = y; K = k; GO = go;father = null; }
}