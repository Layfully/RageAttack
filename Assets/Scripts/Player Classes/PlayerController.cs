using System.Deployment.Internal;
using UnityEngine;

public class PlayerController : Destructor
{
    public UltimateType UltType;
    private Player MyPlayer { get; set; }
    private MovementData MyMovementData { get; set; }
    private float CurrentJumpVelocity { get; set; }
    private float TeleportCooldownCounter { get; set; }
    private float ShootCooldownCounter { get; set; }
    private float LastPressTeleportTime { get; set; }
    private int LastDirection { get; set; }
    private Rigidbody2D MyRigidbody2D { get; set; }
    private bool CanPick { get; set; }
    private GameObject PickableGameObject { get; set; }
    private GameObject PickedGameObject { get; set; }
    private bool IsPlayerMoving { get; set; }
    private GameObject Bullet { get; set; }
    [SerializeField]
    private LayerMask _mask;

    private Animator MyAnimator
    {
        get { return _myAnimator ?? (_myAnimator = GetComponent<Animator>()); }
        set
        {
            _myAnimator = value;
        }
    }

    private Animator _myAnimator;


    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private GameObject _ultimateBulletPrefab;


    private void Start()
    {
        MyRigidbody2D = GetComponent<Rigidbody2D>();
        MyPlayer = GetComponent<Player>();
        MyAnimator = GetComponent<Animator>();
        MyMovementData = MyPlayer.MovementData;

        MyPlayer.DamagedHandler = (sender, args) => Die(args.CurrentHealth);
        MyPlayer.PlayerVitals.Damaged += MyPlayer.DamagedHandler;

        //support axis
        InputEvents.RegisterAxisEvent(MyPlayer.MovementData.HorizontalAxisName, Move);

        //support buttons
        InputEvents.RegisterButtonEvent(InputPhase.OnPressed, MyMovementData.ShootProjectileButtonName, ShootProjectile);
        InputEvents.RegisterButtonEvent(InputPhase.OnPressed, MyMovementData.UltimateButtonName, CastUltimate);
        InputEvents.RegisterButtonEvent(InputPhase.OnPressed, MyMovementData.JumpButtonName, ResetJump);
        InputEvents.RegisterButtonEvent(InputPhase.OnHold, MyMovementData.JumpButtonName, LoadJump);
        InputEvents.RegisterButtonEvent(InputPhase.OnReleased, MyMovementData.JumpButtonName, Jump);
        InputEvents.RegisterButtonEvent(InputPhase.OnHold, MyMovementData.LoadChiButtonName, LoadChi);
        InputEvents.RegisterButtonEvent(InputPhase.OnPressed, MyMovementData.PunchButtonName, Punch);

        //support multiple delegates for one event
        InputEvents.RegisterButtonEvent(InputPhase.OnPressed, MyMovementData.LoadChiButtonName, PickupObject);
        InputEvents.RegisterButtonEvent(InputPhase.OnPressed, MyMovementData.PunchButtonName, ShootObject);

        //support axis as buttons
        InputEvents.RegisterButtonEvent(InputPhase.OnPressed, MyMovementData.HorizontalAxisName, Teleport);

    }

    private void Update()
    {
        ControlFalling();
    }

    private void ControlFalling()
    {
        if (MyRigidbody2D.velocity.y < 0 && !IsGrounded())
        {
            MyRigidbody2D.velocity += Vector2.up * Physics2D.gravity.y * (MyMovementData.FallMultipler - 1) * Time.deltaTime;
            MyAnimator.SetBool("IsFalling", true);
        }

        else if (MyRigidbody2D.velocity.y > 0 && !IsGrounded())
        {
            MyRigidbody2D.velocity += Vector2.up * Physics2D.gravity.y * (MyMovementData.LowJumpMultipler - 1) * Time.deltaTime;
        }
        else
        {
            MyRigidbody2D.velocity = Vector2.Lerp(MyRigidbody2D.velocity, new Vector2(0, MyRigidbody2D.velocity.y), Time.deltaTime * 10);
            MyAnimator.SetBool("IsFalling", false);
        }
    }

    private void ResetJump()
    {
        CurrentJumpVelocity = 0;
    }

    private void LoadJump()
    {
        MyAnimator.ResetTrigger("Jump");

        if (!IsGrounded())
        {
            return;
        }

        MyAnimator.SetBool("IsLoadingJump", true);

        if (IsGrounded())
        {
            CurrentJumpVelocity += (Time.deltaTime) * MyMovementData.JumpLoadTime;
        }

    }

