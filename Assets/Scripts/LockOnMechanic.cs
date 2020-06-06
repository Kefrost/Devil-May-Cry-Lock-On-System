using Cinemachine;
using UnityEngine;

public class LockOnMechanic : MonoBehaviour
{
    public GameObject model;
    private Animator animator;
    private GameObject enemy;
    private bool isLocked;
    [SerializeField]
    private GameObject castPoint;
    public float radiusOfSphereCast;
    public float sphereCastLenght;
    private GetAllEnemies getAllEnemies;
    private CinemachineTargetGroup cinemachineTargetGroup;
    public GameObject defaultTargetPoint;

    // Start is called before the first frame update
    void Start()
    {
        animator = this.gameObject.GetComponentInChildren<Animator>();
        getAllEnemies = this.gameObject.GetComponentInChildren<GetAllEnemies>();
        cinemachineTargetGroup = GameObject.Find("TargetGroup1").GetComponent<CinemachineTargetGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            FindEnemyInFront();
            if (enemy == null)
            {
                FindEnemyInRange();
            }
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            Debug.Log($"Enemy is null? {enemy == null}");
            LockOn();
        }

        else if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            animator.SetBool("CamLock", false);
            isLocked = false;
            enemy = null;
        }

    }

    private void LockOn()
    {
        isLocked = true;

        animator.SetBool("CamLock", true);

        if (enemy != null)
        {
            SetCameraGroupTarget(enemy);
            LookAtEnemy();
        }
        else
        {
            SetCameraGroupTarget(defaultTargetPoint);
        }
    }

    private void SetCameraGroupTarget(GameObject gameObjectTarget)
    {
        //Set 2 targets in the cinemachine target group component

        CinemachineTargetGroup.Target target;
        target.target = gameObjectTarget.transform;
        target.weight = 1f;
        target.radius = 2f;

        CinemachineTargetGroup.Target player;
        player.target = this.gameObject.transform;
        player.weight = 1.5f;
        player.radius = 0;

        cinemachineTargetGroup.m_Targets.SetValue(player, 0);
        cinemachineTargetGroup.m_Targets.SetValue(target, 1);
    }

    private void LookAtEnemy()
    {
        var temp = enemy.transform.position - model.transform.position;
        Debug.Log($"enemy pos {temp}");

        temp.y = uint.MinValue;

        var lookRotation = Quaternion.LookRotation(temp);
        Debug.Log($"look Rotation {lookRotation}");

        model.transform.rotation = Quaternion.Slerp(model.transform.rotation, lookRotation, 100 * Time.deltaTime);
    }

    private void FindEnemyInFront()
    {
        RaycastHit hit;

        if (Physics.SphereCast(castPoint.transform.position, radiusOfSphereCast, castPoint.transform.forward, out hit, sphereCastLenght))
        {
            if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                enemy = hit.collider.gameObject;
            }
        }
    }

    private void FindEnemyInRange()
    {
        var enemies = getAllEnemies.Enemies;

        if (enemies != null)
        {
            GameObject closestEnemy = null;

            float closestEnemyPos = Mathf.Infinity;

            Vector3 currentPos = this.transform.position;

            foreach (var enemy in enemies)
            {
                Vector3 directionToTarget = enemy.transform.position - currentPos;
                float sqrToTarget = directionToTarget.sqrMagnitude;
                if (sqrToTarget < closestEnemyPos)
                {
                    closestEnemyPos = sqrToTarget;

                    closestEnemy = enemy.gameObject;
                }
            }

            enemy = closestEnemy;
        }
    }

    public bool IsLocked()
    {
        return isLocked;
    }
}
