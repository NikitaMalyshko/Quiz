using static System.Console;
using System.Collections.Generic;
using System.Linq;
using System;

// Пользователь
class User
{
    public string Login { get; set; } // Логин 
    public string Password { get; set; } // Пароль
    public DateTime BirthDate { get; set; } // ДР
    public List<QuizResult> QuizHistory { get; set; }

    public User(string login, string password, DateTime birthDate)
    {
        this.Login = login; 
        this.Password = password;
        this.BirthDate = birthDate;
        this.QuizHistory = new List<QuizResult>(); // Инициализация пустой строки
    }
}

// Хранение результатов
class QuizResult
{
    public string QuizType { get; set; } // Тип викторины
    public int CorrectAnswers { get; set; } // Кол-во правильных ответов
    public DateTime Date { get; set; } // Дата прохождения
}

// Вопросы
class Question
{
    public string Text { get; set; } //Содержание вопросов
    public List<string> Options { get; set; } //Варианты ответов
    public List<int> CorrectAnswers { get; set; } //Индексы правильных ответов
}

// Викторина
class Quiz
{
    public string Type { get; set; } // Название викторин
    public List<Question> Questions { get; set; } // Список викторин
}

class Program
{
    static List<User> users = new List<User>();
    static List<Quiz> quizzes = new List<Quiz>();
    static User currentUser = null;

    //Массивы
    //Вопросы
    static readonly string[] historyQuestions = new string[]
    {
        "1. В каком году началась Вторая мировая война?",
        "2. Кто был первым президентом США?",
        "3. В каком году произошла Октябрьская революция в России?",
        "4. Кто был первым императором Римской империи?",
        "5. В каком году пала Византийская империя?",
        "6. Кто открыл Америку?",
        "7. В каком году началась Первая мировая война?",
        "8. Кто был первым космонавтом?",
        "9. В каком году был основан Санкт-Петербург?",
        "10. Кто был первым царём династии Романовых?",
        "11. Кто был последним российским императором?",
        "12. В каком году была крещена Русь?",
        "13. Кто автор 'Повести временных лет'?",
        "14. Какой народ основал Карфаген?",
        "15. Кто правил Францией во время Наполеоновских войн?",
        "16. Где произошло первое сражение Великой Отечественной войны?",
        "17. Кто был первым канцлером объединённой Германии?",
        "18. Как звали жену Ивана Грозного?",
        "19. Кто завоевал Византию в 1453 году?",
        "20. Где проходил Ялтинский мир?",
    };
    static readonly string[] geographyQuestions = new string[]
    {
        "1. Какая самая длинная река в мире?",
        "2. Какая река протекает через Москву?",
        "3. Какая самая высокая гора?",
        "4. Какая страна занимает наибольшую площадь?",
        "5. Столица Канады?",
        "6. Где находится Сахара?",
        "7. Какая страна имеет наибольшее количество людей?",
        "8. Какое море самое соленое в мире",
        "9. Где находится Гранд-Каньон?",
        "10. Какая страна разделена на 17 тысяч островов?",
        "11. Какой материк является самым большим по площади?",
        "12. Какой океан самый глубокий?",
        "13. Какая страна имеет самую длинную береговую линию?",
        "14. Какой город является столицей Австралии?",
        "15. Какая страна имеет форму сапога на карте?",
        "16. В какой стране находится гора Эверест?",
        "17. На каком материке находится озеро Виктория?",
        "18. Как называется самый высокий водопад в мире?",
        "19. Какая пустыня является самой большой по площади на Земле?",
        "20. В каком полушарии находится Бразилия?",
    };
    static readonly string[] biologyQuestions = new string[]
    {
        "1. Какой орган отвечает за фильтрацию крови",
        "2. К какому царству относятся бактерии",
        "3. Какой орган животного отвечает за поддержание равновесия и ориентации в пространстве?",
        "4. Какое вещество необходимо растениям для фотосинтеза",
        "5. Какой организм не имеет ядра в клетке",
        "6. Какой из органов является центральной частью нервной системы",
        "7. Сколько хромосом у человека",
        "8. Какой витамин вырабатывается под действием солнечного света",
        "9. Какие клетки отвечают за транспорт кислорода в крови",
        "10. Какой ученый считается основателем генетики",
        "11. Какая структура у насекомых отвечает за обоняние",
        "12. Какой организм способен к регенерации целого тела из одной части",
        "13. Какая часть мозга отвечает за координацию движений",
        "14. Как называется тип симбиотических отношений, при которых один организм получает пользу, а другой не страдает",
        "15. Какой тип ткани проводит воду и минеральные вещества в растении",
        "16. Сколько пар хромосом у человека",
        "17. Какая железа вырабатывает инсулин",
        "18. Как называется научное название человека как биологического вида",
        "19. Какой процесс обеспечивает превращение солнечной энергии в химическую в растениях?",
        "20. Какая молекула является носителем наследственной информации",
    };

