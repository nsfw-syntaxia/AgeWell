using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace DOLERA_AgeWell
{
    internal class HealthRecords
    {
        private static string baseDirectory = AppContext.BaseDirectory;
        private static string healthrecordsDirectory = Path.Combine(baseDirectory, "AGEWELL_Database", "HealthRecords_List");
        private static string archiveDirectory = Path.Combine(baseDirectory, "AGEWELL_Database", "HealthRecords_List", "Archive");

        ExceptionHandling exceptionHandling = new ExceptionHandling();

        public void cHealthRecords()
        {
            void readData()
            {
                string filePath = Path.Combine(baseDirectory, "AGEWELL_Files", "cHEALTHRECORDS_AgeWell.txt");

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
                                    listDoctorConsultation(residentUsername);
                                    continueKey("Press any key to return to menu.");
                                    break;

                                case 2:
                                    listHealthAssessment(residentUsername);
                                    continueKey("Press any key to return to menu.");
                                    break;

                                case 3:
                                    archiveHealthRecords(residentUsername);
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

        public void healthRecords(string username)
        {
            void readData()
            {
                string filePath = Path.Combine(baseDirectory, "AGEWELL_Files", "rgHEALTHRECORDS_AgeWell.txt");

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
                            listDoctorConsultation(username);
                            continueKey("Press any key to return to menu.");
                            break;

                        case 2:
                            listHealthAssessment(username);
                            continueKey("Press any key to return to menu.");
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

        public void medicalProfile(string username)
        {
            string baseFolderPath = Path.Combine(healthrecordsDirectory, "Medical Profiles");
            string medicalProfileFilePath = Path.Combine(baseFolderPath, $"{username}.txt");
            string residentFilePath = Path.Combine("AGEWELL_Database", "ResidentAccount_Files", $"{username}.txt");

            if (!Directory.Exists(baseFolderPath))
            {
                Directory.CreateDirectory(baseFolderPath);
            }

            if (!File.Exists(residentFilePath))
            {
                exceptionHandling.message("Error: File not found.", ConsoleColor.DarkRed);
                Thread.Sleep(1000);
                return;
            }

            string fullName = string.Empty;
            string age = string.Empty;
            string sex = string.Empty;
            string contactNumber = string.Empty;
            string birthDate = string.Empty;
            string bloodType = string.Empty;

            try
            {
                using (StreamReader sr = new StreamReader(residentFilePath))
                {
                    string line;
                    bool extractRName = false;

                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.Contains("Name:") && !extractRName)
                        {
                            string[] parts = line.Split(new string[] { "\t" }, StringSplitOptions.None);

                            if (parts.Length > 11)
                            {
                                fullName = parts[11].Trim();
                                extractRName = true;
                            }
                        }
                        else if (line.Contains("Date of Birth:"))
                        {
                            birthDate = extractData(line, "Date of Birth:");
                        }
                        else if (line.Contains("Age:"))
                        {
                            age = extractData(line, "Age:");
                        }
                        else if (line.Contains("Sex:"))
                        {
                            sex = extractData(line, "Sex:");
                        }
                        else if (line.Contains("Contact Number:"))
                        {
                            contactNumber = extractData(line, "Contact Number:");
                        }
                    }
                }

                DateTime dob = DateTime.Parse(birthDate);
                string formatDOB = dob.ToString("MM/dd/yyyy");

                if (!File.Exists(medicalProfileFilePath))
                {
                    Console.Clear();
                    bloodType = bloodTypes();

                    using (StreamWriter sw = new StreamWriter(medicalProfileFilePath))
                    {
                        sw.WriteLine();
                        sw.WriteLine($"\tName:\t\t{fullName}");
                        sw.WriteLine($"\tDate of Birth:\t{formatDOB}");
                        sw.WriteLine($"\tAge:\t\t{age}");
                        sw.WriteLine($"\tSex:\t\t{sex}");
                        sw.WriteLine($"\tContact Number:\t{contactNumber}");
                        sw.WriteLine($"\tBlood Type:\t{bloodType}");
                    }
                }
                else
                {
                    using (StreamReader sr = new StreamReader(medicalProfileFilePath))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (line.Contains("Blood Type:"))
                            {
                                bloodType = extractData(line, "Blood Type:");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exceptionHandling.message($"Error: {ex.Message}", ConsoleColor.DarkRed);
                Thread.Sleep(1000);
                return;
            }
        }

        public void viewMedicalProfile(string username)
        {
            string medicalProfileFilePath = Path.Combine(healthrecordsDirectory, "Medical Profiles", $"{username}.txt");

            if (!File.Exists(medicalProfileFilePath))
            {
                exceptionHandling.message("Error: File not found.", ConsoleColor.DarkRed);
                Thread.Sleep(1000);
                return;
            }

            string fullName = string.Empty;
            string age = string.Empty;
            string sex = string.Empty;
            string contactNumber = string.Empty;
            string birthDate = string.Empty;
            string bloodType = string.Empty;

            try
            {
                string[] lines = File.ReadAllLines(medicalProfileFilePath);

                foreach (string line in lines)
                {
                    if (line.Contains("Name:"))
                    {
                        fullName = extractData(line, "Name:");
                    }
                    else if (line.Contains("Age:"))
                    {
                        age = extractData(line, "Age:");
                    }
                    else if (line.Contains("Sex:"))
                    {
                        sex = extractData(line, "Sex:");
                    }
                    else if (line.Contains("Contact Number:"))
                    {
                        contactNumber = extractData(line, "Contact Number:");
                    }
                    else if (line.Contains("Date of Birth:"))
                    {
                        birthDate = extractData(line, "Date of Birth:");
                    }
                    else if (line.Contains("Blood Type:"))
                    {
                        bloodType = extractData(line, "Blood Type:");
                    }
                }

                var table = new Table
                {
                    Padding = 3,
                    HeaderTextAlignRight = false,
                    RowTextAlignRight = false
                };

                table.AddRow("Name", "Age", "Gender");
                table.AddRow(fullName, age, sex);
                table.AddRow("Contact Number", "Date of Birth", "Blood Type");
                table.AddRow(contactNumber, birthDate, bloodType);

                centerTable(table);
            }
            catch (Exception ex)
            {
                exceptionHandling.message($"Error: {ex.Message}", ConsoleColor.DarkRed);
                Thread.Sleep(1000);
                return;
            }
        }

        public void recordDoctorConsultation(string residentUsername, string aDate, string aTime)
        {
            string baseFolderPath = Path.Combine(healthrecordsDirectory, "Doctor Consultations");

            if (!Directory.Exists(baseFolderPath))
            {
                Directory.CreateDirectory(baseFolderPath);
            }

            string validDate = aDate.Replace(":", "").Replace("/", "");
            string validTime = aTime.Replace(":", "");

            string dcFilePath = Path.Combine(baseFolderPath, $"{residentUsername}_{validDate}_{validTime}.txt");

            if (File.Exists(dcFilePath))
            {
                exceptionHandling.message("Doctor consultation already recorded!", ConsoleColor.DarkRed);
                Thread.Sleep(1000);
                return;
            }

            Console.Write("\n\n\n");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("\n\n\t\t\t\t\t\t\t\tStatus: ");

            string statusOptions = "[1] Inpatient Care\t[2] Outpatient Care";
            int consoleWidth = Console.WindowWidth;
            int padding = (consoleWidth - statusOptions.Length) / 2;

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\n\n{new string(' ', padding)}{statusOptions}");

            string status;

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.SetCursorPosition(72, 5);
                string statusInput = Console.ReadLine();

                if (statusInput == "1" || statusInput == "2")
                {
                    status = statusInput == "1" ? "Inpatient Care" : "Outpatient Care";
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
                    Console.Write(new string(' ', "Error: Invalid input. Please try again.".Length));
                    Console.SetCursorPosition(63, 24);
                }
            }


            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("\n\n\n\t\t\t\t\t\t\t\tReason: ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            string reason = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("\n\t\t\t\t\t\t\t\tPhysician: ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            string physician = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("\t\t\t\t\t\t\t\tHospital: ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            string hospital = Console.ReadLine();

            List<string> medicalConditions = new List<string>();

            int countMC = 1;
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("\n\t\t\t\t\t\t\t\tMedical Conditions ('DONE' to finish)");
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write($"\t\t\t\t\t\t\t\t{countMC}: ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                string condition = Console.ReadLine();
                if (condition.Equals("done", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }
                medicalConditions.Add(condition);
                countMC++;
            }

            List<string> symptoms = new List<string>();

            int countSymptoms = 1;
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("\n\t\t\t\t\t\t\t\tSymptoms ('DONE' to finish)");
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write($"\t\t\t\t\t\t\t\t{countSymptoms}: ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                string symptom = Console.ReadLine();
                if (symptom.Equals("done", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }
                symptoms.Add(symptom);
                countSymptoms++;
            }

            List<(string Examination, string Result)> examinationResults = new List<(string, string)>();

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write("\n\t\t\t\t\t\t\t\tExamination: ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                string examination = Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write("\t\t\t\t\t\t\t\tResults: ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                string result = Console.ReadLine();

                examinationResults.Add((examination, result));

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("\n\t\t\t\t\t\t\t\tAdd Examinations and Results (Y/N)? ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                if (Console.ReadLine().Equals("n", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }
            }

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("\n\t\t\t\t\t\t\t\tDiagnosis: ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            string diagnosis = Console.ReadLine();

            List<(string Name, string Dosage, string Frequency)> medications = new List<(string, string, string)>();

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("\n\t\t\t\t\t\t\t\tPrescribed Medication");
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write("\t\t\t\t\t\t\t\tName: ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                string name = Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write("\t\t\t\t\t\t\t\tDosage: ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                string dosage = Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write("\t\t\t\t\t\t\t\tFrequency: ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                string frequency = Console.ReadLine();

                medications.Add((name, dosage, frequency));

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("\n\t\t\t\t\t\t\t\tAdd Medication (Y/N)? ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                if (Console.ReadLine().Equals("n", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }
            }

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("\n\t\t\t\t\t\t\t\tNotes (Enter on a blank line to finish)");
            string notes = string.Empty;
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write("\t\t\t\t\t\t\t\t");
                string line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                {
                    break;
                }
                notes += "\t> " + line + "\n";
            }

            using (StreamWriter writer = new StreamWriter(dcFilePath))
            {
                writer.WriteLine();
                writer.WriteLine($"\tDate: {aDate}");
                writer.WriteLine($"\tTime: {aTime}");
                writer.WriteLine();
                writer.WriteLine($"\tStatus: {status}");
                writer.WriteLine($"\tPhysician: {physician}");
                writer.WriteLine($"\tHospital: {hospital}");
                writer.WriteLine();
                writer.WriteLine("\tMedical Conditions");
                foreach (var condition in medicalConditions)
                {
                    writer.WriteLine($"\t> {condition}");
                }
                writer.WriteLine();
                writer.WriteLine("\tSymptoms");
                foreach (var symptom in symptoms)
                {
                    writer.WriteLine($"\t> {symptom}");
                }
                foreach (var exam in examinationResults)
                {
                    writer.WriteLine($"\n\tExamination: {exam.Examination}\n\tResult: {exam.Result}");
                }
                writer.WriteLine();
                writer.WriteLine($"\tDiagnosis: {diagnosis}");
                writer.WriteLine();
                writer.WriteLine("\tPrescribed Medications");
                foreach (var med in medications)
                {
                    writer.WriteLine($"\t> {med.Name}\t({med.Dosage}, {med.Frequency})");
                }
                writer.WriteLine();
                writer.WriteLine("\tNotes");
                writer.WriteLine(notes.TrimEnd());
            }

            exceptionHandling.message("Doctor consultation record saved successfully!", ConsoleColor.DarkGreen);
            Thread.Sleep(3000);
        }

        public void recordHealthAssessment(string residentUsername, string aDate, string aTime)
        {
            try
            {
                string baseFolderPath = Path.Combine(healthrecordsDirectory, "Health Assessments");

                if (!Directory.Exists(baseFolderPath))
                {
                    Directory.CreateDirectory(baseFolderPath);
                }

                string validDate = aDate.Replace(":", "").Replace("/", "");
                string validTime = aTime.Replace(":", "");

                string haFilePath = Path.Combine(baseFolderPath, $"{residentUsername}_{validDate}_{validTime}.txt");

                if (File.Exists(haFilePath))
                {
                    exceptionHandling.message("Health assessment already recorded!", ConsoleColor.DarkRed);
                    Thread.Sleep(1000);
                    return;
                }

                Console.Write("\n\n\n");

                double height = 0, weight = 0, bodyTemperature = 0, bloodOxygen = 0, bloodGlucose = 0;
                int pulse = 0, respiratoryRate = 0, systolic = 0, diastolic = 0;

                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write("\n\n\t\t\t\t\t\t\t\tHeight (meters): ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                while (!double.TryParse(Console.ReadLine(), out height) || height <= 0)
                {
                    exceptionHandling.message("Invalid input. Please try again.", ConsoleColor.DarkRed);
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.Write("\n\n\t\t\t\t\t\t\t\tHeight (meters): ");
                }

                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write("\n\n\t\t\t\t\t\t\t\tWeight (kilograms): ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                while (!double.TryParse(Console.ReadLine(), out weight) || weight <= 0)
                {
                    exceptionHandling.message("Invalid input. Please try again.", ConsoleColor.DarkRed);
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.Write("\n\n\t\t\t\t\t\t\t\tWeight (kilograms): ");
                }

                double bmi = calculateBMI(height, weight);
                string bmiClassification = classifyBMI(bmi);

                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write("\n\n\t\t\t\t\t\t\t\tBody Temperature (°C): ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                while (!double.TryParse(Console.ReadLine(), out bodyTemperature) || bodyTemperature <= 0)
                {
                    exceptionHandling.message("Invalid input. Please try again.", ConsoleColor.DarkRed);
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.Write("\n\n\t\t\t\t\t\t\t\tBody Temperature (°C): ");
                }

                string temperatureClassification = classifyTemperature(bodyTemperature);

                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write("\n\n\t\t\t\t\t\t\t\tPulse (beats per minute): ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                while (!int.TryParse(Console.ReadLine(), out pulse) || pulse <= 0)
                {
                    exceptionHandling.message("Invalid input. Please try again.", ConsoleColor.DarkRed);
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.Write("\n\n\t\t\t\t\t\t\t\tPulse (beats per minute): ");
                }

                string pulseClassification = classifyPulse(pulse);

                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write("\n\n\t\t\t\t\t\t\t\tRespiratory Rate (breaths per minute): ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                while (!int.TryParse(Console.ReadLine(), out respiratoryRate) || respiratoryRate <= 0)
                {
                    exceptionHandling.message("Invalid input. Please try again.", ConsoleColor.DarkRed);
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.Write("\n\n\t\t\t\t\t\t\t\tRespiratory Rate (breaths per minute): ");
                }

                string respiratoryClassification = classifyRespiratoryRate(respiratoryRate);

                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write("\n\n\t\t\t\t\t\t\t\tBlood Oxygen (%): ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                while (!double.TryParse(Console.ReadLine(), out bloodOxygen) || bloodOxygen < 0 || bloodOxygen > 100)
                {
                    exceptionHandling.message("Invalid input. Please try again.", ConsoleColor.DarkRed);
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.Write("\n\n\t\t\t\t\t\t\t\tBlood Oxygen (%): ");
                }

                string oxygenClassification = classifyBloodOxygen(bloodOxygen);

                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write("\n\n\t\t\t\t\t\t\t\tBlood Pressure (systolic/diastolic): ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                string[] bloodPressure = Console.ReadLine().Split('/');
                while (bloodPressure.Length != 2 || !int.TryParse(bloodPressure[0], out systolic) || !int.TryParse(bloodPressure[1], out diastolic))
                {
                    exceptionHandling.message("Invalid input. Please try again.", ConsoleColor.DarkRed);
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.Write("\n\n\t\t\t\t\t\t\t\tBlood Pressure (systolic/diastolic): ");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    bloodPressure = Console.ReadLine().Split('/');
                }

                string bpClassification = classifyBloodPressure(systolic, diastolic);

                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write("\n\n\t\t\t\t\t\t\t\tBlood Glucose Level (mg/dL): ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                while (!double.TryParse(Console.ReadLine(), out bloodGlucose) || bloodGlucose <= 0)
                {
                    exceptionHandling.message("Invalid input. Please try again.", ConsoleColor.DarkRed);
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.Write("\n\n\t\t\t\t\t\t\t\tBlood Glucose Level (mg/dL): ");
                }

                string glucoseClassification = classifyBloodGlucose(bloodGlucose);

                try
                {
                    using (StreamWriter writer = new StreamWriter(haFilePath, append: false))
                    {
                        writer.WriteLine();
                        writer.WriteLine($"\tDate: {aDate}");
                        writer.WriteLine($"\tTime: {aTime}");
                        writer.WriteLine();
                        writer.WriteLine($"\tHeight: {height}m");
                        writer.WriteLine($"\tWeight: {weight}kg");
                        writer.WriteLine($"\tBMI: {bmi:F2} ({bmiClassification})");
                        writer.WriteLine();
                        writer.WriteLine($"\tBody Temperature: {bodyTemperature}°C ({temperatureClassification})");
                        writer.WriteLine($"\tPulse: {pulse} bpm ({pulseClassification})");
                        writer.WriteLine($"\tRespiratory Rate: {respiratoryRate} breaths per minute ({respiratoryClassification})");
                        writer.WriteLine($"\tBlood Oxygen: {bloodOxygen}% ({oxygenClassification})");
                        writer.WriteLine($"\tBlood Pressure: {systolic}/{diastolic} mmHg ({bpClassification})");
                        writer.WriteLine($"\tBlood Glucose Level: {bloodGlucose} mg/dL ({glucoseClassification})");
                    }

                    exceptionHandling.message("Health assessment record saved successfully!", ConsoleColor.DarkGreen);
                    Thread.Sleep(3000);
                }
                catch (IOException)
                {
                    Console.WriteLine("Error: Sorry, program could not write to file.");
                }
            }
            catch (Exception ex)
            {
                exceptionHandling.message($"Error: {ex.Message}", ConsoleColor.DarkRed);
            }
        }

        public void listDoctorConsultation(string username)
        {
            string currentMonth = DateTime.Now.ToString("MMMM yyyy");
            string baseFolderPath = Path.Combine(healthrecordsDirectory, "Doctor Consultations");

            if (!Directory.Exists(baseFolderPath))
            {
                exceptionHandling.message("Error: Directory not found.", ConsoleColor.DarkRed);
                Thread.Sleep(1000);
                return;
            }

            string[] consultationFiles = Directory.GetFiles(baseFolderPath, $"{username}_*.txt");

            if (consultationFiles.Length == 0)
            {
                exceptionHandling.message($"No doctor consultation records for {username} in {currentMonth}.", ConsoleColor.DarkRed);
                Thread.Sleep(1000);
                return;
            }

            List<(DateTime dateTime, string fileName)> consultations = new List<(DateTime, string)>();

            foreach (string file in consultationFiles)
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                string[] parts = fileName.Split('_');

                if (parts.Length >= 2)
                {
                    string datePart = parts[1];
                    string timePart = parts[2];

                    if (DateTime.TryParseExact(datePart + timePart, "MMddyyyyHHmm", null, System.Globalization.DateTimeStyles.None, out DateTime consultationDateTime))
                    {
                        consultations.Add((consultationDateTime, file));
                    }
                }
            }

            var sortedConsultations = consultations.OrderBy(c => c.dateTime).ToList();

            Console.Clear();

            string message = $"[ {username}'s Doctor Consultation Records in {currentMonth} ]";
            int windowWidth = Console.WindowWidth;
            int padding = (windowWidth - message.Length) / 2;

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\n\n\n\n\n\n\n\n\n\n{new string(' ', padding)}{message}\n\n");

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("\t\t\t\t\t\t\t\t\tDate\t\t\tTime");

            int consultationNumber = 1;
            foreach (var consultation in sortedConsultations)
            {
                Console.WriteLine($"\t\t\t\t\t\t\t\t[{consultationNumber}]\t{consultation.dateTime:MM/dd/yyyy}\t\t{consultation.dateTime:HH:mm}");
                consultationNumber++;
            }

            int dcNumber;
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.SetCursorPosition(63, 29);
                Console.WriteLine("File Record Number: ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.SetCursorPosition(83, 29);

                if (int.TryParse(Console.ReadLine(), out dcNumber) && dcNumber >= 1 && dcNumber <= sortedConsultations.Count)
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

            string doctorconsultationFile = sortedConsultations[dcNumber - 1].fileName;
            viewDoctorConsultation(username, doctorconsultationFile);
        }

        public void listHealthAssessment(string username)
        {
            string currentMonth = DateTime.Now.ToString("MMMM yyyy");
            string baseFolderPath = Path.Combine(healthrecordsDirectory, "Health Assessments");

            if (!Directory.Exists(baseFolderPath))
            {
                exceptionHandling.message("Error: Directory not found.", ConsoleColor.DarkRed);
                Thread.Sleep(1000);
                return;
            }

            string[] assessmentFiles = Directory.GetFiles(baseFolderPath, $"{username}_*.txt");

            if (assessmentFiles.Length == 0)
            {
                exceptionHandling.message($"No health assessment records for {username} in {currentMonth}.", ConsoleColor.DarkRed);
                Thread.Sleep(1000);
                return;
            }

            List<(DateTime dateTime, string fileName)> assessments = new List<(DateTime, string)>();

            foreach (string file in assessmentFiles)
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                string[] parts = fileName.Split('_');

                if (parts.Length >= 2)
                {
                    string datePart = parts[1];
                    string timePart = parts[2];

                    if (DateTime.TryParseExact(datePart + timePart, "MMddyyyyHHmm", null, System.Globalization.DateTimeStyles.None, out DateTime assessmentDateTime))
                    {
                        assessments.Add((assessmentDateTime, file));
                    }
                }
            }

            var sortedAssessments = assessments.OrderBy(a => a.dateTime).ToList();

            Console.Clear();

            string message = $"[ {username}'s Health Assessment Records in {currentMonth} ]";
            int windowWidth = Console.WindowWidth;
            int padding = (windowWidth - message.Length) / 2;

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\n\n\n\n\n\n\n\n\n\n{new string(' ', padding)}{message}\n\n");

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("\t\t\t\t\t\t\t\t\tDate\t\t\tTime");

            int assessmentNumber = 1;
            foreach (var assessment in sortedAssessments)
            {
                Console.WriteLine($"\t\t\t\t\t\t\t\t[{assessmentNumber}]\t{assessment.dateTime:MM/dd/yyyy}\t\t{assessment.dateTime:HH:mm}");
                assessmentNumber++;
            }

            int haNumber;
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.SetCursorPosition(63, 29);
                Console.WriteLine("File Record Number: ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.SetCursorPosition(83, 29);

                if (int.TryParse(Console.ReadLine(), out haNumber) && haNumber >= 1 && haNumber <= sortedAssessments.Count)
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

            string healthAssessmentFile = sortedAssessments[haNumber - 1].fileName;
            viewHealthAssessment(username, healthAssessmentFile);
        }

        public void viewDoctorConsultation(string residentUsername, string dcfile)
        {
            if (!File.Exists(dcfile))
            {
                exceptionHandling.message("Error: File not found.", ConsoleColor.DarkRed);
                Thread.Sleep(1000);
                return;
            }

            Console.Clear();

            string message = "[ Doctor Consultation ]";
            int windowWidth = Console.WindowWidth;
            int padding = (windowWidth - message.Length) / 2;

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\n\n\n{new string(' ', padding)}{message}\n\n");

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            viewMedicalProfile(residentUsername);

            try
            {
                string[] lines = File.ReadAllLines(dcfile);
                string consultationDate = string.Empty;
                string consultationTime = string.Empty;
                string status = string.Empty;
                string physician = string.Empty;
                string hospital = string.Empty;
                List<string> medicalConditions = new List<string>();
                List<string> symptoms = new List<string>();
                List<string> examinations = new List<string>();
                List<string> examinationResults = new List<string>();
                string diagnosis = string.Empty;
                List<(string Name, string Dosage, string Frequency)> prescribedMedications = new List<(string, string, string)>();
                List<string> notes = new List<string>();

                string currentSection = string.Empty;
                string currentExamination = string.Empty;
                string currentResult = string.Empty;

                foreach (string line in lines)
                {
                    string trimLine = line.TrimStart('>', '\t').TrimStart();

                    if (line.Contains("Date:"))
                    {
                        consultationDate = extractData(line, "Date:");
                    }
                    else if (line.Contains("Time:"))
                    {
                        consultationTime = extractData(line, "Time:");
                    }
                    else if (line.Contains("Status:"))
                    {
                        status = extractData(line, "Status:");
                    }
                    else if (line.Contains("Physician:"))
                    {
                        physician = extractData(line, "Physician:");
                    }
                    else if (line.Contains("Hospital:"))
                    {
                        hospital = extractData(line, "Hospital:");
                    }
                    else if (line.Contains("Medical Conditions"))
                    {
                        currentSection = "MedicalConditions";
                        continue;
                    }
                    else if (line.Contains("Symptoms"))
                    {
                        currentSection = "Symptoms";
                        continue;
                    }
                    else if (line.Contains("Examination:"))
                    {
                        currentSection = "Examination";
                        currentExamination = trimLine.Substring("Examination:".Length).Trim();
                    }
                    else if (line.Contains("Result:"))
                    {
                        currentResult = trimLine.Substring("Result:".Length).Trim();

                        if (!string.IsNullOrEmpty(currentExamination) && !string.IsNullOrEmpty(currentResult))
                        {
                            examinations.Add(currentExamination);
                            examinationResults.Add(currentResult);

                            currentExamination = string.Empty;
                            currentResult = string.Empty;
                        }
                    }
                    else if (line.Contains("Diagnosis:"))
                    {
                        diagnosis = extractData(line, "Diagnosis:");
                    }
                    else if (line.Contains("Prescribed Medications"))
                    {
                        currentSection = "PrescribedMedications";
                        continue;
                    }
                    else if (line.Contains("Notes"))
                    {
                        currentSection = "Notes";
                        continue;
                    }

                    if (!string.IsNullOrWhiteSpace(trimLine))
                    {
                        switch (currentSection)
                        {
                            case "MedicalConditions":
                                medicalConditions.Add(trimLine);
                                break;

                            case "Symptoms":
                                symptoms.Add(trimLine);
                                break;

                            case "PrescribedMedications":

                                if (string.IsNullOrEmpty(trimLine)) continue;

                                if (trimLine.Contains("\t"))
                                {
                                    string[] parts = trimLine.Split('\t');

                                    if (parts.Length >= 2)
                                    {
                                        string name = parts[0].Trim();
                                        string dosageAndFrequency = parts[1].Trim();

                                        string dosage = "";
                                        string frequency = "";

                                        int startIndex = dosageAndFrequency.IndexOf('(');
                                        int endIndex = dosageAndFrequency.IndexOf(')');

                                        if (startIndex >= 0 && endIndex >= 0)
                                        {
                                            string insideParentheses = dosageAndFrequency.Substring(startIndex + 1, endIndex - startIndex - 1);

                                            string[] dosageParts = insideParentheses.Split(',');

                                            if (dosageParts.Length >= 2)
                                            {
                                                dosage = dosageParts[0].Trim();
                                                frequency = dosageParts[1].Trim();
                                            }
                                        }

                                        prescribedMedications.Add((name, dosage, frequency));
                                    }
                                }
                                break;

                            case "Notes":
                                notes.Add(trimLine);
                                break;
                        }
                    }
                }

                var table1 = new Table
                {
                    Padding = 8,
                    HeaderTextAlignRight = false,
                    RowTextAlignRight = false
                };

                table1.AddRow("Date & Time", $"{consultationDate} & {consultationTime}");
                table1.AddRow("Status", status);
                table1.AddRow("Hospital", hospital);
                table1.AddRow("Physician", physician);
                table1.AddRow("Diagnosis", diagnosis);

                centerTable(table1);

                var table2 = new Table
                {
                    Padding = 9,
                    HeaderTextAlignRight = false,
                    RowTextAlignRight = false
                };

                table2.AddRow("Medical Condition", "Symptoms");

                int maxCount = Math.Max(medicalConditions.Count, symptoms.Count);

                for (int i = 0; i < maxCount; i++)
                {
                    string condition = i < medicalConditions.Count ? medicalConditions[i] : string.Empty;
                    string symptom = i < symptoms.Count ? symptoms[i] : string.Empty;

                    table2.AddRow(condition, symptom);
                }

                centerTable(table2);

                var table3 = new Table
                {
                    Padding = 9,
                    HeaderTextAlignRight = false,
                    RowTextAlignRight = false
                };

                table3.AddRow("Examination", "Results");

                for (int i = 0; i < examinations.Count; i++)
                {
                    table3.AddRow(examinations[i], examinationResults[i]);
                }

                centerTable(table3);

                var table4 = new Table
                {
                    Padding = 6,
                    HeaderTextAlignRight = false,
                    RowTextAlignRight = false
                };

                table4.AddRow("Medication", "Dosage", "Frequency");

                foreach (var med in prescribedMedications)
                {
                    table4.AddRow(med.Name, med.Dosage, med.Frequency);
                }

                centerTable(table4);

                var table5 = new Table
                {
                    Padding = 15,
                    HeaderTextAlignRight = false,
                    RowTextAlignRight = false
                };

                table5.AddRow("Notes");

                foreach (var n in notes)
                {
                    table5.AddRow(n);
                }

                centerTable(table5);
            }
            catch (Exception ex)
            {
                exceptionHandling.message($"Error: {ex.Message}", ConsoleColor.DarkRed);
                Thread.Sleep(1000);
                return;
            }
        }

        public void viewHealthAssessment(string residentUsername, string haFile)
        {
            if (!File.Exists(haFile))
            {
                exceptionHandling.message("Error: File not found.", ConsoleColor.DarkRed);
                Thread.Sleep(1000);
                return;
            }

            Console.Clear();

            string message = "[ Health Assessment ]";
            int windowWidth = Console.WindowWidth;
            int padding = (windowWidth - message.Length) / 2;

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\n\n\n{new string(' ', padding)}{message}\n\n");

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            viewMedicalProfile(residentUsername);

            try
            {
                string[] lines = File.ReadAllLines(haFile);

                string assessmentDate = string.Empty;
                string assessmentTime = string.Empty;
                string height = string.Empty;
                string weight = string.Empty;
                string bmi = string.Empty;
                string bmiClassification = string.Empty;
                string bodyTemperature = string.Empty;
                string temperatureClassification = string.Empty;
                string pulse = string.Empty;
                string pulseClassification = string.Empty;
                string respiratoryRate = string.Empty;
                string respiratoryClassification = string.Empty;
                string bloodOxygen = string.Empty;
                string oxygenClassification = string.Empty;
                string bloodPressure = string.Empty;
                string bpClassification = string.Empty;
                string bloodGlucose = string.Empty;
                string glucoseClassification = string.Empty;

                foreach (string line in lines)
                {
                    if (line.Contains("Date:"))
                    {
                        assessmentDate = extractData(line, "Date:");
                    }
                    else if (line.Contains("Time:"))
                    {
                        assessmentTime = extractData(line, "Time:");
                    }
                    else if (line.Contains("Height:"))
                    {
                        height = extractData(line, "Height:");
                    }
                    else if (line.Contains("Weight:"))
                    {
                        weight = extractData(line, "Weight:");
                    }
                    else if (line.Contains("BMI:"))
                    {
                        string[] bmiParts = extractData(line, "BMI:").Split('(');
                        bmi = bmiParts[0].Trim();
                        bmiClassification = bmiParts.Length > 1 ? bmiParts[1].Trim(')', ' ') : string.Empty;
                    }
                    else if (line.Contains("Body Temperature:"))
                    {
                        string[] tempParts = extractData(line, "Body Temperature:").Split('(');
                        bodyTemperature = tempParts[0].Trim();
                        temperatureClassification = tempParts.Length > 1 ? tempParts[1].Trim(')', ' ') : string.Empty;
                    }
                    else if (line.Contains("Pulse:"))
                    {
                        string[] pulseParts = extractData(line, "Pulse:").Split('(');
                        pulse = pulseParts[0].Trim();
                        pulseClassification = pulseParts.Length > 1 ? pulseParts[1].Trim(')', ' ') : string.Empty;
                    }
                    else if (line.Contains("Respiratory Rate:"))
                    {
                        string[] respParts = extractData(line, "Respiratory Rate:").Split('(');
                        respiratoryRate = respParts[0].Trim();
                        respiratoryClassification = respParts.Length > 1 ? respParts[1].Trim(')', ' ') : string.Empty;
                    }
                    else if (line.Contains("Blood Oxygen:"))
                    {
                        string[] oxygenParts = extractData(line, "Blood Oxygen:").Split('(');
                        bloodOxygen = oxygenParts[0].Trim();
                        oxygenClassification = oxygenParts.Length > 1 ? oxygenParts[1].Trim(')', ' ') : string.Empty;
                    }
                    else if (line.Contains("Blood Pressure:"))
                    {
                        string[] bpParts = extractData(line, "Blood Pressure:").Split('(');
                        bloodPressure = bpParts[0].Trim();
                        bpClassification = bpParts.Length > 1 ? bpParts[1].Trim(')', ' ') : string.Empty;
                    }
                    else if (line.Contains("Blood Glucose Level:"))
                    {
                        string[] glucoseParts = extractData(line, "Blood Glucose Level:").Split('(');
                        bloodGlucose = glucoseParts[0].Trim();
                        glucoseClassification = glucoseParts.Length > 1 ? glucoseParts[1].Trim(')', ' ') : string.Empty;
                    }
                }

                var table1 = new Table
                {
                    Padding = 8,
                    HeaderTextAlignRight = false,
                    RowTextAlignRight = false
                };

                table1.AddRow("Date & Time", $"{assessmentDate} & {assessmentTime}");
                table1.AddRow("Height", height);
                table1.AddRow("Weight", weight);

                centerTable(table1);

                var table2 = new Table
                {
                    Padding = 4,
                    HeaderTextAlignRight = false,
                    RowTextAlignRight = false
                };

                table2.AddRow("Vital Signs", "Results", "Classification");
                table2.AddRow("Body Temperature", bodyTemperature, temperatureClassification);
                table2.AddRow("Pulse", pulse, pulseClassification);
                table2.AddRow("Respiratory Rate", respiratoryRate, respiratoryClassification);
                table2.AddRow("Blood Oxygen", bloodOxygen, oxygenClassification);
                table2.AddRow("Blood Pressure", bloodPressure, bpClassification);

                centerTable(table2);

                var table3 = new Table
                {
                    Padding = 6,
                    HeaderTextAlignRight = false,
                    RowTextAlignRight = false
                };

                table3.AddRow("Health Metrics", "Results", "Classification");
                table3.AddRow("BMI", bmi, bmiClassification);
                table3.AddRow("Blood Glucose Level", bloodGlucose, glucoseClassification);

                centerTable(table3);
            }
            catch (Exception ex)
            {
                exceptionHandling.message($"Error: {ex.Message}", ConsoleColor.DarkRed);
                Thread.Sleep(1000);
                return;
            }
        }

        public void archiveHealthRecords(string residentUsername)
        {
            string currentMonth = DateTime.Now.ToString("MMMM yyyy");

            string archiveDoctorConsultationsFile = Path.Combine(archiveDirectory, $"Archived_Doctor_Consultations_{currentMonth}.txt");
            string archiveHealthAssessmentsFile = Path.Combine(archiveDirectory, $"Archived_Health_Assessments_{currentMonth}.txt");

            string doctorConsultationsFolderPath = Path.Combine(healthrecordsDirectory, "Doctor Consultations");
            string healthAssessmentsFolderPath = Path.Combine(healthrecordsDirectory, "Health Assessments");

            if (Directory.Exists(doctorConsultationsFolderPath))
            {
                string[] files = Directory.GetFiles(doctorConsultationsFolderPath, $"{residentUsername}_*.txt");

                foreach (string file in files)
                {
                    using (StreamReader reader = new StreamReader(file))
                    {
                        using (StreamWriter writer = new StreamWriter(archiveDoctorConsultationsFile, true))
                        {
                            writer.WriteLine($"\n\t[ {residentUsername}'s Doctor Consultation Records for {currentMonth} ]");
                            writer.WriteLine(reader.ReadToEnd());
                            writer.WriteLine($"\t{new string('*', 10)}");
                        }
                    }

                    File.Delete(file);
                }

                exceptionHandling.message($"Doctor consultation records for {residentUsername} in {currentMonth} archived successfully!", ConsoleColor.DarkGreen);
                Thread.Sleep(3000);
            }
            else
            {
                exceptionHandling.message($"No doctor consultation records found for {residentUsername} to archive.", ConsoleColor.DarkRed);
                Thread.Sleep(1000);
            }

            if (Directory.Exists(healthAssessmentsFolderPath))
            {
                string[] files = Directory.GetFiles(healthAssessmentsFolderPath, $"{residentUsername}_*.txt");

                foreach (string file in files)
                {
                    using (StreamReader reader = new StreamReader(file))
                    {
                        using (StreamWriter writer = new StreamWriter(archiveHealthAssessmentsFile, true))
                        {
                            writer.WriteLine($"\n\t[ {residentUsername}'s Health Assessment Records for {currentMonth} ]");
                            writer.WriteLine(reader.ReadToEnd());
                            writer.WriteLine($"\t{new string('*', 10)}");
                        }
                    }

                    File.Delete(file);
                }

                exceptionHandling.message($"Health assessment records for {residentUsername} in {currentMonth} archived successfully!", ConsoleColor.DarkGreen);
                Thread.Sleep(3000);
            }
            else
            {
                exceptionHandling.message($"No health assessment records found for {residentUsername} to archive.", ConsoleColor.DarkRed);
                Thread.Sleep(1000);
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

        private static string extractData(string line, string fieldName)
        {
            int index = line.IndexOf(fieldName) + fieldName.Length;
            string data = line.Substring(index).Trim();
            return data;
        }

        private static string bloodTypes()
        {
            int blood;
            string bloodType = string.Empty;
            string[] bloodTypes = { "A+", "A-", "B+", "B-", "O+", "O-", "AB+", "AB-" };

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.SetCursorPosition(71, 19);
                Console.WriteLine("Blood Type: ");

                bloods("[1] A+   [2] A-   [3] B+    [4] B-");
                bloods("[5] O+   [6] O-   [7] AB+   [8] AB-");

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.SetCursorPosition(83, 19);
                if (int.TryParse(Console.ReadLine(), out blood) && blood >= 1 && blood <= bloodTypes.Length)
                {
                    bloodType = bloodTypes[blood - 1];
                    break;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.SetCursorPosition(63, 26);
                    Console.WriteLine("Error: Invalid input. Please try again.");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Thread.Sleep(1000);

                    Console.SetCursorPosition(63, 26);
                    Console.Write(new string(' ', Console.WindowWidth - 80));
                    Console.SetCursorPosition(63, 26);
                }
            }

            void bloods(string options)
            {
                int width = Console.WindowWidth;
                int padding = (width - options.Length) / 2;

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"\n{new string(' ', Math.Max(0, padding))}{options}");
            }

            return bloodType;
        }

        private double calculateBMI(double height, double weight)
        {
            return weight / (height * height);
        }

        private string classifyBMI(double bmi)
        {
            if (bmi < 18.5)
            {
                return "Underweight";
            }
            else if (bmi >= 18.5 && bmi < 24.9)
            {
                return "Normal";
            }
            else if (bmi >= 25 && bmi < 29.9)
            {
                return "Overweight";
            }
            else
            {
                return "Obese";
            }
        }

        private string classifyTemperature(double temp)
        {
            if (temp < 36)
            {
                return "Hypothermia";
            }
            else if (temp <= 37.5)
            {
                return "Normal";
            }
            else
            {
                return "Fever";
            }
        }

        private string classifyPulse(int pulse)
        {
            if (pulse < 60)
            {
                return "Bradycardia";
            }
            else if (pulse <= 100)
            {
                return "Normal";
            }
            else
            {
                return "Tachycardia";
            }
        }

        private string classifyRespiratoryRate(int rate)
        {
            if (rate < 12)
            {
                return "Bradypnea";
            }
            else if (rate <= 20)
            {
                return "Normal";
            }
            else
            {
                return "Tachypnea";
            }
        }

        private string classifyBloodOxygen(double oxygen)
        {
            if (oxygen >= 95)
            {
                return "Normal";
            }
            else
            {
                return "Hypoxemia";
            }
        }

        private string classifyBloodPressure(int systolic, int diastolic)
        {
            if (systolic < 120 && diastolic < 80)
            {
                return "Normal";
            }
            else if (systolic <= 139 || diastolic <= 89)
            {
                return "Prehypertension";
            }
            else
            {
                return "Hypertension";
            }
        }

        private string classifyBloodGlucose(double glucose)
        {
            if (glucose < 70)
            {
                return "Hypoglycemia";
            }
            else if (glucose <= 140)
            {
                return "Normal";
            }
            else
            {
                return "Hyperglycemia";
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