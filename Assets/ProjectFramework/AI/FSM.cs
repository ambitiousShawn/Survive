using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Shawn.ProjectFramework
{
    /// <summary>
    /// ����״̬������ģ�飺
    ///     1.�ṩ״̬��Ĺ����ӿ�
    ///     2.�ṩ�洢����״̬�������
    ///     3.�ṩ���⿪�ŵ�״̬�ı�API
    ///     4.�洢��ǰ״̬��Ϣ
    /// </summary>
    /// 

    ///����״̬�Ľӿ�
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
         �洢ʹ��״̬������������Ҫ��������Ϣ
         */
        // �����ƶ��ٶ�
        public float speed = 5f;
        // ��ʼ�ٶ�
        public float originalSpeed = 5f;
        // ת���ٶ�
        public float turnSpeed = 180f;

        // ���
        public GameObject player;
        // ����
        public GameObject enemy;
        // �洢Ѳ��·��
        public Transform[] waypoints;

        // ��ǰѲ��·���±�
        public int waypointIndex = 0;
        // ��ս��ⷶΧ
        public float radius = 2f;
        // �˺�
        public float damageRatio = 0.3f;
        // �������
        public float duration = 5f;

        public Vector3 relative;
        // ��ʱ��
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
        /// ��ʼ��״̬��
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

    #region �˴�дÿ����Ϸ�����Ӧ��״̬��
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
            // ��ҽ�ԶʱĬ��״̬
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
            // �����ؾ߻�������
            if (parameter.player.GetComponent<PlayerController>().isHide || !parameter.player.GetComponent<PlayerController>().isMoving)
            {
                fsm.TransitionState(FSM.E_StateType.Patrol);
            }
        }

        // ����
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

            // ����
            animator.SetTrigger("attack");
        }

        public void Exit()
        {

        }

        public void Stay()
        {
            parameter.player.GetComponent<Player>().TakeRatioDamage(parameter.damageRatio);
            // ����֮���л�Ĭ��״̬
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
            // ��������״̬
            parameter.enemy.GetComponent<Animator>().SetFloat("Speed", parameter.speed);
        }

        public void Exit()
        {
            // ���ֵ��ˣ���ʼ׷��
            parameter.enemy.GetComponent<Animator>().SetFloat("Speed", 0f);
        }

        public void Stay()
        {
            Patrol();
            if(parameter.relative.magnitude > parameter.radius)
            {
                // ��ɫ������δ����
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

        // ʵ��Ѳ�ߺ�������AI
        void Patrol()
        {
            // �������Ŀ��·���㣬����
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
            // ���㷽������������ת����
            Vector3 targetDirection = relative - pos;
            var rotate = Quaternion.LookRotation(targetDirection);
            parameter.enemy.transform.rotation = Quaternion.RotateTowards(parameter.enemy.transform.rotation, rotate, parameter.turnSpeed * Time.deltaTime);

            // ʹ��MoveTowards�������Ƶ�����Ŀ��λ���ƶ���ʱ��ƽ�������ƶ���ͨ��Time.deltaTime���ƣ�
            parameter.enemy.transform.position = Vector3.MoveTowards(pos, relative, parameter.speed * Time.deltaTime);
        }
    }
    #endregion


}