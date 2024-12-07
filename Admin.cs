using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace DOLERA_AgeWell
{
    internal class Admin : AdminLogin
    {
        UserTypeList userTypeList = new UserTypeList();
        ExceptionHandling exceptionHandling = new ExceptionHandling();

        public void aMenu()
        {
            while (true)
            {
                try
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkCyan;

                    UserMenu userMenu = new UserMenu();
                    userMenu.adminMenu();

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.SetCursorPosition(71, 18);
                    Console.WriteLine("Welcome, Administrator!");

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
                            rList();
                            break;

                        case 2:
                            gListMenu();
                            break;

                        case 3:
                            cListMenu();
                            break;

                        case 4:
                            allUsers();
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

        public void rList()
        {
            void readData()
            {
                string baseDirectory = AppContext.BaseDirectory;
                string filePath = Path.Combine(baseDirectory, "AGEWELL_Files", "rList_AgeWell.txt");

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
                            aMenu();
                            break;

                        case 1:
                            withoutPC();
                            break;

                        case 2:
                            withPC();
                            break;

                        case 3:
                            Console.Clear();
                            allResidents();
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

        public void withoutPC()
        {
            string baseDirectory = AppContext.BaseDirectory;
            string filePath = Path.Combine(baseDirectory, "AGEWELL_Database", "Users_List", "Residents_withoutPersonalCaregiver_List.txt");

            List<(string username, string name)> unassignedResidents = new List<(string username, string name)>();

            void readData()
            {
                try
                {
                    if (!File.Exists(filePath))
                    {
                        exceptionHandling.message("No residents without personal caregivers found.", ConsoleColor.DarkRed);
                        Thread.Sleep(1000);
                        return;
                    }

                    string[] lines = File.ReadAllLines(filePath);

                    for (int i = 3; i < lines.Length; i++)
                    {
                        string line = lines[i].Trim();
                        string[] parts = line.Split(new string[] { "\t\t" }, StringSplitOptions.None);

                        if (parts.Length >= 2)
                        {
                            string username = parts[0].Trim();
                            string name = parts[1].Trim();
                            unassignedResidents.Add((username, name));
                        }
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    exceptionHandling.message("Error: You do not have permission to access this file.", ConsoleColor.DarkRed);
                    Thread.Sleep(1000);
                }
                catch (IOException ex)
                {
                    exceptionHandling.message($"Error: {ex.Message}", ConsoleColor.DarkRed);
                    Thread.Sleep(1000);
                }
            }

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            readData();

            while (true)
            {
                Console.Clear();

                string headerMessage = "[ Residents With No Personal Caregiver ]";
                int consoleWidth = Console.WindowWidth;
                int headerPadding = (consoleWidth - headerMessage.Length) / 2;

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"\n\n\n\n\n\n\n\n\n\n{new string(' ', Math.Max(0, headerPadding))}{headerMessage}\n\n");

                var table = new Table
                {
                    Padding = 5,
                    HeaderTextAlignRight = false,
                    RowTextAlignRight = false
                };

                table.Header("Username", "Name");

                foreach (var resident in unassignedResidents)
                {
                    table.AddRow(resident.username, resident.name);
                }

                string tableOutput = table.ToString();

                int tableWidth = tableOutput.Split('\n')[0].Length;
                int tablePadding = (consoleWidth - tableWidth) / 2;

                string centeredTable = string.Join(
                    Environment.NewLine,
                    tableOutput.Split('\n').Select(line => new string(' ', Math.Max(0, tablePadding)) + line)
                );

                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine(centeredTable);

                string message = "Assign Caregiver to Resident (Y/N)? ";
                int textPadding = (consoleWidth - message.Length) / 2;

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write($"\n\n{new string(' ', Math.Max(0, textPadding))}{message}");

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                string assignKey = Console.ReadLine()?.ToLower();

                if (assignKey == "y")
                {
                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                    Console.Write(new string(' ', consoleWidth));
                    Console.SetCursorPosition(0, Console.CursorTop);

                    cList(1);
                    assignPC();
                    break;
                }
                else if (assignKey == "n")
                {
                    rList();
                    break;
                }
                else
                {
                    exceptionHandling.message("Error: Invalid option. Please try again.", ConsoleColor.DarkRed);
                    Thread.Sleep(1000);
                }
            }
        }

        public void assignPC()
        {
            string residentUsername;
            string caregiverUsername;

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.SetCursorPosition(66, 44);
                Console.WriteLine("Resident's Username: ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.SetCursorPosition(87, 44);
                residentUsername = Console.ReadLine();

                if (verifyResident(residentUsername))
                {
                    break;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.SetCursorPosition(69, 47);
                    Console.WriteLine("Error: Resident not found.");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Thread.Sleep(1000);

                    Console.SetCursorPosition(69, 47);
                    Console.Write(new string(' ', Console.WindowWidth - 80));
                    Console.SetCursorPosition(69, 47);
                }
            }

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.SetCursorPosition(65, 46);
                Console.WriteLine("Caregiver's Username: ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.SetCursorPosition(87, 46);
                caregiverUsername = Console.ReadLine();

                if (verifyCaregiver(caregiverUsername))
                {
                    break;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.SetCursorPosition(69, 49);
                    Console.WriteLine("Error: Caregiver not found.");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Thread.Sleep(1000);

                    Console.SetCursorPosition(69, 49);
                    Console.Write(new string(' ', Console.WindowWidth - 80));
                    Console.SetCursorPosition(69, 49);
                }
            }

            updateResidentswithPC(residentUsername, caregiverUsername);

            exceptionHandling.message("Caregiver assigned successfully!", ConsoleColor.DarkGreen);
            Thread.Sleep(3000);
        }

        private bool verifyResident(string username)
        {
            string baseDirectory = AppContext.BaseDirectory;
            string filePath = Path.Combine(baseDirectory, "AGEWELL_Database", "Users_List", "Residents_withoutPersonalCaregiver_List.txt");

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains(username))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool verifyCaregiver(string username)
        {
            string baseDirectory = AppContext.BaseDirectory;
            string filePath = Path.Combine(baseDirectory, "AGEWELL_Database", "Users_List", "Caregivers_List.txt");

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains(username))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void updateResidentswithPC(string residentUsername, string caregiverUsername)
        {
            string baseDirectory = AppContext.BaseDirectory;
            string withoutPCFilePath = Path.Combine(baseDirectory, "AGEWELL_Database", "Users_List", "Residents_withoutPersonalCaregiver_List.txt");
            string withPCFilePath = Path.Combine(baseDirectory, "AGEWELL_Database", "Users_List", "Residents_withPersonalCaregiver_List.txt");

            List<string> residents = new List<string>();

            if (!File.Exists(withPCFilePath))
            {
                File.Create(withPCFilePath).Dispose();
            }

            bool isFirstWriteNPC = !File.Exists(withoutPCFilePath) || new FileInfo(withoutPCFilePath).Length == 0;
            bool isFirstWritePC = !File.Exists(withPCFilePath) || new FileInfo(withPCFilePath).Length == 0;

            using (StreamReader reader = new StreamReader(withoutPCFilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains(residentUsername))
                    {
                        line = $"{line}\t\t{caregiverUsername}";

                        bool isResidentPC = false;
                        using (StreamReader readerPC = new StreamReader(withPCFilePath))
                        {
                            string linePC;
                            while ((linePC = readerPC.ReadLine()) != null)
                            {
                                if (linePC.Contains(residentUsername))
                                {
                                    isResidentPC = true;
                                    break;
                                }
                            }
                        }

                        if (!isResidentPC)
                        {
                            using (StreamWriter writer = new StreamWriter(withPCFilePath, true))
                            {
                                if (isFirstWritePC)
                                {
                                    writer.WriteLine($"\n\n\t\t\t\t\t\t\t\tUsername\t\tName\t\t\tCaregiver");
                                    isFirstWritePC = false;
                                }
                                writer.WriteLine(line);
                            }
                        }
                    }
                    else
                    {
                        residents.Add(line);
                    }
                }
            }

            using (StreamWriter writer = new StreamWriter(withoutPCFilePath))
            {
                if (isFirstWriteNPC)
                {
                    writer.WriteLine($"\n\n\t\t\t\t\t\t\t\tUsername\t\tName");
                    isFirstWriteNPC = false;
                }

                foreach (var resident in residents)
                {
                    writer.WriteLine(resident);
                }
            }
        }

        public void withPC()
        {
            string baseDirectory = AppContext.BaseDirectory;
            string filePath = Path.Combine(baseDirectory, "AGEWELL_Database", "Users_List", "Residents_withPersonalCaregiver_List.txt");

            List<(string username, string name)> assignedResidents = new List<(string username, string name)>();

            void readData()
            {
                try
                {
                    if (!File.Exists(filePath))
                    {
                        exceptionHandling.message("No residents with personal caregivers found.", ConsoleColor.DarkRed);
                        Thread.Sleep(1000);
                        return;
                    }

                    string[] lines = File.ReadAllLines(filePath);

                    for (int i = 3; i < lines.Length; i++)
                    {
                        string line = lines[i].Trim();
                        string[] parts = line.Split(new string[] { "\t\t" }, StringSplitOptions.None);

                        if (parts.Length >= 2)
                        {
                            string username = parts[0].Trim();
                            string name = parts[1].Trim();
                            assignedResidents.Add((username, name));
                        }
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    exceptionHandling.message("Error: You do not have permission to access this file.", ConsoleColor.DarkRed);
                    Thread.Sleep(1000);
                }
                catch (IOException ex)
                {
                    exceptionHandling.message($"Error: {ex.Message}", ConsoleColor.DarkRed);
                    Thread.Sleep(1000);
                }
            }

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            readData();

            Console.Clear();

            string headerMessage = "[ Residents With Personal Caregiver ]";
            int consoleWidth = Console.WindowWidth;
            int headerPadding = (consoleWidth - headerMessage.Length) / 2;

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\n\n\n\n\n\n\n\n\n\n{new string(' ', Math.Max(0, headerPadding))}{headerMessage}\n\n");

            var table = new Table
            {
                Padding = 5,
                HeaderTextAlignRight = false,
                RowTextAlignRight = false
            };

            table.Header("Username", "Name");

            foreach (var resident in assignedResidents)
            {
                table.AddRow(resident.username, resident.name);
            }

            string tableOutput = table.ToString();

            int tableWidth = tableOutput.Split('\n')[0].Length;
            int tablePadding = (consoleWidth - tableWidth) / 2;

            string centeredTable = string.Join(
                Environment.NewLine,
                tableOutput.Split('\n').Select(line => new string(' ', Math.Max(0, tablePadding)) + line)
            );

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(centeredTable);

            continueKey("Press any key to return to menu.");
            rList();
        }

        public void allResidents()
        {
            string baseDirectory = AppContext.BaseDirectory;
            string directoryPath = Path.Combine(baseDirectory, "AGEWELL_Database", "ResidentAccount_Files");

            if (Directory.Exists(directoryPath))
            {
                try
                {
                    string[] files = Directory.GetFiles(directoryPath, "*.txt");

                    if (files.Length == 0)
                    {
                        exceptionHandling.message("Error: No resident files found in the directory.", ConsoleColor.DarkRed);
                        return;
                    }

                    int rCount = 1;

                    foreach (string filePath in files)
                    {
                        string residentData = File.ReadAllText(filePath);

                        string number = $"[ {rCount} ]";
                        int windowWidth = Console.WindowWidth;
                        int padding = (windowWidth - number.Length) / 2;

                        string newlines = rCount == 1 ? "\n\n\n" : "\n\n";

                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($"{newlines}{new string(' ', padding)}{number}\n");
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.WriteLine(residentData);

                        rCount++;
                    }

                    continueKey("Press any key to return to menu.");
                    rList();
                }
                catch (Exception ex)
                {
                    exceptionHandling.message($"Error: {ex.Message}", ConsoleColor.DarkRed);
                }
            }
            else
            {
                exceptionHandling.message("Error: Directory not found.", ConsoleColor.DarkRed);
            }
        }

        public void gListMenu()
        {
            void readData()
            {
                string baseDirectory = AppContext.BaseDirectory;
                string filePath = Path.Combine(baseDirectory, "AGEWELL_Files", "gList_AgeWell.txt");

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
                            aMenu();
                            break;

                        case 1:
                            Console.Clear();
                            gList();
                            continueKey("Press any key to return to menu.");
                            gListMenu();
                            break;

                        case 2:
                            Console.Clear();
                            allGuardians();
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

        public void gList()
        {
            string baseDirectory = AppContext.BaseDirectory;
            string filePath = Path.Combine(baseDirectory, "AGEWELL_Database", "Users_List", "Guardians_List.txt");

            List<(string username, string name)> guardians = new List<(string username, string name)>();

            void readData()
            {
                try
                {
                    if (!File.Exists(filePath))
                    {
                        exceptionHandling.message("No registered guardians found.", ConsoleColor.DarkRed);
                        Thread.Sleep(1000);
                        return;
                    }

                    string[] lines = File.ReadAllLines(filePath);

                    for (int i = 3; i < lines.Length; i++)
                    {
                        string line = lines[i].Trim();
                        string[] parts = line.Split(new string[] { "\t\t" }, StringSplitOptions.None);

                        if (parts.Length >= 2)
                        {
                            string username = parts[0].Trim();
                            string name = parts[1].Trim();
                            guardians.Add((username, name));
                        }
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    exceptionHandling.message("Error: You do not have permission to access this file.", ConsoleColor.DarkRed);
                    Thread.Sleep(1000);
                }
                catch (IOException ex)
                {
                    exceptionHandling.message($"Error: {ex.Message}", ConsoleColor.DarkRed);
                    Thread.Sleep(1000);
                }
            }

            Console.ForegroundColor = ConsoleColor.DarkCyan;

            readData();

            Console.Clear();

            string headerMessage = "[ Registered Guardians ]";
            int consoleWidth = Console.WindowWidth;
            int headerPadding = (consoleWidth - headerMessage.Length) / 2;

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\n\n\n\n\n\n\n\n\n\n{new string(' ', Math.Max(0, headerPadding))}{headerMessage}\n\n");

            var table = new Table
            {
                Padding = 5,
                HeaderTextAlignRight = false,
                RowTextAlignRight = false
            };

            table.Header("Username", "Name");

            foreach (var guardian in guardians)
            {
                table.AddRow(guardian.username, guardian.name);
            }

            string tableOutput = table.ToString();

            int tableWidth = tableOutput.Split('\n')[0].Length;
            int tablePadding = (consoleWidth - tableWidth) / 2;

            string centeredTable = string.Join(
                Environment.NewLine,
                tableOutput.Split('\n').Select(line => new string(' ', Math.Max(0, tablePadding)) + line)
            );

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(centeredTable);
        }

        public void allGuardians()
        {
            string baseDirectory = AppContext.BaseDirectory;
            string directoryPath = Path.Combine(baseDirectory, "AGEWELL_Database", "GuardianAccount_Files");

            if (Directory.Exists(directoryPath))
            {
                try
                {
                    string[] files = Directory.GetFiles(directoryPath, "*.txt");

                    if (files.Length == 0)
                    {
                        exceptionHandling.message("Error: No guardian files found in the directory.", ConsoleColor.DarkRed);
                        return;
                    }

                    int gCount = 1;

                    foreach (string filePath in files)
                    {
                        string guardianData = File.ReadAllText(filePath);

                        string number = $"[ {gCount} ]";
                        int windowWidth = Console.WindowWidth;
                        int padding = (windowWidth - number.Length) / 2;

                        string newlines = gCount == 1 ? "\n\n\n" : "\n\n";

                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($"{newlines}{new string(' ', padding)}{number}\n");
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.WriteLine(guardianData);

                        gCount++;
                    }

                    continueKey("Press any key to return to menu.");
                    gListMenu();
                }
                catch (Exception ex)
                {
                    exceptionHandling.message($"Error: {ex.Message}", ConsoleColor.DarkRed);
                }
            }
            else
            {
                exceptionHandling.message("Error: Directory not found.", ConsoleColor.DarkRed);
            }
        }

        public void cListMenu()
        {
            void readData()
            {
                string baseDirectory = AppContext.BaseDirectory;
                string filePath = Path.Combine(baseDirectory, "AGEWELL_Files", "cList_AgeWell.txt");

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
                            aMenu();
                            break;

                        case 1:
                            Console.Clear();
                            cList();
                            continueKey("Press any key to return to menu.");
                            cListMenu();
                            break;

                        case 2:
                            Console.Clear();
                            allCaregivers();
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

        public void cList(int newline = 10)
        {
            string baseDirectory = AppContext.BaseDirectory;
            string filePath = Path.Combine(baseDirectory, "AGEWELL_Database", "Users_List", "Caregivers_List.txt");

            List<(string username, string name)> caregivers = new List<(string username, string name)>();

            void readData()
            {
                try
                {
                    if (!File.Exists(filePath))
                    {
                        exceptionHandling.message("No available caregivers found.", ConsoleColor.DarkRed);
                        Thread.Sleep(1000);
                        return;
                    }

                    string[] lines = File.ReadAllLines(filePath);

                    for (int i = 3; i < lines.Length; i++)
                    {
                        string line = lines[i].Trim();
                        string[] parts = line.Split(new string[] { "\t\t" }, StringSplitOptions.None);

                        if (parts.Length >= 2)
                        {
                            string username = parts[0].Trim();
                            string name = parts[1].Trim();
                            caregivers.Add((username, name));
                        }
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    exceptionHandling.message("Error: You do not have permission to access this file.", ConsoleColor.DarkRed);
                    Thread.Sleep(1000);
                }
                catch (IOException ex)
                {
                    exceptionHandling.message($"Error: {ex.Message}", ConsoleColor.DarkRed);
                    Thread.Sleep(1000);
                }
            }

            Console.ForegroundColor = ConsoleColor.DarkCyan;

            readData();

            string headerMessage = "[ Available Caregivers ]";
            int consoleWidth = Console.WindowWidth;
            int headerPadding = (consoleWidth - headerMessage.Length) / 2;

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\n{new string('\n', newline - 1)}{new string(' ', Math.Max(0, headerPadding))}{headerMessage}\n\n");

            var table = new Table
            {
                Padding = 5,
                HeaderTextAlignRight = false,
                RowTextAlignRight = false
            };

            table.Header("Username", "Name");

            foreach (var caregiver in caregivers)
            {
                table.AddRow(caregiver.username, caregiver.name);
            }

            string tableOutput = table.ToString();

            int tableWidth = tableOutput.Split('\n')[0].Length;
            int tablePadding = (consoleWidth - tableWidth) / 2;

            string centeredTable = string.Join(
                Environment.NewLine,
                tableOutput.Split('\n').Select(line => new string(' ', Math.Max(0, tablePadding)) + line)
            );

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(centeredTable);
        }

        public void allCaregivers()
        {
            string baseDirectory = AppContext.BaseDirectory;
            string directoryPath = Path.Combine(baseDirectory, "AGEWELL_Database", "CaregiverAccount_Files");

            if (Directory.Exists(directoryPath))
            {
                try
                {
                    string[] files = Directory.GetFiles(directoryPath, "*.txt");

                    if (files.Length == 0)
                    {
                        exceptionHandling.message("Error: No caregiver files found in the directory.", ConsoleColor.DarkRed);
                        return;
                    }

                    int cCount = 1;

                    foreach (string filePath in files)
                    {
                        string caregiverData = File.ReadAllText(filePath);

                        string number = $"[ {cCount} ]";
                        int windowWidth = Console.WindowWidth;
                        int padding = (windowWidth - number.Length) / 2;

                        string newlines = cCount == 1 ? "\n\n\n" : "\n\n";

                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($"{newlines}{new string(' ', padding)}{number}\n");
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.WriteLine(caregiverData);

                        cCount++;
                    }

                    continueKey("Press any key to return to menu.");
                    cListMenu();
                }
                catch (Exception ex)
                {
                    exceptionHandling.message($"Error: {ex.Message}", ConsoleColor.DarkRed);
                }
            }
            else
            {
                exceptionHandling.message("Error: Directory not found.", ConsoleColor.DarkRed);
            }
        }

        public void allUsers()
        {
            string baseDirectory = AppContext.BaseDirectory;
            string residentFolder = Path.Combine(baseDirectory, "AGEWELL_Database", "ResidentAccount_Files");
            string guardianFolder = Path.Combine(baseDirectory, "AGEWELL_Database", "GuardianAccount_Files");
            string caregiverFolder = Path.Combine(baseDirectory, "AGEWELL_Database", "CaregiverAccount_Files");

            try
            {
                var residentUsernames = Directory.Exists(residentFolder)
                    ? Directory.GetFiles(residentFolder, "*.txt").Select(file => Path.GetFileNameWithoutExtension(file)).ToList()
                    : new List<string>();

                var guardianUsernames = Directory.Exists(guardianFolder)
                    ? Directory.GetFiles(guardianFolder, "*.txt").Select(file => Path.GetFileNameWithoutExtension(file)).ToList()
                    : new List<string>();

                var caregiverUsernames = Directory.Exists(caregiverFolder)
                    ? Directory.GetFiles(caregiverFolder, "*.txt").Select(file => Path.GetFileNameWithoutExtension(file)).ToList()
                    : new List<string>();

                int residentCount = residentUsernames.Count;
                int guardianCount = guardianUsernames.Count;
                int caregiverCount = caregiverUsernames.Count;
                int totalUsers = residentCount + guardianCount + caregiverCount;

                Console.Clear();

                string title = "[ AgeWell Users ]";
                int consoleWidth = Console.WindowWidth;
                int titlePadding = (consoleWidth - title.Length) / 2;

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"\n\n\n\n\n{new string(' ', titlePadding)}{title}\n\n");

                var table = new Table
                {
                    Padding = 5,
                    HeaderTextAlignRight = false,
                    RowTextAlignRight = false
                };

                table.Header("Residents", "Guardians", "Caregivers");

                int maxRows = Math.Max(residentCount, Math.Max(guardianCount, caregiverCount));
                for (int i = 0; i < maxRows; i++)
                {
                    string resident = i < residentCount ? residentUsernames[i] : string.Empty;
                    string guardian = i < guardianCount ? guardianUsernames[i] : string.Empty;
                    string caregiver = i < caregiverCount ? caregiverUsernames[i] : string.Empty;

                    table.AddRow(resident, guardian, caregiver);
                }

                table.AddRow($"R Users: {residentCount.ToString()}", $"G Users: {guardianCount.ToString()}", $"C Users: {caregiverCount.ToString()}");
                table.AddRow(string.Empty, string.Empty, $"Total: {totalUsers.ToString()}");

                Console.ForegroundColor = ConsoleColor.DarkCyan;
                centerTable(table);

                deleteUserData(residentFolder, guardianFolder, caregiverFolder);
            }
            catch (UnauthorizedAccessException)
            {
                exceptionHandling.message("Error: You do not have permission to access these folders.", ConsoleColor.DarkRed);
            }
            catch (Exception ex)
            {
                exceptionHandling.message($"Error: {ex.Message}", ConsoleColor.DarkRed);
            }
        }

        private void deleteUserData(string residentFolder, string guardianFolder, string caregiverFolder)
        {
            try
            {
                string baseDirectory = AppContext.BaseDirectory;
                string medicationDirectoryPath = Path.Combine(baseDirectory, "AGEWELL_Database", "Medication_List");
                string appointmentsDirectoryPath = Path.Combine(baseDirectory, "AGEWELL_Database", "Appointments_List");
                string healthProfilesDirectoryPath = Path.Combine(baseDirectory, "AGEWELL_Database", "HealthRecords_List", "Medical Profiles");
                string doctorConsultationsDirectoryPath = Path.Combine(baseDirectory, "AGEWELL_Database", "HealthRecords_List", "Doctor Consultations");
                string healthAssessmentsDirectoryPath = Path.Combine(baseDirectory, "AGEWELL_Database", "HealthRecords_List", "Health Assessments");

                bool isFound = false;
                string user = string.Empty;

                while (!isFound)
                {
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.Write("\n\t\t\t\t\t\t\t\tUsername (Enter to exit): ");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    user = Console.ReadLine();

                    if (string.IsNullOrEmpty(user))
                    {
                        return;
                    }

                    string[] folders = { residentFolder, guardianFolder, caregiverFolder };
                    foreach (var folder in folders)
                    {
                        if (Directory.Exists(folder))
                        {
                            string filePath = Path.Combine(folder, $"{user}.txt");
                            if (File.Exists(filePath))
                            {
                                isFound = true;
                                File.Delete(filePath);
                                break;
                            }
                        }
                    }

                    if (!isFound)
                    {
                        exceptionHandling.message($"Error: Username '{user}' not found in any folder.", ConsoleColor.DarkRed);
                        Thread.Sleep(1000);
                    }
                }

                deleteUserFiles(user, medicationDirectoryPath, appointmentsDirectoryPath, healthProfilesDirectoryPath, doctorConsultationsDirectoryPath, healthAssessmentsDirectoryPath);
                removeUserList(user);

                exceptionHandling.message($"User data of '{user}' has been successfully deleted.", ConsoleColor.DarkGreen);
                Thread.Sleep(3000);

                allUsers();
            }
            catch (Exception ex)
            {
                exceptionHandling.message($"Error: {ex.Message}", ConsoleColor.DarkRed);
            }
        }

        private void deleteUserFiles(string username, string medicationDirectoryPath, string appointmentsDirectoryPath, string healthProfilesDirectoryPath, string doctorConsultationsDirectoryPath, string healthAssessmentsDirectoryPath)
        {
            try
            {
                string currentMonth = DateTime.Now.ToString("MMMM yyyy");

                string medicationFilePath = Path.Combine(medicationDirectoryPath, $"{username}_{currentMonth}_Medication.txt");
                if (File.Exists(medicationFilePath))
                {
                    File.Delete(medicationFilePath);
                }

                string appointmentsFilePath = Path.Combine(appointmentsDirectoryPath, $"{username}_{currentMonth}_Appointments.txt");
                if (File.Exists(appointmentsFilePath))
                {
                    File.Delete(appointmentsFilePath);
                }

                string healthProfileFilePath = Path.Combine(healthProfilesDirectoryPath, $"{username}.txt");
                if (File.Exists(healthProfileFilePath))
                {
                    File.Delete(healthProfileFilePath);
                }

                foreach (var file in Directory.GetFiles(doctorConsultationsDirectoryPath, $"{username}*"))
                {
                    if (File.Exists(file))
                    {
                        File.Delete(file);
                    }
                }

                foreach (var file in Directory.GetFiles(healthAssessmentsDirectoryPath, $"{username}*"))
                {
                    if (File.Exists(file))
                    {
                        File.Delete(file);
                    }
                }
            }
            catch (Exception ex)
            {
                exceptionHandling.message($"Error: {ex.Message}", ConsoleColor.DarkRed);
            }
        }

        private void removeUserList(string username)
        {
            string baseDirectory = AppContext.BaseDirectory;
            string usersDirectoryPath = Path.Combine(baseDirectory, "AGEWELL_Database", "Users_List");

            string[] listFilePaths =
            {
                Path.Combine(usersDirectoryPath, "Residents_withoutPersonalCaregiver_List.txt"),
                Path.Combine(usersDirectoryPath, "Residents_withPersonalCaregiver_List.txt"),
                Path.Combine(usersDirectoryPath, "Guardians_List.txt"),
                Path.Combine(usersDirectoryPath, "Caregivers_List.txt")
            };

            try
            {
                foreach (var listFilePath in listFilePaths)
                {
                    if (File.Exists(listFilePath))
                    {
                        List<string> lines = File.ReadAllLines(listFilePath).ToList();
                        lines = lines.Where(line => !line.Contains(username)).ToList();
                        File.WriteAllLines(listFilePath, lines);
                    }
                }
            }
            catch (Exception ex)
            {
                exceptionHandling.message($"Error: {ex.Message}", ConsoleColor.DarkRed);
            }
        }

        private void centerTable(Table table)
        {
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