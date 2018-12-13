using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Proyecto3
{
    class Program
    {
        private static ArrayList arr;
        private static int[] arrAux;
        private static DateTime Jan1st1970 = new DateTime
                    (1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        private static bool mem = false; // Flag de selección de técnica
        private static bool tab = false; // Flag de selección de técnica
        private static bool fi = false; // Flag de muestra de entrada
        private static bool fr = false; // Flag de muestra de resultados
        private static bool ft = false; // Flag de muestra de tiempo de computación
        private static bool c = false;  // Flag de limpieza de fichero de resultados

        public Program(String path)
        {
            try
            {
                arr = new NumberFileManager().ReadFromFile(path);
            }
            catch (IOException)
            {
                Console.WriteLine("Can't read data from file: " + path);
                arr = new ArrayList();
            }
        }

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("For macOS, first: mcs Program.cs NumberFileManager.cs");
                Console.WriteLine("Syntax: mono Program.exe [-mem | -tab | -fi | -fr | -ft | -c] input_filename");
                Console.WriteLine("(You can input more than one filename at once)");
                Console.WriteLine("-mem: Use only memoization technique");
                Console.WriteLine("-tab: Use only tabulation technique");
                Console.WriteLine("-fi: Show read numbers from file(s)");
                Console.WriteLine("-ft: Show time spent computing the results");
                Console.WriteLine("-c:  Clear the results log file before writing new results");
                return;
            }

            ArrayList paths = new ArrayList();
            String arg;
            for (int i = 0; i < args.Length; i++)
            {
                arg = args[i];
                switch (arg)
                {
                    case "-mem":
                        mem = true;
                        continue;
                    case "-tab":
                        tab = true;
                        continue;
                    case "-fi":
                        fi = true;
                        continue;
                    case "-fr":
                        fr = true;
                        continue;
                    case "-ft":
                        ft = true;
                        continue;
                    case "-c":
                        c = true;
                        continue;
                }
                paths.Add(arg);
            }

            if (c) new NumberFileManager().CleanResults();

            // Procesamos cada uno de los ficheros introducidos por parámetro
            String stringAux;
            for (int i = 0; i < paths.Count; i++)
            {
                stringAux = (String)paths[i];
                if (paths.Count > 1)
                {
                    Console.WriteLine("Reading from file: " + stringAux);
                }

                Program p = new Program(stringAux);
                if (fi) p.ShowInput(); // Mostramos los valores leídos del fichero

                double t = 0;
                arrAux = (int[])arr.ToArray(typeof(int));

                t = CurrentTimeMillis();
                if (!tab)
                {
                    Memoization(arrAux);
                    t = CurrentTimeMillis() - t;
                    new NumberFileManager().WriteResult(stringAux, "Memoization", t);
                    if (ft) Console.WriteLine("Process time:\t" + t / 1000);
                }

                t = CurrentTimeMillis();
                if (!mem)
                {
                    Tabulation(arrAux);
                    t = CurrentTimeMillis() - t;
                    new NumberFileManager().WriteResult(stringAux, "Tabulation", t);
                    if (ft) Console.WriteLine("Process time:\t" + t / 1000);
                }
            }
        }

        public static long CurrentTimeMillis()
        {
            return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
        }

        public void ShowInput()
        {
            Console.WriteLine("*** Input");
            for (int i = 0; i < arr.Count; i++)
            {
                Console.WriteLine(i + " => " + (int)arr[i]);
            }
            Console.WriteLine("-------------------------------");
        }

        private static int Memoization(int[] arr)
        {
            Dictionary<int, int> dic = new Dictionary<int, int>();
            Dictionary<int, int> values = Lis(arr, dic, arr.Length - 1);
            int mx = 1;

            foreach (KeyValuePair<int, int> i in values)
            {
                if (mx < i.Value)
                {
                    mx = i.Value;
                }
            }
            Console.WriteLine("Number of changes required (Memoization): " + (arr.Length - mx));
            return (arr.Length - mx);
        }
          
        private static Dictionary<int,int> Lis(int[] arr, Dictionary<int,int> dp, int n)
        {
            if (n == 0)
            {
                dp[n] = 1;
            } else 
            {
                dp = Lis(arr, dp, n - 1);
                dp[n] = 1;
                for (int i = 0; i < n; i ++) 
                {
                    if (arr[n] > arr[i] && dp[n] < dp[i] + 1)
                    {
                        dp[n] = dp[i] + 1;
                    }
                }
            }
            return dp;
        }


        private static int Tabulation(int[] arr)
        {
            int n = arr.Length;
            int[] lis = new int[n];
            lis[0] = 1;

            for (int i = 1; i < n; i ++)
            {
                lis[i] = 1;
                for (int j = 0; j < i; j ++)
                {
                    if (arr[i] > arr[j] && lis[i] < lis[j] + 1)
                    {
                        lis[i] = lis[j] + 1;
                    }
                }
            }
            int maximum = 0;
            for (int i = 0; i < n; i ++) 
            {
                if (maximum < lis[i]) 
                {
                    maximum = lis[i];
                }
            }
            Console.WriteLine("Number of changes required (Tabulation): " + (n - maximum));
            return (n - maximum);      
        }
    }
}
