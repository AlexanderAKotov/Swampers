/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arhive : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void MoveTo(CharacterSetting C, Vector3Int Target)// Идем до целевой клетки, пока не закончится скорость или не дойдем.
    {
        int X = Target.x;
        int Y = Target.y;
        if (!MainBattleScript._Map[X, Y].Property.AllowStanding || MainBattleScript._Map[X, Y].C != null)
        {
            Debug.Log("Клетка занята или недоступна, возврат");
            return;
        }
        int[,] SpeedMap = new int[MainBattleScript.MapWidth, MainBattleScript.MapHeight];
        SpeedMap[X, Y] = 0;
        Debug.Log("Начало заполнения карты скоростей");
        FillSpeedMap(new Vector2Int(X, Y), C, ref SpeedMap, 1);
        Debug.Log("Карта скоростей заполнена");
        if (SpeedMap[C.x, C.y] > 0)
        {
            bool Check = true;
            while (Check)
            {
                Debug.Log("Прогон while");
                Check = false;
                if (C.x != X || C.y != Y)
                {
                    List<Vector2Int> HexList = GetNearbyHexWithMapRangeCheck(new Vector2Int(C.x, C.y));
                    int Current = -1;
                    Vector2Int CurrentHex = new Vector2Int(-1, -1);
                    foreach (Vector2Int Cell in HexList)
                    {
                        if (SpeedMap[Cell.x, Cell.y] >= 0 && (SpeedMap[Cell.x, Cell.y] < Current || Current == -1))
                        {
                            Current = SpeedMap[Cell.x, Cell.y];
                            CurrentHex = Cell;
                        }
                    }
                    if (CurrentHex.x != -1 && CurrentHex.y != -1)
                    {
                        Check = CharacterMove(C, new Vector3Int(CurrentHex.x, CurrentHex.y, 0));
                        Debug.Log("Была попытка встать" + CurrentHex.x.ToString() + " | " + CurrentHex.y.ToString() + " | " + Current.ToString());
                    }
                }
            }
        }
        else
        {
            Debug.Log("Нет прохода");
        }

        /*int Dx = Target.x - C.x;
        int Dy = Target.y - C.y;
        int Distance = GetDistanceBetweenHexes(new Vector3Int(C.x, C.y, 0), new Vector3Int(Target.x, Target.y, 0));
        if (Dx == 0 && Dy == 0)
            return false;
        List<Vector3Int> PotentialCell = new List<Vector3Int>(); //Потенциальные клетки карты, которые ближе к цели
        List<Vector3Int> SameDistanceCell = new List<Vector3Int>(); //Клетки карты, которые не приближают и не отдаляют
        List<Vector3Int> NearbyCell = new List<Vector3Int>(); //Соседние клетки карты
        int EvenY = C.y % 2;
        int SectorNumber; //C верхнего, равного единице, и по часовой.
        bool IsDiagonal;  //
        NearbyCell.Add(new Vector3Int(C.x, C.y + 1, 0));
        NearbyCell.Add(new Vector3Int(C.x, C.y - 1, 0));
        NearbyCell.Add(new Vector3Int(C.x - 1 + EvenY * 2, C.y, 0));
        NearbyCell.Add(new Vector3Int(C.x + 1 - EvenY * 2, C.y, 0));
        NearbyCell.Add(new Vector3Int(C.x - 1 + EvenY * 2, C.y + 1, 0));
        NearbyCell.Add(new Vector3Int(C.x - 1 + EvenY * 2, C.y - 1, 0));
        foreach (Vector3Int Cell in NearbyCell)
        {
            int CurrentDistance = GetDistanceBetweenHexes(Cell, new Vector3Int(Target.x, Target.y, 0));
            if (CurrentDistance < Distance)
                PotentialCell.Add(Cell);
            if (CurrentDistance == Distance)
                SameDistanceCell.Add(Cell);
        }
       
        bool Moved = false;
        Debug.Log(Dx.ToString() + " " + Dy.ToString());
        foreach (Vector3Int Cell in PotentialCell)
            Debug.Log("Потенциальная клетка" + Cell.x.ToString() + " " + Cell.y.ToString());
        foreach (Vector3Int Cell in SameDistanceCell)
            Debug.Log("Обходная клетка" + Cell.x.ToString() + " " + Cell.y.ToString());
        while (!Moved && (PotentialCell.Count > 0 || SameDistanceCell.Count > 0))
        {
            if (PotentialCell.Count > 0)
            {
                int CellNumber = Random.Range(0, PotentialCell.Count);
                Vector3Int CurrentCell = PotentialCell[CellNumber];
                if (CharacterMove(C, CurrentCell))
                {
                    Moved = true;
                    break;
                }
                else
                {
                    PotentialCell.RemoveAt(CellNumber);
                    continue;
                }
            }
            if (SameDistanceCell.Count > 0)
            {
                int CellNumber = Random.Range(0, SameDistanceCell.Count);
                Vector3Int CurrentCell = SameDistanceCell[CellNumber];
                if (CharacterMove(C, CurrentCell))
                {
                    Moved = true;
                    break;
                }
                else
                {
                    SameDistanceCell.RemoveAt(CellNumber);
                    continue;
                }
            }
        }
        return Moved; */
//    }
//}
