﻿using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace DataAccessLayer
{
    public interface IUserDAO
    {
        void registerUser(User user);

        bool checkExistingUser(String email);

        bool checkUserToConfirm(String email, int numconf);

        void confrimUser(String email);

        bool checkConfirmed(String email);

        bool checkLogin(String email, String password);

        void setUserPassCodeChanged(String email, int codPass);

        bool checkUserCodePass(String email, int codPass);

        void changeUserPassword(String email, String newPassword);

        bool checkUserPassword(String email, String newPassword);

        User getUser(String email, String password);


        bool checkExistingTarea(TareaGenerica tarea);

        void registerTarea(TareaGenerica tarea);

        DataSet getSubjects(String alumno);

        (DataTable, SqlDataAdapter) getTareasGenericas();

        DataTable getTareasGenericasAsig();

        DataSet getTareasGenericas(string alumno);

        DataTable getTareasGenericas(string alumno, string codAsig);

        (DataTable, SqlDataAdapter) getTareasEstudiante(string alumno);

        void updateTareasEstudiante(string email, string codTarea, string he, int hrc);

        bool checkExistingTareaEstudiante(string codTarea);

        DataTable getTareasGenericasAsig(String codAsig);

    }
}