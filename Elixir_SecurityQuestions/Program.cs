using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Elixir_SecurityQuestions
{
    class QuestionAnswer
    {
        public string Question { get; set; }
        public string Answer { get; set; }

    }
    class Program
    {
        private static readonly List<string> questionList = new List<string>()
        {
            "In what city were you born?",
            "What is the name of your favorite pet?",
            "What is your mother&#39;s maiden name?",
            "What high school did you attend?",
            "What was the mascot of your high school?",
            "What was the make of your first car?",
            "What was your favorite toy as a child?",
            "Where did you meet your spouse?",
            "What is your favorite meal?",
            "Who is your favorite actor / actress?",
            "What is your favorite album?"
        };

        static void Main(string[] args)
        {
            Console.WriteLine("Security Questions");
            Console.WriteLine("The user can stop the program at any point with CTRL - C");
            
            while (true)
            {
                AskName();
            }
        }
        public static void AskName()
        {
            // Ask the user to type the  name.
            Console.WriteLine("Hi, what is your name?");
            var name = Console.ReadLine();

            if (File.Exists(Path.Combine(@"../../../Data/", $"{name}.txt")))
                AnswerFlow(name);
            else
                StoreFlow(name);
        }
        public static void StoreFlow(string name)
            {
            var userQnA = new List<QuestionAnswer>();
            Console.WriteLine("Would you like to store answers to security questions?");
            if (Console.ReadLine() == "yes")
            {
              
                for (int index = 0; index < questionList.Count; index++)
                    Console.WriteLine($"{index}: {questionList[index]}");

               var selectedQuestions= PromptQuestionList();
                for (int index = 0; index < questionList.Count; index++)
                {
                    if (selectedQuestions.Contains(index))
                    {
                       
                        Console.WriteLine(questionList[index]);
                        Console.Write("Answer: ");
    
                        userQnA.Add(new QuestionAnswer { Question = questionList[index], Answer = Console.ReadLine() });
                    }
                }
                Console.WriteLine("Prompt username to Store the answers or No to not Store the answers?");

                if (Console.ReadLine() != "No")
                {
                    if (!Directory.Exists(@"../../../Data/"))
                       Directory.CreateDirectory(@"../../../Data/");
                   
                    using StreamWriter outputFile = new StreamWriter(Path.Combine(@"../../../Data/", $"{name}.txt"), true);
                    outputFile.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(userQnA));
                }
                
            }

        }

        public static IEnumerable<int> PromptQuestionList()
        {
            Console.WriteLine("Please select  3 questions to answer. ex: 3,5,8");
            var selectedQuestions = Console.ReadLine().Split(',').Select(i => int.Parse(i));
            while (selectedQuestions.Count()!=3)
                Console.Write("Please select minimum 3 questions to answer. ex: 3,5,8. Try again: ");
            return selectedQuestions;

        }

        public static void AnswerFlow(string name)
        {

            string jsonObject = File.ReadAllText(Path.Combine(@"../../../Data/", $"{name}.txt"));
            var qna = JsonConvert.DeserializeObject<List<QuestionAnswer>>(jsonObject);
            Console.WriteLine("Please answer one of security questions?");
            var successfulAnswer = false;
            foreach (var q in qna)
            {
                
                Console.WriteLine(q.Question);
                Console.Write("Answer: ");
                if (q.Answer.Equals(Console.ReadLine()))
                {
                    successfulAnswer = true;
                    break;
                }
                   
            }
            if(!successfulAnswer)
            Console.WriteLine("We ran out of Security Questions");


        }
    }
}
