using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Shawn.EditorFramework
{
    //���ܷ���EditorĿ¼��
    public static class AssemblyManager
    {
        public static Assembly assembly = Assembly.GetExecutingAssembly();

        public static Type[] types = assembly.GetTypes();
    }

}