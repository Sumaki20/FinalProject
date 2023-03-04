using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    Animator animator;
    int isWalkingHash;
    int isRunningHash;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("Walking");
        isRunningHash = Animator.StringToHash("Running");
    }

    // Update is called once per frame
    void Update()
    {
        bool Running = animator.GetBool(isRunningHash);
        bool Walking = animator.GetBool(isWalkingHash);
        bool forwardPressed = Input.GetKey("w");
        bool runPressed = Input.GetKey("left shift");

        // Check press w key to walk
        if (!Walking && forwardPressed)
        {
            animator.SetBool(isWalkingHash, true);
        }

        if (Walking && !forwardPressed)
        {
            animator.SetBool(isWalkingHash, false);
        }
        if (!Running && (forwardPressed && runPressed))
        {
            animator.SetBool(isRunningHash, true);
        }
        if (Running && (!forwardPressed || !runPressed))
        {
            animator.SetBool(isRunningHash, false);
        }
    }
}
