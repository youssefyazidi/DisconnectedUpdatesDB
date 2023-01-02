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
        static SqlConnection con;
        static DataSet ds = new DataSet("Biblio");
        static SqlDataAdapter livreAdapter = new SqlDataAdapter();

        static void Main(string[] args)
        {
            InitialiseDS();

            //appel de addLivre
            addLivre("L06", "Developper en HTML", "Ahmed", 7);

            //appel de update
           updateLivre("L02", "Physique", "Saida", 5);

            //appel de delete
           deleteLivre("L04");
            Console.WriteLine("avant acceptation");
            displayLivres();

            //validation des changement dans le Dataset
            //les opérations effectuées dans le DS sont des opérations temporaires
            //Vous pouvez les confirmer(AcceptChanges) ou les annuler(RejectChanges)
            //cancel
            //ds.RejectChanges();

            //Mise à jour de la base de données d'origine-avant acceptation

            //Il nous faut obligatoire les requetes LMD
            //2 manières de définir les requete LMD
            //- manuellement ou automatiquement(- adapter par table)
            //Creation manuelle
            CreateInsert();

            //Creation auto - donne les 3 requetes LMD via un type 
            // SqlCommandBuilder

            SqlCommandBuilder builder = new SqlCommandBuilder(livreAdapter);
            /*Console.WriteLine(builder.GetInsertCommand().CommandText);
            Console.WriteLine(builder.GetDeleteCommand().CommandText);*/

            //Envoyer les mise à jours vers la bd
            livreAdapter.Update(ds.Tables["Livre"]);
            //Confirmation
            //doit etre faite aprs les MSJ Database
            ds.AcceptChanges();
            Console.WriteLine("apres MSJ-Acceptation");
            displayLivres();

           
            Console.ReadKey();

        }

        static void InitialiseDS()
        {
            con = 
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
                if (row.RowState != DataRowState.Deleted)
                {
                    Console.Write(row.RowState + " : ");
                    Console.WriteLine("Code = {0}, Titre={1}, Auteur={2}, Exemplaires={3}",
                        row["CodeL"],
                        row["Titre"],
                        row["Auteur"],
                        row["NbExemplaires"]
                        );
                }
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

        static void deleteLivre(string code)
        {
            //Rechercher de la ligne
            DataRow[] rows = 
                ds.Tables["Livre"].Select("CodeL='" + code + "'");
            //Appel de la fonction Delete() pour supprimer
            //marquer la ligne comme ligne a supprimer
            rows[0].Delete();
        }

        static void CreateInsert()
        {
            //Demo de la creation manuelle
            //la requete parametrée
            SqlCommand cmd = 
                new SqlCommand
                ("INSERT INTO LIVRE VALUES(@codel,@titre,@auteur,@nbexemplaire)");
            cmd.Connection = con;

            SqlParameter param =
                new SqlParameter("codel", SqlDbType.VarChar, 10, "CodeL");
            cmd.Parameters.Add(param);
            param =
                new SqlParameter("titre", SqlDbType.VarChar, 10, "Titre");
            cmd.Parameters.Add(param);
            param =
                new SqlParameter("auteur", SqlDbType.VarChar, 10, "Auteur");
            cmd.Parameters.Add(param);
            param =
                new SqlParameter("nbexemplaire", SqlDbType.VarChar, 10, "NbExemplaires");
            cmd.Parameters.Add(param);

            livreAdapter.InsertCommand = cmd;

        }
    }
}
