// Elijah Graham - ecg0068_PA4

using System;
internal class Program
{
    private static void Main(string[] args)
    
{
    bool exitProgram = false;    // sets the program termination to false (or off)

    do
    {
        DisplayMenu();   // execution/call of displaymenu module
        char choice = GetUserChoice();  // execution/call of getuserchoice module

        switch(choice) // sets up the main menu cases to call the respective modules when the user enters the value of each case
        {
            case '1':
                Console.WriteLine("\nYou have chosen to calculate your GPA. \n");  // calls gpacalc if chosen
                gpaCalc();
                break;
            case '2':
                Console.WriteLine("\nYou have chosen to calculate your class attendance percentage \n");  // calls attendancepercent if chosen
                attendancePercent();
                break;
            case '3':
                Console.WriteLine("\nYou have chosen to determine how many class days are remaining until summer break. \n");  // calls daysleft if chosen
                daysLeft();
                break;
            case '4':
                Console.WriteLine("\nThank you for choosing to use the program! \n");  // ends program if chosen
                exitProgram = true;
                break;
            default:
                Console.WriteLine("\nThe option you have chosen is not valid. Please select a valid option. \n");  // prompts user to try again if input is not valid (one of the above options)
                break;
        }
        Console.WriteLine();
        Console.WriteLine();
    }while(!exitProgram);  // requires the program to run the above when '4' is NOT chosen; this allows the user to return to the main menu after each module use
}

    static void DisplayMenu() // displays the user's choices. the lines correspond to their respective cases from above
    {
        Console.WriteLine("Select a function from the list. Input 'Z' at any point during the program to return to the main menu.");
        Console.WriteLine("1. GPA Calculator");
        Console.WriteLine("2. Class Attendance Percentage");
        Console.WriteLine("3. Class Days Remaining");
        Console.WriteLine("4. Exit");
    }

    static char GetUserChoice() // stores the user's input for use in the main menu. this is then used to determine the case to select
    {
        Console.WriteLine("\nEnter your option.");
        return Console.ReadKey().KeyChar;
    }

    static void gpaCalc()
    {
        int totalPts = 0; // sets variable to 0
        int classes = 4;  // pre-determined number of classes (4 for this assignment)
        int classPts = 0; // sets variable to 0
        double gpa;       // creation of variable
        Console.WriteLine("This program will calculate your GPA based on 4 classes at 3 credit hours per class."); // explanation of what program is doing
        for (int course = 1; course <= classes; course++) // sets program to ask for class letter grades for as long as the course number is less than classes (4 in assignment). ticks up after each ask
        {
            Console.WriteLine($"Please enter your letter grade for class #{course}. Do not include any '+' or '-'."); // asks user to input grade for class. uses course variable to list the course number
            string classGrade = Console.ReadLine().ToUpper(); // convert the input to uppercase so 'xyz' is always 'XYZ' -- easier to make if statements
                if (classGrade == "Z") // terminates the module if entered
                {
                    Console.WriteLine("The program has been terminated."); // alert to user
                    return; // return to main menu
                }
                else if (classGrade == "A") classPts = 4; // this line (and next four) assigning quality point values to their respective letter grades
                else if (classGrade == "B") classPts = 3;
                else if (classGrade == "C") classPts = 2;
                else if (classGrade == "D") classPts = 1;
                else if (classGrade == "F") classPts = 0;
                else
                {
                Console.WriteLine("The grade that you have entered is not valid. Please try again."); // prompts user to re-enter grade if not valid
                course--; // downticks the course number if the above error message occurs; this ensures we will always have the correct number of grades entered
                continue;
                }
            totalPts += classPts; // sets our totalpts value to the calcuated classpts from the for statement
        }
        gpa = (double) totalPts / classes; // calculates GPA by averaging the totalpts by the number of classes; this is acceptable since all the classes have identical credit hour values
        Console.WriteLine($"Your GPA is {gpa:F2}"); // display GPA to the user!
    }

