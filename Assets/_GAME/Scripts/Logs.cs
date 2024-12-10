
using UnityEngine;
public class Logs{
    private static bool isDebug = true;
    public static void Log(string txt) {
        if(isDebug) Debug.Log(txt);
    }
    
    public static void LogWarning(string txt) {
        if(isDebug) Debug.LogWarning(txt);
    }
    
    public static void LogError(string txt) {
        if(isDebug) Debug.LogError(txt);
    }

    public void LogAttack()
    {
        Debug.Log("Attack");
    }
}