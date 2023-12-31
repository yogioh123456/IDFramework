
using UnityEngine;

[DevPriority(3)]
public class DevToolCreateWeapon {
    [DevConsole("创建武器/近战武器/魔刀")]
    public static void TestDevMethodCreateDemon() {
        Debug.Log("魔刀！");
    }
    
    [DevConsole("创建武器/近战武器/圣剑平台")]
    public static void TestDevMethodCreateHolyPlatform() {
    }
    
    [DevConsole("创建武器/近战武器/魔刀平台")]
    public static void TestDevMethodCreateDemonPlatform() {
    }

    [DevConsole("创建武器/步枪/AKM")]
    public static void TestDevMethodCreateLiZiPao() {

    }
    
    [DevConsole("创建武器/能量武器/滋滋泵")]
    public static void TestDevMethodCreateZiZiBeng() {

    }
}