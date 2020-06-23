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

    protected override void Start()
    {
        agent.stoppingDistance = stop;                  // 代理器.停止距離 = 停止距離
        target = GameObject.Find("殭屍").transform;     // 目標 = 殭屍 
    }

    protected override void Track()
    {
        // 如果 目標 為 空值 跳出
        if (target == null) return;
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
