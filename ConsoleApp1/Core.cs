﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using space1;

namespace ConsoleApp1
{
    public class coreBuild : core //实现接口
    {
         public int gen_chain_word(string[] words, int len, string[] result, char head, char tail, bool enable_loop)
        {
            
            if (!((head <= 'z' && head >= 'a') || (head == '\0')))
            {
                throw new Exception("首字母不符合规范");
            }
            if (!((tail <= 'z' && tail >= 'a') || (tail == '\0')))
            {
                throw new Exception("尾字母不符合规范");
            }

            if (head == '\0') head = '#';
            if (tail == '\0') tail = '#';

            if (!enable_loop) //没有-r的情况
            {
                ReadFile readFile = new ReadFile();
                readFile.buildWordList(words);
                readFile.run2();
                Topo tp = new Topo();
                tp.run(head, tail);
                return tp.printDistance();
            }
            else //有
            {
                ReadFile readFile = new ReadFile();
                readFile.buildWordList(words);
                WordChainProcessor wcp = new WordChainProcessor(ReadFile.GetWordChainUnDo(), 'w', head, tail);
                WordChain wchain = wcp.getResultChain();
                return wchain.getWeight();
            }
        }
        public int gen_chain_char(string[] words, int len, string[] result, char head, char tail, bool enable_loop)
        {
            if (!((head <= 'z' && head >= 'a') || (head == '\0')))
            {
                throw new Exception("首字母不符合规范");
            }
            if (!((tail <= 'z' && tail >= 'a') || (tail == '\0')))
            {
                throw new Exception("尾字母不符合规范");
            }

            if (head == '\0') head = '#';
            if (tail == '\0') tail = '#';


            Word.setWeightChosen('c');
            if (!enable_loop) //没有-r的情况
            {
                ReadFile readFile = new ReadFile();
                readFile.buildWordList(words);
                readFile.run2();
                Topo tp = new Topo();
                tp.run(head, tail);
                return tp.printDistance();
            }
            else //有
            {
                return 0;
            }
            throw new NotImplementedException();
        }

        public WordChain build_chain_word(string[] words, int len, string[] result, char head, char tail, bool enable_loop)
        {
            if (!enable_loop) //没有-r的情况
            {
                ReadFile readFile = new ReadFile();
                readFile.buildWordList(words);
                readFile.run2();
                Topo tp = new Topo();
                tp.run(head, tail);
                return tp.getLongesChain();
            }
            else //有
            {
                return new WordChain();
            }
            throw new NotImplementedException();
        }

        public WordChain build_chain_char(string[] words, int len, string[] result, char head, char tail, bool enable_loop)
        {
            Word.setWeightChosen('c');
            if (!enable_loop) //没有-r的情况
            {
                ReadFile readFile = new ReadFile();
                readFile.buildWordList(words);
                readFile.run2();
                Topo tp = new Topo();
                tp.run(head, tail);
                return tp.getLongesChain();
            }
            else //有
            {
                return new WordChain();
            }
            throw new NotImplementedException();
        }
    }


    interface core
    {
        int gen_chain_word(string [] words, int len, string [] result, char head, char tail, bool enable_loop);
        int gen_chain_char(string [] words, int len, string [] result, char head, char tail, bool enable_loop);
        WordChain build_chain_word(string[] words, int len, string[] result, char head, char tail, bool enable_loop);
        WordChain build_chain_char(string[] words, int len, string[] result, char head, char tail, bool enable_loop);
    }
}
