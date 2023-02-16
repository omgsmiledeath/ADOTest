using System.Data;
using System.Data.SQLite;
namespace ADOTest;
class Program
{
    static void Main(string[] args)
    {
        try
        {
            SQLiteConnection con = new SQLiteConnection(@"Data Source = /Users/kseniabelaevskaa/Projects/Bases/AdoTest.db");
            con.Open();

           using (SQLiteCommand sqlcom = new SQLiteCommand(con))
           {
               sqlcom.CommandText = @"INSERT INTO Workers (firstName,lastName,phoneNumber) VALUES ('раз','dva','8800553535')";
               sqlcom.ExecuteNonQuery();
           }
            var da = new SQLiteDataAdapter(@"SELECT * FROM Workers", con);
            var dt = new DataTable();
            da.Fill(dt);
            
            SQLiteCommand delc = new SQLiteCommand(@"DELETE FROM Workers where oid ='3'",con);

            delc.ExecuteNonQuery();
            da.Update(dt);
            PrintTable(dt);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        finally
        {
            Console.WriteLine("DOne");
        }
    }
    public static void PrintTable(DataTable dt)
    {
        foreach (DataColumn column in dt.Columns)
            Console.Write($"{column.ColumnName}\t     ");
        Console.WriteLine();

        foreach (DataRow row in dt.Rows)
        {

            var cells = row.ItemArray;
            foreach (object cell in cells)
                Console.Write($"{cell}\t\t");
            Console.WriteLine();
        }
    }
}

