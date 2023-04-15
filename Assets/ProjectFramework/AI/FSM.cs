using System;
using System.Collections;
using System.Collections.Generic;
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
        public int moveSpeed = 5; //比如移动速度
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
        /// 初始化状态机
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
            Debug.Log("物体进入Idle状态");
        }

        public void Exit()
        {
            Debug.Log("物体离开了Idle状态");
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
            Debug.Log("物体进入了Walk状态");
        }

        public void Exit()
        {
            Debug.Log("物体离开了Walk状态");
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