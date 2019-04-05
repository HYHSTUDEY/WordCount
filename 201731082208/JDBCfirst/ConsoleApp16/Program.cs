using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace JDBCfirst
{
   
    class WordCount
    {
        static void Main(string[] args)
        {
            String path = Console.ReadLine();//读入文件
            String r, article = null;
            StreamReader reader = new StreamReader(path, Encoding.Default);
            Hashtable ht = new Hashtable(StringComparer.OrdinalIgnoreCase);           
            try
            {
                while ((r = reader.ReadLine()) != null)
                {
                    article += (r + "\n");
                }
                Console.WriteLine(article);
            }
            catch (IOException e)
            {
                Console.WriteLine(e.ToString());
            }
            reader.Close();
            WordCount p = new WordCount();
            p.Lines(article);//统计行数 
            p.Character(article);//统计字符数
            string[] str = p.Words(article);//统计单词总数
            p.Frequency(str, path);//统计单词出现的次数
            p.WordSort(path);//单词频率排序
            Console.ReadKey();
        }
        //单词频率排序
        public void WordSort(string path)
        {
            //String path = Console.ReadLine();
            if (!File.Exists(path))
            {
                Console.WriteLine("文件不存在！");
                return;
            }
            StreamReader reader = new StreamReader(path, Encoding.Default);
            Hashtable ht = new Hashtable(StringComparer.OrdinalIgnoreCase);
            string line = reader.ReadLine();

            string[] wordArr = null;
            int num = 0;
            while (line.Length > 0)
            {
                wordArr = line.Split(' ');
                foreach (string s in wordArr)
                {
                    if (s.Length == 0)
                        continue;
                    //去除标点
                    line = Regex.Replace(line, @"[\p{P}*]", "", RegexOptions.Compiled);
                    //将单词加入哈希表
                    if (ht.ContainsKey(s))
                    {
                        num = Convert.ToInt32(ht[s]) + 1;
                        ht[s] = num;
                    }
                    else
                    {
                        ht.Add(s, 1);
                    }
                }
                line = reader.ReadLine();
            }
            ArrayList keysList = new ArrayList(ht.Keys);
            //对Hashtable中的Keys按字母序排列
            keysList.Sort();
            //按次数进行插入排序【稳定排序】，所以相同次数的单词依旧是字母序
            string tmp = String.Empty;
            int valueTmp = 0;
            for (int i = 1; i < keysList.Count; i++)
            {
                tmp = keysList[i].ToString();
                valueTmp = (int)ht[keysList[i]];//次数
                int j = i;
                while (j > 0 && valueTmp > (int)ht[keysList[j - 1]])
                {
                    keysList[j] = keysList[j - 1];
                    j--;
                }
                keysList[j] = tmp;//j=0
            }
            //打印出来 
            Console.WriteLine("排序结果：\n");
            foreach (object item in keysList)
            {
               
                Console.WriteLine((string)item + ":" + valueTmp);
            }
        }
        //统计字符数
        public void Character(string c)
        {

            int character = 0;
            char[] ch = c.ToCharArray();
            for (int i = 0; i < ch.Length; i++)
            {
                if (ch[i] > 127)
                    continue;
                else
                    character++;
            }
            Console.WriteLine("字符数:" + (character - 1));
        }
        //统计单词总数
        public string[] Words(string c)
        {
            int count = 0;
            int num = 0;
            string c1 = c.ToLower();
            char[] content = c1.ToCharArray();
            string[] str = new string[content.Length];
            for (int i = 0; i < content.Length; i++)
            {
                str[i] = "";
            }
            for (int i = 0; i < content.Length; i++)
            {
                //找出类似单词的字符串
                if ((content[i] > 47 && content[i] < 58) || (content[i] > 96 && content[i] < 123))
                {
                    num++;
                }
                //判断字符串是否为一个单词
                else
                {
                    if (num < 4)
                    {
                        num = 0;
                        break;
                    }
                    else
                    {
                        for (int j = i - num; j < i - num + 4; j++)
                        {
                            if (content[j] < 97 || content[j] > 122)
                            {
                                num = 0;
                                break;
                            }
                        }
                        if (num != 0)
                        {

                            for (int k = i - num; k < i; k++)
                            {
                                str[count] += content[k].ToString();
                            }
                            count++;
                        }
                        num = 0;
                    }
                }
            }
            Console.WriteLine("单词数:" + count);
            return str;
        }
        //统计行数 
        public void Lines(string c)
        {
            char[] ch = c.ToCharArray();
            int line = 0;
            for (int i = 0; i < ch.Length; i++)
            {
                if (ch[i] == '\n')
                {
                    line++;
                }
            }
            Console.WriteLine("行数:" + line);
        }
        //统计单词出现的次数
        public void Frequency(string[] str, string path)
        {
            Dictionary<string, int> dic = new Dictionary<string, int>();
            StreamWriter writer = new StreamWriter(path, true);
            for (int i = 0; i < str.Length - 1; i++)
            {
                int T = 1;
                if (str[i] == "")
                    continue;
                for (int j = i + 1; j < str.Length; j++)
                {
                    if (str[i] == str[j])
                    {
                        T++;
                        str[j] = "";
                    }
                }
                dic.Add(str[i], T);
            }
            var dicSort = dic.OrderByDescending(objDic => objDic.Value).ThenBy(objDic => objDic.Key);//排序
            foreach (KeyValuePair<string, int> kvp in dicSort)
            {
                Console.WriteLine("<" + kvp.Key + ">:" + kvp.Value);
                writer.WriteLine(kvp.Key);
            }
            writer.Close();
        }
    }
}

