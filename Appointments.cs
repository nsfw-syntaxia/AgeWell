using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace DOLERA_AgeWell
{
    internal class Appointments : Caregiver
    {
        public Appointments(string username, string password, string firstName, string middleName, string lastName, int age, string sex, string contactNumber, UserTypeList userTypeList, string specialization, string emailAddress) :
                base(username, password, firstName, middleName, lastName, age, sex, contactNumber, userTypeList, specialization, emailAddress)
        {

        }

        private static string baseDirectory = AppContext.BaseDirectory;
        private static string appointmentsDirectory = Path.Combine(baseDirectory, "AGEWELL_Database", "Appointments_List");
        private static string archiveDirectory = Path.Combine(baseDirectory, "AGEWELL_Database", "Appointments_List", "Archive");

        ExceptionHandling exceptionHandling = new ExceptionHandling();

        public void cAppointments()
        {
            void readData()
            {
                string filePath = Path.Combine(baseDirectory, "AGEWELL_Files", "cAPPOINTMENTS_AgeWell.txt");

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
                            return;

                        case 1:
                            viewAllAppointments();
                            break;

                        case 2:
                            Console.Clear();
                            assignedResidents();
                            addAppointment();
                            break;

                        case 3:
                            Console.Clear();
                            assignedResidents();
                            updateAppointmentStatus();
                            break;

                        case 4:
                            Console.Clear();
                            assignedResidents();
                            rescheduleAppointment();
                            break;

                        case 5:
                            Console.Clear();
                            assignedResidents();
                            removeAppointment();
                            break;

                        case 6:
                            archiveAppointment();
                            return;

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

        private bool verifyResident(string username)
        {
            string filePath = Path.Combine(baseDirectory, "AGEWELL_Database", "Users_List", "Residents_withPersonalCaregiver_List.txt");

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

        public void addAppointment()
        {
            string residentUsername;

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.SetCursorPosition(62, 27);
                Console.WriteLine("Resident's Username: ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.SetCursorPosition(83, 27);
                residentUsername = Console.ReadLine();

                if (verifyResident(residentUsername))
                {
                    viewAppointment(residentUsername);
                    continueKey("Press any key to continue.");

                    Console.Clear();

                    DateTime date;
                    while (true)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.SetCursorPosition(52, 15);
                        Console.WriteLine("Appointment Date (MM/DD/YYYY): ");
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.SetCursorPosition(83, 15);
                        string appointmentDate = Console.ReadLine();

                        if (DateTime.TryParseExact(appointmentDate, "MM/dd/yyyy", null, System.Globalization.DateTimeStyles.None, out date))
                        {
                            break;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.SetCursorPosition(63, 18);
                            Console.WriteLine("Error: Invalid input. Please try again.");
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Thread.Sleep(1000);

                            Console.SetCursorPosition(63, 18);
                            Console.Write(new string(' ', Console.WindowWidth - 80));
                            Console.SetCursorPosition(63, 18);
                        }
                    }

                    DateTime time;
                    while (true)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.SetCursorPosition(57, 17);
                        Console.WriteLine("Appointment Time (HH:MM): ");
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.SetCursorPosition(83, 17);
                        string appointmentTime = Console.ReadLine();

                        if (DateTime.TryParseExact(appointmentTime, "HH:mm", null, System.Globalization.DateTimeStyles.None, out time))
                        {
                            break;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.SetCursorPosition(63, 20);
                            Console.WriteLine("Error: Invalid input. Please try again.");
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Thread.Sleep(1000);

                            Console.SetCursorPosition(63, 20);
                            Console.Write(new string(' ', Console.WindowWidth - 80));
                            Console.SetCursorPosition(63, 20);
                        }
                    }

                    int appointmentType;
                    while (true)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.SetCursorPosition(65, 19);
                        Console.WriteLine("Appointment type: ");

                        string options = "[1] Doctor Consultation\t[2] Health Assessment";
                        int width = Console.WindowWidth;
                        int padding = (width - options.Length) / 2;

                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($"\n{new string(' ', padding)}{options}");

                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.SetCursorPosition(83, 19);
                        appointmentType = int.Parse(Console.ReadLine());
                        if (appointmentType == 1 || appointmentType == 2)
                        {
                            break;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.SetCursorPosition(63, 24);
                            Console.WriteLine("Error: Invalid input. Please try again.");
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Thread.Sleep(1000);

                            Console.SetCursorPosition(63, 24);
                            Console.Write(new string(' ', Console.WindowWidth - 80));
                            Console.SetCursorPosition(63, 24);
                        }
                    }

                    string type = appointmentType == 1 ? "Doctor Consultation" : "Health Assessment";
                    string status = "Scheduled";
                    string currentMonth = DateTime.Now.ToString("MMMM yyyy");
                    string appointmentFilePath = Path.Combine(appointmentsDirectory, $"{residentUsername}_{currentMonth}_Appointments.txt");

                    bool isFileEmpty = !File.Exists(appointmentFilePath) || new FileInfo(appointmentFilePath).Length == 0;

                    string caregiverUsername = username;

                    using (StreamWriter writer = new StreamWriter(appointmentFilePath, true))
                    {
                        if (isFileEmpty)
                        {
                            writer.WriteLine($"\tCaregiver: {caregiverUsername}");
                            writer.WriteLine("\n\t\t\t\t\t\tDate\t\tTime\t\tAppointment\t\tStatus");
                        }
                        writer.WriteLine($"\t\t\t\t\t\t{date:MM/dd/yyyy}\t{time:HH:mm}\t\t{type}\t{status}");
                    }

                    Console.WriteLine("\n");
                    exceptionHandling.message($"Appointment added successfully for {residentUsername}.", ConsoleColor.DarkGreen);
                    Thread.Sleep(3000);

                    viewAppointment(residentUsername);
                    continueKey("Press any key to return to menu.");

                    break;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.SetCursorPosition(69, 30);
                    Console.WriteLine("Error: Resident not found.");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Thread.Sleep(1000);

                    Console.SetCursorPosition(69, 30);
                    Console.Write(new string(' ', Console.WindowWidth - 80));
                    Console.SetCursorPosition(69, 30);
                }
            }
        }

        public void viewAllAppointments()
        {
            string currentMonth = DateTime.Now.ToString("MMMM yyyy");

            if (!Directory.Exists(appointmentsDirectory))
            {
                exceptionHandling.message("Error: Directory not found.", ConsoleColor.DarkRed);
                Thread.Sleep(1000);
                return;
            }

            string caregiverUsername = username;

            string[] appointmentFiles = Directory.GetFiles(appointmentsDirectory, "*_Appointments.txt", SearchOption.TopDirectoryOnly);

            List<(DateTime dateTime, string details, string residentUsername)> allAppointments = new List<(DateTime, string, string)>();

            foreach (string file in appointmentFiles)
            {
                string[] lines = File.ReadAllLines(file);

                if (lines.Length <= 3) continue;

                string fileName = Path.GetFileNameWithoutExtension(file);
                string[] fileParts = fileName.Split('_');
                string residentUsername = fileParts[0];

                if (lines[0].Contains($"Caregiver: {caregiverUsername}"))
                {
                    for (int i = 3; i < lines.Length; i++)
                    {
                        string line = lines[i];
                        string[] parts = line.Split(new string[] { "\t" }, StringSplitOptions.None);

                        if (parts.Length >= 2)
                        {
                            string datePart = parts[6].Trim();
                            string timePart = parts[7].Trim();

                            if (DateTime.TryParseExact(datePart, "MM/dd/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime date) &&
                                DateTime.TryParseExact(timePart, "HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime time))
                            {
                                DateTime dateTime = date.Date + time.TimeOfDay;
                                allAppointments.Add((dateTime, line, residentUsername));
                            }
                        }
                    }
                }
            }

            allAppointments.Sort((a, b) => a.dateTime.CompareTo(b.dateTime));

            if (allAppointments.Count == 0)
            {
                exceptionHandling.message($"No appointments in {currentMonth}.", ConsoleColor.DarkRed);
                continueKey("Press any key to return to menu.");
                return;
            }

            var table = new Table
            {
                Padding = 3,
                HeaderTextAlignRight = false,
                RowTextAlignRight = false
            };

            table.Header("Resident", "Date", "Time", "Appointment", "Status");

            foreach (var appointment in allAppointments)
            {
                string[] parts = appointment.details.Split(new string[] { "\t" }, StringSplitOptions.None);
                if (parts.Length >= 4)
                {
                    string date = parts[6];
                    string time = parts[7];
                    string details = parts[9];
                    string status = parts.Length > 10 ? parts[10] : "Scheduled";
                    table.AddRow(appointment.residentUsername, date, time, details, status);
                }
            }

            string tableOutput = table.ToString();

            int consoleWidth = Console.WindowWidth;
            int tableWidth = tableOutput.Split('\n')[0].Length;
            int tablePadding = (consoleWidth - tableWidth) / 2;

            string centeredTable = string.Join(
                Environment.NewLine,
                tableOutput.Split('\n').Select(line => new string(' ', Math.Max(0, tablePadding)) + line)
            );

            Console.Clear();
            string message = $"[ Appointments in {currentMonth} ]";
            int windowWidth = Console.WindowWidth;
            int padding = (windowWidth - message.Length) / 2;

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\n\n\n\n\n\n\n\n\n\n{new string(' ', padding)}{message}\n\n");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(centeredTable);

            continueKey("Press any key to return to menu.");
        }

        public void viewAppointment(string residentUsername)
        {
            string currentMonth = DateTime.Now.ToString("MMMM yyyy");
            string residentAppointmentFile = Path.Combine(appointmentsDirectory, $"{residentUsername}_{currentMonth}_Appointments.txt");

            if (!File.Exists(residentAppointmentFile))
            {
                exceptionHandling.message($"No appointments for {residentUsername} in {currentMonth}.", ConsoleColor.DarkRed);
                Thread.Sleep(1000);
                return;
            }

            string[] lines = File.ReadAllLines(residentAppointmentFile);
            List<(DateTime dateTime, string details)> residentAppointments = new List<(DateTime, string)>();

            for (int i = 3; i < lines.Length; i++)
            {
                string line = lines[i];
                string[] parts = line.Split(new string[] { "\t" }, StringSplitOptions.None);

                if (parts.Length >= 2)
                {
                    string datePart = parts[6].Trim();
                    string timePart = parts[7].Trim();

                    if (DateTime.TryParseExact(datePart, "MM/dd/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime date) &&
                        DateTime.TryParseExact(timePart, "HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime time))
                    {
                        DateTime dateTime = date.Date + time.TimeOfDay;
                        residentAppointments.Add((dateTime, line));
                    }
                }
            }

            residentAppointments.Sort((a, b) => a.dateTime.CompareTo(b.dateTime));

            var table = new Table
            {
                Padding = 3,
                HeaderTextAlignRight = false,
                RowTextAlignRight = false
            };

            table.Header("Date", "Time", "Appointment", "Status");

            foreach (var appointment in residentAppointments)
            {
                string[] parts = appointment.details.Split(new string[] { "\t" }, StringSplitOptions.None);
                if (parts.Length >= 4)
                {
                    string date = parts[6];
                    string time = parts[7];
                    string details = parts[9];
                    string status = parts.Length > 10 ? parts[10] : "Scheduled";
                    table.AddRow(date, time, details, status);
                }
            }

            string tableOutput = table.ToString();

            int consoleWidth = Console.WindowWidth;
            int tableWidth = tableOutput.Split('\n')[0].Length;
            int tablePadding = (consoleWidth - tableWidth) / 2;

            string centeredTable = string.Join(
                Environment.NewLine,
                tableOutput.Split('\n').Select(line => new string(' ', Math.Max(0, tablePadding)) + line)
            );

            Console.Clear();

            string message = $"[ {residentUsername}'s Appointments in {currentMonth} ]";
            int windowWidth = Console.WindowWidth;
            int padding = (windowWidth - message.Length) / 2;

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\n\n\n\n\n\n\n\n\n\n{new string(' ', padding)}{message}\n\n");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(centeredTable);
        }

        public void updateAppointmentStatus()
        {
            string residentUsername;

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.SetCursorPosition(62, 27);
                Console.WriteLine("Resident's Username: ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.SetCursorPosition(83, 27);
                residentUsername = Console.ReadLine();

                if (verifyResident(residentUsername))
                {
                    string currentMonth = DateTime.Now.ToString("MMMM yyyy");
                    string appointmentFilePath = Path.Combine(appointmentsDirectory, $"{residentUsername}_{currentMonth}_Appointments.txt");

                    if (!File.Exists(appointmentFilePath) || new FileInfo(appointmentFilePath).Length == 0)
                    {
                        exceptionHandling.message($"No appointments for {residentUsername} in {currentMonth}.", ConsoleColor.DarkRed);
                        Thread.Sleep(1000);
                        break;
                    }

                    Console.Clear();

                    string[] appointments = File.ReadAllLines(appointmentFilePath);
                    string message = $"[ {residentUsername}'s Appointments in {currentMonth} ]";
                    int windowWidth = Console.WindowWidth;
                    int padding = (windowWidth - message.Length) / 2;

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"\n\n\n\n\n\n\n\n\n\n{new string(' ', padding)}{message}\n\n");
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine("\t\t\t\t\t\t\tDate\t\tTime\t\tAppointment\t\tStatus");

                    int aNumber = 1;
                    for (int i = 3; i < appointments.Length; i++)
                    {
                        string appointmentDetails = appointments[i].Replace("\t\t\t\t\t\t", "\t");

                        Console.WriteLine($"\t\t\t\t\t\t[{aNumber}] {appointmentDetails}");
                        aNumber++;
                    }

                    int allAppointments = appointments.Length - 3;
                    int appointmentNumber;
                    while (true)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.SetCursorPosition(63, 29);
                        Console.WriteLine("Appointment Number: ");
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.SetCursorPosition(83, 29);

                        if (int.TryParse(Console.ReadLine(), out appointmentNumber) && appointmentNumber >= 1 && appointmentNumber <= allAppointments)
                        {
                            break;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.SetCursorPosition(63, 32);
                            Console.WriteLine("Error: Invalid input. Please try again.");
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Thread.Sleep(1000);

                            Console.SetCursorPosition(63, 32);
                            Console.Write(new string(' ', Console.WindowWidth - 80));
                            Console.SetCursorPosition(63, 32);
                        }
                    }

                    string newStatus;
                    int status;
                    while (true)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.SetCursorPosition(71, 31);
                        Console.WriteLine("New Status: ");

                        string statusOptions = "[1] Done\t[2] Canceled";
                        int width = Console.WindowWidth;
                        int tPadding = (width - statusOptions.Length) / 2;

                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($"\n{new string(' ', tPadding)}{statusOptions}");

                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.SetCursorPosition(83, 31);
                        newStatus = Console.ReadLine();

                        if (int.TryParse(newStatus, out status) && (status == 1 || status == 2))
                        {
                            break;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.SetCursorPosition(63, 36);
                            Console.WriteLine("Error: Invalid input. Please try again.");
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Thread.Sleep(1000);

                            Console.SetCursorPosition(63, 36);
                            Console.Write(new string(' ', Console.WindowWidth - 80));
                            Console.SetCursorPosition(63, 36);
                        }
                    }

                    newStatus = status == 1 ? "Done" : "Canceled";

                    string[] appointmentData = appointments[appointmentNumber + 2].Split('\t');
                    appointmentData[^1] = newStatus;
                    appointments[appointmentNumber + 2] = string.Join("\t", appointmentData);

                    File.WriteAllLines(appointmentFilePath, appointments);

                    Console.Write("\n\n");
                    exceptionHandling.message("Appointment status updated successfully!", ConsoleColor.DarkGreen);
                    Thread.Sleep(3000);

                    if (newStatus == "Done")
                    {
                        HealthRecords healthRecords = new HealthRecords();
                        healthRecords.medicalProfile(residentUsername);

                        string[] appointmentDetails = appointments[appointmentNumber + 2].Split('\t');

                        string aDate = appointmentDetails[6];
                        string aTime = appointmentDetails[7];
                        string aType = appointmentDetails[9];

                        if (aType == "Doctor Consultation")
                        {
                            Console.Clear();
                            healthRecords.recordDoctorConsultation(residentUsername, aDate, aTime);
                        }
                        else if (aType == "Health Assessment")
                        {
                            Console.Clear();
                            healthRecords.recordHealthAssessment(residentUsername, aDate, aTime);
                        }
                        break;
                    }

                    Console.Clear();
                    viewAppointment(residentUsername);
                    continueKey("Press any key to return to menu.");

                    break;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.SetCursorPosition(69, 30);
                    Console.WriteLine("Error: Resident not found.");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Thread.Sleep(1000);

                    Console.SetCursorPosition(69, 30);
                    Console.Write(new string(' ', Console.WindowWidth - 80));
                    Console.SetCursorPosition(69, 30);
                }
            }
        }

        public void rescheduleAppointment()
        {
            string residentUsername;

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.SetCursorPosition(62, 27);
                Console.WriteLine("Resident's Username: ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.SetCursorPosition(83, 27);
                residentUsername = Console.ReadLine();

                if (verifyResident(residentUsername))
                {
                    string currentMonth = DateTime.Now.ToString("MMMM yyyy");
                    string appointmentFilePath = Path.Combine(appointmentsDirectory, $"{residentUsername}_{currentMonth}_Appointments.txt");

                    if (!File.Exists(appointmentFilePath) || new FileInfo(appointmentFilePath).Length == 0)
                    {
                        exceptionHandling.message($"No appointments for {residentUsername} in {currentMonth}.", ConsoleColor.DarkRed);
                        Thread.Sleep(1000);
                        break;
                    }

                    Console.Clear();

                    string[] appointments = File.ReadAllLines(appointmentFilePath);
                    string message = $"[ {residentUsername}'s Appointments in {currentMonth} ]";
                    int windowWidth = Console.WindowWidth;
                    int padding = (windowWidth - message.Length) / 2;

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"\n\n\n\n\n\n\n\n\n\n{new string(' ', padding)}{message}\n\n");
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine("\t\t\t\t\t\t\tDate\t\tTime\t\tAppointment\t\tStatus");

                    int aNumber = 1;
                    for (int i = 3; i < appointments.Length; i++)
                    {
                        string appointmentDetails = appointments[i].Replace("\t\t\t\t\t\t", "\t");

                        Console.WriteLine($"\t\t\t\t\t\t[{aNumber}] {appointmentDetails}");
                        aNumber++;
                    }

                    int allAppointments = appointments.Length - 3;
                    int appointmentNumber;
                    while (true)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.SetCursorPosition(63, 29);
                        Console.WriteLine("Appointment Number: ");
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.SetCursorPosition(83, 29);

                        if (int.TryParse(Console.ReadLine(), out appointmentNumber) && appointmentNumber >= 1 && appointmentNumber <= allAppointments)
                        {
                            break;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.SetCursorPosition(63, 32);
                            Console.WriteLine("Error: Invalid input. Please try again.");
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Thread.Sleep(1000);

                            Console.SetCursorPosition(63, 32);
                            Console.Write(new string(' ', Console.WindowWidth - 80));
                            Console.SetCursorPosition(63, 32);
                        }
                    }

                    DateTime newDate;
                    string appointmentDate;
                    while (true)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.SetCursorPosition(48, 31);
                        Console.WriteLine("New Appointment Date (MM/DD/YYYY): ");
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.SetCursorPosition(83, 31);
                        appointmentDate = Console.ReadLine();

                        if (DateTime.TryParseExact(appointmentDate, "MM/dd/yyyy", null, System.Globalization.DateTimeStyles.None, out newDate))
                        {
                            break;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.SetCursorPosition(63, 34);
                            Console.WriteLine("Error: Invalid input. Please try again.");
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Thread.Sleep(1000);

                            Console.SetCursorPosition(63, 34);
                            Console.Write(new string(' ', Console.WindowWidth - 80));
                            Console.SetCursorPosition(63, 34);
                        }
                    }

                    DateTime newTime;
                    string appointmentTime;
                    while (true)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.SetCursorPosition(53, 33);
                        Console.WriteLine("New Appointment Time (HH:MM): ");
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.SetCursorPosition(83, 33);
                        appointmentTime = Console.ReadLine();

                        if (DateTime.TryParseExact(appointmentTime, "HH:mm", null, System.Globalization.DateTimeStyles.None, out newTime))
                        {
                            break;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.SetCursorPosition(63, 36);
                            Console.WriteLine("Error: Invalid input. Please try again.");
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Thread.Sleep(1000);

                            Console.SetCursorPosition(63, 36);
                            Console.Write(new string(' ', Console.WindowWidth - 80));
                            Console.SetCursorPosition(63, 36);
                        }
                    }

                    string[] appointmentParts = appointments[3 + (appointmentNumber - 1)].Split(new string[] { "\t" }, StringSplitOptions.None);
                    string updatedAppointment = $"\t\t\t\t\t\t{newDate:MM/dd/yyyy}\t{newTime:HH:mm}\t\t{appointmentParts[9]}\t{appointmentParts[10]}";
                    appointments[3 + (appointmentNumber - 1)] = updatedAppointment;

                    File.WriteAllLines(appointmentFilePath, appointments);

                    exceptionHandling.message("Appointment rescheduled successfully!", ConsoleColor.DarkGreen);
                    Thread.Sleep(3000);

                    Console.Clear();
                    viewAppointment(residentUsername);
                    continueKey("Press any key to return to menu.");

                    break;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.SetCursorPosition(69, 30);
                    Console.WriteLine("Error: Resident not found.");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Thread.Sleep(1000);

                    Console.SetCursorPosition(69, 30);
                    Console.Write(new string(' ', Console.WindowWidth - 80));
                    Console.SetCursorPosition(69, 30);
                }
            }
        }

        public void removeAppointment()
        {
            string residentUsername;

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.SetCursorPosition(62, 27);
                Console.WriteLine("Resident's Username: ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.SetCursorPosition(83, 27);
                residentUsername = Console.ReadLine();

                if (verifyResident(residentUsername))
                {
                    string currentMonth = DateTime.Now.ToString("MMMM yyyy");
                    string appointmentFilePath = Path.Combine(appointmentsDirectory, $"{residentUsername}_{currentMonth}_Appointments.txt");

                    if (!File.Exists(appointmentFilePath) || new FileInfo(appointmentFilePath).Length == 0)
                    {
                        exceptionHandling.message($"No appointments for {residentUsername} in {currentMonth}.", ConsoleColor.DarkRed);
                        Thread.Sleep(1000);
                        break;
                    }

                    Console.Clear();

                    string[] appointments = File.ReadAllLines(appointmentFilePath);
                    string message = $"[ {residentUsername}'s Appointments in {currentMonth} ]";
                    int windowWidth = Console.WindowWidth;
                    int padding = (windowWidth - message.Length) / 2;

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"\n\n\n\n\n\n\n\n\n\n{new string(' ', padding)}{message}\n\n");
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine("\t\t\t\t\t\t\tDate\t\tTime\t\tAppointment\t\tStatus");

                    int aNumber = 1;
                    for (int i = 3; i < appointments.Length; i++)
                    {
                        string appointmentDetails = appointments[i].Replace("\t\t\t\t\t\t", "\t");

                        Console.WriteLine($"\t\t\t\t\t\t[{aNumber}] {appointmentDetails}");
                        aNumber++;
                    }

                    int allAppointments = appointments.Length - 3;
                    int appointmentNumber;
                    while (true)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.SetCursorPosition(63, 29);
                        Console.WriteLine("Appointment Number: ");
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.SetCursorPosition(83, 29);

                        if (int.TryParse(Console.ReadLine(), out appointmentNumber) && appointmentNumber >= 1 && appointmentNumber <= allAppointments)
                        {
                            break;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.SetCursorPosition(63, 32);
                            Console.WriteLine("Error: Invalid input. Please try again.");
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Thread.Sleep(1000);

                            Console.SetCursorPosition(63, 32);
                            Console.Write(new string(' ', Console.WindowWidth - 80));
                            Console.SetCursorPosition(63, 32);
                        }
                    }

                    int appointmentIndex = 3 + (appointmentNumber - 1);
                    List<string> updatedAppointments = appointments.ToList();
                    updatedAppointments.RemoveAt(appointmentIndex);

                    File.WriteAllLines(appointmentFilePath, updatedAppointments);

                    exceptionHandling.message("Appointment removed successfully!", ConsoleColor.DarkGreen);
                    Thread.Sleep(3000);

                    Console.Clear();
                    viewAppointment(residentUsername);
                    continueKey("Press any key to return to menu.");

                    break;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.SetCursorPosition(69, 30);
                    Console.WriteLine("Error: Resident not found.");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Thread.Sleep(1000);

                    Console.SetCursorPosition(69, 30);
                    Console.Write(new string(' ', Console.WindowWidth - 80));
                    Console.SetCursorPosition(69, 30);
                }
            }
        }

        public void archiveAppointment()
        {
            string currentMonth = DateTime.Now.ToString("MMMM yyyy");
            string archiveFile = Path.Combine(archiveDirectory, $"Archived_Appointments_{currentMonth}.txt");

            if (!Directory.Exists(appointmentsDirectory))
            {
                exceptionHandling.message("Error: Directory not found.", ConsoleColor.DarkRed);
                Thread.Sleep(1000);
                return;
            }

            string[] appointmentFiles = Directory.GetFiles(appointmentsDirectory, $"*_{currentMonth}_Appointments.txt");

            if (appointmentFiles.Length == 0)
            {
                exceptionHandling.message("No appointment files found to archive.", ConsoleColor.DarkRed);
                Thread.Sleep(1000);
                return;
            }

            using (StreamWriter writer = new StreamWriter(archiveFile, true))
            {
                foreach (string appointmentFile in appointmentFiles)
                {
                    string fileName = Path.GetFileNameWithoutExtension(appointmentFile);
                    string residentUsername = fileName.Substring(0, fileName.IndexOf("_"));

                    using (StreamReader reader = new StreamReader(appointmentFile))
                    {
                        string[] lines = reader.ReadToEnd().Split(new[] { Environment.NewLine }, StringSplitOptions.None);

                        writer.WriteLine($"\n\t[ {residentUsername}'s Appointments for {currentMonth} ]");
                        writer.WriteLine("\n\tDate\t\tTime\t\tAppointment\t\tStatus");

                        for (int i = 2; i < lines.Length; i++)
                        {
                            if (string.IsNullOrWhiteSpace(lines[i])) continue;

                            string[] parts = lines[i].Split(new string[] { "\t" }, StringSplitOptions.None);
                            if (parts.Length >= 4)
                            {
                                string date = parts[6];
                                string time = parts[7];
                                string details = parts[9];
                                string status = parts.Length > 10 ? parts[10] : "Scheduled";

                                string formattedLine = $"\t{date}\t{time}\t\t{details}\t{status}";

                                writer.WriteLine(formattedLine);
                            }
                        }

                        writer.WriteLine($"\n\t{new string('*', 10)}");
                    }

                    File.Delete(appointmentFile);
                }
            }

            exceptionHandling.message($"All appointments in {currentMonth} archived successfully!", ConsoleColor.DarkGreen);
            Thread.Sleep(3000);
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