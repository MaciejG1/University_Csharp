// See https://aka.ms/new-console-template for more information


using System.ComponentModel;

int HPplayer=0, shieldplayer=15, attackplayer=20;
int HPmonster=0, shieldmonster=10, attackmonster=30;
int chooseOfPlayer, zadaneobrazenia, round=1, attackmeteor=0,monsterSupport=0;

Random rdm = new Random();

Console.WriteLine("Witaj graczu!\n");


    //nadanie statystyk gracza
   
    Console.Write("Podaj statystyki swojej postaci:\nPodaj punkty zdrowia postaci w zakresie od 1 do 200: ");
    HPplayer = InputRead(1,200);
    Console.Write("Podaj obrone swojej postaci w zakresie od 1 do 50:  ");
    shieldplayer = InputRead(1,50);
    Console.Write("Podaj atak swojej postaci w zakresie od 1 do 50: ");
    attackplayer = InputRead(1,50);

    //nadanie statystyk potwora

    Console.Write("\n\nPodaj statystyki potwora:\nPodaj punkty zdrowia potwora w zakresie od 1 do 200: ");
    HPmonster = InputRead(1,200);
    Console.Write("Podaj obrone potwora w zakresie od 1 do 50: ");
    shieldmonster = InputRead(1,50);
    Console.Write("Podaj atak potwora w zakresie od 1 do 50: ");
    attackmonster = InputRead(1,50);

    Console.Clear();

//losoowość wybrania kto zaczyna 

chooseOfPlayer = rdm.Next(1, 99);

if (chooseOfPlayer % 2 == 0)
{
    Console.WriteLine("rozpoczyna gracz");

}
else
{
    Console.WriteLine("rozpoczyna potwor");
}

//rozgrywka

while ((HPplayer > 0 && HPmonster > 0))
{
    Console.Clear();
    if (round > 20)
    {
        Console.WriteLine("Wygląda na to, że czarodzieje się zezłościli i zesłali deszcz meteorytów na pole bitwy!\nOd teraz każdy uczestnik bitwy będzie otrzymywał co ture 30pkt obrażeń.\nZabij potwora zanim skończy ci się życie!");
        attackmeteor = 30;
    }
    else if (round > 10) 
    {
        Console.WriteLine("Potwór zwołał wsparcie. Od teraz atak potwora zwiększył się o 20pkt. Pospiesz się zanim będzie za późno!");
        monsterSupport = 20;
    }
    Console.WriteLine("Runda nr: " + round+"\n");
    if (chooseOfPlayer % 2 == 0)
    {
        Console.ForegroundColor
           = ConsoleColor.Blue;
        Console.WriteLine("Atakuje gracz:");

        zadaneobrazenia = attackplayer - shieldmonster;
        if (zadaneobrazenia < 0)
            zadaneobrazenia = 0;
        HPmonster = HPmonster - zadaneobrazenia;
        HPmonster -= attackmeteor;
        Console.WriteLine("potwor zblokowal " + shieldmonster + " pkt obrażeń\ngracz zadał " + (zadaneobrazenia + attackmeteor) + " pkt obrazeń",Console.ForegroundColor);
        Console.WriteLine("potwór ma juz tylko: " + HPmonster + " pkt zdrowia\n",Console.ForegroundColor);
       
    }
    else
    {
        Console.ForegroundColor
         = ConsoleColor.Red;
        Console.WriteLine("Atakuje potwor:");

        zadaneobrazenia = attackmonster + monsterSupport - shieldplayer;
        
        if (zadaneobrazenia < 0)
            zadaneobrazenia = 0;
        HPplayer = HPplayer - zadaneobrazenia;
        HPplayer -= attackmeteor;
        Console.WriteLine("Gracz zblokowal " + shieldplayer + " pkt obrażeń\nPotwór zadał " + (zadaneobrazenia+attackmeteor) + " pkt obrazeń");
        Console.WriteLine("gracz ma juz tylko: " + HPplayer + " pkt zdrowia\n");
        

    }

    //wyświetlenie statystyk postaci

    Console.ForegroundColor
          = ConsoleColor.White;
    Console.WriteLine("\n_____________________________________");
    Console.WriteLine("                  Statystyki gracza:");
    Console.WriteLine("pkt zdrowia:             " + HPplayer);
    Console.WriteLine("pkt pancerza:            " + shieldplayer);
    Console.WriteLine("pkt ataku:               " + attackplayer + "\n_____________________________________");
    Console.WriteLine("                  Statystyki potwora:");
    Console.WriteLine("pkt zdrowia:             " + HPmonster);
    Console.WriteLine("pkt pancerza:            " + shieldmonster );
    Console.WriteLine("pkt ataku:               " + attackmonster  + "\n_____________________________________");
    Console.WriteLine("                  Statystyki ogólne:");
    Console.WriteLine("Numer rundy:             " + round );
    Console.WriteLine("wsparcie potwora:        " + monsterSupport);
    Console.WriteLine("nalot meteorytów:        " + attackmeteor );
    Console.WriteLine("_____________________________________");
    round++;
    chooseOfPlayer++;
    Console.ReadKey();
}

//odpytanie kto wygrał
if (HPplayer <= 0)
{
    Console.WriteLine("\nWygrał potwór to starcie");
}
else if (HPmonster <= 0)
    Console.WriteLine("\nwygrał gracz to starcie");
Console.ReadKey();



//metoda pobierania i sprawdzania wartosci zadanej przez uzytkownika
static int InputRead (int min = 0, int max = 200)
{
    int number=0;
    string input;
    while (true)
    {
        
        input = Console.ReadLine();
        if (int.TryParse(input, out number))
        {
            if ((number >= min) && (number <= max))
                break;
            else 
                Console.WriteLine($"Podałeś wartość z poza dozwolonego zakresu: {min}-{max}. Spróbuj ponownie");
        }
        else 
            Console.WriteLine("Wprowadziłeś/aś błędny format danych. Spróbuj ponownie");
    }
    return number;
}



