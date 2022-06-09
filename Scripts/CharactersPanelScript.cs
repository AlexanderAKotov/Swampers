using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharactersPanelScript : MonoBehaviour
{
    public GameObject CharactersPanel; //Панель с персонажами и статами.
    public Button Warrior1Button; // Кнопка с иконкой и именем первого бойца в списке.
    public Text Warrior1NameText; // Имя первого бойца на кнопке.
        // Выбранный боец.
    public Text FullNameWarriorText; // Полное имя выбранного бойца.
    public Text RaceDataText; // Раса выбранного бойца.
    public Text StrenghtDataText; // Сила выбранного бойца.
    public Text EnduranceDataText; // Выносливость выбранного бойца.
    public Text AgilityDataText; // Проворность выбранного бойца.
    public Text SpeedDataText; // Скорость и стоимость шага выбранного бойца.
    public Text HPDataText; // Здоровье выбранного бойца.
    public Text InjuryThresholdDataText; // Порог ранения выбранного бойца.
    public Text DodgeDataText; // Шанс уклонения выбранного бойца.
    public Text ParryDataText; // Шанс парирования выбранного бойца.
    public Text DamagDataText; // Урон выбранного бойца.
    public Text AccuracyDataText; // Точность выбранного бойца (первая правая рука).
    public Text CritChanceDataText; // Шанс критического удара выбранного бойца (первая правая рука).
    public Text InitiativeDataText; // Инициатива выбранного бойца.
    //public Text ; //  выбранного бойца.
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
