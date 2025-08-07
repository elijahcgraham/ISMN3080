// Eli Graham - ecg0068

using System;

class Program
{
    
    private static void Main(string[] args)
    {
    //Module calls and variables        
        int pkgPrice = 99; // price of individual pkgs

        int pkgQty = getPkgs();
        double discPercent = calcDisc(pkgQty);
        outputTotal(pkgQty, pkgPrice, discPercent);
    }

    // Module to capture quantity and perform a validation 
    static int getPkgs()
    {
        int pkgQty = 0;
        string pkgConfirm = "";
        do
        {
            // prompts user to enter number of pkgs to be purchased
            Console.WriteLine("Please enter the number of packages you are purchasing."); 
            string pkgInput = Console.ReadLine(); // reads and stores the input
           
            // validates that the entry is NOT a string, double, or negative integer (or 0)  
            if (pkgInput.Contains(".") || !int.TryParse((pkgInput), out pkgQty) || (pkgQty <= 0))
                Console.WriteLine("Please enter a valid number of packages. Your order must be a positive integer and contain no decimals or letters.");
            else
            {
                // checks with user to confirm the amount entered is correct
                Console.WriteLine($"To confirm, you are purchasing {pkgQty} packages. If this is correct, enter 'Y'. If this is not correct, enter 'N'.");
                pkgConfirm = Console.ReadLine();  // reads confirmation and returns to the do-while statement
            }
        } 
        while (pkgConfirm != "Y");  // repeat until user confirms the amount is correct

        return pkgQty;  // returns the quantity of packages to module
    }
  
    //Module to calculate percentage discount based off number of packages
    static double calcDisc(int pkgQty)
    {
        double discPercent; // variable declared for discount percentage to be stored
        
        if (pkgQty < 9) 
        {  
            discPercent = 0; // no discount for less than 9 pkgs
        }
        else if (pkgQty <= 19)
        {
            discPercent = .2; // 20% discount for 9-19 pkgs
        }
        else if (pkgQty <= 49)
        {
            discPercent = .3; // 30% discount for 19-49 pkgs
        }
        else if (pkgQty <=99)
        {
            discPercent = .4; // 40% discount for 49-99 pkgs
        }
        else
        {
            discPercent = .5; // 50% discount for over 99 packages
        }
        return discPercent; // the correct percentage is returned to the module based on pkgQty
        
    }


    //Module to output data
   
    static void outputTotal(int pkgQty, int pkgPrice, double discPercent)
    {
        double fullPrice = pkgQty * pkgPrice; // find full price 
        double totalAmount = fullPrice * (1 - discPercent); // full price with discount
        Math.Round(fullPrice, 2); // round to 2 decimal places (currency)
        Math.Round(totalAmount, 2); // same as above
        
        Console.WriteLine($"Discount: {discPercent * 100}%"); // display discount as percent instead of decimal
        Console.WriteLine($"Full Price Amount: ${fullPrice:F2}"); // display full price w/o discount (to two decimal places)
        Console.WriteLine($"Total Amount of Purchase: ${totalAmount:F2}"); // display total price charged w/ discount (to two decimal places)
    }


}
