using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace DOLERA_AgeWell
{
    internal class AgeWell
    {
        static void Main(string[] args)
        {
            SystemStart systemStart = new SystemStart();
            systemStart.loadingSystem();

            Thread.Sleep(100);
            Console.Clear();

            systemStart.logUser();
        }
    }
    
    interface IWriteData
    {
        void writeData();
    }

    interface IUpdateData
    {
        void updateProfile();
        void updateAge();
        void updateEmailAddress();
        void updateContactNumber();
        void updatePassword();
    }

    internal class Table
    {
        private const string TopLeftJoint = "╔";
        private const string TopRightJoint = "╗";
        private const string BottomLeftJoint = "╚";
        private const string BottomRightJoint = "╝";
        private const string TopJoint = "╦";
        private const string BottomJoint = "╩";
        private const string LeftJoint = "╠";
        private const string MiddleJoint = "╬";
        private const string RightJoint = "╣";
        private const char HorizontalLine = '═';
        private const string VerticalLine = "║";

        private string[] _headers;
        private List<string[]> _rows = new List<string[]>();
        public int Padding { get; set; } = 1;
        public bool HeaderTextAlignRight { get; set; }
        public bool RowTextAlignRight { get; set; }
        public void Header(params string[] headers)
        {
            _headers = headers;
        }
        public void AddRow(params string[] row)
        {
            _rows.Add(row);
        }
        public void ClearRows()
        {
            _rows.Clear();
        }
        private int[] GetMaxCellWidths(List<string[]> table)
        {
            var maximumColumns = 0;
            foreach (var row in table)
            {
                if (row.Length > maximumColumns)
                    maximumColumns = row.Length;
            }

            var maximumCellWidths = new int[maximumColumns];
            for (int i = 0; i < maximumCellWidths.Count(); i++)
                maximumCellWidths[i] = 0;

            var paddingCount = 0;
            if (Padding > 0)
            {
                paddingCount = Padding * 2;
            }

            foreach (var row in table)
            {
                for (int i = 0; i < row.Length; i++)
                {
                    var maxWidth = row[i].Length + paddingCount;

                    if (maxWidth > maximumCellWidths[i])
                        maximumCellWidths[i] = maxWidth;
                }
            }

            return maximumCellWidths;
        }
        private StringBuilder CreateTopLine(int[] maximumCellWidths, int rowColumnCount, StringBuilder formattedTable)
        {
            for (int i = 0; i < rowColumnCount; i++)
            {
                if (i == 0 && i == rowColumnCount - 1)
                    formattedTable.AppendLine(string.Format("{0}{1}{2}", TopLeftJoint, string.Empty.PadLeft(maximumCellWidths[i], HorizontalLine), TopRightJoint));
                else if (i == 0)
                    formattedTable.Append(string.Format("{0}{1}", TopLeftJoint, string.Empty.PadLeft(maximumCellWidths[i], HorizontalLine)));
                else if (i == rowColumnCount - 1)
                    formattedTable.AppendLine(string.Format("{0}{1}{2}", TopJoint, string.Empty.PadLeft(maximumCellWidths[i], HorizontalLine), TopRightJoint));
                else
                    formattedTable.Append(string.Format("{0}{1}", TopJoint, string.Empty.PadLeft(maximumCellWidths[i], HorizontalLine)));
            }

            return formattedTable;
        }
        private StringBuilder CreateBottomLine(int[] maximumCellWidths, int rowColumnCount, StringBuilder formattedTable)
        {
            for (int i = 0; i < rowColumnCount; i++)
            {
                if (i == 0 && i == rowColumnCount - 1)
                    formattedTable.AppendLine(string.Format("{0}{1}{2}", BottomLeftJoint, string.Empty.PadLeft(maximumCellWidths[i], HorizontalLine), BottomRightJoint));
                else if (i == 0)
                    formattedTable.Append(string.Format("{0}{1}", BottomLeftJoint, string.Empty.PadLeft(maximumCellWidths[i], HorizontalLine)));
                else if (i == rowColumnCount - 1)
                    formattedTable.AppendLine(string.Format("{0}{1}{2}", BottomJoint, string.Empty.PadLeft(maximumCellWidths[i], HorizontalLine), BottomRightJoint));
                else
                    formattedTable.Append(string.Format("{0}{1}", BottomJoint, string.Empty.PadLeft(maximumCellWidths[i], HorizontalLine)));
            }

            return formattedTable;
        }
        private StringBuilder CreateValueLine(int[] maximumCellWidths, string[] row, bool alignRight, StringBuilder formattedTable)
        {
            int cellIndex = 0;
            int lastCellIndex = row.Length - 1;

            var paddingString = string.Empty;
            if (Padding > 0)
                paddingString = string.Concat(Enumerable.Repeat(' ', Padding));

            foreach (var column in row)
            {
                var restWidth = maximumCellWidths[cellIndex];
                if (Padding > 0)
                    restWidth -= Padding * 2;

                var cellValue = alignRight ? column.PadLeft(restWidth, ' ') : column.PadRight(restWidth, ' ');

                if (cellIndex == 0 && cellIndex == lastCellIndex)
                    formattedTable.AppendLine(string.Format("{0}{1}{2}{3}{4}", VerticalLine, paddingString, cellValue, paddingString, VerticalLine));
                else if (cellIndex == 0)
                    formattedTable.Append(string.Format("{0}{1}{2}{3}", VerticalLine, paddingString, cellValue, paddingString));
                else if (cellIndex == lastCellIndex)
                    formattedTable.AppendLine(string.Format("{0}{1}{2}{3}{4}", VerticalLine, paddingString, cellValue, paddingString, VerticalLine));
                else
                    formattedTable.Append(string.Format("{0}{1}{2}{3}", VerticalLine, paddingString, cellValue, paddingString));

                cellIndex++;
            }
            return formattedTable;
        }
        private StringBuilder CreateHeaderLine(int[] maximumCellWidths, int previousRowColumnCount, int rowColumnCount, StringBuilder formattedTable)
        {
            var maximumCells = Math.Max(previousRowColumnCount, rowColumnCount);

            for (int i = 0; i < maximumCells; i++)
            {
                if (i == 0 && i == maximumCells - 1)
                {
                    formattedTable.AppendLine(string.Format("{0}{1}{2}", LeftJoint, string.Empty.PadLeft(maximumCellWidths[i], HorizontalLine), RightJoint));
                }
                else if (i == 0)
                {
                    formattedTable.Append(string.Format("{0}{1}", LeftJoint, string.Empty.PadLeft(maximumCellWidths[i], HorizontalLine)));
                }
                else if (i == maximumCells - 1)
                {
                    if (i > previousRowColumnCount)
                        formattedTable.AppendLine(string.Format("{0}{1}{2}", TopJoint, string.Empty.PadLeft(maximumCellWidths[i], HorizontalLine), TopRightJoint));
                    else if (i > rowColumnCount)
                        formattedTable.AppendLine(string.Format("{0}{1}{2}", BottomJoint, string.Empty.PadLeft(maximumCellWidths[i], HorizontalLine), BottomRightJoint));
                    else if (i > previousRowColumnCount - 1)
                        formattedTable.AppendLine(string.Format("{0}{1}{2}", MiddleJoint, string.Empty.PadLeft(maximumCellWidths[i], HorizontalLine), TopRightJoint));
                    else if (i > rowColumnCount - 1)
                        formattedTable.AppendLine(string.Format("{0}{1}{2}", MiddleJoint, string.Empty.PadLeft(maximumCellWidths[i], HorizontalLine), BottomRightJoint));
                    else
                        formattedTable.AppendLine(string.Format("{0}{1}{2}", MiddleJoint, string.Empty.PadLeft(maximumCellWidths[i], HorizontalLine), RightJoint));
                }
                else
                {
                    if (i > previousRowColumnCount)
                        formattedTable.Append(string.Format("{0}{1}", TopJoint, string.Empty.PadLeft(maximumCellWidths[i], HorizontalLine)));
                    else if (i > rowColumnCount)
                        formattedTable.Append(string.Format("{0}{1}", BottomJoint, string.Empty.PadLeft(maximumCellWidths[i], HorizontalLine)));
                    else
                        formattedTable.Append(string.Format("{0}{1}", MiddleJoint, string.Empty.PadLeft(maximumCellWidths[i], HorizontalLine)));
                }
            }
            return formattedTable;
        }
        public override string ToString()
        {
            var table = new List<string[]>();

            var firstRowIsHeader = false;
            if (_headers?.Any() == true)
            {
                table.Add(_headers);
                firstRowIsHeader = true;
            }

            if (_rows?.Any() == true)
                table.AddRange(_rows);

            if (!table.Any())
                return string.Empty;

            var formattedTable = new StringBuilder();

            var previousRow = table.FirstOrDefault();
            var nextRow = table.FirstOrDefault();

            int[] maximumCellWidths = GetMaxCellWidths(table);

            formattedTable = CreateTopLine(maximumCellWidths, nextRow.Count(), formattedTable);

            int rowIndex = 0;
            int lastRowIndex = table.Count - 1;

            for (int i = 0; i < table.Count; i++)
            {
                var row = table[i];

                var align = RowTextAlignRight;
                if (i == 0 && firstRowIsHeader)
                    align = HeaderTextAlignRight;

                formattedTable = CreateValueLine(maximumCellWidths, row, align, formattedTable);
                previousRow = row;
                if (rowIndex != lastRowIndex)
                {
                    nextRow = table[rowIndex + 1];
                    formattedTable = CreateHeaderLine(maximumCellWidths, previousRow.Count(), nextRow.Count(), formattedTable);
                }
                rowIndex++;
            }
            formattedTable = CreateBottomLine(maximumCellWidths, previousRow.Count(), formattedTable);
            return formattedTable.ToString();
        }
    }

    internal class SystemStart
    {
        UserTypeList userTypeList = new UserTypeList();
        ExceptionHandling exceptionHandling = new ExceptionHandling();
        
        public void loadingSystem()
        {
            void readData()
            {
                string baseDirectory = AppContext.BaseDirectory;
                string filePath = Path.Combine(baseDirectory, "AGEWELL_Files", "OPEN_AgeWell.txt");

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

            int consoleWidth = Console.WindowWidth;

            Thread.Sleep(1000);

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            readData();

            int loadingBarStartLine = Console.CursorTop + 7;
            Console.SetCursorPosition(0, loadingBarStartLine);

            for (int x = 20; x <= 100; x += 5)
            {
                string loadingText = $"Loading... {x} %";
                int loadingTextPosition = (consoleWidth - loadingText.Length) / 2;

                Console.SetCursorPosition(loadingTextPosition, loadingBarStartLine - 2);
                Console.Write(loadingText);

                int barWidth = x / 2;
                string bar = new string('█', barWidth) + new string('░', (100 / 2) - barWidth);
                int barPosition = (consoleWidth - (100 / 2)) / 2;

                Console.SetCursorPosition(barPosition, loadingBarStartLine);
                Console.Write(bar);

                Thread.Sleep(100);

                if (x == 50 || x == 90 || x == 83 || x == 96)
                {
                    Thread.Sleep(100);
                }
            }
        }
        
        public void logUser()
        {
            void readData()
            {
                string baseDirectory = AppContext.BaseDirectory;
                string filePath = Path.Combine(baseDirectory, "AGEWELL_Files", "OPEN_AgeWell.txt");

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
            
            string[] options = { "L O G I N", "R E G I S T E R", "E X I T" };
            int selectedOption = 0;
            ConsoleKey key;

            do
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                readData();

                printMenu(options, selectedOption);

                key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.UpArrow)
                {
                    selectedOption = (selectedOption == 0) ? options.Length - 1 : selectedOption - 1;
                }
                else if (key == ConsoleKey.DownArrow)
                {
                    selectedOption = (selectedOption == options.Length - 1) ? 0 : selectedOption + 1;
                }

            } while (key != ConsoleKey.Enter);

            methods(selectedOption);

            void printMenu(string[] options, int selectedOption)
            {
                int windowWidth = Console.WindowWidth;

                Console.WriteLine("\n\n");

                for (int i = 0; i < options.Length; i++)
                {
                    int padding = (windowWidth - options[i].Length) / 2;
                    string line = new string(' ', windowWidth);

                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.DarkCyan;

                    if (i == selectedOption)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkYellow;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }

                    Console.WriteLine(line.Substring(0, padding) + options[i] + line.Substring(padding + options[i].Length));
                    Console.ResetColor();
                }
            }

            void methods(int option)
            {
                switch (option)
                {
                    case 0:
                        loginMenu();
                        break;

                    case 1:
                        registerMenu();
                        break;

                    case 2:
                        Environment.Exit(0);
                        break;
                }
            }
        }

        public void loginMenu()
        {
            void readData()
            {
                string baseDirectory = AppContext.BaseDirectory;
                string filePath = Path.Combine(baseDirectory, "AGEWELL_Files", "LOGIN_AgeWell.txt");

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

            string[] options = { "U S E R", "A D M I N I S T R A T O R", "B A C K" };
            int selectedOption = 0;
            ConsoleKey key;

            do
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                readData();

                printMenu(options, selectedOption);

                key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.UpArrow)
                {
                    selectedOption = (selectedOption == 0) ? options.Length - 1 : selectedOption - 1;
                }
                else if (key == ConsoleKey.DownArrow)
                {
                    selectedOption = (selectedOption == options.Length - 1) ? 0 : selectedOption + 1;
                }

            } while (key != ConsoleKey.Enter);

            methods(selectedOption);

            void printMenu(string[] options, int selectedOption)
            {
                int windowWidth = Console.WindowWidth;

                Console.WriteLine("\n\n");

                for (int i = 0; i < options.Length; i++)
                {
                    int padding = (windowWidth - options[i].Length) / 2;
                    string line = new string(' ', windowWidth);

                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.DarkCyan;

                    if (i == selectedOption)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkYellow;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }

                    Console.WriteLine(line.Substring(0, padding) + options[i] + line.Substring(padding + options[i].Length));
                    Console.ResetColor();
                }
            }

            void methods(int option)
            {
                switch (option)
                {
                    case 0:
                        loginUser(userTypeList);
                        break;

                    case 1:
                        loginAdmin();
                        break;

                    case 2:
                        logUser();
                        break;
                }
            }
        }

        public void loginUser(UserTypeList userTypeList)
        {
            void readData()
            {
                string baseDirectory = AppContext.BaseDirectory;
                string filePath = Path.Combine(baseDirectory, "AGEWELL_Files", "uLOGIN_AgeWell.txt");

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
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                readData();

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.SetCursorPosition(86, 16);
                string username = Console.ReadLine();
                Console.SetCursorPosition(86, 17);
                string password = readPassword();

                UserLogin userLogin = new UserLogin(username, password, " ", " ", " ", 0, " ", " ", userTypeList);

                string userType = userLogin.loginUser(username, password);
                if (userType != null)
                {
                    Console.WriteLine();
                    exceptionHandling.message($"Login successful as {userType}!", ConsoleColor.DarkGreen);
                    Thread.Sleep(3000);

                    if (userType == "Resident")
                    {
                        Resident resident = new Resident(username, password, " ", " ", " ", 0, " ", " ", userTypeList, 0, 0, 0, " ", " ");
                        resident.rMenu();
                        break;
                    }
                    else if (userType == "Guardian")
                    {
                        Guardian guardian = new Guardian(username, password, " ", " ", " ", 0, " ", " ", userTypeList, " ");
                        guardian.gMenu();
                        break;
                    }
                    else if (userType == "Caregiver")
                    {
                        Caregiver caregiver = new Caregiver(username, password, " ", " ", " ", 0, " ", " ", userTypeList, " ", " ");
                        caregiver.cMenu();
                        break;
                    }
                    break;
                }
                else
                {
                    Console.WriteLine();
                    exceptionHandling.message("Error: Invalid credentials. Please try again.", ConsoleColor.DarkRed);
                    Thread.Sleep(1000);
                }
            }
        }

        public void loginAdmin()
        {
            void readData()
            {
                string baseDirectory = AppContext.BaseDirectory;
                string filePath = Path.Combine(baseDirectory, "AGEWELL_Files", "aLOGIN_AgeWell.txt");

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
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                readData();

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.SetCursorPosition(86, 16);
                string username = Console.ReadLine();
                Console.SetCursorPosition(86, 17);
                string password = readPassword();

                AdminLogin adminLogin = new AdminLogin();

                if (adminLogin.loginAdmin(username, password))
                {
                    Console.WriteLine();
                    exceptionHandling.message("Login successful as Administrator!", ConsoleColor.DarkGreen);
                    Thread.Sleep(3000);

                    Admin admin = new Admin();
                    admin.aMenu();
                    break;
                }
                else
                {
                    Console.WriteLine();
                    exceptionHandling.message("Error: Invalid credentials. Please try again.", ConsoleColor.DarkRed);
                    Thread.Sleep(1000);
                }
            }
        }

        private string readPassword()
        {
            string password = "";
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    break;
                }
                else if (keyInfo.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password.Substring(0, password.Length - 1);
                    Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                    Console.Write(" ");
                    Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    password += keyInfo.KeyChar;
                    Console.Write("*");
                }
            }
            return password;
        }
        
        public void registerMenu()
        {
            void readData()
            {
                string baseDirectory = AppContext.BaseDirectory;
                string filePath = Path.Combine(baseDirectory, "AGEWELL_Files", "REGISTER_AgeWell.txt");

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

            string[] options = { "R E S I D E N T", "G U A R D I A N", "C A R E G I V E R", "B A C K" };
            int selectedOption = 0;
            ConsoleKey key;

            do
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                readData();

                printMenu(options, selectedOption);

                key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.UpArrow)
                {
                    selectedOption = (selectedOption == 0) ? options.Length - 1 : selectedOption - 1;
                }
                else if (key == ConsoleKey.DownArrow)
                {
                    selectedOption = (selectedOption == options.Length - 1) ? 0 : selectedOption + 1;
                }

            } while (key != ConsoleKey.Enter);

            methods(selectedOption);

            void printMenu(string[] options, int selectedOption)
            {
                int windowWidth = Console.WindowWidth;

                Console.WriteLine("\n\n");

                for (int i = 0; i < options.Length; i++)
                {
                    int padding = (windowWidth - options[i].Length) / 2;
                    string line = new string(' ', windowWidth);

                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.DarkCyan;

                    if (i == selectedOption)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkYellow;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }

                    Console.WriteLine(line.Substring(0, padding) + options[i] + line.Substring(padding + options[i].Length));
                    Console.ResetColor();
                }
            }

            void methods(int option)
            {
                switch (option)
                {
                    case 0:
                        registerResident(userTypeList);
                        break;

                    case 1:
                        registerGuardian(userTypeList);
                        break;

                    case 2:
                        registerCaregiver(userTypeList);
                        break;

                    case 3:
                        logUser();
                        break;
                }
            }
        }

        public void registerResident(UserTypeList userTypeList)
        {
            void readData()
            {
                string baseDirectory = AppContext.BaseDirectory;
                string filePath = Path.Combine(baseDirectory, "AGEWELL_Files", "rREGISTER_AgeWell.txt");

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

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            readData();

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.SetCursorPosition(80, 16);
            string rFirstName = Console.ReadLine();
            Console.SetCursorPosition(80, 17);
            string rMiddleName = Console.ReadLine();
            Console.SetCursorPosition(80, 18);
            string rLastName = Console.ReadLine();

            int rBMonth, rBDay, rBYear, rAge;
            string rSex;

            while (true)
            {
                try
                {
                    Console.SetCursorPosition(80, 20);
                    rBMonth = int.Parse(Console.ReadLine());

                    if (rBMonth < 1 || rBMonth > 12)
                    {
                        throw new ArgumentException("Month of birth must be between 01 and 12.");
                    }
                    break;
                }
                catch (FormatException)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.SetCursorPosition(52, 28);
                    Console.WriteLine("Error: Invalid input. Please try again.");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Thread.Sleep(1000);

                    Console.SetCursorPosition(52, 28);
                    Console.Write(new string(' ', Console.WindowWidth - 80));
                    Console.SetCursorPosition(80, 20);
                }
                catch (ArgumentException ex)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.SetCursorPosition(52, 28);
                    Console.WriteLine($"Error: {ex.Message}");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Thread.Sleep(1000);

                    Console.SetCursorPosition(52, 28);
                    Console.Write(new string(' ', Console.WindowWidth - 80));
                    Console.SetCursorPosition(80, 20);
                }
            }

            while (true)
            {
                try
                {
                    Console.SetCursorPosition(83, 20);
                    rBDay = int.Parse(Console.ReadLine());

                    if (rBDay < 1 || rBDay > 31)
                    {
                        throw new ArgumentException("Day of birth must be between 01 and 31.");
                    }
                    break;
                }
                catch (FormatException)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.SetCursorPosition(52, 28);
                    Console.WriteLine("Error: Invalid input. Please try again.");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Thread.Sleep(1000);

                    Console.SetCursorPosition(52, 28);
                    Console.Write(new string(' ', Console.WindowWidth - 80));
                    Console.SetCursorPosition(83, 20);
                }
                catch (ArgumentException ex)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.SetCursorPosition(52, 28);
                    Console.WriteLine($"Error: {ex.Message}");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Thread.Sleep(1000);

                    Console.SetCursorPosition(52, 28);
                    Console.Write(new string(' ', Console.WindowWidth - 80));
                    Console.SetCursorPosition(83, 20);
                }
            }

            while (true)
            {
                try
                {
                    Console.SetCursorPosition(86, 20);
                    rBYear = int.Parse(Console.ReadLine());
                    break;
                }
                catch (FormatException)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.SetCursorPosition(52, 28);
                    Console.WriteLine("Error: Invalid input. Please try again.");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Thread.Sleep(1000);

                    Console.SetCursorPosition(52, 28);
                    Console.Write(new string(' ', Console.WindowWidth - 80));
                    Console.SetCursorPosition(86, 20);
                }
            }

            while (true)
            {
                try
                {
                    Console.SetCursorPosition(80, 21);
                    rAge = int.Parse(Console.ReadLine());

                    if (rAge <= 0)
                    {
                        throw new ArgumentException("Age must be greater than zero.");
                    }

                    break;
                }
                catch (FormatException)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.SetCursorPosition(80, 21);
                    Console.WriteLine("Invalid input. Please try again.");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Thread.Sleep(1000);

                    Console.SetCursorPosition(80, 21);
                    Console.Write(new string(' ', Console.WindowWidth - 80));
                    Console.SetCursorPosition(80, 21);
                }
                catch (ArgumentException ex)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.SetCursorPosition(80, 21);
                    Console.WriteLine($"{ex.Message}");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Thread.Sleep(1000);

                    Console.SetCursorPosition(80, 21);
                    Console.Write(new string(' ', Console.WindowWidth - 80));
                    Console.SetCursorPosition(80, 21);
                }
            }

            while (true)
            {
                try
                {
                    Console.SetCursorPosition(80, 22);
                    rSex = Console.ReadLine();

                    if (rSex.Equals("Male", StringComparison.OrdinalIgnoreCase) || rSex.Equals("Female", StringComparison.OrdinalIgnoreCase))
                    {
                        rSex = char.ToUpper(rSex[0]) + rSex.Substring(1).ToLower();
                        break;
                    }
                    else
                    {
                        throw new ArgumentException("Sex must be 'Male' or 'Female'.");
                    }
                }
                catch (ArgumentException ex)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.SetCursorPosition(80, 22);
                    Console.WriteLine($"{ex.Message}");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Thread.Sleep(1000);

                    Console.SetCursorPosition(80, 22);
                    Console.Write(new string(' ', Console.WindowWidth - 80));
                    Console.SetCursorPosition(80, 22);
                }
            }

            Console.SetCursorPosition(80, 23);
            string rContactNumber = Console.ReadLine();
            Console.SetCursorPosition(80, 25);
            string rPassword = Console.ReadLine();

            string guardianName = "None";

            if (userTypeList.registeredGuardians.Count > 0)
            {
                var guardian = userTypeList.registeredGuardians.Last();
                guardianName = $"{guardian.firstName} {guardian.middleName} {guardian.lastName}";
            }

            ResidentAccount residentacc = new ResidentAccount(" ", rPassword, rFirstName, rMiddleName, rLastName, rAge, rSex, rContactNumber, userTypeList, rBMonth, rBDay, rBYear, " ", guardianName);

            residentacc.username = residentacc.generateUsername();
            residentacc.birthDate();
            residentacc.writeData();
            userTypeList.addResident(residentacc);

            exceptionHandling.message($"Registration successful! Your username is {residentacc.username}.", ConsoleColor.DarkGreen);
            Thread.Sleep(3000);

            loginMenu();
        }

        public void registerGuardian(UserTypeList userTypeList)
        {
            void readData()
            {
                string baseDirectory = AppContext.BaseDirectory;
                string filePath = Path.Combine(baseDirectory, "AGEWELL_Files", "gREGISTER_AgeWell.txt");

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

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            readData();

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.SetCursorPosition(80, 16);
            string gFirstName = Console.ReadLine();
            Console.SetCursorPosition(80, 17);
            string gMiddleName = Console.ReadLine();
            Console.SetCursorPosition(80, 18);
            string gLastName = Console.ReadLine();

            int gAge;
            string gSex, gEmailAddress;

            while (true)
            {
                try
                {
                    Console.SetCursorPosition(80, 20);
                    gAge = int.Parse(Console.ReadLine());

                    if (gAge <= 0)
                    {
                        throw new ArgumentException("Age must be greater than zero.");
                    }

                    break;
                }
                catch (FormatException)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.SetCursorPosition(80, 20);
                    Console.WriteLine("Invalid input. Please try again.");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Thread.Sleep(1000);

                    Console.SetCursorPosition(80, 20);
                    Console.Write(new string(' ', Console.WindowWidth - 80));
                    Console.SetCursorPosition(80, 20);
                }
                catch (ArgumentException ex)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.SetCursorPosition(80, 20);
                    Console.WriteLine($"{ex.Message}");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Thread.Sleep(1000);

                    Console.SetCursorPosition(80, 20);
                    Console.Write(new string(' ', Console.WindowWidth - 80));
                    Console.SetCursorPosition(80, 20);
                }
            }

            while (true)
            {
                try
                {
                    Console.SetCursorPosition(80, 21);
                    gSex = Console.ReadLine();

                    if (gSex.Equals("Male", StringComparison.OrdinalIgnoreCase) || gSex.Equals("Female", StringComparison.OrdinalIgnoreCase))
                    {
                        gSex = char.ToUpper(gSex[0]) + gSex.Substring(1).ToLower();
                        break;
                    }
                    else
                    {
                        throw new ArgumentException("Sex must be 'Male' or 'Female'.");
                    }
                }
                catch (ArgumentException ex)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.SetCursorPosition(80, 21);
                    Console.WriteLine($"{ex.Message}");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Thread.Sleep(1000);

                    Console.SetCursorPosition(80, 21);
                    Console.Write(new string(' ', Console.WindowWidth - 80));
                    Console.SetCursorPosition(80, 21);
                }
            }

            while (true)
            {
                try
                {
                    Console.SetCursorPosition(80, 22);
                    gEmailAddress = Console.ReadLine();

                    if (gEmailAddress.Contains("@"))
                    {
                        if (gEmailAddress.EndsWith(".com") || gEmailAddress.EndsWith(".ph"))
                        {
                            break;
                        }
                        else
                        {
                            throw new Exception("Email address must end with '.com' or '.ph'.");
                        }
                    }
                    else
                    {
                        throw new Exception("Email address must contain '@'.");
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.SetCursorPosition(80, 22);
                    Console.WriteLine($"{ex.Message}");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Thread.Sleep(1000);

                    Console.SetCursorPosition(80, 22);
                    Console.Write(new string(' ', Console.WindowWidth - 80));
                    Console.SetCursorPosition(80, 22);
                }
            }

            Console.SetCursorPosition(80, 23);
            string gContactNumber = Console.ReadLine();
            Console.SetCursorPosition(80, 25);
            string gPassword = Console.ReadLine();

            GuardianAccount guardianacc = new GuardianAccount(" ", gPassword, gFirstName, gMiddleName, gLastName, gAge, gSex, gContactNumber, userTypeList, gEmailAddress);

            guardianacc.username = guardianacc.generateUsername();
            guardianacc.writeData();
            userTypeList.addGuardian(guardianacc);

            exceptionHandling.message($"Registration successful! Your username is {guardianacc.username}.", ConsoleColor.DarkGreen);
            Thread.Sleep(3000);

            registerResident(userTypeList);
        }

        public void registerCaregiver(UserTypeList userTypeList)
        {
            void readData()
            {
                string baseDirectory = AppContext.BaseDirectory;
                string filePath = Path.Combine(baseDirectory, "AGEWELL_Files", "cREGISTER_AgeWell.txt");

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

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            readData();

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.SetCursorPosition(80, 16);
            string cFirstName = Console.ReadLine();
            Console.SetCursorPosition(80, 17);
            string cMiddleName = Console.ReadLine();
            Console.SetCursorPosition(80, 18);
            string cLastName = Console.ReadLine();
            Console.SetCursorPosition(80, 20);
            string cSpecialization = Console.ReadLine();

            int cAge;
            string cSex, cEmailAddress;

            while (true)
            {
                try
                {
                    Console.SetCursorPosition(80, 22);
                    cAge = int.Parse(Console.ReadLine());

                    if (cAge <= 0)
                    {
                        throw new ArgumentException("Age must be greater than zero.");
                    }

                    break;
                }
                catch (FormatException)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.SetCursorPosition(80, 22);
                    Console.WriteLine("Invalid input. Please try again.");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Thread.Sleep(1000);

                    Console.SetCursorPosition(80, 22);
                    Console.Write(new string(' ', Console.WindowWidth - 80));
                    Console.SetCursorPosition(80, 22);
                }
                catch (ArgumentException ex)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.SetCursorPosition(80, 22);
                    Console.WriteLine($"{ex.Message}");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Thread.Sleep(1000);

                    Console.SetCursorPosition(80, 22);
                    Console.Write(new string(' ', Console.WindowWidth - 80));
                    Console.SetCursorPosition(80, 22);
                }
            }

            while (true)
            {
                try
                {
                    Console.SetCursorPosition(80, 23);
                    cSex = Console.ReadLine();

                    if (cSex.Equals("Male", StringComparison.OrdinalIgnoreCase) || cSex.Equals("Female", StringComparison.OrdinalIgnoreCase))
                    {
                        cSex = char.ToUpper(cSex[0]) + cSex.Substring(1).ToLower();
                        break;
                    }
                    else
                    {
                        throw new ArgumentException("Sex must be 'Male' or 'Female'.");
                    }
                }
                catch (ArgumentException ex)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.SetCursorPosition(80, 23);
                    Console.WriteLine($"{ex.Message}");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Thread.Sleep(1000);

                    Console.SetCursorPosition(80, 23);
                    Console.Write(new string(' ', Console.WindowWidth - 80));
                    Console.SetCursorPosition(80, 23);
                }
            }

            while (true)
            {
                try
                {
                    Console.SetCursorPosition(80, 24);
                    cEmailAddress = Console.ReadLine();

                    if (cEmailAddress.Contains("@"))
                    {
                        if (cEmailAddress.EndsWith(".com") || cEmailAddress.EndsWith(".ph"))
                        {
                            break;
                        }
                        else
                        {
                            throw new Exception("Email address must end with '.com' or '.ph'.");
                        }
                    }
                    else
                    {
                        throw new Exception("Email address must contain '@'.");
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.SetCursorPosition(80, 24);
                    Console.WriteLine($"{ex.Message}");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Thread.Sleep(1000);

                    Console.SetCursorPosition(80, 24);
                    Console.Write(new string(' ', Console.WindowWidth - 80));
                    Console.SetCursorPosition(80, 24);
                }
            }

            Console.SetCursorPosition(80, 25);
            string cContactNumber = Console.ReadLine();
            Console.SetCursorPosition(80, 27);
            string cPassword = Console.ReadLine();

            CaregiverAccount caregiveracc = new CaregiverAccount(" ", cPassword, cFirstName, cMiddleName, cLastName, cAge, cSex, cContactNumber, userTypeList, cSpecialization, cEmailAddress);

            caregiveracc.username = caregiveracc.generateUsername();
            caregiveracc.writeData();
            userTypeList.addCaregiver(caregiveracc);

            exceptionHandling.message($"Registration successful! Your username is {caregiveracc.username}.", ConsoleColor.DarkGreen);
            Thread.Sleep(3000);

            loginMenu();
        }
    }

    internal class ExceptionHandling
    {
        public void message(string message, ConsoleColor color)
        {
            int windowWidth = Console.WindowWidth;
            int padding = (windowWidth - message.Length) / 2;

            Console.ForegroundColor = color;
            Console.WriteLine($"\n\n{new string(' ', padding)}{message}");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
        }
    }

    internal class UserMenu
    {
        ExceptionHandling exceptionHandling = new ExceptionHandling();

        public void residentMenu()
        {
            string baseDirectory = AppContext.BaseDirectory;
            string filePath = Path.Combine(baseDirectory, "AGEWELL_Files", "rMENU_AgeWell.txt");

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

        public void guardianMenu()
        {
            string baseDirectory = AppContext.BaseDirectory;
            string filePath = Path.Combine(baseDirectory, "AGEWELL_Files", "gMENU_AgeWell.txt");

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

        public void caregiverMenu()
        {
            string baseDirectory = AppContext.BaseDirectory;
            string filePath = Path.Combine(baseDirectory, "AGEWELL_Files", "cMENU_AgeWell.txt");

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

        public void adminMenu()
        {
            string baseDirectory = AppContext.BaseDirectory;
            string filePath = Path.Combine(baseDirectory, "AGEWELL_Files", "aMENU_AgeWell.txt");

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
    }
}