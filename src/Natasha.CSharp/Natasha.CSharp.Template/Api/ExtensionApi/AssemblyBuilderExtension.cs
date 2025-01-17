﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using Natasha.CSharpEngine;
using Natasha.Error;
using Natasha.Error.Model;
using Natasha.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;



public static class AssemblyBuilderExtension
{

    public static Type GetTypeFromFullName(this AssemblyCSharpBuilder builder, string typeName)
    {

        Assembly assembly = builder.GetAssembly();
        try
        {
            return assembly.GetTypes().First(item => item.GetDevelopName() == typeName);
        }
        catch (Exception ex)
        {
            CompileWrapperExceptions exception = new($"无法在程序集 {builder.Compiler.AssemblyName} 中找到该类型 {typeName}！错误信息:{ex.Message}");
            if (builder.Exceptions.Count > 0)
            {
                exception.ErrorFlag = ExceptionKind.Type;
            }
            else
            {
                exception.ErrorFlag = ExceptionKind.Assembly;
            }
            exception.CompilerExcpetions.AddRange(builder.Exceptions);
            throw exception;
        }

    }
    public static Type GetTypeFromShortName(this AssemblyCSharpBuilder builder, string typeName)
    {

        Assembly assembly = builder.GetAssembly();
        try
        {
            return assembly.GetTypes().First(item => item.Name == typeName);
        }
        catch (Exception ex)
        {
            CompileWrapperExceptions exception = new($"无法在程序集 {builder.Compiler.AssemblyName} 中找到该类型 {typeName}！错误信息:{ex.Message}");
            if (builder.Exceptions.Count > 0)
            {
                exception.ErrorFlag = ExceptionKind.Type;
            }
            else
            {
                exception.ErrorFlag = ExceptionKind.Assembly;
            }
            exception.CompilerExcpetions.AddRange(builder.Exceptions);
            throw exception;
        }

    }

    public static MethodInfo GetMethodFromFullName(this AssemblyCSharpBuilder builder, string typeName, string methodName)
    {

        var type = builder.GetTypeFromFullName(typeName);
        try
        {
            var info = type.GetMethod(methodName);
            if (info == null)
            {
                throw new Exception("获取方法返回空!");
            }
            return info;
        }
        catch (Exception ex)
        {

            CompileWrapperExceptions exception = new($"无法在类型 {typeName} 中找到该方法 {methodName}！错误信息:{ex.Message}");
            if (builder.Exceptions.Count > 0)
            {
                exception.ErrorFlag = ExceptionKind.Method;
            }
            else
            {
                exception.ErrorFlag = ExceptionKind.Assembly;
            }
            exception.CompilerExcpetions.AddRange(builder.Exceptions);
            throw exception;

        }

    }
    public static MethodInfo GetMethodFromShortName(this AssemblyCSharpBuilder builder, string typeName, string methodName)
    {

        var type = builder.GetTypeFromShortName(typeName);
        try
        {
            var info = type.GetMethod(methodName);
            if (info == null)
            {
                throw new Exception("获取方法返回空!");
            }
            return info;
        }
        catch (Exception ex)
        {

            CompileWrapperExceptions exception = new($"无法在类型 {typeName} 中找到该方法 {methodName}！错误信息:{ex.Message}");
            if (builder.Exceptions.Count > 0)
            {
                exception.ErrorFlag = ExceptionKind.Method;
            }
            else
            {
                exception.ErrorFlag = ExceptionKind.Assembly;
            }
            exception.CompilerExcpetions.AddRange(builder.Exceptions);
            throw exception;

        }

    }




    public static Delegate GetDelegateFromFullName(this AssemblyCSharpBuilder builder, string typeName, string methodName, Type delegateType, object? target = null)
    {

        var info = builder.GetMethodFromFullName(typeName, methodName);
        try
        {

            return info.CreateDelegate(delegateType, target);

        }
        catch (Exception ex)
        {

            CompileWrapperExceptions exception = new($"在类型 {typeName} 中找到的方法 {methodName} 向 {delegateType.FullName} 转换时出错！错误信息:{ex.Message}");
            if (builder.Exceptions!.Count == 0)
            {
                exception.ErrorFlag = ExceptionKind.Delegate;
            }
            else
            {
                exception.ErrorFlag = ExceptionKind.Assembly;
            }
            exception.CompilerExcpetions.AddRange(builder.Exceptions);
            throw exception;

        }

    }
    public static T GetDelegateFromFullName<T>(this AssemblyCSharpBuilder builder, string typeName, string methodName, object? target = null) where T : Delegate
    {
        return (T)GetDelegateFromFullName(builder, typeName, methodName, typeof(T), target);
    }
    public static Delegate GetDelegateFromShortName(this AssemblyCSharpBuilder builder, string typeName, string methodName, Type delegateType, object? target = null)
    {

        var info = builder.GetMethodFromShortName(typeName, methodName);

        try
        {

            return info.CreateDelegate(delegateType, target);

        }
        catch (Exception ex)
        {

            CompileWrapperExceptions exception = new($"在类型 {typeName} 中找到的方法 {methodName} 向 {delegateType.FullName} 转换时出错！错误信息:{ex.Message}");
            if (builder.Exceptions!.Count == 0)
            {
                exception.ErrorFlag = ExceptionKind.Delegate;
            }
            else
            {
                exception.ErrorFlag = ExceptionKind.Assembly;
            }
            exception.CompilerExcpetions.AddRange(builder.Exceptions);
            throw exception;

        }

    }
    public static T GetDelegateFromShortName<T>(this AssemblyCSharpBuilder builder, string typeName, string methodName, object? target = null) where T : Delegate
    {
        return (T)GetDelegateFromShortName(builder, typeName, methodName, typeof(T), target);
    }

