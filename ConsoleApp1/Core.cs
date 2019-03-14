using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using space1;

namespace ConsoleApp1
{
    public class coreBuild : core //实现接口
    {
        public WordChain get_chainByLine(string line, char head, char tail, bool enable_loop, char wc) //通过一个输入，来返回一个wordList的输出
        {
            ReadFile rf = new ReadFile();
            string[] strList = rf.getListByString(line);
            rf.buildWordList(strList);
            if (wc == 'c')
            {
                return build_chain_word(strList, 0, strList, head, tail, enable_loop);
            }
            else
            {
                return build_chain_char(strList, 0, strList, head, tail, enable_loop);
            }
        }

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

            Word.setWeightChosen('w');

            if (!enable_loop) //没有-r的情况
            {
                ReadFile readFile = new ReadFile();
                readFile.buildWordList(words);
                readFile.run2();
                Topo tp = new Topo();
                tp.run(head, tail);
                return tp.getLongesChain().getWeight();
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
                return tp.getLongesChain().getWeight();
            }
            else //有
            {
                ReadFile readFile = new ReadFile();
                readFile.buildWordList(words);
                WordChainProcessor wcp = new WordChainProcessor(ReadFile.GetWordChainUnDo(), 'c', head, tail);
                WordChain wchain = wcp.getResultChain();
                return wchain.getWeight();
            }
            throw new NotImplementedException();
        }

        public WordChain build_chain_word(string[] words, int len, string[] result, char head, char tail, bool enable_loop)
        {
            Word.setWeightChosen('w');

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
                ReadFile readFile = new ReadFile();
                readFile.buildWordList(words);
                WordChainProcessor wcp = new WordChainProcessor(ReadFile.GetWordChainUnDo(), 'w', head, tail);
                WordChain wchain = wcp.getResultChain();
                return wchain;
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
                ReadFile readFile = new ReadFile();
                readFile.buildWordList(words);
                WordChainProcessor wcp = new WordChainProcessor(ReadFile.GetWordChainUnDo(), 'c', head, tail);
                WordChain wchain = wcp.getResultChain();
                return wchain;
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
