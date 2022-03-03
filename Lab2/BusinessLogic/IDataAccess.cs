﻿using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    interface IDataAccess
    {
        bool registerUser(String email, String password, String name, String surname, String role, int numConfirmation);

        bool confirmUser(String email, int id);

        bool checkConfirmed(String email);

        bool login(String email, String password);

        bool checkUserRegistered(String email);

        void changeUserCodePass(String email, int codpass);

        void changeUserPassword(String email, String newPassword);

        bool checkUserCodePass(String email, int codpass);

        bool checkUserPassword(String email, String newPassword);

        User getUser(String email, String password);

    }
}