    public static AssemblyCSharpBuilder AutoUsing(this AssemblyCSharpBuilder builder)
    {
        builder.CustomUsingShut = false;
        return builder;
    }
    public static AssemblyCSharpBuilder CustomUsing(this AssemblyCSharpBuilder builder)
    {
        builder.CustomUsingShut = true;
        return builder;
    }
    public static AssemblyCSharpBuilder ThrowCompilerError(this AssemblyCSharpBuilder builder)
    {
        builder.CompileErrorBehavior = ExceptionBehavior.OnlyThrow;
        return builder;
    }
    public static AssemblyCSharpBuilder LogAndThrowCompilerError(this AssemblyCSharpBuilder builder)
    {
        builder.CompileErrorBehavior = ExceptionBehavior.LogAndThrow;
        return builder;
    }
    //public static AssemblyCSharpBuilder LogCompilerError(this AssemblyCSharpBuilder builder)
    //{
    //    builder.CompileErrorBehavior = ExceptionBehavior.Log;
    //    return builder;
    //}
    //public static AssemblyCSharpBuilder IgnoreCompilerError(this AssemblyCSharpBuilder builder)
    //{
    //    builder.CompileErrorBehavior = ExceptionBehavior.None;
    //    return builder;
    //}
    public static AssemblyCSharpBuilder EnableSucceedLog(this AssemblyCSharpBuilder builder)
    {
        builder.NeedSucceedLog = true;
        return builder;
    }
    public static AssemblyCSharpBuilder DisableSucceedLog(this AssemblyCSharpBuilder builder)
    {
        builder.NeedSucceedLog = false;
        return builder;
    }


    public static AssemblyCSharpBuilder LogAndThrowSyntaxError(this AssemblyCSharpBuilder builder)
    {
        builder.SyntaxErrorBehavior = ExceptionBehavior.LogAndThrow;
        return builder;
    }
    public static AssemblyCSharpBuilder ThrowSyntaxError(this AssemblyCSharpBuilder builder)
    {
        builder.SyntaxErrorBehavior = ExceptionBehavior.OnlyThrow;
        return builder;
    }
    //public static AssemblyCSharpBuilder ForceAddSyntax(this AssemblyCSharpBuilder builder)
    //{
    //    builder.SyntaxErrorBehavior = ExceptionBehavior.Ignore;
    //    return builder;
    //}
    //public static AssemblyCSharpBuilder LogSyntaxError(this AssemblyCSharpBuilder builder)
    //{
    //    builder.SyntaxErrorBehavior = ExceptionBehavior.Log;
    //    return builder;
    //}

    //public static AssemblyCSharpBuilder IgnoreSyntaxError(this AssemblyCSharpBuilder builder)
    //{
    //    builder.SyntaxErrorBehavior = ExceptionBehavior.None;
    //    return builder;
    //}

    public static AssemblyCSharpBuilder SetAssemblyName(this AssemblyCSharpBuilder builder, string assemblyName)
    {
        builder.Compiler.AssemblyName = assemblyName;
        return builder;
    }

    public static AssemblyCSharpBuilder SetRetryLimit(this AssemblyCSharpBuilder builder, int maxRetry)
    {
        builder.RetryLimit = maxRetry;
        return builder;
    }

    public static AssemblyCSharpBuilder EnableNullableCompile(this AssemblyCSharpBuilder builder)
    {
        builder.Compiler.NullableCompileOption = NullableContextOptions.Enable;
        return builder;
    }
    public static AssemblyCSharpBuilder DisableNullableCompile(this AssemblyCSharpBuilder builder)
    {
        builder.Compiler.NullableCompileOption = NullableContextOptions.Disable;
        return builder;
    }

    public static AssemblyCSharpBuilder SetOutputFolder(this AssemblyCSharpBuilder builder, string folder)
    {
        builder.OutputFolder = folder;
        return builder;
    }


    public static AssemblyCSharpBuilder SetDllFilePath(this AssemblyCSharpBuilder builder, string dllFilePath)
    {
        builder.Compiler.DllFilePath = dllFilePath;
        return builder;
    }


    public static AssemblyCSharpBuilder SetPdbFilePath(this AssemblyCSharpBuilder builder, string pdbFilePath)
    {
        builder.Compiler.PdbFilePath = pdbFilePath;
        return builder;
    }


    public static AssemblyCSharpBuilder SetXmlFilePath(this AssemblyCSharpBuilder builder, string xmlFilePath)
    {
        builder.Compiler.XmlFilePath = xmlFilePath;
        return builder;
    }
}