using HospiSafe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospiSafe.Services
{
    //Singleton con el usuario de la sesion para manejo de su rol y accesos
    public static class SessionManager
    {
        private static Usuario? _currentUser; //usuario actual

        public static event Action? CurrentUserChanged;

        public static Usuario? CurrentUser
        {
            get
            {
                return _currentUser;
            }

            set
            {
                if (_currentUser != value)
                {
                    _currentUser = value;
                    CurrentUserChanged?.Invoke();
                }
            }
        }
    }
}
