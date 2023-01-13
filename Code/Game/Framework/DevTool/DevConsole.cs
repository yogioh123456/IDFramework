using System;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public class DevConsole : Attribute {
    public string name;
    public DevConsole(string str) {
        this.name = str;
    }
}

[AttributeUsage(AttributeTargets.Class)]
public class DevPriority : Attribute {
    public int priority;
    public DevPriority(int priority) {
        this.priority = priority;
    }
}
