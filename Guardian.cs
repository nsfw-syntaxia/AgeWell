using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace DOLERA_AgeWell
{
    internal class Guardian : GuardianAccount, IUpdateData
    {
        public Guardian(string username, string password, string firstName, string middleName, string lastName, int age, string sex, string contactNumber, UserTypeList userTypeList, string emailAddress) :
                   base(username, password, firstName, middleName, lastName, age, sex, contactNumber, userTypeList, emailAddress)
        {

        }

        UserTypeList userTypeList = new UserTypeList();
        ExceptionHandling exceptionHandling = new ExceptionHandling();

        public void gMenu()
        {
            while (true)
            {
                try
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkCyan;

                    UserMenu userMenu = new UserMenu();
                    userMenu.guardianMenu();

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.SetCursorPosition(72, 18);
                    Console.WriteLine($"Welcome, {username}!");

                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write("\n\n\n\n\n\n\n\n\n\n\t\t\t\t\t\t\t\t\t          ");
                    int key = int.Parse(Console.ReadLine());

                    switch (key)
                    {
                        case 0:
                            SystemStart systemStart = new SystemStart();
                            systemStart.logUser();
                            break;

                        case 1:
                            gProfile();
                            break;

                        case 2:
                            viewMedication();
                            continueKey("Press any key to return to menu.");
                            break;

                        case 3:
                            viewAppointments();
                            continueKey("Press any key to return to menu.");
                            break;

                        case 4:
                            viewHealthRecords();
                            break;

                        default:
                            throw new FormatException("Invalid option. Please try again.");
                    }
                }
                catch (FormatException ex)
                {
                    exceptionHandling.message($"Error: {ex.Message}", ConsoleColor.DarkRed);
                    Thread.Sleep(1000);
                }
            }
        }

        public void gProfile()
        {
            void readData()
            {
                string baseDirectory = AppContext.BaseDirectory;
                string filePath = Path.Combine(baseDirectory, "AGEWELL_Database", "GuardianAccount_Files", $"{username}.txt");

                if (File.Exists(filePath))
                {
                    try
                    {
                        using (StreamReader reader = new StreamReader(filePath))
                        {
                            string profileData = reader.ReadToEnd();
                            string message = $"[ {username}'s Profile ]";
                            int windowWidth = Console.WindowWidth;
                            int padding = (windowWidth - message.Length) / 2;

                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine($"\n\n\n\n\n\n\n\n\n\n{new string(' ', padding)}{message}\n");
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            Console.WriteLine(profileData);
                        }
                    }
                    catch (Exception ex)
                    {
                        exceptionHandling.message($"Error: {ex.Message}", ConsoleColor.DarkRed);
                        Thread.Sleep(1000);
                    }
                }
                else
                {
                    exceptionHandling.message("Error: User profile not found.", ConsoleColor.DarkRed);
                    Thread.Sleep(1000);
                }
            }

            while (true)
            {
                Console.Clear();

                readData();

                string message = "Update Profile (Y/N)? ";
                int consoleWidth = Console.WindowWidth;
                int textPadding = (consoleWidth - message.Length) / 2;

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write($"\n\n{new string(' ', textPadding)}{message}");

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                string updateKey = Console.ReadLine().ToLower();
                if (updateKey == "y")
                {
                    updateProfile();
                    break;
                }
                else if (updateKey == "n")
                {
                    gMenu();
                    break;
                }
                else
                {
                    exceptionHandling.message("Error: Invalid option. Please try again.", ConsoleColor.DarkRed);
                    Thread.Sleep(1000);
                }
            }
        }

        public void updateProfile()
        {
            void readData()
            {
                string baseDirectory = AppContext.BaseDirectory;
                string filePath = Path.Combine(baseDirectory, "AGEWELL_Files", "gPROFILE_AgeWell.txt");

                try
                {
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            Console.WriteLine(line);
                        }
                    }
                }
                catch (FileNotFoundException)
                {
                    exceptionHandling.message("Error: File not found.", ConsoleColor.DarkRed);
                }
                catch (UnauthorizedAccessException)
                {
                    exceptionHandling.message("Error: You do not have permission to access this file.", ConsoleColor.DarkRed);
                }
                catch (Exception ex)
                {
                    exceptionHandling.message($"Error: {ex.Message}", ConsoleColor.DarkRed);
                }
            }

            while (true)
            {
                try
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkCyan;

                    readData();

                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write("\n\n\t\t\t\t\t\t\t\t\t          ");
                    int key = int.Parse(Console.ReadLine());

                    switch (key)
                    {
                        case 0:
                            gMenu();
                            break;

                        case 1:
                            updateAge();
                            break;

                        case 2:
                            updateEmailAddress();
                            break;

                        case 3:
                            updateContactNumber();
                            break;

                        case 4:
                            updatePassword();
                            break;

                        default:
                            throw new FormatException("Invalid option. Please try again.");
                    }
                }
                catch (FormatException ex)
                {
                    exceptionHandling.message($"Error: {ex.Message}", ConsoleColor.DarkRed);
                    Thread.Sleep(1000);
                }
            }
        }

        public void updateAge()
        {
            while (true)
            {
                try
                {
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.SetCursorPosition(78, 29);
                    Console.WriteLine("Age: ");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.SetCursorPosition(83, 29);
                    int newAge = int.Parse(Console.ReadLine());

                    if (newAge <= 0)
                    {
                        throw new ArgumentException("Age must be greater than zero.");
                    }

                    string baseDirectory = AppContext.BaseDirectory;
                    string filePath = Path.Combine(baseDirectory, "AGEWELL_Database", "GuardianAccount_Files", $"{username}.txt");

                    var lines = new List<string>(File.ReadAllLines(filePath));

                    for (int i = 0; i < lines.Count; i++)
                    {
                        if (lines[i].Contains("Age:"))
                        {
                            lines[i] = $"\t\t\t\t\t\t\t\tAge:\t\t\t{newAge}";
                            break;
                        }
                    }

                    File.WriteAllLines(filePath, lines);

                    exceptionHandling.message("Age updated successfully!", ConsoleColor.DarkGreen);
                    Thread.Sleep(3000);

                    gMenu();

                    break;
                }
                catch (FormatException)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.SetCursorPosition(62, 32);
                    Console.WriteLine("Error: Invalid input. Please try again.");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Thread.Sleep(1000);

                    Console.SetCursorPosition(62, 32);
                    Console.Write(new string(' ', Console.WindowWidth - 80));
                    Console.SetCursorPosition(62, 31);
                }
                catch (ArgumentException ex)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.SetCursorPosition(62, 32);
                    Console.WriteLine($"Error: {ex.Message}");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Thread.Sleep(1000);

                    Console.SetCursorPosition(62, 32);
                    Console.Write(new string(' ', Console.WindowWidth - 80));
                    Console.SetCursorPosition(62, 32);
                }
                catch (IOException)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.SetCursorPosition(56, 32);
                    Console.WriteLine("Error: Sorry, program could not access the file.");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Thread.Sleep(1000);

                    Console.SetCursorPosition(56, 32);
                    Console.Write(new string(' ', Console.WindowWidth - 80));
                    Console.SetCursorPosition(56, 32);
                }
            }
        }

        public void updateEmailAddress()
        {
            while (true)
            {
                try
                {
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.SetCursorPosition(68, 29);
                    Console.WriteLine("Email Address: ");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.SetCursorPosition(83, 29);
                    string newEA = Console.ReadLine();

                    if (newEA.Contains("@"))
                    {
                        if (newEA.EndsWith(".com") || newEA.EndsWith(".ph"))
                        {

                        }
                        else
                        {
                            throw new Exception("Email address must end with '.com' or '.ph'.");
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Email address must contain '@'.");
                    }

                    string baseDirectory = AppContext.BaseDirectory;
                    string filePath = Path.Combine(baseDirectory, "AGEWELL_Database", "GuardianAccount_Files", $"{username}.txt");

                    var lines = new List<string>(File.ReadAllLines(filePath));

                    for (int i = 0; i < lines.Count; i++)
                    {
                        if (lines[i].Contains("Email Address:"))
                        {
                            lines[i] = $"\t\t\t\t\t\t\t\tEmail Address:\t\t{newEA}";
                            break;
                        }
                    }

                    File.WriteAllLines(filePath, lines);

                    exceptionHandling.message("Email address updated successfully!", ConsoleColor.DarkGreen);
                    Thread.Sleep(3000);

                    gMenu();

                    break;
                }
                catch (ArgumentException ex)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.SetCursorPosition(62, 32);
                    Console.WriteLine($"Error: {ex.Message}");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Thread.Sleep(1000);

                    Console.SetCursorPosition(62, 32);
                    Console.Write(new string(' ', Console.WindowWidth - 80));
                    Console.SetCursorPosition(62, 32);
                }
                catch (IOException)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.SetCursorPosition(56, 32);
                    Console.WriteLine("Error: Sorry, program could not access the file.");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Thread.Sleep(1000);

                    Console.SetCursorPosition(56, 32);
                    Console.Write(new string(' ', Console.WindowWidth - 80));
                    Console.SetCursorPosition(56, 32);
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.SetCursorPosition(56, 32);
                    Console.WriteLine($"Error: {ex.Message}");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Thread.Sleep(1000);

                    Console.SetCursorPosition(56, 32);
                    Console.Write(new string(' ', Console.WindowWidth - 80));
                    Console.SetCursorPosition(56, 32);
                }
            }
        }

        public void updateContactNumber()
        {
            while (true)
            {
                try
                {
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.SetCursorPosition(67, 29);
                    Console.WriteLine("Contact Number: ");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.SetCursorPosition(83, 29);
                    string newCN = Console.ReadLine();

                    string baseDirectory = AppContext.BaseDirectory;
                    string filePath = Path.Combine(baseDirectory, "AGEWELL_Database", "GuardianAccount_Files", $"{username}.txt");

                    var lines = new List<string>(File.ReadAllLines(filePath));

                    for (int i = 0; i < lines.Count; i++)
                    {
                        if (lines[i].Contains("Contact Number:"))
                        {
                            lines[i] = $"\t\t\t\t\t\t\t\tContact Number:\t\t{newCN}";
                            break;
                        }
                    }

                    File.WriteAllLines(filePath, lines);

                    exceptionHandling.message("Contact number updated successfully!", ConsoleColor.DarkGreen);
                    Thread.Sleep(3000);

                    gMenu();

                    break;
                }
                catch (IOException)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.SetCursorPosition(56, 32);
                    Console.WriteLine("Error: Sorry, program could not access the file.");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Thread.Sleep(1000);

                    Console.SetCursorPosition(56, 32);
                    Console.Write(new string(' ', Console.WindowWidth - 80));
                    Console.SetCursorPosition(56, 32);
                }
            }
        }

        public void updatePassword()
        {
            while (true)
            {
                try
                {
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.SetCursorPosition(73, 29);
                    Console.WriteLine("Password: ");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.SetCursorPosition(83, 29);
                    string newPassword = Console.ReadLine();

                    string baseDirectory = AppContext.BaseDirectory;
                    string filePath = Path.Combine(baseDirectory, "AGEWELL_Database", "GuardianAccount_Files", $"{username}.txt");

                    var lines = new List<string>(File.ReadAllLines(filePath));

                    for (int i = 0; i < lines.Count; i++)
                    {
                        if (lines[i].Contains("Password:"))
                        {
                            lines[i] = $"\t\t\t\t\t\t\t\tPassword:\t\t{newPassword}";
                            break;
                        }
                    }

                    File.WriteAllLines(filePath, lines);

                    exceptionHandling.message("Password updated successfully!", ConsoleColor.DarkGreen);
                    Thread.Sleep(3000);

                    gMenu();

                    break;
                }
                catch (IOException)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.SetCursorPosition(56, 32);
                    Console.WriteLine("Error: Sorry, program could not access the file.");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Thread.Sleep(1000);

                    Console.SetCursorPosition(56, 32);
                    Console.Write(new string(' ', Console.WindowWidth - 80));
                    Console.SetCursorPosition(56, 32);
                }
            }
        }

        public void viewMedication()
        {
            string baseDirectory = AppContext.BaseDirectory;
            string guardianFilePath = Path.Combine(baseDirectory, "AGEWELL_Database", "GuardianAccount_Files", $"{username}.txt");

            if (!File.Exists(guardianFilePath))
            {
                exceptionHandling.message("Error: File not found.", ConsoleColor.DarkRed);
                return;
            }

            string guardianName = null;

            foreach (var line in File.ReadLines(guardianFilePath))
            {
                if (line.Contains("Name:"))
                {
                    guardianName = line.Split(new[] { "Name:" }, StringSplitOptions.None)[1].Trim();
                    break;
                }
            }

            if (string.IsNullOrEmpty(guardianName))
            {
                exceptionHandling.message("Error: Guardian's name not found.", ConsoleColor.DarkRed);
                return;
            }

            string residentDirectory = Path.Combine(baseDirectory, "AGEWELL_Database", "ResidentAccount_Files");

            string[] residentFiles = Directory.GetFiles(residentDirectory, "*.txt");

            bool foundResident = false;

            foreach (var residentFile in residentFiles)
            {
                string residentUsername = null;
                string residentGuardianName = null;

                foreach (var line in File.ReadLines(residentFile))
                {
                    if (line.Contains("Username:"))
                    {
                        residentUsername = line.Split(new[] { "Username:" }, StringSplitOptions.None)[1].Trim();
                    }
                    else if (line.Contains("Guardian's Name:"))
                    {
                        residentGuardianName = line.Split(new[] { "Guardian's Name:" }, StringSplitOptions.None)[1].Trim();
                    }

                    if (residentUsername != null && residentGuardianName != null)
                    {
                        break;
                    }
                }

                if (residentGuardianName?.Equals(guardianName, StringComparison.OrdinalIgnoreCase) == true)
                {
                    foundResident = true;

                    Medication medication = new Medication();
                    medication.viewMedication(residentUsername);
                }
            }

            if (!foundResident)
            {
                exceptionHandling.message("Error: No residents found with the guardian.", ConsoleColor.DarkRed);
            }
        }

        public void viewAppointments()
        {
            string baseDirectory = AppContext.BaseDirectory;
            string guardianFilePath = Path.Combine(baseDirectory, "AGEWELL_Database", "GuardianAccount_Files", $"{username}.txt");

            if (!File.Exists(guardianFilePath))
            {
                exceptionHandling.message("Error: File not found.", ConsoleColor.DarkRed);
                return;
            }

            string guardianName = null;

            foreach (var line in File.ReadLines(guardianFilePath))
            {
                if (line.Contains("Name:"))
                {
                    guardianName = line.Split(new[] { "Name:" }, StringSplitOptions.None)[1].Trim();
                    break;
                }
            }

            if (string.IsNullOrEmpty(guardianName))
            {
                exceptionHandling.message("Error: Guardian's name not found.", ConsoleColor.DarkRed);
                return;
            }

            string residentDirectory = Path.Combine(baseDirectory, "AGEWELL_Database", "ResidentAccount_Files");

            string[] residentFiles = Directory.GetFiles(residentDirectory, "*.txt");

            bool foundResident = false;

            foreach (var residentFile in residentFiles)
            {
                string residentUsername = null;
                string residentGuardianName = null;

                foreach (var line in File.ReadLines(residentFile))
                {
                    if (line.Contains("Username:"))
                    {
                        residentUsername = line.Split(new[] { "Username:" }, StringSplitOptions.None)[1].Trim();
                    }
                    else if (line.Contains("Guardian's Name:"))
                    {
                        residentGuardianName = line.Split(new[] { "Guardian's Name:" }, StringSplitOptions.None)[1].Trim();
                    }

                    if (residentUsername != null && residentGuardianName != null)
                    {
                        break;
                    }
                }

                if (residentGuardianName?.Equals(guardianName, StringComparison.OrdinalIgnoreCase) == true)
                {
                    foundResident = true;

                    Appointments appointments = new Appointments(username, password, " ", " ", " ", 0, " ", " ", userTypeList, " ", " ");
                    appointments.viewAppointment(residentUsername);
                }
            }

            if (!foundResident)
            {
                exceptionHandling.message("Error: No residents found with the guardian.", ConsoleColor.DarkRed);
            }
        }

        public void viewHealthRecords()
        {
            string baseDirectory = AppContext.BaseDirectory;
            string guardianFilePath = Path.Combine(baseDirectory, "AGEWELL_Database", "GuardianAccount_Files", $"{username}.txt");

            if (!File.Exists(guardianFilePath))
            {
                exceptionHandling.message("Error: File not found.", ConsoleColor.DarkRed);
                return;
            }

            string guardianName = null;

            foreach (var line in File.ReadLines(guardianFilePath))
            {
                if (line.Contains("Name:"))
                {
                    guardianName = line.Split(new[] { "Name:" }, StringSplitOptions.None)[1].Trim();
                    break;
                }
            }

            if (string.IsNullOrEmpty(guardianName))
            {
                exceptionHandling.message("Error: Guardian's name not found.", ConsoleColor.DarkRed);
                return;
            }

            string residentDirectory = Path.Combine(baseDirectory, "AGEWELL_Database", "ResidentAccount_Files");

            string[] residentFiles = Directory.GetFiles(residentDirectory, "*.txt");

            bool foundResident = false;

            foreach (var residentFile in residentFiles)
            {
                string residentUsername = null;
                string residentGuardianName = null;

                foreach (var line in File.ReadLines(residentFile))
                {
                    if (line.Contains("Username:"))
                    {
                        residentUsername = line.Split(new[] { "Username:" }, StringSplitOptions.None)[1].Trim();
                    }
                    else if (line.Contains("Guardian's Name:"))
                    {
                        residentGuardianName = line.Split(new[] { "Guardian's Name:" }, StringSplitOptions.None)[1].Trim();
                    }

                    if (residentUsername != null && residentGuardianName != null)
                    {
                        break;
                    }
                }

                if (residentGuardianName?.Equals(guardianName, StringComparison.OrdinalIgnoreCase) == true)
                {
                    foundResident = true;

                    HealthRecords healthRecords = new HealthRecords();
                    healthRecords.healthRecords(residentUsername);
                }
            }

            if (!foundResident)
            {
                exceptionHandling.message("Error: No residents found with the guardian.", ConsoleColor.DarkRed);
            }
        }

        private void continueKey(string message)
        {
            int windowWidth = Console.WindowWidth;
            int padding = (windowWidth - message.Length) / 2;

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\n\n\n{new string(' ', padding)}{message}\n");

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("\t\t\t\t\t\t\t\t\t          ");
            Console.ReadKey();
        }
    }
}