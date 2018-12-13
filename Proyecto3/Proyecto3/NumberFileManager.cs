using System;
using System.Collections;
using System.IO;
namespace Proyecto3
{
    public class NumberFileManager
    {

        /** 
         * Lee de un fichero los valores de los números que usaremos.
         */
        public ArrayList ReadFromFile(String path)
        {
            String aux;
            ArrayList arr = new ArrayList();

            try 
            {
                StreamReader sr = new StreamReader(path);
                while ((aux = sr.ReadLine()) != null)
                {
                    arr.Add(Int32.Parse(aux));
                }
                sr.Close();
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return arr;
        }

        // Escribe en un fichero de resultados el tiempo de ejecución de un fichero
        public void WriteResult(String path, String technique, double t)
        {
            try
            {
                StreamWriter sw = new StreamWriter("CSHARP_RESULTS.txt",true);
                sw.WriteLine("Test " + path + " " + technique + " Process time: " + t / 1000);
                sw.Close();
            } catch (IOException e) 
            {
                Console.WriteLine(e.Message);
            }
        }

        // Limpia el fichero de resultados
        public void CleanResults() {
            try
            {
                StreamWriter sw = new StreamWriter("CSHARP_RESULTS.txt");
                sw.WriteLine("");
                sw.Close();
            } catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
