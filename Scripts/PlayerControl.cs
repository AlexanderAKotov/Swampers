/***********************************************************/
/**  © 2018 NULLcode Studio. All Rights Reserved.
/**  Разработано в рамках проекта: https://null-code.ru/
/**  Подписка на Рatreon: https://www.patreon.com/null_code
/***********************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{

    public float speed = 2; // скорость движения персонажа
    public float speedAnimator = 1; // множитель скорости проигрывания клипов анимации
    public Animator animator;
    public bool diagonallyMove = true; // использовать движение по диагонали
    private Move lastMoved;
    private Vector3 playerPos;
    public bool isMove { get; private set; }
    private List<Node> path = new List<Node>();
    private float delta, magnitude;
    private int moveIndex, lastX, lastY;
    private bool getStop;
    private Vector2Int current, last;
    public GameObject PausePanel;
    public GameObject SityPanel;
    public GameObject Panel;
    public Text PrintedAreaType;
    public Text CurrentSpeedValue;
    public Text AreaModValue;
    public Text CurrentWalkerType;
    public Text WalkerName;
    public Text PrintedSityType;
    public WalkerSetting Walker;
    private static bool _OnPause = false;
    public static bool MouseOnPanel = false;
    public static PlayerControl Player;
    public static bool NewGame;
    public static bool GlobalMapIsActive = true;
    public static int Days = 0;//Текущий день
    public static float Hours = 0;//Текущее время
    public static int HoursInDay = 10; //Часов в сутках
    public static float HourEventCheck = 0; //Время следующей проверки события
    public static int EventsInDay = 0; //Колличество событий, уже произошедших за день
    void OnDrawGizmos()
    {
        if (path != null && path.Count > 0)
        {
            for (int i = 0; i < path.Count; i++)
            {
                Gizmos.color = new Color(0, 255, 0, .25f);
                Gizmos.DrawCube(path[i].worldPosition, new Vector3(Pathfinding.cellSize.x, Pathfinding.cellSize.y, 1));
            }
        }
    }

    void Start()
    {
        Player = this;
        Physics.queriesHitTriggers = true;
        animator.SetFloat("speed", speedAnimator);
        playerPos = Pathfinding.CalculatePosition(transform.position);
        transform.position = playerPos;
        lastMoved = Move.Down;
        AnimationSet(Move.Stop);
        Walker = (WalkerSetting)WalkerObjects.TurtleMiddle.CreateClone();
        speed = Walker.SpeedBase;
        ChangeWalkerInfoText();
        DontDestroyOnLoad(this);
        if (NewGame) //Если новая игра, то создаем случайного персонажа.
        {
            for (int i = 0; i < 5; i++)
            {
                Walker.Inventory.Add((ItemSetting)ItemObject.BoneShiv.CreateClone());
            }
            for (int i = 0; i < 5; i++)
            {
                Walker.Inventory.Add((ItemSetting)ItemObject.BatterySmall.CreateClone());
            }
            Walker.Inventory.Add((ItemSetting)ItemObject.RustySword.CreateClone());
            Walker.Inventory.Add((ItemSetting)ItemObject.CareerGnawer.CreateClone());
            Walker.Inventory.Add((ItemSetting)ItemObject.Baldric.CreateClone());
            Walker.Inventory.Add((ItemSetting)ItemObject.HeavyScrapArmor.CreateClone());
            Walker.Crew.Add(CharacterSetting.CreateRandomCharacter(Walker, Random.Range(0, 4), true));
            Walker.Crew.Add(CharacterSetting.CreateRandomCharacter(Walker, Random.Range(0, 4), true));
            Walker.Crew.Add(CharacterSetting.CreateRandomCharacter(Walker, Random.Range(0, 4), true));
            for (int i = 0; i < Walker.Crew.Count; i++)
            {
                int WeaponRoll = Random.Range(0, 4);
                int ArmorRoll = Random.Range(0, 3);
                if (WeaponRoll == 1)
                    Walker.Crew[i].Arms[0] = (ItemSetting)ItemObject.BoneShiv.CreateClone();
                else if (WeaponRoll == 2)
                    Walker.Crew[i].Arms[0] = (ItemSetting)ItemObject.RustySword.CreateClone();
                else if (WeaponRoll == 3)
                {
                    Walker.Crew[i].Arms[0] = (ItemSetting)ItemObject.CareerGnawer.CreateClone();
                    Walker.Crew[i].Arms[1] = Walker.Crew[i].Arms[0];
                }
                if (ArmorRoll == 1)
                    Walker.Crew[i].Armor = (ItemSetting)ItemObject.Baldric.CreateClone();
                else if (ArmorRoll == 2)
                    Walker.Crew[i].Armor = (ItemSetting)ItemObject.HeavyScrapArmor.CreateClone();
            }
            NewGame = false;
        }
    }

    void Update()
    {
        if (GlobalMapIsActive)
        {
            float TimeMove = Time.deltaTime * 0.5f;
            if (Mathf.Floor(Hours) < Mathf.Floor(Hours + TimeMove))
            {
                foreach (CharacterSetting C in Walker.Crew)
                {
                    C.ReduceAllConditionDuration(GlobalEnumerators.DurationMeasurementUnit.Hour, 1);
                }
            }
            Hours += TimeMove;
            if (Hours > HoursInDay)
            {
                EndDay();
            }
            if (Hours > HourEventCheck)
            {
                HourEventCheck += Random.Range(0.5f, 1.5f);
                float EventRoll = Random.Range(0f, 1f);
                float EventChance = (Hours / HoursInDay) * 1.05f / (EventsInDay == 0 ? 1 : Mathf.Pow(EventsInDay + 1, 2f));
                if (EventRoll < EventChance)
                {
                    PlayEvent();
                    EventsInDay++;
                }
            }
            if (Input.GetMouseButtonDown(0))
            {
                if (!MouseOnPanel)
                {
                    if (!isMove)
                    {
                        playerPos = transform.position;
                        BuildPath(Pathfinding.WorldToArray(playerPos), Pathfinding.Cursor, true);

                        if (path != null && path.Count > 1)
                        {
                            isMove = true;
                            getStop = false;
                        }

                        current = Pathfinding.Cursor;
                    }

                    last = Pathfinding.Cursor;
                }
            }
            if (Input.GetKeyDown("space"))
            {
                OnPause = !OnPause;
            }
            PlayerMove();
        }
    }

    Vector3 MoveTowards(Vector3 current, Vector3 target, float maxDistanceDelta, out float magnitude)
    {
        Vector3 a = target - current;
        magnitude = a.magnitude;
        if (magnitude <= maxDistanceDelta || magnitude == 0f)
        {
            magnitude = 0;
            return target;
        }
        return current + a / magnitude * maxDistanceDelta;
    }

    void BuildPath(Vector2Int start, Vector2Int end, bool isDelta)
    {
        moveIndex = 1;
        if (isDelta) if (!isMove) delta = .1f; else delta = 0;
        lastX = start.x;
        lastY = start.y;
        path = Pathfinding.FindPath(start, end, diagonallyMove);
    }

    void PlayerMove()
    {
        if (!isMove) return;

        transform.position = MoveTowards(transform.position, playerPos, speed * Time.deltaTime, out magnitude);

        if (getStop && magnitude == 0)
        {
            playerPos = transform.position;
            AnimationSet(Move.Stop);
            isMove = false;
            return;
        }

        if (magnitude <= delta)
        {
            if (path != null && moveIndex + 1 >= path.Count)
            {
                delta = 0;
            }
            else
            {
                delta = .1f;
            }

            if (path != null && path.Count > 1 && moveIndex < path.Count)
            {
                playerPos = path[moveIndex].worldPosition;

                SwitchAnim();

                if (moveIndex + 1 < path.Count)
                {
                    if (lastX != path[moveIndex + 1].x && lastY != path[moveIndex + 1].y)
                    {
                        delta = 0;
                    }
                }

                lastX = path[moveIndex].x;
                lastY = path[moveIndex].y;

                moveIndex++;
            }
            else
            {
                getStop = true;
            }

            if (current != last)
            {
                BuildPath(Pathfinding.WorldToArray(playerPos), last, false);

                if (path == null || path.Count <= 1)
                {
                    getStop = true;
                }

                last = current;
            }
        }
    }

    void SwitchAnim()
    {
        Vector2 type = (playerPos - transform.position).normalized;
        type = new Vector2(Mathf.Round(type.x), Mathf.Round(type.y));

        if (type.x > 0.0f && type.y == 0.0f)
        {
            // MOVE RIGHT
            AnimationSet(Move.Right);
        }
        else if (type.x < 0.0f && type.y == 0.0f)
        {
            // MOVE LEFT
            AnimationSet(Move.Left);
        }
        else if (type.x == 0.0f && type.y > 0.0f)
        {
            // MOVE UP
            AnimationSet(Move.Up);
        }
        else if (type.x == 0.0f && type.y < 0.0f)
        {
            // MOVE DOWN
            AnimationSet(Move.Down);
        }
        else if (type.x < 0.0f && type.y < 0.0f)
        {
            // MOVE DOWN LEFT
            AnimationSet(Move.DownLeft);
        }
        else if (type.x > 0.0f && type.y < 0.0f)
        {
            // MOVE DOWN RIGHT
            AnimationSet(Move.DownRight);
        }
        else if (type.x < 0.0f && type.y > 0.0f)
        {
            // MOVE UP LEFT
            AnimationSet(Move.UpLeft);
        }
        else if (type.x > 0.0f && type.y > 0.0f)
        {
            // MOVE UP RIGHT
            AnimationSet(Move.UpRight);
        }
    }

    void AnimationSet(Move val)
    {
        switch (val)
        {
            case Move.Left:
                animator.Play("walk_left");
                break;

            case Move.Right:
                animator.Play("walk_right");
                break;

            case Move.Up:
                animator.Play("walk_up");
                break;

            case Move.Down:
                animator.Play("walk_down");
                break;

            case Move.DownLeft:
                animator.Play("walk_down_left");
                break;

            case Move.DownRight:
                animator.Play("walk_down_right");
                break;

            case Move.UpLeft:
                animator.Play("walk_up_left");
                break;

            case Move.UpRight:
                animator.Play("walk_up_right");
                break;

            case Move.Stop:
                switch (lastMoved)
                {
                    case Move.Left:
                        animator.Play("idle_left");
                        break;

                    case Move.Right:
                        animator.Play("idle_right");
                        break;

                    case Move.Up:
                        animator.Play("idle_up");
                        break;

                    case Move.Down:
                        animator.Play("idle_down");
                        break;

                    case Move.DownLeft:
                        animator.Play("idle_down_left");
                        break;

                    case Move.DownRight:
                        animator.Play("idle_down_right");
                        break;

                    case Move.UpLeft:
                        animator.Play("idle_up_left");
                        break;

                    case Move.UpRight:
                        animator.Play("idle_up_right");
                        break;
                }
                break;
        }

        lastMoved = val;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Sity")
        {
            MouseOnPanel = true;
            OnPause = true;
            SityPanel.SetActive(true);
            Panel.SetActive(false);
            isMove = false;
            SityCollider Sity = collision.gameObject.GetComponent<SityCollider>(); // Получаем компонент в переменную Sity.
            PrintedSityType.text = Sity.Sity.SityName + " (" + Sity.SityName + ")";

        }
        else
        {
            AreaCollider Area = collision.gameObject.GetComponent<AreaCollider>(); // Получаем компонент в переменную Area.
            float WalkerSpeedModAbsUsed = 0;
            float WalkerSpeedModProcUsed = 1;
            for (int i = 0; i < Walker.AreaSpeedMod.Length; i++)
            {
                if (Walker.AreaSpeedMod[i].AreaType == Area.Area.AreaType)
                {
                    WalkerSpeedModAbsUsed = Walker.AreaSpeedMod[i].ModSpeedAbs;
                    WalkerSpeedModProcUsed = Walker.AreaSpeedMod[i].ModSpeedProc;
                }
            }
            this.speed = (this.Walker.SpeedBase + Area.Area.ModSpeedAbs + WalkerSpeedModAbsUsed) * Area.Area.ModSpeedProc * WalkerSpeedModProcUsed;
            PrintedAreaType.text = Area.Area.AreaName + " (" + Area.AreaName + ")";
            float ModSpeedSumm = this.speed - this.Walker.SpeedBase;
            AreaModValue.text = ModSpeedSumm.ToString();
            CurrentSpeedValue.text = this.speed.ToString() + MouseOnPanel.ToString();
            //Debug.Log(collision.gameObject.GetComponent<AreaCollider>().Area.AreaType);
        }
    }
    void ChangeWalkerInfoText()
    {
        //CurrentWalkerType.text = this.Walker.WalkerType;
        CurrentWalkerType.text = this.Walker.WalkerType.ToString();
        WalkerName.text = this.Walker.Name;
    }
    public bool OnPause
    {
        get => _OnPause;
        set
        {
            Time.timeScale = value ? 0 : 1;
            _OnPause = value;
            PausePanel.SetActive(value);
        }
    }
    public void EndDay()
    {
        Days++;
        Hours = 0;
        EventsInDay = 0;
        HourEventCheck = 0;
        foreach (CharacterSetting C in Walker.Crew)
        {
            C.ReduceAllConditionDuration(GlobalEnumerators.DurationMeasurementUnit.Day, 1);
        }

    }
    public void PlayEvent()
    {
        Debug.Log("Произошло событие, день/час: " + Days.ToString() + " / " + Hours.ToString("0.####"));
    }

}
