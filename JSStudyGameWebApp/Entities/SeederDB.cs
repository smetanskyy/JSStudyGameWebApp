using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JSStudyGameWebApp.Entities
{
    public class SeederDB
    {
        private static void SeedGame(AppDbContext context)
        {
            #region PlayerInfo
            Player admin = new Player
            {
                Login = "StepanSm",
                Email = "ivanna.pugaiko@gmail.com",
                Password = "Stepan123456",
                IsAdmin = true
            };
            var player = context.Players.SingleOrDefault(p => p.Login == admin.Login);
            if (player == null)
            {
                context.Add(admin);
                context.SaveChanges();
            }
            else
                admin.Id = player.Id;

            var playerAddInfo = context.PlayersAdditionalInfo.SingleOrDefault(p => p.IdPlayerAdditionalInfo == admin.Id);

            PlayerAdditionalInfo adminAddInfo = new PlayerAdditionalInfo
            {
                IdPlayerAdditionalInfo = admin.Id,
                Name = "Stepan",
                Surname = "Smetanskyy",
                Photo = "stepanPhoto.jpg",
                BirthDate = new DateTime(1988, 4, 24),
                Gender = true
            };

            if (playerAddInfo == null)
            {
                context.Add(adminAddInfo);
                context.SaveChanges();
            }
            #endregion

            #region Sections
            List<Section> sections = new List<Section>();
            sections.Add(new Section
            {
                IdSection = 1,
                NameOFSection = "Введение"
            });
            sections.Add(new Section
            {
                IdSection = 2,
                NameOFSection = "Основы JavaScript"
            });
            sections.Add(new Section
            {
                IdSection = 3,
                NameOFSection = "Качество кода"
            });
            sections.Add(new Section
            {
                IdSection = 4,
                NameOFSection = "Объекты: основы"
            });
            sections.Add(new Section
            {
                IdSection = 5,
                NameOFSection = "Типы данных"
            });
            sections.Add(new Section
            {
                IdSection = 6,
                NameOFSection = "Продвинутая работа с функциями"
            });
            sections.Add(new Section
            {
                IdSection = 7,
                NameOFSection = "Свойства объекта, их конфигурация"
            });
            sections.Add(new Section
            {
                IdSection = 8,
                NameOFSection = "Прототипы, наследование"
            });
            sections.Add(new Section
            {
                IdSection = 9,
                NameOFSection = "Классы"
            });
            sections.Add(new Section
            {
                IdSection = 10,
                NameOFSection = "Обработка ошибок"
            });
            sections.Add(new Section
            {
                IdSection = 11,
                NameOFSection = "Промисы, async/await"
            });
            sections.Add(new Section
            {
                IdSection = 12,
                NameOFSection = "Генераторы, продвинутая итерация"
            });
            sections.Add(new Section
            {
                IdSection = 13,
                NameOFSection = "Модули"
            });
            sections.Add(new Section
            {
                IdSection = 14,
                NameOFSection = "Разное"
            });

            foreach (var section in sections)
            {
                var sectionDB = context.Sections.SingleOrDefault(s => s.NameOFSection == section.NameOFSection);
                if (sectionDB == null)
                {
                    context.Add(section);
                    context.SaveChanges();
                }
            }
            #endregion

            #region Tests
            List<Test> tests = new List<Test>();
            tests.Add(new Test
            {
                Id = 1,
                IdSection = 1,
                Question = "Когда JavaScript создавался, у него было другое имя. Какое?",
                AnswerB = "\"LiveScript\"",
                CorrectAnswer = "\"LiveScript\"",
                AnswerA = "\"Live\"",
                AnswerC = "\"Script\"",
                AnswerD = "\"Java\"",
                Reference = "https://learn.javascript.ru/intro"
            });
            tests.Add(new Test
            {
                Id = 2,
                IdSection = 1,
                Question = "JavaScript и Java это одно и тоже?",
                CorrectAnswer = "Нет",
                AnswerA = "Да",
                AnswerB = "Нет",
                AnswerC = "Мама где море",
                AnswerD = "Пойду в солдаты",
                Reference = "https://learn.javascript.ru/intro"
            });
            tests.Add(new Test
            {
                Id = 3,
                IdSection = 1,
                Question = "Какое полное название JavaScript?",
                CorrectAnswer = "ECMAScript",
                AnswerA = "SomeScript",
                AnswerB = "ECMAScript",
                AnswerC = "JS the best",
                AnswerD = "Strip",
                Reference = "https://learn.javascript.ru/intro"
            });
            tests.Add(new Test
            {
                Id = 4,
                IdSection = 1,
                Question = "Назовите спецификацию которая содержит самую глубокую, детальную и формализованную информацию о JavaScript?",
                AnswerA = "Спецификация для чайников",
                AnswerB = "Помню на английском было",
                AnswerC = "Спецификация ECMA-262",
                AnswerD = "Спецификация CI4",
                CorrectAnswer = "Спецификация ECMA-262",
                Reference = "https://learn.javascript.ru/manuals-specifications"
            });
            tests.Add(new Test
            {
                Id = 5,
                IdSection = 1,
                Question = "Назовите популярные редакторы кода?",
                AnswerA = "Visual Studio Code; Atom; Sublime Text; Notepad++; Vim и Emacs",
                AnswerB = "Не будем ссориться",
                AnswerC = "Нужна спросить у Брендана Эйха",
                AnswerD = "Ответ где то рядом",
                CorrectAnswer = "Visual Studio Code; Atom; Sublime Text; Notepad++; Vim и Emacs",
                Reference = "https://learn.javascript.ru/code-editors"
            });
            tests.Add(new Test
            {
                Id = 6,
                IdSection = 1,
                Question = "В большинстве браузеров, работающих под Windows, инструменты разработчика можно открыть, нажав: ",
                AnswerA = "F12",
                AnswerB = "Кнопку дверного звонка",
                AnswerC = "Esc",
                AnswerD = "F1",
                CorrectAnswer = "F12",
                Reference = "https://learn.javascript.ru/devtools"
            });
            tests.Add(new Test
            {
                Id = 7,
                IdSection = 2,
                Question = "Как отобразит сообщение \"Я JavaScript!\":",
                AnswerA = "cout( \"Я JavaScript!\" );",
                AnswerB = "WriteLine( \"Я JavaScript!\" );",
                AnswerC = "MessageBox.Show( \"Я JavaScript!\" );",
                AnswerD = "alert( \"Я JavaScript!\" );",
                CorrectAnswer = "alert( \"Я JavaScript!\" );",
                Reference = "https://learn.javascript.ru/hello-world"
            });
            tests.Add(new Test
            {
                Id = 8,
                IdSection = 2,
                Question = "Для добавления кода JavaScript на страницу используется тег:",
                CorrectAnswer = "<script>",
                AnswerA = "<crisps>",
                AnswerB = "<javacrisps>",
                AnswerC = "<script>",
                AnswerD = "<javascript>",
                Reference = "https://learn.javascript.ru/hello-world"
            });
            tests.Add(new Test
            {
                Id = 9,
                IdSection = 2,
                Question = "Скрипт во внешнем файле можно вставить с помощью:",
                AnswerA = "<crisps src=\"path/to/script.js\"></script>",
                AnswerB = "<script src=\"path/to/script.js\"></script>",
                AnswerC = "<script url=\"path/to/script.js\"></script>",
                AnswerD = "<javascript src=\"path/to/script.js\"></script>",
                CorrectAnswer = "<script src=\"path/to/script.js\"></script>",
                Reference = "https://learn.javascript.ru/hello-world"
            });
            tests.Add(new Test
            {
                Id = 10,
                IdSection = 2,
                Question = "Какой правильный способ для комментарии в JavaScript?",
                AnswerA = "<!-- Это комментарий. -->",
                AnswerB = "// Это комментарий.",
                AnswerC = "Ответы B и D",
                AnswerD = "/* Это комментарий. */",
                CorrectAnswer = "Ответы B и D",
                Reference = "https://learn.javascript.ru/structure"
            });
            tests.Add(new Test
            {
                Id = 11,
                IdSection = 2,
                Question = "Мы можем объявить переменные для хранения данных с помощью ключевых слов:",
                AnswerA = "var, let или const",
                AnswerB = "int, char, bool",
                AnswerC = "short, long",
                AnswerD = "float, double, decimal",
                CorrectAnswer = "var, let или const",
                Reference = "https://learn.javascript.ru/variables"
            });
            tests.Add(new Test
            {
                Id = 12,
                IdSection = 2,
                Question = "В JavaScript есть ограничения, касающиеся имён переменных. Назовите их:",
                AnswerA = "Имя переменной должно содержать только буквы, цифры или символы $ и _",
                AnswerB = "Ответы A и D",
                AnswerC = "Имя переменной должно содержать только буквы, цифры или символы",
                AnswerD = "Первый символ не должен быть цифрой",
                CorrectAnswer = "Ответы A и D",
                Reference = "https://learn.javascript.ru/variables"
            });
            tests.Add(new Test
            {
                Id = 13,
                IdSection = 2,
                Question = "Назовите основные типы в JavaScript",
                AnswerA = "number, string, boolean, null, undefined, object, symbol",
                AnswerB = "number, string, boolean",
                AnswerC = "number, string, boolean, null",
                AnswerD = "number, string, boolean, null, undefined",
                CorrectAnswer = "number, string, boolean, null, undefined, object, symbol",
                Reference = "https://learn.javascript.ru/types"
            });
            tests.Add(new Test
            {
                Id = 14,
                IdSection = 2,
                Question = "Что выведет этот скрипт?\n   let name = \"Ilya\";\n   alert( `hello ${\"name\"}` );",
                AnswerA = "hello 1",
                AnswerB = "hello name",
                AnswerC = "hello Ilya",
                AnswerD = "hello",
                CorrectAnswer = "hello name",
                Reference = "https://learn.javascript.ru/types"
            });
            tests.Add(new Test
            {
                Id = 15,
                IdSection = 2,
                Question = "Оператор typeof возвращает тип аргумента. Результатом вызова typeof null будет:",
                AnswerA = "undefined",
                AnswerB = "symbol",
                AnswerC = "object",
                AnswerD = "function",
                CorrectAnswer = "object",
                Reference = "https://learn.javascript.ru/types"
            });
            tests.Add(new Test
            {
                Id = 16,
                IdSection = 2,
                Question = "",
                AnswerA = "",
                AnswerB = "",
                AnswerC = "",
                AnswerD = "",
                CorrectAnswer = "",
                Reference = ""
            });
            tests.Add(new Test
            {
                Id = 17,
                IdSection = 2,
                Question = "",
                AnswerA = "",
                AnswerB = "",
                AnswerC = "",
                AnswerD = "",
                CorrectAnswer = "",
                Reference = ""
            });
            tests.Add(new Test
            {
                Id = 18,
                IdSection = 2,
                Question = "",
                AnswerA = "",
                AnswerB = "",
                AnswerC = "",
                AnswerD = "",
                CorrectAnswer = "",
                Reference = ""
            });
            tests.Add(new Test
            {
                Id = 19,
                IdSection = 2,
                Question = "",
                AnswerA = "",
                AnswerB = "",
                AnswerC = "",
                AnswerD = "",
                CorrectAnswer = "",
                Reference = ""
            });
            tests.Add(new Test
            {
                Id = 20,
                IdSection = 2,
                Question = "",
                AnswerA = "",
                AnswerB = "",
                AnswerC = "",
                AnswerD = "",
                CorrectAnswer = "",
                Reference = ""
            });
            tests.Add(new Test
            {
                Id = 21,
                IdSection = 2,
                Question = "",
                AnswerA = "",
                AnswerB = "",
                AnswerC = "",
                AnswerD = "",
                CorrectAnswer = "",
                Reference = ""
            });
            tests.Add(new Test
            {
                Id = 22,
                IdSection = 2,
                Question = "",
                AnswerA = "",
                AnswerB = "",
                AnswerC = "",
                AnswerD = "",
                CorrectAnswer = "",
                Reference = ""
            });
            tests.Add(new Test
            {
                Id = 23,
                IdSection = 2,
                Question = "",
                AnswerA = "",
                AnswerB = "",
                AnswerC = "",
                AnswerD = "",
                CorrectAnswer = "",
                Reference = ""
            });
            tests.Add(new Test
            {
                Id = 24,
                IdSection = 2,
                Question = "",
                AnswerA = "",
                AnswerB = "",
                AnswerC = "",
                AnswerD = "",
                CorrectAnswer = "",
                Reference = ""
            });
            tests.Add(new Test
            {
                Id = 25,
                IdSection = 2,
                Question = "",
                AnswerA = "",
                AnswerB = "",
                AnswerC = "",
                AnswerD = "",
                CorrectAnswer = "",
                Reference = ""
            });
            tests.Add(new Test
            {
                Id = 26,
                IdSection = 2,
                Question = "",
                AnswerA = "",
                AnswerB = "",
                AnswerC = "",
                AnswerD = "",
                CorrectAnswer = "",
                Reference = ""
            });
            tests.Add(new Test
            {
                Id = 27,
                IdSection = 2,
                Question = "",
                AnswerA = "",
                AnswerB = "",
                AnswerC = "",
                AnswerD = "",
                CorrectAnswer = "",
                Reference = ""
            });
            tests.Add(new Test
            {
                Id = 28,
                IdSection = 2,
                Question = "",
                AnswerA = "",
                AnswerB = "",
                AnswerC = "",
                AnswerD = "",
                CorrectAnswer = "",
                Reference = ""
            });
            tests.Add(new Test
            {
                Id = 29,
                IdSection = 2,
                Question = "",
                AnswerA = "",
                AnswerB = "",
                AnswerC = "",
                AnswerD = "",
                CorrectAnswer = "",
                Reference = ""
            });
            tests.Add(new Test
            {
                Id = 30,
                IdSection = 2,
                Question = "",
                AnswerA = "",
                AnswerB = "",
                AnswerC = "",
                AnswerD = "",
                CorrectAnswer = "",
                Reference = ""
            });
            tests.Add(new Test
            {
                Id = 31,
                IdSection = 2,
                Question = "",
                AnswerA = "",
                AnswerB = "",
                AnswerC = "",
                AnswerD = "",
                CorrectAnswer = "",
                Reference = ""
            });
            tests.Add(new Test
            {
                Id = 32,
                IdSection = 2,
                Question = "",
                AnswerA = "",
                AnswerB = "",
                AnswerC = "",
                AnswerD = "",
                CorrectAnswer = "",
                Reference = ""
            });
            tests.Add(new Test
            {
                Id = 33,
                IdSection = 2,
                Question = "",
                AnswerA = "",
                AnswerB = "",
                AnswerC = "",
                AnswerD = "",
                CorrectAnswer = "",
                Reference = ""
            });
            tests.Add(new Test
            {
                Id = 34,
                IdSection = 2,
                Question = "",
                AnswerA = "",
                AnswerB = "",
                AnswerC = "",
                AnswerD = "",
                CorrectAnswer = "",
                Reference = ""
            });
            tests.Add(new Test
            {
                Id = 35,
                IdSection = 2,
                Question = "",
                AnswerA = "",
                AnswerB = "",
                AnswerC = "",
                AnswerD = "",
                CorrectAnswer = "",
                Reference = ""
            });

            foreach (var test in tests)
            {
                var testDB = context.Tests.SingleOrDefault(t => t.Question == test.Question);
                if (testDB == null)
                {
                    context.Add(test);
                    context.SaveChanges();
                }
            }
            #endregion
        }
        public static void SeedData(IServiceProvider services, IHostingEnvironment env, IConfiguration config)
        {
            using (var scope = services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                SeederDB.SeedGame(context);
            }
        }
    }
}
