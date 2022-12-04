namespace PWebApi.Models
{
    public class ConexionBD
    {
        private static ConexionBD instance = null;
        public string conn = "";

        protected ConexionBD()
        {
            conn = "User Id=ADMIN; Password=Sebastian.silva123; Data Source=ex_high";
        }

        public static ConexionBD Instance
        {
            get
            {
                if (instance == null)
                    instance = new ConexionBD();
                return instance;
            }
        }
    }
}