    //Ответы
    static readonly string[][] historyAnswers = new string[][]
    {
        new string[] { "1939", "Что?", "1941", "Рассказать, когда началась Волынская резня" },
        new string[] { "Джордж Вашингтон", "Томас Джефферсон", "Авраам Линкольн", "Джон Адамс" },
        new string[] { "1917", "1918", "1916", "1915" },
        new string[] { "Октавиан Август", "Юлий Цезарь", "Нерон", "Калигула" },
        new string[] { "1453", "1454", "1452", "1455" },
        new string[] { "Христофор Колумб", "Америго Веспуччи", "Васко да Гама", "Фернан Магеллан" },
        new string[] { "1914", "1915", "1913", "1916" },
        new string[] { "Юрий Гагарин", "Алан Шепард", "Герман Титов", "Нил Армстронг" },
        new string[] { "1703", "1700", "1705", "1701" },
        new string[] { "Михаил Фёдорович", "Алексей Михайлович", "Пётр I", "Фёдор Иоаннович" },
        new string[] { "Николай II", "Александр III", "Петр II", "Павел I" },
        new string[] { "988", "1010", "1054", "980" },
        new string[] { "Нестор", "Кирилл", "Владимир Мономах", "Лев Толстой" },
        new string[] { "Финикийцы", "Египтяне", "Греки", "Римляне" },
        new string[] { "Наполеон Бонапарт", "Людовик XVI", "Карл Великий", "Робеспьер" },
        new string[] { "Бресткая крепость", "Москва", "Сталинград", "Курск" },
        new string[] { "Отто фон Бисмарк", "Гельмут Коль", "Австрийский художник", "Фридрих Великий" },
        new string[] { "Анастасия Романова", "Екатерина Великая", "Мария Федоровна", "Елизавета Петровна" },
        new string[] { "Османы", "Франки", "Русские", "Персы" },
        new string[] { "Крым", "Германия", "США", "Англия" }
    };
    static readonly string[][] geographyAnswers = new string[][]
    {
        new string[]{"Амазонка", "Нил", "Янцзы", "Миссисипи", },
        new string[]{"Москва-река", "Волга", "Дон", "Нева", },
        new string[]{"Эверест", "Килиманджаро", "Эльбрус", "Мак-Кинли", },
        new string[]{"Россия", "Канада", "США", "Китай", },
        new string[]{"Оттава", "Торонто", "Ванкувер", "Монреаль", },
        new string[]{"Африка", "Азия", "Южная Америка", "Австралия", },
        new string[]{"Китай", "Германия", "Россия", "Франция", },
        new string[]{"Мертвое", "Каспийское", "Красное", "Средиземное", },
        new string[]{"США", "Канада", "Аргентина", "ЮАР", },
        new string[]{"Индонезия", "Япония", "Филиппины", "Мальдивы", },
        new string[]{"Азия", "Европа", "Северная Америка", "Африка", },
        new string[]{"Тихий", "Северный Ледовитый", "Индийский", "Атлантический", },
        new string[]{"Канада", "Австралия", "Россия", "Индонезия", },
        new string[]{"Канберра", "Сидней", "Мельбурн", "Брисбен", },
        new string[]{"Италия", "Франция", "Греция", "Испания", },
        new string[]{"Непал", "Китай", "Пакистан", "Индия", },
        new string[]{"Африка", "Южная Америка", "Азия", "Австралия", },
        new string[]{"Анхель", "Ниагарский", "Виктория", "Игуасу", },
        new string[]{"Антарктическая", "Сахара", "Гоби", "Каракумы", },
        new string[]{"Южное", "Северное", "Восточное", "Западное", },
    };
    static readonly string[][] biologyAnswers = new string[][]
    {
        new string[]{"Почки", "Легкие", "Печень", "Седце", },
        new string[]{"Прокариоты", "Животные", "Грибы", "Растения", },
        new string[]{"Вестибулярный аппарат", "Сердце", "Печень", "Глаза", },
        new string[]{"Углекислый газ", "Водород", "Азот", "Кислород", },
        new string[]{"Бактерия", "Вирус", "Амеба", "Гриб", },
        new string[]{"Спинной мозг", "Сердце", "Легкие", "Печень", },
        new string[]{"46", "44", "23", "48", },
        new string[]{"D", "B", "C", "A", },
        new string[]{"Эритроциты", "Плазма", "Тромбоциты", "Лейкоциты", },
        new string[]{"Грегор Мендель", "Чарльз Дарвин", "Роберт Кох", "Луи Пастер", },
        new string[]{"Антенны (усики)", "Глазки", "Хоботок", "Крылья", },
        new string[]{"Планария", "", "", "", },
        new string[]{"Мозжечок", "Гипофиз", "Гипоталамус", "Продолговатый мозг", },
        new string[]{"Комменсализм", "Мутуализм", "Конкуренция", "Паразитизм", },
        new string[]{"Ксилема", "Луб", "Мезофилл", "Камбий", },
        new string[]{"23", "22", "46", "24", },
        new string[]{"Поджелудочная", "Гипофиз", "Щитовидная", "Надпочечники", },
        new string[]{"Homo sapiens", "Homo erectus", "Homo naturalis", "Homo habilis", },
        new string[]{"Фотосинтез", "Испарение", "Дыхание", "Разложение", },
        new string[]{"ДНК", "РНК", "Глюкоза", "Белок", },
    };

