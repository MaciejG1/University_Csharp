
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using System.Text.RegularExpressions;

    bool quit = true;

while (quit)
{

    Console.WriteLine("1. Zagraj\n2. Jak grać?\n3. statystyki\n4. wyjdz");
    int choice = ChoiceMenu();
    switch (choice)
    {
        case 1:
            {
                Game();
                break;
            }
        case 2:
            {
                HowToPlay();
                break;
            }
        case 3:
            {
                ReadResultFromFile();
                break;
            }
        case 4:
            {
                quit = false;
                break;
            }
    }
}


//metody programowe
static void HowToPlay()
{
    Console.Clear();
    Console.WriteLine("Gra jest wzorowana na popularnej grze \"Wisielec\", " +
    "w której należy odgadnąć ukryte słowo.\n" +
    "Gra kończy się gdy odgadniemy wszystkie litery słowa lub skończą nam się życia a wisielec" +
    " zakończy swój żywot.\nPowodzenia!");
    Console.WriteLine("\n\nNaciśnij dowolny przycisk aby wyjść");
    Console.ReadKey();
    Console.Clear();
}
static void HangedMan(int lifes)
{
    switch (lifes)
       {
        case 5:
            {
                Console.WriteLine("     ____");
                Console.WriteLine("    |   |");
                Console.WriteLine("    |   O");
                Console.WriteLine("    |");
                Console.WriteLine("   _|");
                break;
            }
        case 4:
            {
                Console.WriteLine("     ____");
                Console.WriteLine("    |   |");
                Console.WriteLine("    |   O");
                Console.WriteLine("    |   |");
                Console.WriteLine("   _|");
                break;
            }
        case 3:
            {
                Console.WriteLine("     ____");
                Console.WriteLine("    |   |");
                Console.WriteLine("    |   O");
                Console.WriteLine("    |  /|");
                Console.WriteLine("   _|");
                break;
            }
        case 2:
            {
                Console.WriteLine("     ____");
                Console.WriteLine("    |   |");
                Console.WriteLine("    |   O");
                Console.WriteLine("    |  /|\\");
                Console.WriteLine("   _|");
                break;
            }
        case 1:
            {
                Console.WriteLine("     ____");
                Console.WriteLine("    |   |");
                Console.WriteLine("    |   O");
                Console.WriteLine("    |  /|\\");
                Console.WriteLine("   _|  /");
                break;
            }
        case 0:
            {
                Console.WriteLine("     ____");
                Console.WriteLine("    |   |");
                Console.WriteLine("    |   O");
                Console.WriteLine("    |  /|\\");
                Console.WriteLine("   _|  / \\");
                break;
            }
        default:
            {
                Console.WriteLine("     ____");
                Console.WriteLine("    |   |");
                Console.WriteLine("    |");
                Console.WriteLine("    |");
                Console.WriteLine("   _|");
                break;
            }

    }
}
static int ChoiceMenu()
{
    int number = 0;
    string input;
    
    Console.Write("Wprowadź opcje która chcesz wybrać (wpisz numer): ");
    input=Console.ReadLine();

    while (true)
    {
        if (int.TryParse(input, out number))
            break;
        else
        {
            Console.WriteLine("wprowadziłeś zły format danych. Spróbuj ponownie.");
            input = Console.ReadLine();
        }
    }
    return number;
}
static char UserLetterInput()
{
    char inputLetter;
    string inputLetterString;
    
    Console.Write("\nWprowadź litere: ");
  
    while (true)
    {
        inputLetterString = Convert.ToString(Console.ReadLine());
        if (char.TryParse(inputLetterString, out inputLetter)&& !(int.TryParse(inputLetterString, out int temp)))
        {
            break;
        }
        else
        {
            Console.WriteLine("wprowadziłeś zły format danych. Spróbuj ponownie.");
        }
    }
    return inputLetter;
}

