using HospiSafe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospiSafe.Services
{
    //Creamos un diccionario que recoge los roles y un set de strings con los modulos a los que tiene permisos
    public static class PermisosService
    {
        private static readonly Dictionary<RolUsuario, HashSet<string>> PermisosRoles = new()
        {
            // Administrador: con acceso a todo
            {
                RolUsuario.Admin, new HashSet<string>
                {
                    "Pacientes",
                    "Citas",
                    "Laboratorio",
                    "Radiologia",
                    "Usuarios"
                }
            },

            // Personal
            {
                RolUsuario.Personal, new HashSet<string>
                {
                    "Pacientes",
                    "Citas",
                    "Laboratorio",
                    "Radiologia",
                    "Usuarios"
                }
            },

            // T.laboratorio: solo laboratorio
            {
                RolUsuario.TecnicoLaboratorio, new HashSet<string>
                {
                    "Laboratorio"
                }
            },

            // t.Rayos: solo radiología
            {
                RolUsuario.TecnicoRayos, new HashSet<string>
                {
                    "Radiologia"
                }
            },

            // paciente: sin acceso
            {
                RolUsuario.Paciente, new HashSet<string>()
            }
        };

        // permisos de acceso, modulos es var de salida
        public static bool PuedeAccederA(Usuario? usuario, string modulo)
        {
            if (usuario == null)
                return false;

            if (!PermisosRoles.TryGetValue(usuario.Rol, out var modulos))
                return false;

            return modulos.Contains(modulo); //si contiene el modulo devuelve true
        }
    }
}
