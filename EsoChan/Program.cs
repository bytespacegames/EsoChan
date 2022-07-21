using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace EsoChan
{
    class Program
    {
        static int currentLine = 1;
        static List<string> lines = new List<string>();
        static string filename = "code.chan";

        static bool frozen = true;
        static string modifyingVariable = "none";

        static Dictionary<string, int> variables = new Dictionary<string, int>();

        static void Main(string[] args)
        {
            TextReader tr = new StreamReader(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\" + @filename);

            string reader = tr.ReadLine();
            while (reader != null)
            {
                lines.Add(reader);

                reader = tr.ReadLine();
            }

            while (currentLine <= lines.Count)
            {
                string line = lines[currentLine - 1];
                string[] inputs = line.ToLower().Split(" ");

                if (inputs[0] == ">")
                {
                    if (frozen)
                    {
                        if (inputs[1] == "be" && inputs[2] == "program")
                        {
                            frozen = false;
                        }
                    } else
                    {
                        switch (inputs[1])
                        {
                            case "view":
                                modifyingVariable = inputs[2];
                                break;
                            case "freeze":
                                if (inputs[2] == "program")
                                {
                                    frozen = true;
                                }
                                break;
                            case "set":
                                int value;
                                try
                                {
                                    value = int.Parse(inputs[4]);
                                } catch (FormatException)
                                {
                                    value = variables[inputs[4]];
                                }
                                if (variables.ContainsKey(modifyingVariable))
                                {
                                    variables[modifyingVariable] = value;
                                } else
                                {
                                    variables.Add(modifyingVariable, value);
                                }
                                break;
                            case "print":
                                switch (inputs[2])
                                {
                                    case "string":
                                        int i = line.IndexOf("print string ") + 13;
                                        string input = line.Substring(i);
                                        Console.Write(input);
                                        break;
                                    case "character":
                                        Console.Write(Encoding.ASCII.GetString(new byte[] { (byte) variables[modifyingVariable] }));
                                        break;
                                    case "linebreak":
                                        Console.Write("\n");
                                        break;
                                }
                                break;
                            case "ask":
                                int charInt = (int) Console.ReadKey().KeyChar;
                                if (variables.ContainsKey(modifyingVariable))
                                {
                                    variables[modifyingVariable] = charInt;
                                }
                                else
                                {
                                    variables.Add(modifyingVariable, charInt);
                                }
                                break;
                            case "go":
                                switch (inputs[2])
                                {
                                    case "up":
                                        int amnt = 1;
                                        if (inputs[3] != null)
                                        {
                                            amnt = int.Parse(inputs[3]);
                                        }
                                        variables[modifyingVariable] += amnt;
                                        break;
                                    case "down":
                                        int amntd = 1;
                                        if (inputs[3] != null)
                                        {
                                            amntd = int.Parse(inputs[3]);
                                        }
                                        variables[modifyingVariable] -= amntd;
                                        break;
                                    case "dark":
                                        variables[modifyingVariable] = 0;
                                        break;
                                }
                                break;
                            case "goto":
                                int lValue;
                                try
                                {
                                    lValue = int.Parse(inputs[2]);
                                }
                                catch (FormatException)
                                {
                                    lValue = variables[inputs[2]];
                                }
                                currentLine = lValue - 1;
                                break;
                            case "if":
                                int ifint = 1;
                                if (inputs[2] != null)
                                {
                                    ifint = int.Parse(inputs[2]);
                                }
                                if (variables[modifyingVariable] != ifint)
                                {
                                    currentLine += 1;
                                }
                                break;
                            case "ifnot":
                                int ifntint = 1;
                                if (inputs[2] != null)
                                {
                                    ifntint = int.Parse(inputs[2]);
                                }
                                if (variables[modifyingVariable] == ifntint)
                                {
                                    currentLine += 1;
                                }
                                break;

                        }
                    }

                }
                currentLine += 1;
            }
            Console.Write("\n\nProgram ended.");
            while (true) { }
        }
    }
}
