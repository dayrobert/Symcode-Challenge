/**
 * Code Challenge Symcode
 * 
 * Author: Robert Day <day.c.robert@gmail.com>(p: 617 967 5901)
 * Challenge site: http://yetanotherwhatever.io/tp/60F66F5C-3357-4AA8-B0BF-04FD2DECD8E1.html * 
 * 
 */

using System;
using System.IO;
using System.Runtime.InteropServices.ComTypes;

namespace symantec.challenge.symcode
{
    /// <summary>
    /// A code challenge program for Symantec
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // offer a little, and I mean little, help
                if (args.Length == 0)
                {
                    Console.WriteLine("Application only allows one parameter, the name of the file to encode.");
                    return;
                }

                // do the work
                using (TextReader reader = File.OpenText(args[0]))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        Console.WriteLine(Symcode.EncodeLine(line));
                    }
                }
            }
            catch (Exception x)
            {
                Console.WriteLine("Error occurred: " + x.Message);
            }
        }

        /// <summary>
        /// Function to run sample files provided by problem definition
        /// </summary>
        static void UnitTest()
        {
            using (TextWriter writer = File.CreateText("result.txt"))
            {
                using (TextReader reader = File.OpenText("sample.txt"))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string output = Symcode.EncodeLine(line);
                        writer.WriteLine(output);
                        Console.WriteLine(output);
                    }
                }
            }

            Console.ReadKey();
        }
    }
 }
