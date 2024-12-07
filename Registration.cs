using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace DOLERA_AgeWell
{
    internal class UserTypeList
    {
        ExceptionHandling exceptionHandling = new ExceptionHandling();

        protected internal List<ResidentAccount> registeredResidents = new List<ResidentAccount>();
        protected internal List<GuardianAccount> registeredGuardians = new List<GuardianAccount>();
        protected internal List<CaregiverAccount> registeredCaregivers = new List<CaregiverAccount>();

        public void addResident(ResidentAccount residentacc)
        {
            registeredResidents.Add(residentacc);
            updateResidentswithoutPC(residentacc);
        }

        public void addGuardian(GuardianAccount guardianacc)
        {
            registeredGuardians.Add(guardianacc);
            updateGuardians(guardianacc);
        }

        public void addCaregiver(CaregiverAccount caregiveracc)
        {
            registeredCaregivers.Add(caregiveracc);
            updateCaregivers(caregiveracc);
        }

        public void updateResidentswithoutPC(ResidentAccount residentacc)
        {
            string baseDirectory = AppContext.BaseDirectory;
            string filePath = Path.Combine(baseDirectory, "AGEWELL_Database", "Users_List", "Residents_withoutPersonalCaregiver_List.txt");

            try
            {
                bool isFirstWrite = !File.Exists(filePath) || new FileInfo(filePath).Length == 0;

                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    if (isFirstWrite)
                    {
                        writer.WriteLine($"\n\n\t\t\t\t\t\t\t\tUsername\t\tName");
                    }

                    foreach (var resident in registeredResidents)
                    {
                        writer.WriteLine($"\t\t\t\t\t\t\t\t{residentacc.username}\t\t{residentacc.firstName} {residentacc.lastName}");
                    }
                }
            }
            catch (IOException)
            {
                exceptionHandling.message("Error: Sorry, program could not write to file.", ConsoleColor.DarkRed);
                Thread.Sleep(1000);
            }
        }

        public void updateGuardians(GuardianAccount guardianacc)
        {
            string baseDirectory = AppContext.BaseDirectory;
            string filePath = Path.Combine(baseDirectory, "AGEWELL_Database", "Users_List", "Guardians_List.txt");

            try
            {
                bool isFirstWrite = !File.Exists(filePath) || new FileInfo(filePath).Length == 0;

                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    if (isFirstWrite)
                    {
                        writer.WriteLine($"\n\n\t\t\t\t\t\t\t\tUsername\t\tName");
                    }

                    foreach (var guardian in registeredGuardians)
                    {
                        writer.WriteLine($"\t\t\t\t\t\t\t\t{guardianacc.username}\t\t{guardian.firstName} {guardian.lastName}");
                    }
                }
            }
            catch (IOException)
            {
                exceptionHandling.message("Error: Sorry, program could not write to file.", ConsoleColor.DarkRed);
                Thread.Sleep(1000);
            }
        }

        public void updateCaregivers(CaregiverAccount caregiveracc)
        {
            string baseDirectory = AppContext.BaseDirectory;
            string filePath = Path.Combine(baseDirectory, "AGEWELL_Database", "Users_List", "Caregivers_List.txt");

            try
            {
                bool isFirstWrite = !File.Exists(filePath) || new FileInfo(filePath).Length == 0;

                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    if (isFirstWrite)
                    {
                        writer.WriteLine($"\n\n\t\t\t\t\t\t\t\tUsername\t\tName");
                    }

                    foreach (var caregiver in registeredCaregivers)
                    {
                        writer.WriteLine($"\t\t\t\t\t\t\t\t{caregiveracc.username}\t\t{caregiveracc.firstName} {caregiveracc.lastName}");
                    }
                }
            }
            catch (IOException)
            {
                exceptionHandling.message("Error: Sorry, program could not write to file.", ConsoleColor.DarkRed);
                Thread.Sleep(1000);
            }
        }
    }

    abstract class UserRegistration
    {
        protected internal string username { get; set; }
        protected internal string password { get; set; }
        protected internal string firstName { get; set; }
        protected internal string middleName { get; set; }
        protected internal string lastName { get; set; }
        protected int age { get; set; }
        protected string sex { get; set; }
        protected string contactNumber { get; set; }

        public UserRegistration(string username, string password, string firstName, string middleName, string lastName, int age, string sex, string contactNumber)
        {
            this.username = username;
            this.password = password;
            this.firstName = firstName;
            this.middleName = middleName;
            this.lastName = lastName;
            this.age = age;
            this.sex = sex;
            this.contactNumber = contactNumber;
        }

        abstract public string generateUsername();
    }

    internal class ResidentAccount : UserLogin, IWriteData
    {
        private string residentNumber { get; set; }
        private int rBMonth { get; set; }
        private int rBDay { get; set; }
        private int rBYear { get; set; }
        protected string rbirthDate { get; set; }
        protected internal string guardianName { get; set; }

        private static readonly string baseFolder = "AGEWELL_Database";
        private static readonly string userFolder = Path.Combine(baseFolder, "ResidentAccount_Files");

        ExceptionHandling exceptionHandling = new ExceptionHandling();

        public ResidentAccount(string username, string password, string firstName, string middleName, string lastName, int age, string sex, string contactNumber, UserTypeList userTypeList, int rBMonth, int rBDay, int rBYear, string rbirthDate, string guardianName) :
                   base(username, password, firstName, middleName, lastName, age, sex, contactNumber, userTypeList)
        {
            this.rBMonth = rBMonth;
            this.rBDay = rBDay;
            this.rBYear = rBYear;
            this.rbirthDate = birthDate();
            this.guardianName = guardianName;
        }

        public override string generateUsername()
        {
            if (!Directory.Exists(userFolder))
            {
                Directory.CreateDirectory(userFolder);
            }

            int highestResidentNumber = getResidentNumber();
            residentNumber = (highestResidentNumber + 1).ToString("D2");

            return $"{firstName[0]}{lastName}R{residentNumber}";
        }

        private int getResidentNumber()
        {
            int highestNumber = 0;

            foreach (var filePath in Directory.GetFiles(userFolder, "*.txt"))
            {
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                int residentNumIndex = fileName.LastIndexOf("R");

                if (residentNumIndex > 0 && int.TryParse(fileName[(residentNumIndex + 1)..], out int residentNum))
                {
                    if (residentNum > highestNumber)
                    {
                        highestNumber = residentNum;
                    }
                }
            }

            return highestNumber;
        }

        public string birthDate()
        {
            string monthName = rBMonth switch
            {
                1 => "January",
                2 => "February",
                3 => "March",
                4 => "April",
                5 => "May",
                6 => "June",
                7 => "July",
                8 => "August",
                9 => "September",
                10 => "October",
                11 => "November",
                12 => "December",
                _ => "Invalid month"
            };

            return monthName == "Invalid month" ? "Error: Invalid month provided." : $"{monthName} {rBDay}, {rBYear}";
        }

        public void writeData()
        {
            string baseFolder = "AGEWELL_Database";
            string userFolder = Path.Combine(baseFolder, $"{GetType().Name}_Files");

            if (!Directory.Exists(baseFolder))
            {
                Directory.CreateDirectory(baseFolder);
            }

            if (!Directory.Exists(userFolder))
            {
                Directory.CreateDirectory(userFolder);
            }

            string filePath = Path.Combine(userFolder, $"{username}.txt");
            try
            {
                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    sw.WriteLine();
                    sw.WriteLine($"\t\t\t\t\t\t\t\tUsername:\t\t{username}");
                    sw.WriteLine($"\t\t\t\t\t\t\t\tPassword:\t\t{password}");
                    sw.WriteLine();
                    sw.WriteLine($"\t\t\t\t\t\t\t\tName:\t\t\t{firstName} {middleName} {lastName}");
                    sw.WriteLine($"\t\t\t\t\t\t\t\tDate of Birth:\t\t{rbirthDate}");
                    sw.WriteLine($"\t\t\t\t\t\t\t\tAge:\t\t\t{age}");
                    sw.WriteLine($"\t\t\t\t\t\t\t\tSex:\t\t\t{sex}");
                    sw.WriteLine($"\t\t\t\t\t\t\t\tGuardian's Name:\t{guardianName}");
                    sw.WriteLine($"\t\t\t\t\t\t\t\tContact Number:\t\t{contactNumber}");
                }
            }
            catch (IOException)
            {
                exceptionHandling.message("Error: Sorry, program could not write to file.", ConsoleColor.DarkRed);
                Thread.Sleep(1000);
            }
        }
    }

    internal class GuardianAccount : UserLogin, IWriteData
    {
        private string guardianNumber { get; set; }
        private string emailAddress { get; set; }

        private static readonly string baseFolder = "AGEWELL_Database";
        private static readonly string userFolder = Path.Combine(baseFolder, "GuardianAccount_Files");

        ExceptionHandling exceptionHandling = new ExceptionHandling();

        public GuardianAccount(string username, string password, string firstName, string middleName, string lastName, int age, string sex, string contactNumber, UserTypeList userTypeList, string emailAddress) :
                   base(username, password, firstName, middleName, lastName, age, sex, contactNumber, userTypeList)
        {
            this.emailAddress = emailAddress;
        }

        public override string generateUsername()
        {
            if (!Directory.Exists(userFolder))
            {
                Directory.CreateDirectory(userFolder);
            }

            int highestGuardianNumber = getGuardianNumber();
            guardianNumber = (highestGuardianNumber + 1).ToString("D2");

            return $"{firstName[0]}{lastName}G{guardianNumber}";
        }

        private int getGuardianNumber()
        {
            int highestNumber = 0;

            foreach (var filePath in Directory.GetFiles(userFolder, "*.txt"))
            {
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                int guardianNumIndex = fileName.LastIndexOf("G");

                if (guardianNumIndex > 0 && int.TryParse(fileName[(guardianNumIndex + 1)..], out int guardianNum))
                {
                    if (guardianNum > highestNumber)
                    {
                        highestNumber = guardianNum;
                    }
                }
            }

            return highestNumber;
        }

        public void writeData()
        {
            string baseFolder = "AGEWELL_Database";
            string userFolder = Path.Combine(baseFolder, $"{GetType().Name}_Files");

            if (!Directory.Exists(baseFolder))
            {
                Directory.CreateDirectory(baseFolder);
            }

            if (!Directory.Exists(userFolder))
            {
                Directory.CreateDirectory(userFolder);
            }

            string filePath = Path.Combine(userFolder, $"{username}.txt");
            try
            {
                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    sw.WriteLine();
                    sw.WriteLine($"\t\t\t\t\t\t\t\tUsername:\t\t{username}");
                    sw.WriteLine($"\t\t\t\t\t\t\t\tPassword:\t\t{password}");
                    sw.WriteLine();
                    sw.WriteLine($"\t\t\t\t\t\t\t\tName:\t\t\t{firstName} {middleName} {lastName}");
                    sw.WriteLine($"\t\t\t\t\t\t\t\tAge:\t\t\t{age}");
                    sw.WriteLine($"\t\t\t\t\t\t\t\tSex:\t\t\t{sex}");
                    sw.WriteLine($"\t\t\t\t\t\t\t\tEmail Address:\t\t{emailAddress}");
                    sw.WriteLine($"\t\t\t\t\t\t\t\tContact Number:\t\t{contactNumber}");
                }
            }
            catch (IOException)
            {
                exceptionHandling.message("Error: Sorry, program could not write to file.", ConsoleColor.DarkRed);
                Thread.Sleep(1000);
            }
        }
    }

    internal class CaregiverAccount : UserLogin, IWriteData
    {
        private string caregiverNumber { get; set; }
        protected internal string specialization { get; set; }
        protected internal string emailAddress { get; set; }

        private static readonly string baseFolder = "AGEWELL_Database";
        private static readonly string userFolder = Path.Combine(baseFolder, "CaregiverAccount_Files");

        ExceptionHandling exceptionHandling = new ExceptionHandling();

        public CaregiverAccount(string username, string password, string firstName, string middleName, string lastName, int age, string sex, string contactNumber, UserTypeList userTypeList, string specialization, string emailAddress) :
                base(username, password, firstName, middleName, lastName, age, sex, contactNumber, userTypeList)
        {
            this.specialization = specialization;
            this.emailAddress = emailAddress;
        }

        public override string generateUsername()
        {
            if (!Directory.Exists(userFolder))
            {
                Directory.CreateDirectory(userFolder);
            }

            int highestCaregiverNumber = getCaregiverNumber();
            caregiverNumber = (highestCaregiverNumber + 1).ToString("D2");

            return $"{firstName[0]}{lastName}C{caregiverNumber}";
        }

        private int getCaregiverNumber()
        {
            int highestNumber = 0;

            foreach (var filePath in Directory.GetFiles(userFolder, "*.txt"))
            {
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                int caregiverNumIndex = fileName.LastIndexOf("C");

                if (caregiverNumIndex > 0 && int.TryParse(fileName[(caregiverNumIndex + 1)..], out int caregiverNum))
                {
                    if (caregiverNum > highestNumber)
                    {
                        highestNumber = caregiverNum;
                    }
                }
            }

            return highestNumber;
        }

        public void writeData()
        {
            string baseFolder = "AGEWELL_Database";
            string userFolder = Path.Combine(baseFolder, $"{GetType().Name}_Files");

            if (!Directory.Exists(baseFolder))
            {
                Directory.CreateDirectory(baseFolder);
            }

            if (!Directory.Exists(userFolder))
            {
                Directory.CreateDirectory(userFolder);
            }

            string filePath = Path.Combine(userFolder, $"{username}.txt");
            try
            {
                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    sw.WriteLine();
                    sw.WriteLine($"\t\t\t\t\t\t\t\tUsername:\t\t{username}");
                    sw.WriteLine($"\t\t\t\t\t\t\t\tPassword:\t\t{password}");
                    sw.WriteLine();
                    sw.WriteLine($"\t\t\t\t\t\t\t\tName:\t\t\t{firstName} {middleName} {lastName}");
                    sw.WriteLine($"\t\t\t\t\t\t\t\tSpecialization:\t\t{specialization}");
                    sw.WriteLine($"\t\t\t\t\t\t\t\tAge:\t\t\t{age}");
                    sw.WriteLine($"\t\t\t\t\t\t\t\tSex:\t\t\t{sex}");
                    sw.WriteLine($"\t\t\t\t\t\t\t\tEmail Address:\t\t{emailAddress}");
                    sw.WriteLine($"\t\t\t\t\t\t\t\tContact Number:\t\t{contactNumber}");
                }
            }
            catch (IOException)
            {
                exceptionHandling.message("Error: Sorry, program could not write to file.", ConsoleColor.DarkRed);
                Thread.Sleep(1000);
            }
        }
    }
}