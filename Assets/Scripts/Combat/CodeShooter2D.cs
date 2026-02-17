using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class CodeShooter2D : MonoBehaviour
{
    [SerializeField] private float shootInterval = 0.25f;
    [SerializeField] private Vector2 muzzleOffset = new Vector2(0.55f, 0.12f);
    [SerializeField] private string[] codeLevels =
    {
        "i",
        "if",
        "int",
        "init",
        "while",
        "for(i)",
        "for(i=0;i<n;i++)"
    };

    private float coolDown;
    private int level;
    private PlayerController2D player;

    public int Level => level;
    public string CurrentCode => codeLevels[Mathf.Clamp(level, 0, codeLevels.Length - 1)];

    private void Awake()
    {
        player = GetComponent<PlayerController2D>();
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
            coolDown = shootInterval;
        }
    }

    public void LevelUp()
    {
        level = Mathf.Min(level + 1, codeLevels.Length - 1);
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
        codeProjectile.Initialize(new Vector2(facing, 0f), CurrentCode);
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
