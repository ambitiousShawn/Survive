using System;
using System.Collections;
using System.Collections.Generic;
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
        public int moveSpeed = 5; //�����ƶ��ٶ�
    }

    public class FSM : MonoBehaviour 
    {
        public enum E_StateType
        {
            Idle,
            Walk,
            Attack,
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
        }

        private void InitState(E_StateType type)
        {
            TransitionState(type);
        }

        private void TransitionState(E_StateType state)
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
            Debug.Log("�������Idle״̬");
        }

        public void Exit()
        {
            Debug.Log("�����뿪��Idle״̬");
        }

        public void Stay()
        {
            
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
            Debug.Log("���������Walk״̬");
        }

        public void Exit()
        {
            Debug.Log("�����뿪��Walk״̬");
        }

        public void Stay()
        {
            
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

        }

        public void Exit()
        {

        }

        public void Stay()
        {

        }
    }
    #endregion


}