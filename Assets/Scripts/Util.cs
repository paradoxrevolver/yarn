using System.Collections;
using UnityEngine;

public static class Util {
    public static void PrintList(IEnumerable ic) {
        Debug.Log($"Printing enumerable {ic}:");
        foreach (var v in ic) Debug.Log(v);
    }
}