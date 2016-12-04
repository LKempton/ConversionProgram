using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SOFT153AlgorithmCoursework
{
    // SOFT153 Assignment for algorithms!
    // Lee Kempton
    // Note: There is a filename variable in the ReadValues method, this has to be changed to the filepath of where convert.txt is stored on your machine for the program to run.

   static class Program
    {      
        static void Main(string[] args)
        {
            // Used to call to the other class once the program has started.
            Algorithm myProgram = new Algorithm();
            myProgram.StartProgram();
        } 
    }

   class Node
   {
       // Class to store the 3 seperate variables and the pointer to the next node in the list.
       public string firstUnit;
       public string secondUnit;
       public double convertFactor;

       public Node next;
   }

   class List
   {
       // Class to store the list.
       public Node firstNode;
   }

   class Algorithm
   {
       public void StartProgram()
       {
           string userInput;
           string userDecision;
           string[] splitValues;
           // Cut the string at comma ',' and spaces ' '.
           char[] delimiter = { ',', ' ' };

           // Variables to store the three inputs easily.
           double amount;
           string firstUnit;
           string secondUnit;
           string output;

           bool isValid = false;

           List list = new List();
           Node node = new Node();

           // Method used to read in values from the file so it can be split and stored in the list.
           ReadValues(list, node);

           // Place this entire piece of code in a do-while, this is so the program will keep prompting the user for input until a valid input is entered.
           do
           {
               // Text telling the user what to input and providing an exmaple.
               Console.WriteLine("Please enter an amount and two units, all seperated by a single comma ','...");
               Console.WriteLine("Example of valid entry: 5, ounce, gram...");
               Console.WriteLine("\nEnter a negative number or 0 to exit the program!");

               // Make the userInput variable equal to what the user puts in the console.
               userInput = Console.ReadLine();

               // Used to exit the program and stop crashing if the first character is not a double.
               try
               {
                   // Exits the program if the amount is a negative amount or 0, as per assignment.
                   if (Convert.ToDouble(userInput) <= 0)
                   {
                       // Break out of the while loop.
                       break;
                   }
               }
               catch (FormatException ex) { }
               

               
               // Split at the delimiter and remove any values that are empty.
               splitValues = userInput.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);

               // Make sure there are 3 values, as there are 2 units and a conversion factor, this stops empty lines being put into the list.
               if (splitValues.Length == 3)
               {
                   // Try-catch to stop format exceptions on the amount variable.
                   try
                   {
                       amount = Convert.ToDouble(splitValues[0]);

                       // Exits the program if the amount is a negative amount or 0, as per assignment.
                       if (amount <= 0)
                       {
                           break;
                       }

                       // Use the CheckFormat to make sure the two units only contain a-z or A-Z;
                       if (CheckFormat(splitValues[1]) == true && CheckFormat(splitValues[2]) == true)
                       {
                           // Assuming the values only contain letters, convert them to lower case using the same method used earlier.
                           firstUnit = ConvertToLowerCase(splitValues[1]);
                           secondUnit = ConvertToLowerCase(splitValues[2]);

                           // Make the output string equal to the result from the SearchList Method.
                           output = SearchList(list, firstUnit, secondUnit, amount);

                           // Check the output value isn't null or empty, which is the two cases in which a valid input was not entered.
                           if (output != "null" && output != "")
                           {
                               // Display the output and ask the user if they want to convert another unit.
                               Console.WriteLine(output);

                               Console.WriteLine("\nWould you like to convert another unit? Y/N");

                               // Take the user input for the decision on whether to resart the loop.
                               userDecision = Console.ReadLine();

                               // Takes either y or n and sets isValid to either true or false, making the loop around all the code start again.
                               if (userDecision == "Y" || userDecision == "y")
                               {
                                   isValid = false;
                               }
                               else if (userDecision == "N" || userDecision == "n")
                               {
                                   isValid = true;
                               }
                               // If a valid y/n is not entered then assume yes.
                               else
                               {
                                   isValid = false;
                                   Console.WriteLine("No valid decision made, assuming yes!\n");
                               }
                           }
                       }
                   }
                       // Exception used to stop the wrong type being entered for the amount, displaying an error message and restarting the loop.
                   catch (FormatException ex)
                   {
                       Console.WriteLine("Please make sure your inputs are of the correct type.\n");
                       isValid = false;
                   }   
               }
           }
           while (isValid == false);

           Console.WriteLine("Conversion program complete/exited, press any key to exit!");

           // To stop the program from automatically closing.
           Console.ReadKey();
       }

       void ReadValues(List list, Node node)
       {
           string filename;
           StreamReader reader;

           // Variables used to split the string that is read from the file.
           string lineFromFile;
           string[] splitValues;
           // Cut the string at comma ',' and spaces ' '.
           char[] delimiter = { ',', ' ' };

           // Set the filepath of where convert.txt is located.
           // Change this value to you're own filepath in order to run the program. Alternatively place convert.txt in the Debug folder within the bin folder in your project.
           filename = @"convert.txt";
           // Used a StreamReader on the filename variable to search through the file.
           reader = new StreamReader(filename);

           // Keep looping until the end of the file is reached.
           while (!reader.EndOfStream)
           {
               // For each line assign it to a variable so it can be easily split.
               lineFromFile = reader.ReadLine();
               // Split at the delimiter and remove any values that are empty.
               splitValues = lineFromFile.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);

               // Make sure there are 3 values, as there are 2 units and a conversion factor, this stops empty lines being put into the list.
               if (splitValues.Length == 3)
               {
                   // Convert the two string values to lower case before continuing.
                   splitValues[0] = ConvertToLowerCase(splitValues[0]);
                   splitValues[1] = ConvertToLowerCase(splitValues[1]);

                   // Create a new node each time and assign the first, second and conversion units to the 3 values stored in splitValues[].
                   node = new Node();
                   node.firstUnit = splitValues[0];
                   node.secondUnit = splitValues[1];
                   // Make sure the convertFactor is converted to double.
                   node.convertFactor = Convert.ToDouble(splitValues[2]);

                   // Make the next node equal to the firstNode position.
                   node.next = list.firstNode;
                   list.firstNode = node;
                   
               }
               
           }    
       }

       private string ConvertToLowerCase(string input)
       {
           // Temp value used to store the string as it is changed character by character.
           string temp = "";

           // Loops for each character in the string.
           for (int i = 0; i < input.Length; i++)
           {
               // Checks to see if the ascii value of the character falls between 65 and 90, which means it's a capital letter.
               // Casting the individual character as a char returns it's ascii value.
               if ((char)input[i] >= 65 && (char)input[i] <= 90)
               {
                   // Increase the ascii value by 32, making it the lower case version of the letter.
                   int x = (char)input[i];
                   x += 32;
                   char c = (char)x;
                   // Make the temp string contain the new lower'd character.
                   temp += c;
               }
               else
               {
                   // If the character is already lower case then just add the unaltered version to the temp string.
                   temp += input[i];
               }
           }
           // Return the completed temp string.
           return temp;
       }

       private bool CheckFormat(string input)
       {
           // Similar to the ConvertToLower method in it's checks but instead returns a true of false depending on the ascii value of each character in the string.
           // Bool used to store whether the input is valid and is returned at the end.
           bool isValid = false;

           // Iterate through each character in the input string.
           for (int i = 0; i < input.Length; i++)
           {
               // Check to see if the ascii value falls between 65-90 or 97-122, which represents A-Z or a-z respectively.
               if ((char)input[i] >= 65 && (char)input[i] <= 90 || ((char)input[i] >= 97 && (char)input[i] <= 122))
               {
                   // If this is the case set the isValid bool to true.
                   isValid = true;
               }
               else
               {
                   // Else set it to false.
                   isValid = false;

                   // Break the loop so it can't override the fact it is not valid on the next character.
                   break;
               }
           }
           // Return the value to the calling method.
           return isValid;
       }

       private string SearchList(List list, string first, string second, double amount)
       {
           bool isFound = false;
           double newValue;
           string output = "";

           Node node = list.firstNode;
           // Keep going through the nodes until an empty one is found, indicating the end of the linked list.
           while (node != null)
           {
               // Check the user inputs equal both the first and second unit, making sure there is a valid case for that combination of units.
               if (first == node.firstUnit && second == node.secondUnit)
               {
                   // Calculate the value of the conversion and store it in a new value.
                   newValue = amount * node.convertFactor;

                   // Construct the output message so it can be returned and displayed.
                   output = amount + " " + first + "(s)" + " is equal to " + newValue + " " + second + "(s)";

                   // Set isFound to true so the value of output isn't changed to the base case.
                   isFound = true;
               }
               // Iterate through the nodes on each pass of the loop.
               node = node.next;
           }

           // If no valid conversion is found set output to null and display and error message.
           if (isFound == false)
           {
               Console.WriteLine("A valid conversion could not be found.\n");
               output = "null";
           }
           // Return the output to the calling method.
           return output;
       }
   }
}
