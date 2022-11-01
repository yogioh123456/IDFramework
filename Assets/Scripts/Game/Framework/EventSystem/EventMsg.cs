using System;

/// <summary>
/// 给消息用的属性，不过值得注意的是，打了这个标签的方法，同一个类中方法名不能重复
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class EventMsg : Attribute
{

}