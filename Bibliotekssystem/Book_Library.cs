using System.Data;
using System.Reflection;
using Microsoft.VisualBasic;

namespace Testyta;

public class Book
{
    public string? Title {get; set;}
    public string? Author {get; set;} 
    public string? Loaner {get; set;}
    public bool LoanStatus {get; set; }

    public Book(string title, string author, string loaner, bool loanStatus)
        {
            Title = title;
            Author = author;
            Loaner = loaner;
            LoanStatus = loanStatus; 
        }

    public Book(string title)
    {
        Title = title;
    }

    public override string ToString()
    {
        return $"{Title} - {Author} - {Loaner} - {LoanStatus}";
    }
    
}
    class BookList
    {
    public List<Book> availableBooks;
    public List<Book> notAvailableBooks;

    public BookList()
    {
         
        availableBooks = new List<Book>{
        new Book ("Maze Runner", "James Dashner", "" ,true),
        new Book ("Steve Jobs", "Walter Isaacson", "", true),
        new Book ("IT", "Steven King", "", true),
        new Book ("Harry Potter 2", "JK Rowling", "", true),
        }; 

        notAvailableBooks = new List<Book>{
        new Book ("Harry Potter 1", "JK Rowling", "Emil", false),
        new Book ("Hobbit", "J.R.R Tolkien", "Sara", false),
        new Book ("Atomic Habits", "James Clear", "Oskar", false),
        };
    }
    public void AddBookAvailableList(Book book)
    {
        availableBooks.Add(book);
    }
    public void MoveBookToAvailable(string bookTitle, List<Book> notAvailableBooks, List<Book> availableBooks, string loaner, bool loanStatus)
    {
        for (int i = 0; i < notAvailableBooks.Count; i++)
        {
            if (notAvailableBooks[i].Title.Equals(bookTitle))
            {
                Book moveBook = notAvailableBooks[i];
                notAvailableBooks.RemoveAt(i);

                moveBook.Loaner = loaner;
                moveBook.LoanStatus = loanStatus;

                availableBooks.Add(moveBook);
            }
        }
    }
    public void MoveBookToUnavailable(string bookTitle, List<Book> availableBooks, List<Book> notAvailableBooks, string loaner, bool loanStatus)
    {
        for (int i = 0; i < availableBooks.Count; i++)
        {
            if (availableBooks[i].Title.Equals(bookTitle))
            {
                Book moveBook = availableBooks[i];
                availableBooks.RemoveAt(i);

                moveBook.Loaner = loaner;
                moveBook.LoanStatus = loanStatus;

                notAvailableBooks.Add(moveBook);
            }
        }
    }
    public bool IsBookAvailable(string? title)
    {
            return availableBooks.Any(book => string.Equals(book.Title, title, StringComparison.Ordinal));
    }
    public bool IsBookUnavailable(string? title)
    {
            return notAvailableBooks.Any(book => string.Equals(book.Title, title, StringComparison.Ordinal));
    }
    public void ShowAllAvailableBooks()
    {
        foreach (Book book in availableBooks)
        {
            Console.WriteLine($"{book.Title}, {book.Author}, {book.Loaner}, {(book.LoanStatus ? "Tillgänglig" : "Inte tillgänglig")}");
        }
    }
    public void ShowAllUnavailableBooks()
    {
        foreach (Book book in notAvailableBooks)
        {
            Console.WriteLine($"{book.Title}, {book.Author}, {book.Loaner}, {(book.LoanStatus ? "Tillgänglig" : "Inte tillgänglig")}");
        }
    }
    public void ShowLoanedBooks(List<Book> books, string loanerName)
    {
        bool foundInNotAvailable = false;

        foreach (Book book in notAvailableBooks)
        {

            if (book.Loaner == loanerName)
            {
                Console.WriteLine($"{book.Title}, {book.Author}");
                Console.WriteLine("");
                foundInNotAvailable = true;
            }
        }
        if (!foundInNotAvailable)
            {
                Console.WriteLine("Inga böcker lånade i ditt namn.");
            }
        Console.WriteLine("Tryck ENTER för att fortsätta.");
        Console.ReadKey();
        Console.WriteLine("");
    }
}   