    static void Main()
    {
        InitializeQuizzes(); // Заполнение списков викторин

        // Основной цикл программы (Это база)
        while (true)
        {
            if (currentUser == null)
                ShowLoginMenu(); // Показ меню входа/регистрации
            else
                ShowMainMenu(); // Показ основного меню
        }
    }

    // Инициализация викторин
    static void InitializeQuizzes()
    {
        quizzes.Clear();

        // Создание и добавление викторин с вопросами
        quizzes.Add(new Quiz
        {
            Type = "История",
            Questions = CreateQuestions(historyQuestions, historyAnswers)
        });

        quizzes.Add(new Quiz
        {
            Type = "География",
            Questions = CreateQuestions(geographyQuestions, geographyAnswers)
        });

        quizzes.Add(new Quiz
        {
            Type = "Биология",
            Questions = CreateQuestions(biologyQuestions, biologyAnswers)
        });
    }
    // Создание списка вопросов на основе массивов текстов и ответов
    static List<Question> CreateQuestions(string[] questionTexts, string[][] answers)
    {
        var questions = new List<Question>();
        for (int i = 0; i < questionTexts.Length; i++)
        {
            var question = new Question
            {
                Text = questionTexts[i],
                Options = answers[i].ToList(),
                CorrectAnswers = new List<int> { 0 } // По умолчанию первый вариант правильный
            };
            questions.Add(question);
        }
        return questions;
    }


    static Quiz CreateDummyQuiz(string type)
    {
        return new Quiz
        {
            Type = type,
            Questions = Enumerable.Range(1, 10).Select(i => new Question
            {
                Text = $"{type} вопрос {i}?",
                Options = new List<string> { "Вариант 1", "Вариант 2", "Вариант 3", "Вариант 4" },
                CorrectAnswers = new List<int> { 0 }
            }).ToList()
        };
    }

    // Меню входа
    static void ShowLoginMenu()
    {
        Clear();
        WriteLine("1. Вход\n2. Регистрация\n3. Выход");
        string choice = ReadLine();

        switch (choice)
        {
            case "1": 
                Login(); 
                break;
            case "2": 
                Register(); 
                break;
            case "3": 
                Environment.Exit(0); 
                break;
        }
    }

    // Авторизация
    static void Login()
    {
        Write("Введите логин: ");
        string login = ReadLine();
        Write("Введите пароль: ");
        string password = ReadLine();

        // Поиск пользователя по логину и паролю
        currentUser = users.FirstOrDefault(u => u.Login == login && u.Password == password);

        if (currentUser == null)
        {
            WriteLine("Неверный логин или пароль! (Возможно ваши данные отсутствуют в БД)");
            ReadKey();
        }
    }

    // Регистрация
    static void Register()
    {
        Write("Введите логин: ");
        string login = ReadLine();

        if (users.Any(u => u.Login == login))
        {
            WriteLine("Такой логин уже существует!");
            ReadKey();
            return;
        }

        Write("Введите пароль: ");
        string password = ReadLine();

        Write("Введите дату рождения (дд.мм.гггг): ");
        if (DateTime.TryParse(ReadLine(), out DateTime birthDate))
        {
            users.Add(new User(login, password, birthDate));
            WriteLine("Регистрация успешна!");
        }
        else
        {
            WriteLine("Неверный формат даты!");
        }

        ReadKey();
    }

