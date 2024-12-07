using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace DOLERA_AgeWell
{
    internal class UserLogin : UserRegistration
    {
        private UserTypeList userTypeList = new UserTypeList();
        ExceptionHandling exceptionHandling = new ExceptionHandling();

        public UserLogin(string username, string password, string firstName, string middleName, string lastName, int age, string sex, string contactNumber, UserTypeList userTypeList) :
                    base(username, password, firstName, middleName, lastName, age, sex, contactNumber)
        {
            this.userTypeList = userTypeList;
        }

        public override string generateUsername()
        {
            return $"{username}";
        }

        public string loginUser(string username, string password)
        {
            string baseDirectory = AppContext.BaseDirectory;
            string baseFolder = Path.Combine(baseDirectory, "AGEWELL_Database");

            string[] userTypes = { "ResidentAccount_Files", "GuardianAccount_Files", "CaregiverAccount_Files" };

            foreach (var userType in userTypes)
            {
                string userFolder = Path.Combine(baseFolder, userType);

                if (!Directory.Exists(userFolder)) continue;

                string[] files = Directory.GetFiles(userFolder, "*.txt");

                foreach (var file in files)
                {
                    try
                    {
                        using (StreamReader reader = new StreamReader(file))
                        {
                            string line;
                            string fileUsername = null;
                            string filePassword = null;

                            while ((line = reader.ReadLine()) != null)
                            {
                                if (line.Contains("Username:"))
                                {
                                    fileUsername = line.Split(':')[1].Trim();
                                }
                                else if (line.Contains("Password:"))
                                {
                                    filePassword = line.Split(':')[1].Trim();
                                }

                                if (fileUsername == username && filePassword == password)
                                {
                                    return userType.Contains("Resident") ? "Resident" : userType.Contains("Guardian") ? "Guardian" : "Caregiver";
                                }
                            }
                        }
                    }
                    catch (IOException)
                    {
                        exceptionHandling.message("Error: Sorry, program could not read from file.", ConsoleColor.DarkRed);
                        Thread.Sleep(1000);
                    }
                }
            }

            return null;
        }
    }

    internal class AdminLogin
    {
        protected const string adminUsername = "admin";
        protected const string adminPassword = "x!a007";

        public bool loginAdmin(string username, string password)
        {
            return username == adminUsername && password == adminPassword;
        }
    }
}