class Program
{
    static void Main(string[] args)
    {

        BookList bookList = new BookList();
        bool nummer = true;
        bool running = true;
        string title = "";
        string author = "";
        string loaner = "";
        bool loanStatus = true;

        do
        {
        Console.WriteLine("Hej och välkommen, vänligen välj ett alternativ");
        Console.WriteLine("1. Lägg till nya böcker");
        Console.WriteLine("2. Visa tillgängliga böcker & låna böcker");
        Console.WriteLine("3. Lämna tillbaka böcker");
        Console.WriteLine("4. Visa låntagare och deras lånade böcker");
        Console.WriteLine("5. Avsluta");
        string? input = Console.ReadLine();
        nummer = int.TryParse(input, out int cases);

        switch (cases)
        {
            case 1:
            do
            {
                Console.WriteLine("Vad heter boken?");
                string? bookTitle = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(bookTitle))
                {
                    Console.WriteLine("Du måste skriva in en titel.");
                }
                else
                {
                    do
                    {
                    Console.WriteLine("Vem har skrivit boken?");
                    string? bookAuthor = Console.ReadLine();

                        if (string.IsNullOrWhiteSpace(bookAuthor))
                        {
                            Console.WriteLine("Du måste skriva in en författare.");
                        }
                        else
                        {
                            Book newBook = new Book(bookTitle, bookAuthor, loaner, loanStatus);
                            bookTitle = title;
                            bookAuthor = author;
                            loaner = "";
                            loanStatus = true;
                            
                            bookList.AddBookAvailableList(newBook);
                            Console.WriteLine("Bok har lagts till.");
                            Console.WriteLine(""); 
                        break;
                        }
                    } while (true);
                    break;
                }
            } while (true); 
            break;

            case 2:            
                Console.WriteLine("Tillgängliga böcker:");
                bookList.ShowAllAvailableBooks();
                Console.WriteLine("");
                Console.WriteLine("Utlånade böcker:");
                bookList.ShowAllUnavailableBooks();
                bool case2Input = false;
                do
                {
                Console.WriteLine("Vill du låna en bok? (ja/nej)");
                string? wantToLoanBook = Console.ReadLine();

                if (wantToLoanBook?.ToLower() == "ja")
                {
                    do
                    {
                    Console.WriteLine("Ange namnet på boken du vill låna"); 
                    string? bookToLoan = Console.ReadLine(); 

                        if (bookList.IsBookUnavailable(bookToLoan))
                        {
                            Console.WriteLine("Boken är redan utlånad. Vill du låna någon annan bok? (ja/nej)");
                            string? otherBook = Console.ReadLine(); 
                            Console.WriteLine("");

                            if (otherBook?.ToLower() == "ja")
                            {
                                goto case 2;
                            }
                            else if (otherBook?.ToLower() == "nej")
                            {
                                case2Input = true;
                                break;
                            }
                        }
                        else if (!bookList.IsBookAvailable(bookToLoan))
                        {
                            Console.WriteLine("Boken finns inte i listan eller är titeln felstavad. Vill du försöka igen? (ja/nej)");
                            string? tryAgain =  Console.ReadLine();

                            if (tryAgain?.ToLower() == "ja")
                            {
                                continue;
                            }
                            else if (tryAgain?.ToLower() == "nej")
                            {
                                case2Input = true;
                                break;
                            }
                        }
                        else 
                        {
                            do
                            {
                                Console.WriteLine("Vad heter du?");
                                string? loanerName = Console.ReadLine();

                                if (string.IsNullOrWhiteSpace(loanerName))
                                {
                                    Console.WriteLine("Inget namn skrivet. Försök igen.");
                                }
                                else
                                {
                                    bookList.MoveBookToUnavailable(bookToLoan, bookList.availableBooks, bookList.notAvailableBooks, loanerName, false); // Visa meddelande ifall listan är tom
                                    Console.WriteLine("Lån bekräftat."); 
                                    Console.WriteLine("");
                                    case2Input = true;
                                    break;
                                }
                            } while (true);
                        }
                    }while (case2Input == false);
                }
                else if (wantToLoanBook?.ToLower() == "nej")
                {
                    Console.WriteLine("");
                    case2Input = true;
                    break;
                }
                else if (string.IsNullOrWhiteSpace(wantToLoanBook))
                {
                    case2Input = false;
                }
                } while (case2Input == false); 
                break;

            case 3:
            do
            {
                Console.WriteLine("");
                Console.WriteLine("Skriv in titeln på boken du vill lämna tillbaka");
                bookList.ShowAllUnavailableBooks();
                string? returnBook = Console.ReadLine();
                Console.WriteLine("");

                if (string.IsNullOrWhiteSpace(returnBook))
                {
                    Console.WriteLine("Ingen titel inskriven, försök igen.");
                }
                else
                {
                    if (bookList.IsBookUnavailable(returnBook))
                    {
                        bookList.MoveBookToAvailable(returnBook, bookList.notAvailableBooks, bookList.availableBooks, "", true);
                        
                        Console.WriteLine("Din bok är återlämnad.");
                        Console.WriteLine("");
                        Console.WriteLine("Tryck ENTER för att komma tillbaka till menyn.");
                        Console.ReadKey();
                        break;
                    }
                    else if(!bookList.IsBookUnavailable(returnBook))
                    {
                        Console.WriteLine("Kunde inte hitta boken i listan. Försök igen.");                   
                    }
                }
            } while (true);
            break;

            case 4:
            do
            {
                Console.WriteLine("Skriv in ditt namn för att se lånade böcker:");
                string? nameOfLoaner = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(nameOfLoaner))
                {
                    Console.WriteLine("Inget namn inskrivet. Försök igen.");
                }
                else
                {
                    bookList.ShowLoanedBooks(bookList.notAvailableBooks, nameOfLoaner);
                    break;
                }
            } while (true);
            break;

            case 5:
            running = false;
            Console.WriteLine("Programmet avslutas...");
            break;
        }
        } while (running == true);
    }
}