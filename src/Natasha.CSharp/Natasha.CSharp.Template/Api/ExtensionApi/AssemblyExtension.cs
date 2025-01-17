﻿using Natasha.Framework;
using System.Reflection;
using System.Runtime.Loader;

namespace Natasha.CSharp
{
    public static class AssemblyExtension
    {

        /// <summary>
        /// 创建一个程序集编译类
        /// </summary>
        /// <param name="name">程序集名字</param>
        /// <returns></returns>
        public static NAssembly CreateAssembly(this DomainBase domain, string name)
        {

            NAssembly result = new NAssembly(name);
            result.AssemblyBuilder.Compiler.Domain = domain;
            return result;

        }
        public static NAssembly CreateAssembly(this DomainBase domain)
        {

            NAssembly result = new NAssembly();
            result.AssemblyBuilder.Compiler.Domain = domain;
            return result;

        }



        public static DomainBase GetDomain(this Assembly assembly)
        {

            var assemblyDomain = AssemblyLoadContext.GetLoadContext(assembly);
            if (assemblyDomain == AssemblyLoadContext.Default)
            {
                return DomainBase.DefaultDomain!;
            }
            return (DomainBase)assemblyDomain!;

        }




        public static void RemoveReferences(this Assembly assembly)
        {

            GetDomain(assembly).RemoveReference(assembly);

        }



        public static void DisposeDomain(this Assembly assembly)
        {

            var domain = GetDomain(assembly);
            if (domain.Name != "Default")
            {
                domain.Dispose();
            }

        }

    }
}
