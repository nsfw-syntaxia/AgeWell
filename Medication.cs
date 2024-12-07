using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace DOLERA_AgeWell
{
    internal class Medication
    {
        private static string baseDirectory = AppContext.BaseDirectory;
        private static string medicationDirectory = Path.Combine(baseDirectory, "AGEWELL_Database", "Medication_List");
        private static string archiveDirectory = Path.Combine(baseDirectory, "AGEWELL_Database", "Medication_List", "Archive");

        ExceptionHandling exceptionHandling = new ExceptionHandling();

        public void cMedication()
        {
            void readData()
            {
                string filePath = Path.Combine(baseDirectory, "AGEWELL_Files", "cMEDICATION_AgeWell.txt");

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

            string residentUsername;

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.SetCursorPosition(66, 32);
                Console.WriteLine("Resident's Username: ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.SetCursorPosition(87, 32);
                residentUsername = Console.ReadLine();

                if (verifyResident(residentUsername))
                {
                    viewMedication(residentUsername);
                    continueKey("Press any key to continue.");

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
                                    addMedication(residentUsername);
                                    viewMedication(residentUsername);
                                    continueKey("Press any key to return to menu.");
                                    break;

                                case 2:
                                    viewMedication(residentUsername);
                                    administerMedication(residentUsername);
                                    viewMedication(residentUsername);
                                    continueKey("Press any key to return to menu.");
                                    break;

                                case 3:
                                    viewMedication(residentUsername);
                                    removeMedication(residentUsername);
                                    viewMedication(residentUsername);
                                    continueKey("Press any key to return to menu.");
                                    break;

                                case 4:
                                    archiveMedication(residentUsername);
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
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.SetCursorPosition(69, 35);
                    Console.WriteLine("Error: Resident not found.");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Thread.Sleep(1000);

                    Console.SetCursorPosition(69, 35);
                    Console.Write(new string(' ', Console.WindowWidth - 80));
                    Console.SetCursorPosition(69, 35);
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

        public void viewMedication(string residentUsername)
        {
            string currentMonth = DateTime.Now.ToString("MMMM yyyy");
            string residentMedicationFile = Path.Combine(medicationDirectory, $"{residentUsername}_{currentMonth}_Medication.txt");

            if (File.Exists(residentMedicationFile))
            {
                using (StreamReader reader = new StreamReader(residentMedicationFile))
                {
                    Console.Clear();

                    string medicationData = reader.ReadToEnd();
                    string message = $"[ {residentUsername}'s Medication in {currentMonth} ]";
                    int windowWidth = Console.WindowWidth;
                    int padding = (windowWidth - message.Length) / 2;

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"\n\n\n\n\n\n\n\n\n\n{new string(' ', padding)}{message}\n");
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine(medicationData);
                }
            }
            else
            {
                exceptionHandling.message($"No medication records for {residentUsername} in {currentMonth}.", ConsoleColor.DarkRed);
                Thread.Sleep(1000);
            }
        }

        public void addMedication(string residentUsername)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.SetCursorPosition(66, 29);
            Console.WriteLine("Medication Name: ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.SetCursorPosition(83, 29);
            string medicationName = Console.ReadLine();

            string medicationTime;

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.SetCursorPosition(69, 31);
                Console.WriteLine("Time (HH:MM): ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.SetCursorPosition(83, 31);
                medicationTime = Console.ReadLine();
                if (DateTime.TryParseExact(medicationTime, "HH:mm", null, System.Globalization.DateTimeStyles.None, out _))
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

            string medicationDosage;

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.SetCursorPosition(62, 33);
                Console.WriteLine("Dosage ('mg', 'ml'): ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.SetCursorPosition(83, 33);
                medicationDosage = Console.ReadLine();

                if (System.Text.RegularExpressions.Regex.IsMatch(medicationDosage, @"^\d+(mg|ml)$", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
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

            string currentMonth = DateTime.Now.ToString("MMMM yyyy");
            string headerRow = "\n\tMedication\tTime\tDosage\t   " + string.Join("  ", Enumerable.Range(1, 31).Select(day => day.ToString("D2")));
            string dataRow = $"\t{medicationName}\t{medicationTime}\t{medicationDosage}\t " + string.Join("  ", new string[31].Select(_ => " "));

            string residentMedicationFile = Path.Combine(medicationDirectory, $"{residentUsername}_{currentMonth}_Medication.txt");

            bool isFileEmpty = !File.Exists(residentMedicationFile) || new FileInfo(residentMedicationFile).Length == 0;

            using (StreamWriter writer = new StreamWriter(residentMedicationFile, true))
            {
                if (isFileEmpty)
                {
                    writer.WriteLine(headerRow);
                }
                writer.WriteLine(dataRow);
            }

            exceptionHandling.message("Medication added successfully!", ConsoleColor.DarkGreen);
            Thread.Sleep(3000);
        }

        public void administerMedication(string residentUsername)
        {
            string currentMonth = DateTime.Now.ToString("MMMM yyyy");
            int today = DateTime.Now.Day;

            string residentMedicationFile = Path.Combine(medicationDirectory, $"{residentUsername}_{currentMonth}_Medication.txt");

            if (File.Exists(residentMedicationFile))
            {
                string[] lines = File.ReadAllLines(residentMedicationFile);

                for (int i = 2; i < lines.Length; i++)
                {
                    string[] columns = lines[i].Split(new[] { "    " }, StringSplitOptions.None);

                    if (columns.Length < today + 1)
                    {
                        Array.Resize(ref columns, today + 1);

                        for (int j = columns.Length - 1; j < today + 1; j++)
                        {
                            if (columns[j] == null)
                            {
                                columns[j] = " ";
                            }
                        }

                        lines[i] = string.Join("    ", columns);
                    }
                }

                File.WriteAllLines(residentMedicationFile, lines);

                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.SetCursorPosition(66, 29);
                Console.WriteLine("Medication Name: ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.SetCursorPosition(83, 29);
                string medicationName = Console.ReadLine();

                string medicationTime;

                while (true)
                {
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.SetCursorPosition(69, 31);
                    Console.WriteLine("Time (HH:MM): ");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.SetCursorPosition(83, 31);
                    medicationTime = Console.ReadLine();

                    if (DateTime.TryParseExact(medicationTime, "HH:mm", null, System.Globalization.DateTimeStyles.None, out _))
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

                bool medicationFound = false;

                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].Contains(medicationName) && lines[i].Contains(medicationTime))
                    {
                        medicationFound = true;

                        string[] columns = lines[i].Split(new[] { "    " }, StringSplitOptions.None);

                        if (columns.Length > today)
                        {
                            if (columns[today] == "/")
                            {
                                exceptionHandling.message("Error: Medication already marked as administered for today.", ConsoleColor.DarkRed);
                                Thread.Sleep(1000);
                            }
                            else
                            {
                                columns[today] = "/";

                                lines[i] = string.Join("    ", columns);
                                File.WriteAllLines(residentMedicationFile, lines);

                                exceptionHandling.message("Medication marked as administered for today.", ConsoleColor.DarkGreen);
                                Thread.Sleep(3000);
                            }
                        }

                        break;
                    }
                }

                if (!medicationFound)
                {
                    exceptionHandling.message("Error: Medication not found.", ConsoleColor.DarkRed);
                    Thread.Sleep(1000);
                }
            }
            else
            {
                exceptionHandling.message($"No medication records for {residentUsername} in {currentMonth}.", ConsoleColor.DarkRed);
                Thread.Sleep(1000);
            }
        }

        public void removeMedication(string residentUsername)
        {
            string currentMonth = DateTime.Now.ToString("MMMM yyyy");

            string residentMedicationFile = Path.Combine(medicationDirectory, $"{residentUsername}_{currentMonth}_Medication.txt");

            if (File.Exists(residentMedicationFile))
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.SetCursorPosition(66, 29);
                Console.WriteLine("Medication Name: ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.SetCursorPosition(83, 29);
                string medicationName = Console.ReadLine();

                string medicationTime;

                while (true)
                {
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.SetCursorPosition(69, 31);
                    Console.WriteLine("Time (HH:MM): ");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.SetCursorPosition(83, 31);
                    medicationTime = Console.ReadLine();
                    if (DateTime.TryParseExact(medicationTime, "HH:mm", null, System.Globalization.DateTimeStyles.None, out _))
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

                string medicationDosage;

                while (true)
                {
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.SetCursorPosition(62, 33);
                    Console.WriteLine("Dosage ('mg', 'ml'): ");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.SetCursorPosition(83, 33);
                    medicationDosage = Console.ReadLine();

                    if (System.Text.RegularExpressions.Regex.IsMatch(medicationDosage, @"^\d+(mg|ml)$", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
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

                var lines = File.ReadAllLines(residentMedicationFile).ToList();
                bool medicationFound = false;

                for (int i = 0; i < lines.Count; i++)
                {
                    if (lines[i].Contains(medicationName) && lines[i].Contains(medicationTime) && lines[i].Contains(medicationDosage))
                    {
                        lines.RemoveAt(i);
                        medicationFound = true;
                        break;
                    }
                }

                if (medicationFound)
                {
                    File.WriteAllLines(residentMedicationFile, lines);

                    exceptionHandling.message("Medication removed successfully!", ConsoleColor.DarkGreen);
                    Thread.Sleep(3000);
                }
                else
                {
                    exceptionHandling.message("Error: Medication not found.", ConsoleColor.DarkRed);
                    Thread.Sleep(1000);
                }
            }
            else
            {
                exceptionHandling.message($"No medication records for {residentUsername} in {currentMonth}.", ConsoleColor.DarkRed);
                Thread.Sleep(1000);
            }
        }

        public void archiveMedication(string residentUsername)
        {
            string currentMonth = DateTime.Now.ToString("MMMM yyyy");
            string residentMedicationFile = Path.Combine(medicationDirectory, $"{residentUsername}_{currentMonth}_Medication.txt");
            string archiveFile = Path.Combine(archiveDirectory, $"Archived_Medications_{currentMonth}.txt");

            if (File.Exists(residentMedicationFile))
            {
                using (StreamReader reader = new StreamReader(residentMedicationFile))
                {
                    using (StreamWriter writer = new StreamWriter(archiveFile, true))
                    {
                        writer.WriteLine($"\n\t[ {residentUsername}'s Medication in {currentMonth} ]");
                        writer.WriteLine(reader.ReadToEnd());
                        writer.WriteLine($"\t{new string('*', 10)}");
                    }
                }

                File.Delete(residentMedicationFile);

                exceptionHandling.message($"Medication records for {residentUsername} in {currentMonth} archived successfully!", ConsoleColor.DarkGreen);
                Thread.Sleep(3000);
            }
            else
            {
                exceptionHandling.message($"No medication records found for {residentUsername} to archive.", ConsoleColor.DarkRed);
                Thread.Sleep(1000);
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