    private void Jump()
    {

        if (!IsGrounded())
        {
            return;
        }

        MyAnimator.SetBool("IsLoadingJump", false);

        if (IsGrounded())
        {
            if (CurrentJumpVelocity < MyMovementData.MinJumpVelocity)
            {
                CurrentJumpVelocity = MyMovementData.MinJumpVelocity;
            }
            else if (CurrentJumpVelocity > MyMovementData.MaxJumpVelocity)
            {
                CurrentJumpVelocity = MyMovementData.MaxJumpVelocity;
            }
            MyRigidbody2D.velocity = Vector2.up * CurrentJumpVelocity;
        }

        MyAnimator.SetTrigger("Jump");

        MyPlayer.PlayerStats.JumpCounter++;
    }

    private void Move()
    {
        IsPlayerMoving = Mathf.Abs(InputManager.GetAxis(MyMovementData.HorizontalAxisName)) > 0;

        if (IsPlayerMoving)
        {
            MyRigidbody2D.velocity = new Vector2(InputManager.GetAxis(MyMovementData.HorizontalAxisName) * MyMovementData.Speed, MyRigidbody2D.velocity.y);
        }

        if (InputManager.GetAxisRaw(MyMovementData.HorizontalAxisName) != 0)
        {
            MyAnimator.SetFloat("MoveX", InputManager.GetAxisRaw(MyPlayer.MovementData.HorizontalAxisName));
        }
        MyAnimator.SetBool("IsMoving", IsPlayerMoving);
    }

    private void Teleport()
    {
        if (Time.time - LastPressTeleportTime < MyMovementData.DoublePressSpeed && LastDirection == (int)InputManager.GetAxisRaw(MyMovementData.HorizontalAxisName))
        {
            if (TeleportCooldownCounter <= Time.time)
            {
                Vector2 teleportationVector = new Vector2(transform.position.x + LastDirection * MyMovementData.TeleportationDistance, transform.position.y);

                if (!Physics2D.OverlapPoint(new Vector2(teleportationVector.x, teleportationVector.y), MyMovementData.GroundLayer))
                {
                    transform.position = teleportationVector;
                    MyPlayer.PlayerStats.TeleportationCounter++;
                }

                TeleportCooldownCounter = Time.time + MyMovementData.TeleportCooldown;
            }
        }
        LastPressTeleportTime = Time.time;
        LastDirection = (int)InputManager.GetAxisRaw(MyMovementData.HorizontalAxisName);
    }

    private void ShootProjectile()
    {
        if (ShootCooldownCounter <= Time.time)
        {
            if (MyPlayer.PlayerVitals.CurrentChi >= MyMovementData.BulletCost)
            {
                Bullet = Instantiate(_bulletPrefab, transform.position + new Vector3(0.1f * LastDirection, .1f), Quaternion.identity);
                Bullet.GetComponent<Bullet>().MyPlayer = MyPlayer;
                Bullet.GetComponent<Bullet>().Follow = true;
                Physics2D.IgnoreCollision(Bullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());
                MyPlayer.PlayerVitals.UseChi(MyMovementData.BulletCost);
                MyPlayer.PlayerStats.ProjectileCounter++;

                MyAnimator.SetTrigger("Shoot");
                ShootCooldownCounter = Time.time + MyMovementData.ShootCooldown;
            }
        }
    }

    private void PickupObject()
    {
        if (CanPick && PickedGameObject == null)
        {
            MyAnimator.SetTrigger("Pick Up");
            PickedGameObject = PickableGameObject;
            PickedGameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            PickedGameObject.transform.SetParent(gameObject.transform);
            PickedGameObject.transform.localPosition = new Vector3(0, 5, 0);
            Destroy(PickedGameObject.GetComponent<Rigidbody2D>());

            Pickable pickable = PickedGameObject.GetComponent<Pickable>();
            pickable.Rotate = false;
            pickable.MyPlayer = MyPlayer;
            MyAnimator.SetBool("IsHoldingThrowable", true);
        }
    }

    private void ShootObject()
    {
        if (PickedGameObject != null)
        {
            MyAnimator.SetTrigger("Throw");
            Rigidbody2D objectRigidbody = PickedGameObject.AddComponent<Rigidbody2D>();
            Pickable pickable = PickedGameObject.GetComponent<Pickable>();
            pickable.Thrown = true;
            objectRigidbody.AddForce((MyPlayer.PlayerTargetting.EnemyTransform.position - objectRigidbody.transform.position).normalized * 750);
            gameObject.transform.DetachChildren();
           // PickedGameObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            PickedGameObject = null;
            MyAnimator.SetBool("IsHoldingThrowable", false);
        }
    }

