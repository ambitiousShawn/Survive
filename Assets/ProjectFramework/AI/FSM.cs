using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Shawn.ProjectFramework
{
    /// <summary>
    /// 有限状态机管理模块：
    ///     1.提供状态类的公共接口
    ///     2.提供存储所有状态类的容器
    ///     3.提供对外开放的状态改变API
    ///     4.存储当前状态信息
    /// </summary>
    /// 

    ///基础状态的接口
    public interface IState
    {
        void Enter();

        void Stay();

        void Exit();
    }

    [Serializable]
    public class Parameter
    {
        /*
         存储使用状态机的物体所需要的属性信息
         */
        // 控制移动速度
        public float speed = 5f;
        // 初始速度
        public float originalSpeed = 5f;
        // 转向速度
        public float turnSpeed = 180f;

        // 玩家
        public GameObject player;
        // 敌人
        public GameObject enemy;
        // 存储巡逻路线
        public Transform[] waypoints;

        // 当前巡逻路径下标
        public int waypointIndex = 0;
        // 近战检测范围
        public float radius = 2f;
        // 伤害
        public float damageRatio = 0.3f;
        // 攻击间隔
        public float duration = 5f;

        public Vector3 relative;
        // 计时器
        public float count;
    }

    public class FSM : MonoBehaviour 
    {
        public enum E_StateType
        {
            Idle,
            Walk,
            Attack,
            Patrol
        }

        protected Dictionary<E_StateType, IState> states = new Dictionary<E_StateType, IState>();

        private E_StateType currState;
        public Parameter parameter;

        private void Start()
        {
            InitFSM();
            InitState(E_StateType.Idle);
        }

        private void Update()
        {
            parameter.relative = parameter.player.transform.position - parameter.enemy.transform.position;
            states[currState]?.Stay();
        }

        /// <summary>
        /// 初始化状态机
        /// </summary>
        private void InitFSM()
        {
            states.Add(E_StateType.Idle, new IdleState(parameter, this));
            states.Add(E_StateType.Walk, new WalkState(parameter, this));
            states.Add(E_StateType.Attack, new AttackState(parameter, this));
            states.Add(E_StateType.Patrol, new PatrolState(parameter, this));
        }

        private void InitState(E_StateType type)
        {
            TransitionState(type);
        }

        public void TransitionState(E_StateType state)
        {
            states[currState]?.Exit();
            currState = state;
            states[currState].Enter(); 
        }
    }

    #region 此处写每个游戏物体对应的状态类
    class IdleState : IState
    {
        public Parameter parameter;
        public FSM fsm;

        public IdleState(Parameter parameter, FSM fsm)
        {
            this.parameter = parameter;
            this.fsm = fsm;
        }

        public void Enter()
        {
            // 玩家较远时默认状态
            parameter.enemy.GetComponent<Animator>().SetFloat("Speed", 0f);
        }

        public void Exit()
        {
            parameter.enemy.GetComponent<Animator>().SetTrigger("IdleBreak");
        }

        public void Stay()
        {
            if (parameter.relative.magnitude < 10 * parameter.radius)
            {
                if (parameter.count < 1f)
                {
                    parameter.count = 1f;
                }
                else
                {
                    fsm.TransitionState(FSM.E_StateType.Patrol);
                    parameter.count = 0f;
                }
            }
        }
    }

    class WalkState : IState
    {
        public Parameter parameter;
        public FSM fsm;

        public WalkState(Parameter parameter, FSM fsm)
        {
            this.parameter = parameter;
            this.fsm = fsm;
        }

        public void Enter()
        {
            parameter.enemy.GetComponent<Animator>().SetFloat("Speed", parameter.speed);
        }

        public void Exit()
        {
            parameter.enemy.GetComponent<Animator>().SetFloat("Speed", 0f);
        }

        public void Stay()
        {
            Move();
            // 进入载具或者隐藏
            if (parameter.player.GetComponent<PlayerController>().isHide || !parameter.player.GetComponent<PlayerController>().isMoving)
            {
                fsm.TransitionState(FSM.E_StateType.Patrol);
            }
        }

        // 靠近
        private void Move()
        {
            parameter.enemy.transform.position = Vector3.MoveTowards(parameter.enemy.transform.position, parameter.player.transform.position, parameter.speed * Time.deltaTime);
            var rot = Quaternion.LookRotation(parameter.player.transform.position);
            parameter.enemy.transform.rotation = Quaternion.RotateTowards(parameter.enemy.transform.rotation, rot, parameter.turnSpeed * Time.deltaTime);
        }
    }

    class AttackState : IState
    {
        public Parameter parameter;
        public FSM fsm;

        public AttackState(Parameter parameter, FSM fsm)
        {
            this.parameter = parameter;
            this.fsm = fsm;
        }

        public void Enter()
        {
            var animator = parameter.enemy.GetComponent<Animator>();

            // 动画
            animator.SetTrigger("attack");
        }

        public void Exit()
        {

        }

        public void Stay()
        {
            parameter.player.GetComponent<Player>().TakeRatioDamage(parameter.damageRatio);
            // 攻击之后切换默认状态
            if(parameter.count < 1f)
            {
                parameter.count += Time.deltaTime;
            }
            else
            {
                fsm.TransitionState(FSM.E_StateType.Idle);
                parameter.count = 0f;
            }
        }
    }

    class PatrolState : IState
    {
        public Parameter parameter;
        public FSM fsm;

        public PatrolState(Parameter parameter, FSM fsm)
        {
            this.parameter = parameter;
            this.fsm = fsm;
        }

        public void Enter()
        {
            // 进入行走状态
            parameter.enemy.GetComponent<Animator>().SetFloat("Speed", parameter.speed);
        }

        public void Exit()
        {
            // 发现敌人，开始追击
            parameter.enemy.GetComponent<Animator>().SetFloat("Speed", 0f);
        }

        public void Stay()
        {
            Patrol();
            if(parameter.relative.magnitude > parameter.radius)
            {
                // 角色控制且未隐藏
                if (parameter.player.GetComponent<PlayerController>().isMoving && !parameter.player.GetComponent<PlayerController>().isHide)
                {
                    fsm.TransitionState(FSM.E_StateType.Walk);
                }
            }

            if (parameter.relative.magnitude > 10 * parameter.radius)
            {
                fsm.TransitionState(FSM.E_StateType.Idle);
            }
        }

        // 实现巡逻函数，简单AI
        void Patrol()
        {
            // 如果到达目标路径点，更新
            Vector3 way = parameter.waypoints[parameter.waypointIndex].position;
            if (Mathf.Abs(parameter.enemy.transform.position.x - way.x) < 0.05f && Mathf.Abs(parameter.enemy.transform.position.z - way.z) < 0.05f)
            {
                Debug.Log(parameter.waypointIndex);
                parameter.waypointIndex++;
                if (parameter.waypointIndex >= parameter.waypoints.Length)
                {
                    parameter.waypointIndex = 0;
                }
            }

            Vector3 relative = way;
            Vector3 pos = parameter.enemy.transform.position;
            // 计算方向向量，并旋转敌人
            Vector3 targetDirection = relative - pos;
            var rotate = Quaternion.LookRotation(targetDirection);
            parameter.enemy.transform.rotation = Quaternion.RotateTowards(parameter.enemy.transform.rotation, rotate, parameter.turnSpeed * Time.deltaTime);

            // 使用MoveTowards函数控制敌人项目表位置移动（时间平滑缓慢移动，通过Time.deltaTime控制）
            parameter.enemy.transform.position = Vector3.MoveTowards(pos, relative, parameter.speed * Time.deltaTime);
        }
    }
    #endregion


}