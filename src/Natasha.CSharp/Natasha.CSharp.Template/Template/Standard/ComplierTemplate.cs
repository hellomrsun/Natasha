﻿using Microsoft.CodeAnalysis.CSharp;
using Natasha.Framework;
using System;

namespace Natasha.CSharp.Template
{
    public class CompilerTemplate<T> : ALinkTemplate<T> where T : CompilerTemplate<T>, new()
    {
        //使用默认编译器
        public AssemblyCSharpBuilder AssemblyBuilder;
        public Action<AssemblyCSharpBuilder>? OptionAction;

        public CompilerTemplate()
        {

            AssemblyBuilder = new();

        }


        public T ConfigComplier(Func<NatashaCSharpCompiler, NatashaCSharpCompiler> compilerFunc)
        {
            if (compilerFunc != null)
            {
                AssemblyBuilder.Compiler = compilerFunc(AssemblyBuilder.Compiler);
            }
            return Link!;
        }

        public T ConfigCompilation(Func<CSharpCompilation, CSharpCompilation> compilerFunc)
        {
            if (compilerFunc!=null)
            {
                AssemblyBuilder.Compiler.Compilation = compilerFunc(AssemblyBuilder.Compiler.Compilation!);
            }
            return Link!;
        }




        public T AutoUsing()
        {

            AssemblyBuilder.CustomUsingShut = false;
            return Link!;

        }
        public T CustomUsing()
        {
            AssemblyBuilder.CustomUsingShut = true;
            return Link!;
        }




        public T AssemblyName(string name)
        {

            AssemblyBuilder.Compiler.AssemblyName = name;
            return Link!;

        }



        #region 指定编译器的域进行创建
        public static T UseCompiler(AssemblyCSharpBuilder builder, Action<AssemblyCSharpBuilder>? option = default)
        {

            return UseDomain(builder.Compiler.Domain, option);

        }
        #endregion
        #region 指定字符串域创建以及参数
        public static T CreateDomain(string domainName, Action<AssemblyCSharpBuilder>? option = default)
        {

            if (domainName.ToLower() == "default")
            {
                return UseDomain(DomainBase.DefaultDomain, option);
            }
            else
            {
                return UseDomain(DomainManagement.Create(domainName), option);
            }

        }
        #endregion
        #region 指定域创建以及参数
        public static T UseDomain(DomainBase domain, Action<AssemblyCSharpBuilder>? option = default)
        {

            T instance = new();
            instance.AssemblyBuilder.Compiler.Domain = domain;
            instance.OptionAction = option;
            option?.Invoke(instance.AssemblyBuilder);
            return instance;

        }
        #endregion
        #region  Default 默认域创建以及参数
        public static T DefaultDomain(Action<AssemblyCSharpBuilder>? option = default)
        {

            return UseDomain(DomainBase.DefaultDomain, option);

        }


        #endregion
        #region 随机域创建以及参数
        public static T RandomDomain(Action<AssemblyCSharpBuilder>? option = default)
        {

            return UseDomain(DomainManagement.Random, option);

        }
        #endregion

    }
}
