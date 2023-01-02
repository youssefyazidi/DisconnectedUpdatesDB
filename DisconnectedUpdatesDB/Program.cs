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

            //appel de addLivre
            addLivre("L05", "Developper en Java", "Ahmed", 5);

            //appel de update
            updateLivre("L02", "Physique", "Saida", 5);

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


        static void addLivre
            (string code, 
            string titre, 
            string auteur, 
            int exemplaires)
        {
            //Ajouter le livre au DataSet

            DataRow newRow = ds.Tables["Livre"].NewRow();
            newRow["CodeL"] = code;
            newRow["Titre"] = titre;
            newRow["Auteur"] = auteur;
            newRow["NbExemplaires"] = exemplaires;

            ds.Tables["Livre"].Rows.Add(newRow);

            Console.WriteLine(" New Row Inserted in ds");

        }

        static void updateLivre(string code,
            string titre,
            string auteur,
            int exemplaires)
        {
            //Mise à jour dans le DS local

            //rechercher le code
            DataRow[] rows=ds.Tables["Livre"].Select("CodeL='" + code + "'");

            rows[0]["Titre"] = titre;
            rows[0]["Auteur"] = auteur;
            rows[0]["NbExemplaires"] = exemplaires;

            Console.WriteLine(" Update Row in ds");
        }
    }
}
