﻿using Natasha.CSharp.Builder;
using Natasha.CSharp.Template;
using Natasha.Error;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Natasha.CSharp
{

    public class NAssembly : CompilerTemplate<NAssembly>
    {

        public Assembly? Assembly;
        private readonly HashSet<IScript> _builderCache;
        private bool HasChecked;


        public NAssembly(string name) : this()
        {

            AssemblyBuilder.Compiler.AssemblyName = name;

        }




        public NAssembly()
        {

            _builderCache = new HashSet<IScript>();
            HasChecked = false;
        }


        /// <summary>
        /// 移除一个构建类
        /// </summary>
        /// <param name="builder">构建类</param>
        /// <returns></returns>
        public bool Remove(IScript builder)
        {
            return _builderCache.Remove(builder);
        }


        /// <summary>
        /// 直接添加一个合法的类/接口/结构体/枚举
        /// </summary>
        /// <param name="script">脚本代码</param>
        /// <returns></returns>
        public void AddScript(string script)
        {
            AssemblyBuilder.Add(script);
        }


        /// <summary>
        /// 添加一个带有代码的文件
        /// </summary>
        /// <param name="path">代码文件路径</param>
        /// <returns></returns>
        public void AddFile(string path)
        {
            AssemblyBuilder.AddFile(path);
        }

        private T GetBaseOopHandler<T>() where T : OopBuilder<T>, new()
        {
            var @operator = new T().Namespace(AssemblyBuilder.Compiler.AssemblyName);
            _builderCache.Add(@operator);
            return @operator;
        }
        private S GetBaseDelegateHandler<S>() where S : MethodBuilder<S>, new()
        {
            var @operator = new S().ClassOptions(item => item.Namespace(AssemblyBuilder.Compiler.AssemblyName));
            @operator.AssemblyBuilder.Compiler.Domain = AssemblyBuilder.Compiler.Domain;
            _builderCache.Add(@operator);
            return @operator;
        }
       


        /// <summary>
        /// 创建一个类Operator
        /// </summary>
        /// <param name="name">类名</param>
        /// <returns></returns>
        public NClass CreateClass()
        {

            return GetBaseOopHandler<NClass>();

        }


        /// <summary>
        /// 创建一个枚举Operator
        /// </summary>
        /// <param name="name">枚举名</param>
        /// <returns></returns>
        public NEnum CreateEnum()
        {

            return GetBaseOopHandler<NEnum>();

        }


        /// <summary>
        /// 创建一个接口Operator
        /// </summary>
        /// <param name="name">接口名</param>
        /// <returns></returns>
        public NInterface CreateInterface()
        {

            return GetBaseOopHandler<NInterface>();

        }


        /// <summary>
        /// 创建一个结构体Operator
        /// </summary>
        /// <param name="name">结构体名</param>
        /// <returns></returns>
        public NStruct CreateStruct()
        {

            return GetBaseOopHandler<NStruct>();

        }


        /// <summary>
        /// 创建一个FastMethodOperator
        /// </summary>
        /// <param name="name">类名</param>
        /// <returns></returns>
        public FastMethodOperator CreateFastMethod()
        {

            return GetBaseDelegateHandler<FastMethodOperator>();

        }


        /// <summary>
        /// 创建一个FakeMethodOperator
        /// </summary>
        /// <param name="name">类名</param>
        /// <returns></returns>
        public FakeMethodOperator CreateFakeMethod()
        {

            return GetBaseDelegateHandler<FakeMethodOperator>();

        }


        /// <summary>
        /// 进行语法检查
        /// </summary>
        /// <returns></returns>
        public void Check()
        {

            HasChecked = true;
            foreach (var item in _builderCache)
            {
                AssemblyBuilder.Add(item);
            }

        }


        /// <summary>
        /// 对整个程序集进行编译
        /// </summary>
        /// <returns></returns>
        public Assembly GetAssembly()
        {

            if (!HasChecked)
            {
                Check();
            }

            return Assembly = AssemblyBuilder.GetAssembly();

        }


        /// <summary>
        /// 从编译后的缓存中获取类型
        /// </summary>
        /// <param name="name">类名</param>
        /// <returns></returns>
        public Type GetTypeFromShortName(string name)
        {

            if (Assembly == null)
            {
                GetAssembly();
            }
            return Assembly!.GetTypes().First(item => item.Name == name);

        }
        public Type GetTypeFromFullName(string name)
        {

            if (Assembly == null)
            {
                GetAssembly();
            }
            return Assembly!.GetTypes().First(item => item.GetDevelopName() == name);

        }

    }

}
