using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisconnectedUpdatesDB
{
    class Program
    {
        static DataSet ds = new DataSet("Biblio");
        static SqlDataAdapter livreAdapter = new SqlDataAdapter();

        static void Main(string[] args)
        {
            InitialiseDS();

            displayLivres();

            Console.ReadKey();

        }

        static void InitialiseDS()
        {
            SqlConnection con = 
                new 
                SqlConnection(
     @"Data Source=.\SQLEXPRESS;Initial Catalog=Biblio;Integrated Security=true");
            SqlCommand cmd = new SqlCommand("SELECT * FROM Livre");
            cmd.Connection = con;
            livreAdapter.SelectCommand = cmd;
            livreAdapter.Fill(ds, "Livre");
        }
        static void displayLivres()
        {
            foreach (DataRow row in ds.Tables["Livre"].Rows)
            {
                Console.WriteLine("Code = {0}, Titre={1}, Auteur={2}, Exemplaires={3}",
                    row["CodeL"],
                    row["Titre"],
                    row["Auteur"],
                    row["NbExemplaires"]
                    );
            }
        }


        static void addLivre(string code, string titre, string auteur, int exemplaires)
        {
            //Ajouter le livre au DataSet

        }
    }
}
