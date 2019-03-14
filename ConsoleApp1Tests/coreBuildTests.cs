using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConsoleApp1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using space1;

namespace ConsoleApp1.Tests
{
    [TestClass()]
    public class coreBuildTests
    {
        public string[] Read_file(string path_input)
        {
            string line;
            string []allWord = new string[10000];
            string word2store = "";
            int index = -1; //当前AllWord的下标
            string readPath = path_input; //读入文件
            System.IO.StreamReader sr = new System.IO.StreamReader(readPath); //创建读入流

            while ((line = sr.ReadLine()) != null) //读入文件
            {
                //Console.WriteLine(line);
                foreach (char x in line) //对每个字母检测
                {
                    if ((x <= 'Z' && x >= 'A') || (x >= 'a' && x <= 'z'))
                    {
                        word2store = word2store + x;
                    }
                    else
                    {
                        if (word2store.Length > 0)
                        {
                            allWord[++index] = word2store;

                            /*//判断单词是否重复
                            if (!judgeRepeat(word2store.ToLower()))
                            {
                                wordList.Add(new Word(word2store.ToLower()));
                            }*/
                            word2store = "";
                        }
                    }
                }

                if (word2store.Length > 0)
                {
                    allWord[++index] = word2store;

                    /*//判断单词是否重复
                    if (!judgeRepeat(word2store.ToLower()))
                    {
                        wordList.Add(new Word(word2store.ToLower()));
                    }*/
                    word2store = "";
                }
            }
            return allWord;
        }
        
        [TestMethod()]
        public void gen_chain_wordTest1() //没有-r。-w
        {
            string filePath = ("test1.txt");
            char head = '\0', tail = '\0';
            string [] words = Read_file(filePath);
            coreBuild core = new coreBuild();
            int result = core.gen_chain_word(words, 0, words, head, tail, false);
            Assert.AreEqual(result,2);
        }

        [TestMethod()]
        public void gen_chain_wordTest2() //有-r。-w
        {
            string filePath = ("test3.txt");
            char head = '\0', tail = '\0';
            string[] words = Read_file(filePath);
            coreBuild core = new coreBuild();
            int result = core.gen_chain_word(words, 0, words, head, tail, true);
            Assert.AreEqual(result, 6);
        }

        [TestMethod()]
        public void gen_chain_wordTest3()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void gen_chain_charTest1() //没有-r。-c
        {
            string filePath = ("test2.txt");
            char head = '\0', tail = '\0';
            string[] words = Read_file(filePath);
            coreBuild core = new coreBuild();
            int result = core.gen_chain_char(words, 0, words, head, tail, false);
            Assert.AreEqual(result, 28);
            
        }

        [TestMethod()]
        public void gen_chain_charTest2()
        {
            Assert.Fail();
        }

    }
}