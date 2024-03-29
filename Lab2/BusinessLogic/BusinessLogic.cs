﻿using BusinessLogic;
using System;
using System.Net.Mail;
using System.Windows;

using DataAccessLayer;
using EntityLayer;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Security.Cryptography;

namespace BusinessLogicLayer
{
    public class BusinessLogic : IBLGeneral
    {

        private DataAccessLayer.GeneralDAO generalDAO;
        private DataAccessLayer.UserDAO userDAO;

        public BusinessLogic()
        {
            this.generalDAO = new GeneralDAO();
            this.userDAO = new UserDAO(generalDAO.getConnection());
        }

        public int generateNumConfirmation()
        {
            double rId;
            Random rnd = new Random();
            rId = rnd.NextDouble();
            rId = Math.Round((rId) * 9000000) + 1000000;

            return (int)rId;
        }

        public DataSet getSubjectsProfe(string email)
        {
            generalDAO.openConection();
            DataSet ds = userDAO.getSubjectsProfe(email);
            generalDAO.closeConnection();
            return ds;
        }

        public int generateNumPass()
        {
            double rId;
            Random rnd = new Random();
            rId = rnd.NextDouble();
            rId = Math.Round((rId) * 900000) + 100000;

            return (int)rId;
        }



        public bool registerUser(string email, string password, string name, string surname, string role, int numConfirmation)
        {
            generalDAO.openConection();
            if (!userDAO.checkExistingUser(email))
            {
                // Register User
                User user = new User(email, password, name, surname, role, numConfirmation);
                userDAO.registerUser(user);
                generalDAO.closeConnection();
                return true;
            }
            else
            {
                generalDAO.closeConnection();
                return false;
            }
        }


        public bool login(string email, string password)
        {

            generalDAO.openConection();
            password = Md5(password); // Hashed password
            if (userDAO.checkLogin(email, password))
            {
                generalDAO.closeConnection();
                return true;
            }
            else
            {
                generalDAO.closeConnection();
                return false;
            }
        }

        public void sendEmailRegister(string email, int id)
        {
            String server = "smtp.ehu.eus";
            String from = "kandreev001@ikasle.ehu.eus";
            String nCredE = "kandreev001@ikasle.ehu.eus";
            String nCredP = "HADS2022-12";
            String subject = "Confirmar tu email! HADS22-12";
            //String body = $"Para confirmar tu email pulsa aqui: https://localhost:44386/Confirmar.aspx?email={email}&numconf={id}";
            String body = $"Para confirmar tu email pulsa aqui: https://hads22-12.azurewebsites.net/Confirmar.aspx?email={email}&numconf={id}";

            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(server);

                //// Message content
                mail.From = new MailAddress(from);
                mail.To.Add(email);
                mail.Subject = subject;
                mail.Body = body;
                //// Message content

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential(nCredE, nCredP);
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                throw new Exception("Mailing error! " + ex.ToString());
            }
        }