    static void attendancePercent()
    {
        int semDays = 112; // set the number of semester days to the pre-determined value
        int numAbs = 0; // zero out our number of absences value
        Console.WriteLine("This program will calculate your class attendance score based on how many of the 112 class days you missed this semester."); // explains the module
        do
        {
            Console.WriteLine("Please enter the total number of days that you missed class this semester."); // obtains the user's input
            string abs = Console.ReadLine(); // string ensures we can identify if 'z' is used
            if (abs == "Z" || abs == "z")  // terminates module if the user has entered 'z'
            {
                Console.WriteLine("The program has been terminated.");
                return;     // return to main menu upon leaving the module
            }
            else if (abs.Contains(".") || !int.TryParse(abs, out numAbs) || (numAbs < 0) || (numAbs > semDays)) // validates that the non-z input is a whole integer between 0 (no absences) and semDays (all absences)
            {
                Console.WriteLine("The number of days that you have inputted is invalid. Please try again."); // prompts user to try again if above conditions are true
            }
            else // if above conditions are NOT true (so value IS whole integer between 0 and semDays), runs this
            {
                numAbs = Convert.ToInt32(abs);    // convert the string to an integer; this is easy if input passes the above validation checks               
                int presentDays = semDays - numAbs; // calculates the number of days user is in class using total semester days and number of absences
                double percentAttendance = (double) presentDays / semDays * 100; // calculates a percentage using the number of days present divided by total days in semester
        
                Console.WriteLine($"Your class attendance is {percentAttendance:F2}%.\n"); // displays the attendance percentae to the user
                break; // exits this module
            }
        } while (true);
    }

    static void daysLeft()
    {
            Console.WriteLine("This program will calculate the number of days that you have left in the semester, which ends on April 30, 2025."); // explains the module
            Console.WriteLine("Please note that this program can calculate the days remaining from either today's date or a given date."); // informs the user there are two options
        do
        {
            DateTime eodDate = new DateTime(2025,04,30); // sets the end of semester date
            Console.WriteLine("If you would like to use today's date, please input 'today'"); // allows user to use today's date (date of use)
            Console.WriteLine("If you would like to select the date to calculate from, please input 'other'"); // allows user to input their own start date
            string inputDate = Console.ReadLine().ToUpper(); // converts the input to uppercase (assumig it is a letter) to allow the 'if - stop' section below to run
        
            if (inputDate == "Z") // stops and returns to main menu if 'z' entered
            {
                Console.WriteLine("The program has been terminated.");
                return; // returns to main menu
            }
            else if (inputDate == "TODAY") // if user inputs 'today'
            {
                Console.WriteLine("You chose to calculate from today's date."); // reminds user of choice
                DateTime currentDate = DateTime.Now; // sets currentDate variable to the actual current date and time
                TimeSpan Difference = eodDate - currentDate; // subtracts the currentDate from last day of semester
                int daysTo = Difference.Days; // converts the above to an integer
                Console.WriteLine($"The number of days remaining in the semester, as of today, is {daysTo} days."); // displays the days remaining in the semester
                return; // return to main menu
            }
            else if (inputDate == "OTHER") // if user inputs 'other'
            {
                Console.WriteLine("Please input the date you want to calculate from in MM/DD/YYYY format. The date must be BEFORE April 30, 2025.");
                string dateInput = Console.ReadLine(); // string to allow for user to input 'z' if they wish

                DateTime pastDate; // formation of pastDate variable to store the input if it passes validation checks below

                if (dateInput == "Z" || dateInput == "z") // stops module and returns to main menu if 'z' is inputted
                {
                    Console.WriteLine("The program has been terminated.");
                    return; // returns to main menu
                }
                else if (DateTime.TryParse(dateInput, out pastDate)) // validates that the non-z input is a valid date
                {
                    TimeSpan difference = eodDate - pastDate; // if above check returns true, module calculates the difference between end of semester date and the user's input date
                    int pastDaysDifference = difference.Days; // converts above to an integer
                    if (pastDaysDifference >= 0) // confirms that the above integer is less than or equal to 0 (so we cannot have negative days)
                    {
                        Console.WriteLine($"The number of days remaining in the semester, as of {dateInput}, is {pastDaysDifference} days."); // display the above
                        return; // return to main menu
                    }
                    else // runs assuming the above validation check returns false, meaning the integer is negative
                    {
                        Console.WriteLine("The date must be before April 30, 2025. Please try again.");
                    }
                }
                else // runs if the user enters anything other than 'z', 'today', or 'other' and requires them to re-enter their choice
                {
                    Console.WriteLine("You have entered an invalid or incorrectly formatted date. Please try again.");
                }
            }
        } while (true);
    }
}   

