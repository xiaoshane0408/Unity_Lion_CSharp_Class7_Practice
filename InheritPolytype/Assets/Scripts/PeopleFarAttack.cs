using System.Linq;
using UnityEngine;

public class PeopleFarAttack : PeopleTrack
{
    [Header("停止距離"), Range(1, 10)]
    public float stop = 5f;

    [Header("子彈")]
    public GameObject bullet;

    [Header("冷卻時間"), Range(0.1f, 3f)]
    public float cd = 1.5f;

    private float timer;

    public PeopleTrack[] zoombie;
    public float[] dis;

    protected override void Start()
    {
        zoombie = FindObjectsOfType<PeopleTrack>();
        dis = new float[zoombie.Length];
        agent.stoppingDistance = stop;                  // 代理器.停止距離 = 停止距離
        agent.SetDestination(Vector3.zero);
        //target = GameObject.Find("殭屍").transform;     // 目標 = 殭屍 
    }

    protected override void Track()
    {
        for (int i = 0; i < zoombie.Length; i++)
        {
            if (zoombie[i] == null || zoombie[i].transform.tag == "警察")
            {
                // 如果 殭屍死亡 距離 改成 1000
                if (zoombie[i] == null)
                {
                    dis[i] = 1000;
                }
                else
                {
                    dis[i] = 999;  // 與殭屍物件的距離改為999
                }
                continue;           // 繼續 - 跳過並執行下一次迴圈
            }
            dis[i] = Vector3.Distance(transform.position, zoombie[i].transform.position);
        }
        // 判斷最近
        float min = dis.Min();                 // 最小值 = 距離.最小值
        int index = dis.ToList().IndexOf(min); // 索引值 = 距離.轉清單.取得索引值(最小值)
        target = zoombie[index].transform;           // 目標 = 殭屍[索引值].變形
        // 追蹤最近目標
        // 如果 目標 為 空值 跳出
        if (target == null || dis[index] == 999) return;
        agent.SetDestination(target.position);
        // 變形.看著(目標)
        transform.LookAt(target);
        // 如果 代理器.距離 <= 停止距離 就 攻擊
        if(agent.remainingDistance <= stop)
        {
            Attack();
        }
    }

    private void Attack()
    {
        // 計時器 累加 時間
        timer += Time.deltaTime;

        if(timer >= cd)
        {
            // 計時器歸零
            timer = 0;
            // 攻擊動畫
            ani.SetTrigger("攻擊");
            // 生成子彈
            GameObject temp = Instantiate(bullet, transform.position + transform.forward + transform.up, transform.rotation);
            // 添加元件
            Rigidbody rig = temp.AddComponent<Rigidbody>();
            // 子彈添加推力
            rig.useGravity = false;
            rig.AddForce(transform.forward * 1500);
            Destroy(temp, 1.5f);
        }
    }
}