static void Game()
{
  //losowanie wyrazu do gry
    string[] words = new string[] { "styczen","luty","marzec","kwiecien","maj","czerwiec","lipiec","sierpien","wrzesien","pazdziernik","listopad","grudzien" };
    Random rand = new();
    string randomWord = words[rand.Next(words.Length)];
    int lengthRandomWord = randomWord.Length;
    string dashes = new string('-', lengthRandomWord);
  //deklaracja zmiennych
    int numberOfShowingLetter = 0;
    int lifes=6;
    bool checkerChanges = true;
    bool whoWin=false;
    char inputLetter=' ';
    char[] outputWord = new char[lengthRandomWord];
   
    
    for (int i = 0; i < outputWord.Length; i++)
    {
        outputWord[i] = '-';
    }
        Console.Clear();
    Console.WriteLine();
  //przebieg gry 
    while (lifes>0)
    {
      


      //petla od sprawdzania czy litera zostala odgadnieta
        do
        {
           
            if (outputWord[numberOfShowingLetter] == '-')
            {
                if (inputLetter == randomWord[numberOfShowingLetter])
                {
                    outputWord[numberOfShowingLetter] = inputLetter;
                    checkerChanges=true;
                }
                else
                    outputWord[numberOfShowingLetter] = '-';
                  
            }

            numberOfShowingLetter++;
        }
        while (numberOfShowingLetter < lengthRandomWord);
      //sprawdzenie czy gracz odgadnal i odjecie zycia
        if (!checkerChanges)
            lifes--;
        checkerChanges = false;
      //wyswietlenie zyc na ekranie
        Console.Write("Życia: ");
        for (int i = 0; i < lifes; i++)
            Console.Write("<3 ");
        Console.WriteLine("\n");
      //wyswietlenie wyrazu na ekranie
        Console.Write("Wyraz do zgadnięcia:         ");
        for (int i = 0; i < outputWord.Length; i++)
            Console.Write(outputWord[i]);
        Console.WriteLine("\n");
        HangedMan(lifes);

        string checkerWin = string.Concat(outputWord);
      //sprawdzenie wygranej
        if (checkerWin == randomWord)
        {
            Console.Clear();
            Console.WriteLine("wygrales!");
            whoWin = true;
            Console.ReadKey();
            Console.Clear();
            break;
        }
        else if (lifes == 0)
        {
            Console.Clear();
            Console.WriteLine("Przegrałeś. Wygrał Komputer");
            whoWin = false;
            HangedMan(lifes);
            Console.ReadKey();
            Console.Clear();
            break;
        }
        inputLetter = UserLetterInput();

        numberOfShowingLetter = 0;
        Console.Clear();

      
    }
    SaveResultToFile(whoWin);
}

static void SaveResultToFile(bool ifPlayerWin)
{
    {
        string destinationPath = "D:\\studia_C#\\wisielec_konsola\\Wyniki.txt";

        if (!File.Exists(destinationPath))
            File.Create(destinationPath);

        
        StreamReader reader = new StreamReader(destinationPath);
        
            string lines = reader.ReadToEnd();
            reader.Close();
            string playerWord = "gracz";
            string computerWord = "komputer";
            int playerWinsAmount = Regex.Matches(lines, playerWord, RegexOptions.IgnoreCase).Count;
            int computerWinsAmount = Regex.Matches(lines, computerWord, RegexOptions.IgnoreCase).Count;

        StreamWriter writer = new StreamWriter(destinationPath,true);
            if (ifPlayerWin)
            {
                writer.WriteLine("\nWygrał gracz\nłączna ilość wygranych to: " + (playerWinsAmount+1));
            }
            else
            {
                writer.WriteLine("\nWygrał komputer\nłączna ilość wygranych to: " + (computerWinsAmount+1));
            }
            writer.WriteLine("Data zakończenia gry: " + DateTime.Now.ToString());
            writer.Close();
        
        
        
    }
}
static void ReadResultFromFile()
{
    Console.Clear();
    string destinationPath = "D:\\studia_C#\\wisielec_konsola\\Wyniki.txt";
    if (File.Exists(destinationPath))
    {
        StreamReader reader = new StreamReader(destinationPath);
        string[] statistics = new string[] { reader.ReadToEnd() };
        reader.Close();
      
            foreach (string line in statistics)
            {
                Console.WriteLine(line);
            }
    }
    else
    {
        Console.WriteLine("Przykro mi, ale nie odnaleziono pliku z wynikami");
    }
    Console.WriteLine("\n\nNaciśnij dowolny przycisk aby wyjść");
    Console.ReadKey();
    Console.Clear();
}
