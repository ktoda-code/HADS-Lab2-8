﻿using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RegistroUsuariosWeb
{
    public partial class Inicio1 : System.Web.UI.Page
    {
        private BusinessLogicLayer.BusinessLogic bll;
        // Check if in database and confirmed
        // true -> goto corresponding page
        // false -> show error message

        protected void Page_Load(object sender, EventArgs e)
        {
            bll = new BusinessLogicLayer.BusinessLogic();
            divSend.Visible = false;
        }

        protected void LoginButton_Click(object sender, EventArgs e)
        {
            String email = EmailTB.Text;
            String password = PasswordTB.Text;

            if (bll.checkConfirmed(email))
            {
                // login // check type of user
                if (bll.login(email, password))
                {
                    User user = bll.getUser(email, password); // Session add
                    if (user.getRole().Equals("Alumno"))
                    {
                        Response.Redirect("~/Alumnos/Estudiante.aspx");
                    }
                    else if (user.getRole().Equals("Profesor"))
                    {
                        Response.Redirect("~/Profesorado/Profesor.aspx");
                    }
                    // Testing purposes (Demo)
                    //LoginButton.PostBackUrl = "~/Home.aspx"; // ?
                    divSend.Visible = false; // redirect to home
                }
                else
                {
                    divSend.Visible = true;
                }

            }
            else
            {
                divSend.Visible = true;
            }
        }

        protected void RegistroLB_Click(object sender, EventArgs e)
        {

        }

        protected void CambiarPassLB_Click(object sender, EventArgs e)
        {

        }
    }
}