using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public class Database
    {
        public SqlConnection Connection { get; set; }
        public Database(string nomeDB, string nomeServer = "DESKTOP-EP1ODC7")
        {
            Connection = new SqlConnection(
                                           $"Data Source = {nomeServer}; " +
                                           $"Initial Catalog = {nomeDB}; " +
                                           $"Integrated Security = True;"
                                          );
        }
        public bool Update(string query)
        {
            try
            {
                Connection.Open();

                SqlCommand cmd = new SqlCommand(query, Connection);

                int affette = cmd.ExecuteNonQuery();

                return affette > 0;
            }
            catch (Exception e)
            {
                // Stampo in console il messaggio di errore "originale"
                Console.WriteLine(e.Message);
                Console.WriteLine($"ERRORE nella QUERY: \n{query}");

                return false;
            }
            finally
            {
                Connection.Close();
            }
        }
        /*
         * Metodo Update 2 verifica che avvengano piU di una modifica contemporanemente nel database
         */
        public bool Update2(string query)
        {
            try
            {
                Connection.Open();

                SqlCommand cmd = new SqlCommand(query, Connection);

                int affette = cmd.ExecuteNonQuery();

                return affette > 1;
            }
            catch (Exception e)
            {
                // Stampo in console il messaggio di errore "originale"
                Console.WriteLine(e.Message);
                Console.WriteLine($"ERRORE nella QUERY: \n{query}");

                return false;
            }
            finally
            {
                Connection.Close();
            }
        }
        public List<Dictionary<string, string>> Read(string query)
        {
            List<Dictionary<string, string>> ris = new List<Dictionary<string, string>>();

            try
            {
                Connection.Open(); // Apertura connessione

                SqlCommand cmd = new SqlCommand(query, Connection);

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Dictionary<string, string> riga = new Dictionary<string, string>();

                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        riga.Add(dr.GetName(i).ToLower(), dr.GetValue(i).ToString());
                    }

                    ris.Add(riga);
                }

                dr.Close(); // Chiusura data reader
            }
            catch (Exception e)
            {
                // Gestisci eventuali eccezioni
                Console.WriteLine(e.Message);
                Console.WriteLine($"ERRORE nella QUERY: \n{query}");
            }
            finally
            {
                if (Connection.State != System.Data.ConnectionState.Closed)
                {
                    Connection.Close(); // Chiusura connessione nel blocco finally
                }
            }

            return ris;
        }
        public Dictionary<string, string> ReadOne(string query)
        {
            try
            {
                return Read(query)[0];
            }
            catch
            {
                return null;
            }
        }
        public static string CambiaApostrofi(string check)
        {
            string ris = check;
            if (ris is not null)
            {
                if (ris.Contains("'"))
                    ris = ris.Replace("'", "''");
                return ris;
            }
            return ris;
        }
        public static string CambiaVirgole(double prezzo)
        {
            string ris = $"{prezzo}";
            if (ris.Contains(","))
                ris = ris.Replace(",", ".");
            return ris;
        }
        public static string CambiaPunti(string prezzo)
        {
            string ris = $"{prezzo}";
            if (ris.Contains("."))
                ris = ris.Replace(".", ",");
            return ris;
        }
    }
}

