namespace ProyectoFinal.Models
{
    public class Paciente
    {
        public int idPaciente { get; set; }
        public string Nombres { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public int Dni { get; set; }
        public int Telefono { get; set; }
        public string Direccion { get; set; }
        public DateOnly FechaNacimiento { get; set; }
        public string email { get; set; }
        public string contraseña { get; set; }
        public bool estado { get; set; }
    }
}