    // Меню викторины
    static void ShowMainMenu()
    {
        Clear();
        WriteLine($"Добро пожаловать, {currentUser.Login}!");
        WriteLine("1. Начать викторину\n2. История викторин\n3. Топ-20\n4. Настройки\n5. Выход");

        switch (ReadLine())
        {
            case "1": 
                StartQuiz(); 
                break;
            case "2": 
                ShowHistory(); 
                break;
            case "3": 
                ShowTop20(); 
                break;
            case "4": 
                ShowSettings(); 
                break;
            case "5": 
                currentUser = null; 
                break;
        }
    }

    // Старт викторины
    static void StartQuiz()
    {
        Clear();
        WriteLine("Выберите викторину:");
        for (int i = 0; i < quizzes.Count; i++)
        {
            WriteLine($"{i + 1}. {quizzes[i].Type}");
        }

        WriteLine($"{quizzes.Count + 1}. Случайная викторина");

        if (int.TryParse(ReadLine(), out int choice) && choice >= 1 && choice <= quizzes.Count + 1)
        {
            var quiz = (choice == quizzes.Count + 1) ? quizzes[new Random().Next(quizzes.Count)] : quizzes[choice - 1];
            RunQuiz(quiz);
        }
    }

    // Само прохождение викторины
    static void RunQuiz(Quiz quiz)
    {
        int correct = 0; // Счетчик правильных ответов

        foreach (var q in quiz.Questions)
        {
            Clear();
            WriteLine(q.Text);
            for (int i = 0; i < q.Options.Count; i++) 
            {
                WriteLine($"{i + 1}. {q.Options[i]}");
            }

            Write("Введите номер ответа: ");

            // Чтение и обработка ответа пользователя
            var input = ReadLine()?.Split().Where(s => int.TryParse(s, out _)).Select(s => int.Parse(s) - 1).ToList();

            // Проверка правильности ответа
            if (input != null && q.CorrectAnswers.OrderBy(x => x).SequenceEqual(input.OrderBy(x => x)))
            {
                correct++;
            }
        }

        // Сохранение результата
        currentUser.QuizHistory.Add(new QuizResult
        {
            QuizType = quiz.Type,
            CorrectAnswers = correct,
            Date = DateTime.Now
        });

        WriteLine($"\nВы завершили викторину! Результат: {correct} из {quiz.Questions.Count}");
        ReadKey();
    }
    
    // Показ истории прохождения викторин пользователем
    static void ShowHistory()
    {
        Clear();
        if (currentUser.QuizHistory.Count == 0)
        {
            WriteLine("История пуста.");
        }
        else
        {
            foreach (var r in currentUser.QuizHistory)
            {
                WriteLine($"Тип: {r.QuizType}, Баллы: {r.CorrectAnswers}, Дата: {r.Date:dd.MM.yyyy HH:mm}");
            }
        }
        ReadKey();
    }

    // Показ топа 20 результатов по выбранной викторине
    static void ShowTop20()
    {
        Clear();
        WriteLine("Выберите викторину:");
        for (int i = 0; i < quizzes.Count; i++)
            WriteLine($"{i + 1}. {quizzes[i].Type}");

        if (int.TryParse(ReadLine(), out int choice) && choice >= 1 && choice <= quizzes.Count)
        {
            var type = quizzes[choice - 1].Type;
            
            // Формирование топа результатов
            var top = users.SelectMany(u => u.QuizHistory.Select(h => new { u.Login, h }))
                .Where(x => x.h.QuizType == type)
                .OrderByDescending(x => x.h.CorrectAnswers)
                .ThenBy(x => x.h.Date)
                .Take(20)
                .ToList();

            Clear();
            WriteLine($"ТОП-20: {type}\n");

            int rank = 1;
            foreach (var item in top)
                WriteLine($"{rank++}. {item.Login} - {item.h.CorrectAnswers} - {item.h.Date:dd.MM.yyyy HH:mm}");
        }
        ReadKey();
    }

    // Настройки пользователя
    static void ShowSettings()
    {
        Clear();
        WriteLine("1. Изменить пароль\n2. Изменить дату рождения\n3. Назад");
        switch (ReadLine())
        {
            case "1":
                Write("Новый пароль: ");
                currentUser.Password = ReadLine(); // Изменение пароля
                WriteLine("Пароль изменён!");
                break;
            case "2":
                Write("Новая дата рождения (дд.мм.гггг): ");
                if (DateTime.TryParse(ReadLine(), out DateTime newDate))
                {
                    currentUser.BirthDate = newDate; // Изменение даты рождения
                    WriteLine("Дата обновлена.");
                }
                else
                    WriteLine("Неверный формат.");
                break;
        }
        ReadKey();
    }
}