    private void CastUltimate()
    {
        switch (UltType)
        {
            case UltimateType.SpecialBullet:
                if (ShootCooldownCounter <= Time.time)
                {
                    if (MyPlayer.PlayerVitals.CurrentChi >= MyMovementData.SpecialBulletCost)
                    {
                        GameObject bullet = Instantiate(_ultimateBulletPrefab, transform.position + new Vector3(0.1f * LastDirection, .1f), Quaternion.identity);
                        bullet.GetComponent<Bullet>().MyPlayer = MyPlayer;
                        bullet.GetComponent<Bullet>().Follow = true;
                        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());
                        MyPlayer.PlayerVitals.UseChi(MyMovementData.SpecialBulletCost);
                        MyPlayer.PlayerStats.ProjectileCounter++;

                        MyAnimator.SetTrigger("Shoot");
                        ShootCooldownCounter = Time.time + MyMovementData.ShootCooldown;
                    }
                }
                break;
            case UltimateType.MultipleBullets:
                if (ShootCooldownCounter <= Time.time)
                {
                    if (MyPlayer.PlayerVitals.CurrentChi >= MyMovementData.SpecialMultipleBulletCost)
                    {
                        for (int i = 0; i < 7; i++)
                        {
                            GameObject bullet = Instantiate(_bulletPrefab, transform.position + new Vector3(0.1f * LastDirection, Random.Range(-1f, 1f)), Quaternion.identity);
                            bullet.GetComponent<Bullet>().MyPlayer = MyPlayer;
                            bullet.GetComponent<Bullet>().Follow = true;
                            Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());
                            MyPlayer.PlayerStats.ProjectileCounter++;
                        }
                        MyPlayer.PlayerVitals.UseChi(MyMovementData.SpecialMultipleBulletCost);
                        MyAnimator.SetTrigger("Shoot");
                        ShootCooldownCounter = Time.time + MyMovementData.ShootCooldown;
                    }
                }
                break;
            case UltimateType.AboveBullet:
                if (ShootCooldownCounter <= Time.time)
                {
                    if (MyPlayer.PlayerVitals.CurrentChi >= MyMovementData.SpecialAboveCost)
                    {
                        for (int i = 0; i < 7; i++)
                        {
                            Transform enemy = MyPlayer.PlayerTargetting.EnemyTransform;
                            GameObject bullet = Instantiate(_bulletPrefab, enemy.position + new Vector3(0.1f * LastDirection + Random.Range(-2f,2f), 5), Quaternion.identity);
                            bullet.GetComponent<Bullet>().MyPlayer = MyPlayer;
                            bullet.GetComponent<Bullet>().Follow = false;
                            bullet.GetComponent<Bullet>().EnableGravity();
                            Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());
                            MyPlayer.PlayerStats.ProjectileCounter++;
                        }
                        MyPlayer.PlayerVitals.UseChi(MyMovementData.SpecialAboveCost);
                        MyAnimator.SetTrigger("Shoot");
                        ShootCooldownCounter = Time.time + MyMovementData.ShootCooldown;
                    }
                }

                break;
                
            
        }

    }

    private void Punch()
    {
        MyAnimator.SetTrigger(IsGrounded() ? "Punch" : "Kick");


        int oldLayer = gameObject.layer;
        gameObject.layer = 0;

        Collider2D enemy = Physics2D.OverlapArea(transform.position, new Vector2(transform.position.x + (float)LastDirection / 4, transform.position.y + 1), _mask);

        if (enemy)
        {
            enemy.GetComponent<Rigidbody2D>().AddForce(new Vector2(LastDirection, 1) * 1.5f, ForceMode2D.Impulse);
            MyPlayer.PlayerTargetting.EnemyPlayerStats.TakeDamage(Settings.Instance.DamageSettings.PunchDamage);
        }

        gameObject.layer = oldLayer;
    }

    private void LoadChi()
    {
        MyPlayer.PlayerVitals.LoadChi(MyMovementData.ChiLoadAmount);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapArea(new Vector2(transform.position.x - 0.1f, transform.position.y - .3f), new Vector2(transform.position.x + 0.1f, transform.position.y - .7f), MyPlayer.MovementData.GroundLayer);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("terrain") && (col.relativeVelocity.x < MyPlayer.MovementData.VelocityTreshold.x ||
                                                     col.relativeVelocity.x > MyPlayer.MovementData.VelocityTreshold.z ||
                                                     col.relativeVelocity.y < MyPlayer.MovementData.VelocityTreshold.y ||
                                                     col.relativeVelocity.y > MyPlayer.MovementData.VelocityTreshold.w) && col.gameObject.GetComponent<Terrain>() != null)
        {
            Terrain terrain = col.gameObject.GetComponent<Terrain>();
            DrawTransparencyOnTerrain(terrain, col);
            StartCoroutine(terrain.GenerateCollider());
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Pickable"))
        {
            CanPick = true;
            PickableGameObject = other.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Pickable"))
        {
            PickableGameObject = null;
            CanPick = false;
        }
    }

    private void Die(int health)
    {
        if (health <= 0)
        {
            MyAnimator.SetTrigger("Die");
            GameManager.Instance.EndGame();
        }
    }
}