        public void sendEmailChangePassword(string email, int idPass)
        {
            String server = "smtp.ehu.eus";
            String from = "kandreev001@ikasle.ehu.eus";
            String nCredE = "kandreev001@ikasle.ehu.eus";
            String nCredP = "HADS2022-12";
            String subject = "Codigo para cambiar tu password! HADS22-12";
            //String body = $"Este es tu codigo de recuperacion:{idPass} \n Para cambiar tu password utiliza este enlace: https://localhost:44386/CambiarPassword.aspx?email={email}";
            String body = $"Este es tu codigo de recuperacion:{idPass} \n Para cambiar tu password utiliza este enlace: https://hads22-12.azurewebsites.net/CambiarPassword.aspx?email={email}";

            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(server);

                //// Message content
                mail.From = new MailAddress(from);
                mail.To.Add(email);
                mail.Subject = subject;
                mail.Body = body;
                //// Message content

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential(nCredE, nCredP);
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                throw new Exception("Mailing error! " + ex.ToString());
            }
        }

        public bool confirmUser(string email, int id)
        {
            generalDAO.openConection();
            if (userDAO.checkUserToConfirm(email, id))
            {
                // set confirmation to true
                userDAO.confrimUser(email);
                generalDAO.closeConnection();
                return true;
            }
            else
            {
                generalDAO.closeConnection();
                return false;
            }
        }

        public bool checkConfirmed(string email)
        {
            generalDAO.openConection();
            if (userDAO.checkConfirmed(email))
            {
                generalDAO.closeConnection();
                return true;
            }
            else
            {
                generalDAO.closeConnection();
                return false;
            }
        }

        public bool checkUserRegistered(string email)
        {
            generalDAO.openConection();
            if (userDAO.checkExistingUser(email)) // true if user exists
            {
                generalDAO.closeConnection();
                return true;
            }
            else
            {
                generalDAO.closeConnection();
                return false;
            }
        }

        public void changeUserCodePass(string email, int codpass)
        {
            generalDAO.openConection();
            userDAO.setUserPassCodeChanged(email, codpass);
            generalDAO.closeConnection();
        }

        public void changeUserPassword(string email, string newPassword)
        {
            if (checkUserRegistered(email))
            {
                generalDAO.openConection();
                userDAO.changeUserPassword(email, newPassword);
                generalDAO.closeConnection();
            }
            else
            {
                throw new Exception("Malicious entry on change password! Contact Administrator!");
            }
        }

        public bool checkUserCodePass(string email, int codpass)
        {
            generalDAO.openConection();
            if (userDAO.checkUserCodePass(email, codpass))
            {
                generalDAO.closeConnection();
                return true;
            }
            else
            {
                generalDAO.closeConnection();
                return false;
            }
        }

        public bool checkUserPassword(string email, string newPassword)
        {
            generalDAO.openConection();
            if (userDAO.checkUserPassword(email, newPassword))
            {
                generalDAO.closeConnection();
                return true;
            }
            else
            {
                generalDAO.closeConnection();
                return false;
            }
        }

        public User getUser(string email, string password)
        {

            generalDAO.openConection();
            password = Md5(password);
            User user = userDAO.getUser(email, password);
            generalDAO.closeConnection();

            if ((user is null) || (user.getEmail() is null) || (user.getName() is null) || (user.getSurname() is null) || (user.getRole() is null))
            {
                throw new Exception("Database Error - Entity Miss match!");
            }
            else
            {
                return user;
            }
        }

        public bool registerTarea(TareaGenerica tg)
        {
            generalDAO.openConection();
            if (!userDAO.checkExistingTarea(tg))
            {
                // Register Tarea
                userDAO.registerTarea(tg);
                generalDAO.closeConnection();
                return true;
            }
            else
            {
                generalDAO.closeConnection();
                return false;
            }
        }

        public DataSet getSubjects(string alumno)
        {
            generalDAO.openConection();
            DataSet ds = userDAO.getSubjects(alumno);
            generalDAO.closeConnection();
            return ds;
        }

        public DataSet getTareasGenericas(string alumno)
        {
            generalDAO.openConection();
            DataSet ds = userDAO.getTareasGenericas(alumno);
            generalDAO.closeConnection();
            return ds;
        }

        public DataTable getTareasGenericas(string alumno, string codAsig)
        {
            generalDAO.openConection();
            DataTable dt = userDAO.getTareasGenericas(alumno, codAsig);
            generalDAO.closeConnection();
            return dt;
        }

        public DataTable getTareasGenericasAllStudents(string codAsig)
        {
            generalDAO.openConection();
            DataTable dt = userDAO.getTareasGenericasAllStudents(codAsig);
            generalDAO.closeConnection();
            return dt;
        }

        public int getSubjectAvgHoursStudent(string codAsig)
        {
            generalDAO.openConection();
            int avgH = userDAO.getSubjectAvgHoursStudent(codAsig);
            generalDAO.closeConnection();
            return avgH;
        }

        public (DataTable, SqlDataAdapter) getTareasEstudiante(string email)
        {
            generalDAO.openConection();
            (DataTable dt, SqlDataAdapter da) = userDAO.getTareasEstudiante(email);
            generalDAO.closeConnection();
            return (dt, da);
        }

        public bool updateTareasEstudiante(string email, string codTarea, string he, int hrc)
        {
            generalDAO.openConection();
            if (!userDAO.checkExistingTareaEstudiante(codTarea))
            {
                // Register Tarea
                userDAO.updateTareasEstudiante(email, codTarea, he, hrc);
                generalDAO.closeConnection();
                return true;
            }
            else
            {
                generalDAO.closeConnection();
                return false;
            }
        }

        public (DataTable, SqlDataAdapter) getTareasGenericas()
        {

            generalDAO.openConection();
            (DataTable dt, SqlDataAdapter da) = userDAO.getTareasGenericas();
            generalDAO.closeConnection();
            return (dt, da);
        }

        public DataTable getTareasGenericasAsig()
        {
            generalDAO.openConection();
            DataTable dt = userDAO.getTareasGenericasAsig();
            generalDAO.closeConnection();
            return dt;
        }

        public DataTable getTareasGenericasAsig(string codAsig)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Funcion hash obtenida de, https://developpaper.com/encryption-algorithms-md5-sha1/
        /// </summary>
        /// <param name="str"> String to be hashed</param>
        /// <returns>Returns the hashed string</returns>
        public string Md5(string str)
        {
            var buffer = Encoding.UTF8.GetBytes(str);

            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] bytes = md5.ComputeHash(buffer);

            var sb = new StringBuilder();
            foreach (var t in bytes)
            {
                sb.Append(t.ToString("X2"));
            }
            return sb.ToString().ToLower();
        }
    }
}
