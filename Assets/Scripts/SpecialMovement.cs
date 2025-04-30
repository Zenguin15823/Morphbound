using System;
using UnityEngine;

public class SpecialMovement : MonoBehaviour
{
    private float dashTimer;
    private const float dashDuration = 0.3f;
    private const float dashCooldown = 0.8f;

    [NonSerialized] public bool hasJump;
    [NonSerialized] public bool dashReady;
    [NonSerialized] public float dashDir;

    public bool doubleJump;
    public bool dash;

    void Start()
    {
        hasJump = doubleJump;
        dashTimer = dashCooldown;
        dashReady = dash;
    }

    void Update()
    {
        if (dash && dashTimer < dashCooldown)
        {
            dashTimer += Time.deltaTime;
            if (dashTimer > dashCooldown)
                dashReady = true;
        }
    }

    public void useDash(float inputHoriz)
    {
        dashReady = false;
        if (dash)
        {
            dashTimer = 0;
            dashDir = Math.Sign(inputHoriz);
        }
    }

    public bool isDashing()
    {
        return dash && dashTimer < dashDuration;
    }
}
