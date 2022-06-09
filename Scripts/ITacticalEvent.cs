
public interface ITacticalEvent
{
    public delegate void TurnStart(CharacterSetting C);
    public delegate void TurnEnd(CharacterSetting C);
    public delegate void AttackPerformed(CharacterSetting C, CharacterSetting T);
    public delegate void AttackReceived(CharacterSetting C, CharacterSetting T);
    public delegate void OnKill(CharacterSetting C, CharacterSetting T);

    public void TurnStartProcedure(CharacterSetting C);
    public void TurnEndProcedure(CharacterSetting C);
    public void AttackPerformedProcedure(CharacterSetting C, CharacterSetting T);
    public void AttackReceivedProcedure(CharacterSetting C, CharacterSetting T);
    public void OnKillProcedure(CharacterSetting C, CharacterSetting T);
}
/*
//События ITacticalEvent
public ITacticalEvent.TurnStart TurnStart;
public ITacticalEvent.TurnEnd TurnEnd;
public ITacticalEvent.AttackPerformed AttackPerformed;
public ITacticalEvent.AttackReceived AttackReceived;
public ITacticalEvent.OnKill OnKill;

public void TurnStartProcedure(CharacterSetting C)
{
    if (TurnStart != null)
        TurnStart(C);
}
public void TurnEndProcedure(CharacterSetting C)
{
    if (TurnEnd != null)
        TurnEnd(C);
}
public void AttackPerformedProcedure(CharacterSetting C, CharacterSetting T)
{
    if (AttackPerformed != null)
        AttackPerformed(C, T);
}
public void AttackReceivedProcedure(CharacterSetting C, CharacterSetting T)
{
    if (AttackReceived != null)
        AttackReceived(C, T);
}
public void OnKillProcedure(CharacterSetting C, CharacterSetting T)
{
    if (OnKill != null)
        OnKill(C, T);
}
*/