using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace JDBC
{
    class Program
    {
        public class WC
        {
            private string snovel;    // 文件名
            private string[] sParameter; // 参数数组
            private int iCharcount;      // 字符数
            private int iWordcount;      // 单词数
            private int iLinecount;      // 总行数
            public WC()
            {
                this.iCharcount = 0;
                this.iWordcount = 0;
                this.iLinecount = 0;
            }
            public void Operator(string[] sParameter, string snovel)
            {
                this.sParameter = sParameter;
                this.snovel = snovel;

                foreach (string s in sParameter)
                {
                    if (s == "-s")
                    {
                        try
                        {
                            string[] arrPaths = snovel.Split('\\');
                            int pathsLength = arrPaths.Length;
                            string path = "";

                            // 获取输入路径
                            for (int i = 0; i < pathsLength - 1; i++)
                            {
                                arrPaths[i] = arrPaths[i] + '\\';

                                path += arrPaths[i];
                            }

                            // 获取通配符
                            string novel = arrPaths[pathsLength - 1];

                            //  获取符合条件的文件名
                            string[] files = Directory.GetFiles(path, novel);

                            foreach (string vel in files)
                            {
                                Console.WriteLine("文件名：{0}", vel);
                                //SuperCount(file);
                                BaseCount(vel);
                                Display();
                            }
                            break;
                        }
                        catch (IOException ex)
                        {
                            Console.WriteLine(ex.Message);
                            return;
                        }
                    }
                    if (s == "-c" || s == "-w" || s == "-l")
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("参数 {0} 不存在", s);
                        break;
                    }
                }
            }
            private void BaseCount(string novel)
            {
                try
                { // 打开文件
                    FileStream vel = new FileStream(novel, FileMode.Open, FileAccess.Read, FileShare.Read);
                    StreamReader sr = new StreamReader(vel);
                    int nChar;
                    int charcount = 0;
                    int wordcount = 0;
                    int linecount = 0;
                    //定义一个字符数组
                    char[] symbol = { ' ', '\t', ',', '.', '?', '!', ':', ';', '\'', '\"', '\n', '{', '}', '(', ')', '+', '-', '*', '=' };
                    while ((nChar = sr.Read()) != -1)
                    {
                        charcount++;     // 统计字符数
                        foreach (char c in symbol)
                        {
                            if (nChar == (int)c)
                            {
                                wordcount++; // 统计单词数
                            }
                        }
                        if (nChar == '\n')
                        {
                            linecount++; // 统计行数
                        }
                    }
                    iCharcount = charcount;
                    iWordcount = wordcount + 1;
                    iLinecount = linecount + 1;
                    sr.Close();
                }
                catch (IOException ex)
                {
                    Console.WriteLine(ex.Message);
                    return;
                }
            }
            private void Display()
            {
                foreach (string s in sParameter)
                {
                    if (s == "-c")
                    {
                        Console.WriteLine("字 符 数：{0}", iCharcount);
                    }
                    else if (s == "-w")
                    {
                        Console.WriteLine("单 词 数：{0}", iWordcount);
                    }
                    else if (s == "-l")
                    {
                        Console.WriteLine("总 行 数：{0}", iLinecount);
                    }
                }
                Console.WriteLine();
            }
        }
        [STAThread]
        static void Main(string[] args)
        {
            string message = ""; // 存储用户命令
            while (message != "exit")
            {
                Console.Write("wc.exe ");
                message = Console.ReadLine();               // 得到输入命令
                string[] arrMessSplit = message.Split(' '); // 分割命令
                int iMessLength = arrMessSplit.Length;
                string[] sParameter = new string[iMessLength - 1];// 获取命令参数数组
                for (int i = 0; i < iMessLength - 1; i++)
                {
                    sParameter[i] = arrMessSplit[i];
                }             // 获取文件名
                string snovel = arrMessSplit[iMessLength - 1];
            }
        }
    }
}
