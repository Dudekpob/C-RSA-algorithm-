using System;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;

namespace RSA
{
    class Program
    {
     
        //----------------------------------------------
        static int NWD_Calculation(int x, int y)      
        {
            int temp; //
            while (y != 0)              
            {                          
                temp = y;               
                y = x % y;            
                x = temp;              
            };                         
            return x;                  
        }


      static int MOD_ReverseCalculation(int a, int n)
        {
            int a0, n0, p0, p1, q, r, t;   

            p0 = 0; p1 = 1; a0 = a; n0 = n; 
            q = n0 / a0;            
            r = n0 % a0;     
            while (r > 0)                            
            {                                        
                t = p0 - q * p1;                    
                if (t >= 0)                          
                    t = t % n;                        
                else                                 
                    t = n - ((-t) % n);               //     
                p0 = p1; p1 = t;                      //     
                n0 = a0; a0 = r;                      //    
                q = n0 / a0;                          //     
                r = n0 % a0;                          //    
            }
            return p1; //
        }

        //
        static void RSA_Keys()             
        {
            int[] tp = { 11, 13, 17, 19, 23, 29, 31, 37, 41, 43 }; //
            int p, q, phi, n, e, d;                              //
            Random rand = new Random();                         //

            do           //Pętla do while
            {
              
                p = tp[rand.Next(10) % 10];      //
                q = tp[rand.Next(10) % 10];      //
            } while (p == q); //jeśli p = q to 

            phi = (p - 1) * (q - 1);               //  Ø = ( p - 1 ) × ( q - 1 )
            n = p * q;                             // 

            // wyznaczamy wykładniki e i d

            for (e = 3; NWD_Calculation(e, phi) != 1; e += 2) ;  //
            d = MOD_ReverseCalculation(e, phi);                  //

            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Data\jawny.txt"); 
            string files = File.ReadAllText(path);  
            int[] arr = new int[files.Length];      
            for (int i = 0; i < arr.Length; i++)    
            {
                arr[i] = files[i];       
            }

            Horner_MOD(arr, d, e, n); 
        }

      static int Horner_MOD(int[] arr, int a, int w, int n) 
        {
            int pot, q; 
            int wyn;
            wyn = 1;                                                                                
            for (int i = 0; i < arr.Length; i++)                                                     
            {                                      
                pot = arr[i];                                                                    
                wyn = 1;                    
                for (q = w; q > 0; q /= 2)                                                          
                {                                                                                    
                    if (Convert.ToBoolean(q % 2)) wyn = (wyn * pot) % n;     //                      
                    pot = (pot * pot) % n;
                }                                                                               
                arr[i] = wyn;              
            }                                                                                      


            string pathszyfrogram = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Data\szyfrogram.txt"); 
   
            File.WriteAllLines(pathszyfrogram,               
            Array.ConvertAll(arr, x => x.ToString()));        
           
            BigInteger[] odszyfrowany = new BigInteger[arr.Length];
         
            for (int i = 0; i < arr.Length; i++)
            {
             
                odszyfrowany[i] = BigInteger.Pow(arr[i], a) % n;                
            }

            string odszyfrowane = "";
            for (int i = 0; i < odszyfrowany.Length; i++)                       
            {
                odszyfrowane += Convert.ToChar((int)odszyfrowany[i]);        
            }                                                                  

            string pathodszyfrowany = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Data\odszyfrowany.txt"); 
            string fileodszyfrowany = File.ReadAllText(pathszyfrogram);  

            File.WriteAllText(pathodszyfrowany, odszyfrowane);
            return 1;
        }




         static int Main(string[] args)                    
        {                                                 
            RSA_Keys();        
            return 0;                                     
        }
    }
}
