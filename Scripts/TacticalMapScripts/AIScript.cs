using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIScript : object
{
    public struct TargetStruct
    {
        public CharacterSetting Target;
        public int x, y, Distance;

        public TargetStruct(CharacterSetting target, int distance)
        {
            Target = target;
            this.x = target.x;
            this.y = target.y;
            Distance = distance;
        }
    }
    public static TargetStruct GetClosestEnemy(CharacterSetting C)// Получение ближайшего врага, если нет, возвращает пустую структуру TargetStruct
    {
        int Distance = 999999;
        TargetStruct Current = new TargetStruct();
        for (int i = 0; i < MainBattleScript.TurnOrder.Count; i++)
        {
            if (MainBattleScript.TurnOrder[i].C.BattleSide != C.BattleSide)
            {
                int CurrentDistance = MainBattleBehaviour.GetDistanceBetweenHexes(new Vector3Int(C.x, C.y, 0), new Vector3Int(MainBattleScript.TurnOrder[i].C.x, MainBattleScript.TurnOrder[i].C.y, 0));
                if (CurrentDistance < Distance)
                {
                    Current = new TargetStruct(MainBattleScript.TurnOrder[i].C, CurrentDistance);
                    Distance = CurrentDistance;
                }
            }
        }
        return Current;
    }
}
