using System;
using System.Collections.Generic;

// Абстрактний клас для ігор, містить абстрактний метод для розрахунку рейтингу
abstract class Game
{
    public abstract int CalculateRating(int player1Rating, int player2Rating);
}

// Клас стандартної гри, спадкоємець класу Game
class StandardGame : Game
{
    // Реалізація методу для розрахунку рейтингу
    public override int CalculateRating(int player1Rating, int player2Rating)
    {
        // Стандартний спосіб розрахунку рейтингу (наприклад, середнє значення рейтингів двох гравців)
        return (player1Rating + player2Rating) / 2;
    }
}

// Клас тренувальної гри, спадкоємець класу Game
class TrainingGame : Game
{
    // Реалізація методу для розрахунку рейтингу
    public override int CalculateRating(int player1Rating, int player2Rating)
    {
        // Для тренувальних ігор рейтинг не змінюється
        return player1Rating;
    }
}

// Клас гри з одним гравцем, спадкоємець класу Game
class SoloGame : Game
{
    // Реалізація методу для розрахунку рейтингу
    public override int CalculateRating(int player1Rating, int player2Rating)
    {
        // У грі з одним гравцем враховується тільки рейтинг першого гравця
        return player1Rating;
    }
}

// Абстрактний клас гравця, містить методи та властивості, спільні для всіх гравців
abstract class GameAccount
{
    public string UserName { get; private set; }
    public int CurrentRating { get; protected set; }
    public int GamesCount { get; private set; }
    private List<GameResult> gameHistory;

    // Конструктор класу, ініціалізує ім'я, поточний рейтинг та лічильник ігор
    public GameAccount(string userName, int initialRating)
    {
        UserName = userName;
        CurrentRating = initialRating;
        GamesCount = 0;
        gameHistory = new List<GameResult>();
    }

    // Абстрактний метод для розрахунку балів гравця
    public abstract void CalculatePoints(bool isWin, int rating);

    // Метод для виграшу гравця в грі
    public void WinGame(string opponentName, Game game)
    {
        int gameRating = game.CalculateRating(CurrentRating, 0); // Припустимо, що рейтинг опонента дорівнює 0
        CalculatePoints(true, gameRating);
        GamesCount++;
        gameHistory.Add(new GameResult(opponentName, true, gameRating));
    }

    // Метод для програшу гравця в грі
    public void LoseGame(string opponentName, Game game)
    {
        int gameRating = game.CalculateRating(CurrentRating, 0); // Припустимо, що рейтинг опонента дорівнює 0
        CalculatePoints(false, gameRating);
        GamesCount++;
        gameHistory.Add(new GameResult(opponentName, false, gameRating));
    }

    // Метод для отримання статистики ігор гравця
    public void GetStats()
    {
        Console.WriteLine($"Історія ігор для користувача {UserName}:");
        for (int i = 0; i < gameHistory.Count; i++)
        {
            string result = gameHistory[i].IsWin ? "перемога" : "поразка";
            Console.WriteLine($"Гра {i + 1}: Проти {gameHistory[i].OpponentName}, Результат: {result}, Рейтинг гри: {gameHistory[i].Rating}");
        }
    }

    // Внутрішній клас для збереження результатів гри
    private class GameResult
    {
        public string OpponentName { get; }
        public bool IsWin { get; }
        public int Rating { get; }

        // Конструктор класу для ініціалізації результатів гри
        public GameResult(string opponentName, bool isWin, int rating)
        {
            OpponentName = opponentName;
            IsWin = isWin;
            Rating = rating;
        }
    }
}

// Клас гравця стандартної гри, спадкоємець класу GameAccount
class StandardGameAccount : GameAccount
{
    // Конструктор класу гравця стандартної гри
    public StandardGameAccount(string userName, int initialRating) : base(userName, initialRating)
    {
    }

    // Реалізація методу для розрахунку балів гравця
    public override void CalculatePoints(bool isWin, int rating)
    {
        // Збільшення рейтингу при виграші, зменшення при програші
        if (isWin)
        {
            CurrentRating += rating;
        }
        else
        {
            CurrentRating -= rating;
        }
    }
}

// Клас гравця з зменшеною штрафною санкцією за програш, спадкоємець класу GameAccount
class ReducedPenaltyGameAccount : GameAccount
{
    // Конструктор класу гравця з зменшеною штрафною санкцією за програш
    public ReducedPenaltyGameAccount(string userName, int initialRating) : base(userName, initialRating)
    {
    }

    // Реалізація методу для розрахунку балів гравця
    public override void CalculatePoints(bool isWin, int rating)
    {
        // Збільшення рейтингу при виграші, штраф зменшено на половину при програші
        if (isWin)
        {
            CurrentRating += rating;
        }
        else
        {
            CurrentRating -= rating / 2;
        }
    }
}

// Фабрика для створення об'єктів гри
class GameFactory
{
    // Метод для створення об'єкта гри з використанням фабрики
    public Game CreateGame(IGameFactory factory)
    {
        return factory.CreateGame();
    }

    // Метод для конвертації об'єкта гравця конкретного класу до базового типу
    public GameAccount ConvertToBaseType(GameAccountSpecific gameAccountSpecific)
    {
        return gameAccountSpecific; // Припустимо, що є неявне перетворення або надати метод конвертації
    }
}

// Інтерфейс для фабрики гри
interface IGameFactory
{
    Game CreateGame();
}

// Клас фабрики стандартної гри
class StandardGameFactory : IGameFactory
{
    // Метод для створення стандартної гри
    public Game CreateGame()
    {
        return new StandardGame();
    }
}

// Клас фабрики тренувальної гри
class TrainingGameFactory : IGameFactory
{
    // Метод для створення тренувальної гри
    public Game CreateGame()
    {
        return new TrainingGame();
    }
}

// Клас фабрики гри з одним гравцем
class SoloGameFactory : IGameFactory
{
    // Метод для створення гри з одним гравцем
    public Game CreateGame()
    {
        return new SoloGame();
    }
}

// Клас гравця конкретного класу, що розширює функціонал базового класу гравця
class GameAccountSpecific : GameAccount
{
    // Конструктор класу гравця конкретного класу
    public GameAccountSpecific(string userName, int initialRating) : base(userName, initialRating)
    {
    }

    // Implementing the abstract method for calculating player's points
    public override void CalculatePoints(bool isWin, int rating)
    {
        // Implement the logic for calculating points based on win/loss and game rating
        // For example:
        if (isWin)
        {
            CurrentRating += rating;
        }
        else
        {
            CurrentRating -= rating;
        }
    }
}

// Головний клас програми
class Program
{
    // Головний метод програми
    static void Main(string[] args)
    {
        // Створення двох гравців різних типів
        GameAccount player1 = new StandardGameAccount("Гравець1", 1000);
        GameAccount player2 = new ReducedPenaltyGameAccount("Гравець2", 1200);

        // Створення різних типів ігор
        Game standardGame = new StandardGame();
        Game trainingGame = new TrainingGame();
        Game soloGame = new SoloGame();

        // Симуляція ігор
        player1.WinGame(player2.UserName, standardGame);
        player2.LoseGame(player1.UserName, standardGame);

        player1.LoseGame(player2.UserName, trainingGame);
        player2.WinGame(player1.UserName, trainingGame);

        player1.WinGame(player2.UserName, soloGame);
        player2.LoseGame(player1.UserName, soloGame);

        // Виведення статистики для кожного гравця
        Console.WriteLine("Статистика для Гравця 1:");
        player1.GetStats();

        Console.WriteLine("\nСтатистика для Гравця 2:");
        player2.GetStats();
    }
}
