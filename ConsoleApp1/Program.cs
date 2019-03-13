using System;
using System.IO;
using System.Text.RegularExpressions; //正则表达式
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace space1
{
    class main
    {
        private static string[] getCommand()
        {
            string RexStr = "[-r ]?[-h a-z ]?[-t a-z ]?-w|-c .*"; //匹配的正则表达式
            string command = Console.ReadLine();
            Regex spl_reg = new Regex(" "); //通过空格符来分割
            string[] str = spl_reg.Split(command);
            string[] str2return = new string[5];
            int rCount = 0, hCount = 0, tCount = 0, wcCount = 0, i = 0;

            str2return[1] = "#";
            str2return[2] = "#";

            if (!Regex.IsMatch(command, RexStr)) //如果匹配正则表达式则继续，否则报错并结束程序
            {
                Console.WriteLine("命令输入不规范");
                System.Environment.Exit(0);
            }

            str2return[0] = str[0] == "-r"?"a":"b";
            
            for (i=0; i<str.Count(); i++)
            {
                if (str[i] == "-r")
                {
                    rCount++;
                }

                if (str[i] == "-h")
                {
                    hCount++;
                    str2return[1] = str[i + 1];
                }

                if (str[i] == "-t")
                {
                    tCount++;
                    str2return[2] = str[i + 1];
                }

                if(str[i] == "-w" || str[i] == "-c")
                {
                    wcCount++;
                    str2return[3] = str[i];
                    if(i != str.Count() - 2) //
                    {
                        Console.WriteLine("命令输入不规范");
                        System.Environment.Exit(0);
                    }
                }
            }

            str2return[4] = str[i-1];
            
            /*foreach(string item in str2return)
            {
                Console.WriteLine(item);
            }*/

            return str2return; 
        }


        static void Main(String [] args)
        {
            string []command = getCommand();
            ReadFile rf = new ReadFile();
            rf.run(command[4]);
            Word.setWeightChosen(command[3][1]); //设置wc
            Topo tp = new Topo();
            tp.run(command[1][0], command[2][0]); //头尾和权重
        }
    }

    class ReadFile
    {
        public static int[] indexOfAllLetter = new int[26]; //记录所有的字母在wordList中的下标
        public static int[] endOfAllLeter = new int[26]; //记录所有的字母在wordList中停止的位置
        public static List<Word> wordList = new List<Word>(); //存放所有读取的单词的列表
        static string readPath = null;
        private const int size = 26;
        public static string[] allWord = new string[10000]; //总共最多一万个单词


        public void run(string path_input)
        {
            Read_file(path_input);
            buildWordList(allWord);
            run2();
        }

        public void run2()
        {
            wordList.Sort(wordCompare); //对wordlist排序
            setIndexOfAllLetter();
            setEndOfAllLetter();
        }
        
        public void buildWordList(string [] allWordList) //根据allWordList来生成wordlist
        {
            foreach (string str in allWordList)
            {
                if (str == null)
                {
                    break;
                }
                if (!judgeRepeat(str.ToLower()))
                {
                    wordList.Add(new Word(str.ToLower()));
                }
            }
        }

        public void Read_file(string path_input)
        {
            string line;
            string word2store = "";
            int index = -1; //当前AllWord的下标
            readPath = path_input; //读入文件
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

            
            /*foreach(int i in indexOfAllLetter)
            {
                Console.WriteLine(i);
            }

            foreach(Word w in wordList)
            {
                Console.WriteLine(w);
            }*/
            //Console.ReadKey();
        }

        public static WordChain GetWordChainUnDo()
        {
            WordChain wchain = new WordChain();
            foreach (Word w in wordList)
            {
                wchain.addWord(w);
            }
            return wchain;
        }

        public void setEndOfAllLetter()
        {
            int a;
            for (int i=0; i<size-1; i++)
            {
                a = i;
                while (i+1<size && indexOfAllLetter[i + 1] == -1)
                {
                    i++;
                    endOfAllLeter[i] = -1;
                }
                if(i + 1 < size)
                    endOfAllLeter[a] = indexOfAllLetter[i + 1];
            }
            endOfAllLeter[size - 1] = wordList.Count;
        }

        public void setIndexOfAllLetter() //设置每个字母在wordList中第一次出现的下标
        {
            int judge = 0;
            char letterJudge = 'a';

            for (int i = 0; i<26; i++)
            {
                indexOfAllLetter[i] = -1;
            }

            for(int i=0; i<wordList.Count; i++)
            {
                if(wordList[i].Get_head() == letterJudge)
                {
                    indexOfAllLetter[letterJudge - 'a'] = i;
                    while((i + 1) < wordList.Count && wordList[i+1].Get_head() == letterJudge)
                    {
                        i++;
                    }
                    if ((i + 1) < wordList.Count)
                        letterJudge = wordList[i+1].Get_head();
                }
            }
        }

        public bool judgeRepeat(string str2judge) //判断当前字符串在wordlist中是否已经存在
        {
            foreach (Word w in wordList)
            {
                if (str2judge.CompareTo(w.Get_allWord()) == 0)
                {
                    return true; //已经存在则返回真 
                }
            }
            return false;
        }

        private int wordCompare(Word a, Word b)
        {
            return string.Compare(a.Get_allWord(), b.Get_allWord());
        }

    }

    class BuildWordChain
    {
        static int[] indexOfLetter = new int[26]; //记录每个字母是否在当前链出现
        static List<WordChain> chainList = new List<WordChain>(); //按照长度排序（-w, -c）
        static WordChain wordChain = new WordChain();
        static int[] useLetter = new int[ReadFile.wordList.Count]; //记录单词链表中的单词在当前单词链中是否使用
        static int headJudge = -1; //0-26, 对应设定某个英文字母或者不设限
        static int tailJudge = -1; //0-26, 对应设定某个英文字母或者不设限
        static int order_c = 0; //-c 字母
        static int order_w = 0; //-w 单词
        static int choseCW = 0; //0:c, 1:w
        
        //排序标准
        public void setHeadJudge(char x) //x 为a-z和 #
        {
            if (x == '#') headJudge = 26;
            else headJudge = x - 'a';
        }

        public void setTailJudge(char x) //x 为a-z和 #
        {
            if (x == '#') tailJudge = 26;
            else tailJudge = x - 'a';
        }

        public void setCW(char x)
        {
            choseCW = x == 'c' ? 0 : 1;
        }

        private int getMax() //返回当前比较的长度最长值
        {
            return choseCW == 0 ? order_c : order_w;
        }

        private void setMax(int max) //设置当前比较的长度最长值
        {
            if (choseCW == 0) order_c = max;
            else order_w = max;
        }

        public void initial() //初始化各个变量
        {
            wordChain = new WordChain();
            useLetter = new int[ReadFile.wordList.Count];
        }

        public int getHeadJudge()
        {
            return headJudge;
        }
    }

    class WriteFile
    {
        public static List<WordChain> wordChainList = new List<WordChain>(); 
        static string writePath = null;
    }

    class Word
    {
        private char head;
        private char tail;
        private string allWord;
        private static int weightChosen = 0; //0代表w，1代表c

        public Word(string str) //构造方法
        {
            this.allWord = str;
            this.head = str[0];
            this.tail = str[str.Length - 1];
        }
        
        public static void setWeightChosen(char cc)
        {
            if (cc == 'c')
            {
                weightChosen = 1;
                //Console.WriteLine(weightChosen);
            }
        }

        public int getWeight()
        {
            if (weightChosen == 0) return 1;
            return this.allWord.Length;
        }

        public char Get_tail()
        {
            return this.tail;
        }

        public string Get_allWord()
        {
            return this.allWord;
        }

        public char Get_head()
        {
            return this.head;
        }

        public override string ToString()
        {
            return this.allWord;
        }

    }

    class WordChain
    {
        private List<Word> wordChain;
        private int weight;
        private static char word2end; //结束条件，头是尾不是则停止，可以是'#'

        public WordChain()
        {
            this.weight = 0;
            this.wordChain = new List<Word>();
        }

        public static bool buildEnd(char wHead, char wTail) //判断是否应该停止成链，这样的话，不设t就会一直返回false
        {
            if (word2end == wHead && word2end != wTail) return true;
            return false;
        }

        public bool isChain() //返回这条链是否符合条件
        {
            if (word2end == '#') return true;
            if (this.wordChain[this.getSize() - 1].Get_tail() == word2end) return true;
            return false;
        }

        public void copyChain(WordChain wchain)
        {
            this.weight = wchain.weight;
            this.wordChain = new List<Word>(wchain.GetWordChain().ToArray());
        }

        public char getWord2End()
        {
            return word2end;
        }

        public static void setWord2End(char w2end)
        {
            word2end = w2end;
        }

        public List<Word> GetWordChain()
        {
            return this.wordChain;
        }

        public void addWord(Word w)
        {
            this.wordChain.Add(w);
            this.weight += w.getWeight(); //更新权重
        }

        public int getWeight()
        {
            return this.weight;
        }

        public int getSize()
        {
            return this.wordChain.Count;
        }

        public void printChain()
        {
            foreach(Word w in wordChain)
            {
                Console.Write(w+" ");
            }
            Console.WriteLine();
        }
    }

    class Topo //拓扑类
    {
        private const int size = 26, MINI = -100000;
        
        public List<char> topoList = new List<char>(); //拓扑排序的结果
        public List<char> topoListCopy = new List<char>(); //保存拓扑序列的结果
        private static int[,] degreeGraph = new int[size,size]; //判断每个顶点的值
        public static int[] degreeArray = new int[size]; //每个点的入度
        private static int[] distance = new int[size]; //每个点到起点的距离， 0-25分别代表a-z
        private static int[] selfCircle = new int[size]; //判断自环的个数
        private static WordChain[] wordChainList = new WordChain[size]; //到达每个点权重最大的路线

        public void run(char wHead, char wTail)
        {
            WordChain.setWord2End(wTail);
            
            this.initialDegreeGraph();
            this.testHeadTail(wHead, wTail);
            this.setDegreeArray();
            this.topoloSort();
            this.buildChain(wHead);
            this.printDistance();
        }

        public void testHeadTail(char wHead, char wTail) //检测head和tail是否存在
        {
            int head = 0, tail = 0;

            if (wHead != '#')
            {
                for (int i = 0; i < size; i++)
                {
                    if (degreeGraph[wHead - 'a', i] != 0)
                    {
                        head++;
                    }
                }

                if (head == 0)
                {
                    Console.WriteLine("输入文件无法求解");
                    System.Environment.Exit(0);
                }
            }

            if(wTail!='#')
            {   for (int i = 0; i < size; i++)
                {
                    if (degreeGraph[i, wTail - 'a'] != 0)
                    {
                        tail++;
                    }
                }

                if (tail == 0)
                {
                    Console.WriteLine("输入文件无法求解");
                    System.Environment.Exit(0);
                }

            }
        }


        public void initialDegreeGraph()
        {

            for (int i=0; i<size; i++)
            {
                degreeArray[i] = -1;
                selfCircle[i] = 0;
                wordChainList[i] = new WordChain();
            }

            foreach (Word w in ReadFile.wordList)
            {
                if (w.Get_head() == w.Get_tail()) //如果是自环
                {
                    selfCircle[w.Get_head() - 'a']++;
                }
                
                degreeGraph[(w.Get_head() - 'a'), w.Get_tail() - 'a'] = 1;
                degreeArray[(w.Get_head() - 'a')] = 0;  //这个地方应该有问题
                degreeArray[(w.Get_tail() - 'a')] = 0;
                
            }
        }

        public void setDegreeArray() //计算入度
        {
            for (int i=0; i<size; i++)
            {
                for(int j=0; j < size; j++)
                {
                    if(i!=j) //统计入度时不考虑自环
                        degreeArray[i] += degreeGraph[j, i];
                }
            }

            

        }

        public void topoloSort() //拓扑排序算法
        {
            int j;
            for (int i = 0; i < size; i++) //size次循环
            {
                j = 0;
                while (j < size && degreeArray[j] != 0) j++;//找到入度为0的顶点

                if (j == size) break;

                topoList.Add((char)(j+'a'));//保存
                degreeArray[j] = -1;// 设结点j为入度为-1，以免再次输出j

                for (int k = 0; k < size; k++)//更新其他入度点
                    if (degreeGraph[j,k] > 0)
                        degreeArray[k]--;
            }

            judgeCircle();

            topoListCopy = new List<char>(topoList.ToArray()); //将拓扑序列的结果备份

            for(int i=0; i<topoList.Count; i++)
            {
                Console.Write(topoList[i]+" ");
            }
            Console.WriteLine();

        }

        public void judgeCircle() //判断当前topo序列是否存在环，存在则报错
        {
            for (int i = 0; i < size; i++)
            {
                if (degreeArray[i] > 0 || selfCircle[i]>1) //存在环，报错，自环超过一个的情况下，也是有环的，报错
                {
                    Console.WriteLine("当前文本中出现了环，错误");
                    break;
                }
            }
        }

        public void buildChain(char headLetter) //当没有规定headLetter时，传入#
        {
            for (int i = 0; i < size; i++) //将每个点到源点的距离标为负无穷
                distance[i] = MINI;
            if (headLetter == '#') {
                distance[topoList[0]-'a'] = 0; //这里应该有问题
                for (char x = 'a'; x <= 'z'; x++)
                {
                    buildChain(x);
                }
            }
            else
            {
                topoList = new List<char>(topoListCopy.ToArray());
                distance[headLetter - 'a'] = 0;
            }

            while (topoList.Count != 0) //当前拓扑序列非空（在指定起点的情况下，是否需要把之前的全清空）
            {
                //取出拓扑序列的第一个点
                char u = topoList[0];
                topoList.RemoveAt(0);

                //更新所有邻接点的距离
                if (distance[u - 'a'] != MINI)
                {
                    for (int i=ReadFile.indexOfAllLetter[u-'a']; i < ReadFile.endOfAllLeter[u - 'a']; i++)
                    {
                        Word w = ReadFile.wordList[i];
                        
                        if (distance[w.Get_tail() - 'a'] < (distance[w.Get_head() - 'a'] + w.getWeight()))
                        {
                            if (WordChain.buildEnd(w.Get_head(), w.Get_tail())) //到达终点
                            {
                                //在到达终点的时候记录当前最长链和最长路长

                                //将拓扑序列清空，停止循环
                                topoList.Clear();
                                break;
                            }

                            distance[w.Get_tail() - 'a'] = (distance[w.Get_head() - 'a'] + w.getWeight()); //更新距离

                            //更新链
                            wordChainList[w.Get_tail() - 'a'].copyChain(wordChainList[w.Get_head() - 'a']);
                            wordChainList[w.Get_tail() - 'a'].addWord(w);
                        }

                    }
                }

            }
        
        }

        public WordChain getLongesChain()
        {
            int max = 0;
            WordChain wchain = new WordChain();
            for (int i = 0; i < size; i++)
            {
                if (wordChainList[i].getWeight() > max && wordChainList[i].isChain())
                {
                    max = wordChainList[i].getWeight();
                    wchain = wordChainList[i];
                }
            }

            Console.Write(max + " ");
            wchain.printChain();

            return wchain;
        }

        public int printDistance() //输出满足条件的最长链
        {
            int max = 0;
            WordChain wchain = new WordChain();
            for (int i = 0; i < size; i++)
            {
                if (wordChainList[i].getWeight() > max && wordChainList[i].isChain()) 
                {
                    max = wordChainList[i].getWeight();
                    wchain = wordChainList[i];
                }
            }

            Console.Write(max + " ");
            wchain.printChain();

            return max;
            
            /*for(int i=0; i<size; i++)
            {
                Console.Write(distance[i] + " ");
                wordChainList[i].printChain(); //输出链
            }*/
        }

    }
}
