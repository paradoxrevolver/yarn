using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour {
    public void OnMove(InputAction.CallbackContext ctx) {
        Vector2 movement = ctx.ReadValue<Vector2>();
        Debug.Log(movement);
    }
}
