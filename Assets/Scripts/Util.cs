using System.Collections;
using UnityEngine;

public static class Util {
    public static void PrintList(IEnumerable ie) {
        Debug.Log($"Printing list {ie}:");
        foreach (var v in ie) Debug.Log(v);
    }
}