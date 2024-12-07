using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace DOLERA_AgeWell
{
    internal class Caregiver : CaregiverAccount, IUpdateData
    {
        public Caregiver(string username, string password, string firstName, string middleName, string lastName, int age, string sex, string contactNumber, UserTypeList userTypeList, string specialization, string emailAddress) :
                base(username, password, firstName, middleName, lastName, age, sex, contactNumber, userTypeList, specialization, emailAddress)
        {

        }

        UserTypeList userTypeList = new UserTypeList();
        ExceptionHandling exceptionHandling = new ExceptionHandling();

        public void cMenu()
        {
            while (true)
            {
                try
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkCyan;

                    UserMenu userMenu = new UserMenu();
                    userMenu.caregiverMenu();

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
                            cProfile();
                            break;

                        case 2:
                            Console.Clear();
                            assignedResidents();

                            Medication medication = new Medication();
                            medication.cMedication();
                            break;

                        case 3:
                            Appointments appointments = new Appointments(username, password, firstName, middleName, lastName, age, sex, contactNumber, userTypeList, specialization, emailAddress);
                            appointments.cAppointments();
                            break;

                        case 4:
                            Console.Clear();
                            assignedResidents();

                            HealthRecords healthRecords = new HealthRecords();
                            healthRecords.cHealthRecords();
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

        public void cProfile()
        {
            void readData()
            {
                string baseDirectory = AppContext.BaseDirectory;
                string filePath = Path.Combine(baseDirectory, "AGEWELL_Database", "CaregiverAccount_Files", $"{username}.txt");

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
                    cMenu();
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
                string filePath = Path.Combine(baseDirectory, "AGEWELL_Files", "cPROFILE_AgeWell.txt");

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
                            cMenu();
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
                    string filePath = Path.Combine(baseDirectory, "AGEWELL_Database", "CaregiverAccount_Files", $"{username}.txt");

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

                    cMenu();

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
                    Console.SetCursorPosition(62, 32);
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
                    string filePath = Path.Combine(baseDirectory, "AGEWELL_Database", "CaregiverAccount_Files", $"{username}.txt");

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

                    cMenu();

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
                    string filePath = Path.Combine(baseDirectory, "AGEWELL_Database", "CaregiverAccount_Files", $"{username}.txt");

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

                    cMenu();

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
                    string filePath = Path.Combine(baseDirectory, "AGEWELL_Database", "CaregiverAccount_Files", $"{username}.txt");

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

                    cMenu();

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

        public void assignedResidents()
        {
            string baseDirectory = AppContext.BaseDirectory;
            string withPCFilePath = Path.Combine(baseDirectory, "AGEWELL_Database", "Users_List", "Residents_withPersonalCaregiver_List.txt");

            try
            {
                if (!File.Exists(withPCFilePath))
                {
                    exceptionHandling.message("Error: File not found.", ConsoleColor.DarkRed);
                    Thread.Sleep(1000);
                    return;
                }

                List<(string username, string name)> residentsUnderCaregiver = new List<(string, string)>();

                using (StreamReader reader = new StreamReader(withPCFilePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.Contains($"{username}"))
                        {
                            string[] parts = line.Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                            if (parts.Length >= 2)
                            {
                                string residentUsername = parts[0].Trim();
                                string residentName = parts[1].Trim();
                                residentsUnderCaregiver.Add((residentUsername, residentName));
                            }
                        }
                    }
                }

                if (residentsUnderCaregiver.Count > 0)
                {
                    string message = $"[ {username}'s Residents Under Care ]";
                    int windowWidth = Console.WindowWidth;
                    int padding = (windowWidth - message.Length) / 2;

                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"\n\n\n\n\n\n\n\n\n\n{new string(' ', padding)}{message}\n\n");

                    var table = new Table
                    {
                        Padding = 5,
                        HeaderTextAlignRight = false,
                        RowTextAlignRight = false
                    };

                    table.Header("Username", "Name");

                    foreach (var resident in residentsUnderCaregiver)
                    {
                        table.AddRow(resident.username, resident.name);
                    }

                    string tableOutput = table.ToString();
                    int consoleWidth = Console.WindowWidth;
                    int tableWidth = tableOutput.Split('\n')[0].Length;
                    int tablePadding = (consoleWidth - tableWidth) / 2;

                    string centeredTable = string.Join(
                        Environment.NewLine,
                        tableOutput.Split('\n').Select(line => new string(' ', Math.Max(0, tablePadding)) + line)
                    );

                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine(centeredTable);
                }
                else
                {
                    exceptionHandling.message($"No residents found under the care of {username}.", ConsoleColor.DarkRed);
                    Thread.Sleep(1000);

                    cMenu();
                    return;
                }
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
    }
}