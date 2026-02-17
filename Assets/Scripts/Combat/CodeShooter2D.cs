using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class CodeShooter2D : MonoBehaviour
{
    [SerializeField] private float shootInterval = 0.25f;
    [SerializeField] private Vector2 muzzleOffset = new Vector2(0.55f, 0.12f);
    [SerializeField] private string initialCode = "i";
    [SerializeField] private string[] completeExpressions =
    {
        "i==sum(arr)",
        "i==sum(arr)+max(x,y)"
    };

    private float coolDown;
    private string currentCode;
    private int completedExpressionCount;
    private PlayerController2D player;

    public int PowerLevel => completedExpressionCount;
    public string CurrentCode => currentCode;

    private void Awake()
    {
        player = GetComponent<PlayerController2D>();
        currentCode = string.IsNullOrWhiteSpace(initialCode) ? "i" : initialCode;
    }

    private void Update()
    {
        if (coolDown > 0f)
        {
            coolDown -= Time.deltaTime;
        }

        if (player != null && player.IsDead)
        {
            return;
        }

        if (coolDown <= 0f && ReadShootPressed())
        {
            Fire();
            coolDown = Mathf.Max(0.08f, shootInterval - (completedExpressionCount * 0.03f));
        }
    }

    public void AppendToken(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            return;
        }

        currentCode += token;
        TryCompleteExpression();
    }

    public void LevelUp()
    {
        AppendToken("=");
    }

    private void Fire()
    {
        float facing = player != null ? player.FacingDirection : 1f;
        Vector3 spawnPos = transform.position + new Vector3(muzzleOffset.x * facing, muzzleOffset.y, 0f);

        GameObject projectile = new GameObject("CodeProjectile");
        projectile.transform.position = spawnPos;

        CircleCollider2D collider = projectile.AddComponent<CircleCollider2D>();
        collider.isTrigger = true;
        collider.radius = 0.12f;

        CodeProjectile2D codeProjectile = projectile.AddComponent<CodeProjectile2D>();
        codeProjectile.Initialize(new Vector2(facing, 0f), CurrentCode, completedExpressionCount);
    }

    private void TryCompleteExpression()
    {
        if (completeExpressions == null || completeExpressions.Length == 0)
        {
            return;
        }

        if (completedExpressionCount >= completeExpressions.Length)
        {
            return;
        }

        string target = completeExpressions[completedExpressionCount];
        if (string.IsNullOrWhiteSpace(target))
        {
            completedExpressionCount++;
            return;
        }

        if (currentCode.EndsWith(target))
        {
            completedExpressionCount++;
        }
    }

    private bool ReadShootPressed()
    {
#if ENABLE_INPUT_SYSTEM
        if (Keyboard.current != null)
        {
            return Keyboard.current.jKey.wasPressedThisFrame || Keyboard.current.leftCtrlKey.wasPressedThisFrame;
        }

        if (Gamepad.current != null)
        {
            return Gamepad.current.rightShoulder.wasPressedThisFrame;
        }
#endif
    #if ENABLE_LEGACY_INPUT_MANAGER
        return Input.GetKeyDown(KeyCode.J) || Input.GetButtonDown("Fire1");
    #else
        return false;
    #endif
    }
}
