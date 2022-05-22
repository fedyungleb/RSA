using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace RSA
{
    internal class Program
    {
        private static List<int> lim = new List<int>();
        public static bool IsPrime(int n)
        {
            for (int i=2; i < Math.Sqrt(n); i++)
                if (n %i == 0) 
                    return false;
            return true;
        }
        
        public static int GCD(int e,int F)
        {
            if (e == 0)
                return F;
         
            return GCD(F % e, e);
        }

        public static int GCDExtended(int a, int b, int x, int y)
        {
            if (a == 0)
            {
                x = 0;
                y = 1;
                return b;
            }
 
            int x1 = 1, y1 = 1;
            int gcd = GCDExtended(b % a, a, x1, y1);
 
            x = y1 - (b / a) * x1;
            y = x1;
 
            return gcd;
        }

        public static int FindE(int e,int F)
        {
            while (GCD(e,F)!=1)
            {
                e++;
            }

            return e;
        }

        public static int FindD(int d,int e,int F)
        {
            while (d * e % F != 1)
            {
                d++;
            }
            
            return d;
        }

        public static string Encryption(string plainText,int e,int n)
        {
            byte[] src = Encoding.ASCII.GetBytes(plainText);
            byte[] C = new byte[src.Length];

            for (int i = 0; i < src.Length; i++)
            {
                lim.Add((int) src[i] / n);
                C[i]= (byte)(Math.Pow(src[i]-lim[i]*n, e) % n);
            }
            
            return Encoding.ASCII.GetString(C);
        }

        public static string Decryption(int n,int e,string plainText)
        {
            byte[] src = Encoding.ASCII.GetBytes(plainText);
            byte[] C = new byte[src.Length];
            
            int p = 2;

            while (true)
            {
                if(IsPrime(p) && n%p == 0)
                    break;
                p++;
            }

            int q = n / p;
            Console.WriteLine($"p = {p} q = {q}");
            
            int F = (p - 1) * (q - 1);
            
            int d = FindD(2,e, F);
            Console.WriteLine($"d = {d}");
            Console.WriteLine($"e = {e}");
            
            for (int i = 0; i < src.Length; i++)
            {
                C[i] = (byte) (Math.Pow(src[i], d) % n);
                C[i] += (byte) (lim[i] * n);

            }

            return Encoding.ASCII.GetString(C);
        }

        public static void WriteToFile(string plaunText,int n,int e)
        {
            string[] lines =
            {
                $"{plaunText}", $"{n}", $"{e}" 
            };
            File.WriteAllLines("encrypted.txt", lines);
        }

        public static void Main(string[] args)
        {
            Console.WriteLine("Enter plaint text:");
            string plainText = Console.ReadLine();
            
            Console.WriteLine("Enter p:");
            int p = Convert.ToInt32(Console.ReadLine());
            
            Console.WriteLine("Enter q:");
            int q = Convert.ToInt32(Console.ReadLine());

            int n = p * q;

            int F = (p - 1) * (q - 1);

            int e = 2;
            e = FindE(e, F);
            

            string encrypted = Encryption(plainText, e, n);
            Console.WriteLine($"original text: {plainText}");
            Console.WriteLine($"encrypted text01: {encrypted}");
            
            
            WriteToFile(encrypted,n,e);
            
            using (StreamReader readText = new StreamReader("encrypted.txt"))
            {
                encrypted = readText.ReadLine();
                n = Convert.ToInt32(readText.ReadLine());
                e = Convert.ToInt32(readText.ReadLine());
            }
            Console.WriteLine($"encrypted text02: {encrypted}");
            String decrypted = Decryption(n, e, encrypted);
            Console.WriteLine($"decrypted text: {decrypted}");

        }
    }
}