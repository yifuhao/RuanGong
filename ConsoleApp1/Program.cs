using System;
using System.Collections.Generic;
using System.Linq;


namespace space1
{
    class main
    {
        /*private static string[] getCommand()
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

            str2return[0] = str[0] == "-r" ? "a" : "b";

            for (i = 0; i < str.Count(); i++)
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

                if (str[i] == "-w" || str[i] == "-c")
                {
                    wcCount++;
                    str2return[3] = str[i];
                    if (i != str.Count() - 2) //
                    {
                        Console.WriteLine("命令输入不规范");
                        System.Environment.Exit(0);
                    }
                }
            }

            str2return[4] = str[i - 1];

            /*foreach(string item in str2return)
            {
                Console.WriteLine(item);
            }

            return str2return;
        }*/


        static void Main(String[] args)
        {
            /*string[] command = getCommand();
            ReadFile rf = new ReadFile();
            rf.run(command[4]);
            Word.setWeightChosen(command[3][1]); //设置wc
            Topo tp = new Topo();
            tp.run(command[1][0], command[2][0]); //头尾和权重
            */
        }
    }



    class ReadFile
    {
        public static int[] indexOfAllLetter = new int[26]; //记录所有的字母在wordList中的下标
        public static int[] endOfAllLeter = new int[26]; //记录所有的字母在wordList中停止的位置
        public static List<Word> wordList = new List<Word>(); //存放所有读取的单词的列表
        static string readPath = null;
        private const int size = 26;
        private string[] allWord = new string[10000]; //总共最多一万个单词


        public void run(string path_input)
        {
            this.allWord = Read_file(path_input);
            buildWordList(this.allWord);
            run2();
        }

        public string[] getListByString(string line)
        {
            string word2store = "";
            int index = -1;
            string[] allWord = new string[10000];
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

            return allWord;
        }

        public void run2()
        {
            wordList.Sort(wordCompare); //对wordlist排序
            setIndexOfAllLetter();
            setEndOfAllLetter();
        }

        public void buildWordList(string[] allWordList) //根据allWordList来生成wordlist
        {
            wordList = new List<Word>();
            wordList.Clear();

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

        public string[] Read_file(string path_input)
        {
            string line;
            string word2store = "";
            string[] strList = new string[10000];
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
                            strList[++index] = word2store;

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
                    strList[++index] = word2store;

                    /*//判断单词是否重复
                    if (!judgeRepeat(word2store.ToLower()))
                    {
                        wordList.Add(new Word(word2store.ToLower()));
                    }*/
                    word2store = "";
                }
            }

            return strList;
            
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
            for (int i=0; i<size; i++)
            {
                endOfAllLeter[i] = -1;
            }
            char letterJudge = wordList[0].Get_head();
            for (int i=0; i<wordList.Count; i++)
            {
                while (i<wordList.Count &&  wordList[i].Get_head() == letterJudge)
                {
                    i++;
                }
                endOfAllLeter[letterJudge - 'a'] = i;
                if(i<wordList.Count)
                    letterJudge = wordList[i].Get_head();
            }

            endOfAllLeter[wordList[wordList.Count - 1].Get_head() - 'a'] = wordList.Count;
        }

        public void setIndexOfAllLetter() //设置每个字母在wordList中第一次出现的下标
        {
            int judge = 0;
            char letterJudge = wordList[0].Get_head();

            for (int i = 0; i < 26; i++)
            {
                indexOfAllLetter[i] = -1;
            }

            int j = 0;
            while (j < wordList.Count)
            { 
                if (wordList[j].Get_head() == letterJudge)
                {
                    indexOfAllLetter[letterJudge - 'a'] = j;
                    while ((j) < wordList.Count && wordList[j].Get_head() == letterJudge)
                    {
                        j++;
                    }
                    if ((j) < wordList.Count)
                        letterJudge = wordList[j].Get_head();
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
            if (a.Get_head() == b.Get_tail())
            {
                if (a.Get_head() == a.Get_tail()) return -1;
                if (b.Get_head() == b.Get_tail()) return 1;
            }
            return string.Compare(a.Get_allWord(), b.Get_allWord());
        }

    }

    /*class BuildWordChain
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
    }*/


    /*class WriteFile
    {
        public static List<WordChain> wordChainList = new List<WordChain>();
        static string writePath = null;
    }*/

    public class Word
    {
        private char head;
        private char tail;
        private string allWord;
        private static int weightChosen = 0; //0代表w，1代表c

        public bool b_used; //有环图中是否被使用过的标志
        private List<Word> afterWordlist; //后继单词列表

        public Word(string str) //构造方法
        {
            this.allWord = str;
            this.head = str[0];
            this.tail = str[str.Length - 1];
            b_used = false;
            afterWordlist = new List<Word>();
        }

        public static void setWeightChosen(char cc)
        {
            if (cc == 'c')
            {
                weightChosen = 1;
            }
            else if (cc == 'w')
            {
                weightChosen = 0;
            }
            else throw new Exception("-w 和 -c 选择错误");
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

        public List<Word> getAfterWordlist()
        {
            return afterWordlist;
        }

        public override string ToString()
        {
            return this.allWord;
        }

    }

    public class WordChain
    {
        private List<Word> wordChain;
        private int weight;
        private static char word2end; //结束条件，头是尾不是则停止，可以是'#'

        public WordChain()
        {
            this.weight = 0;
            this.wordChain = new List<Word>();
        }

        public WordChain(List<Word> wl)
        {
            this.wordChain = wl;
            this.weight = 0;
            foreach(Word w in wl)
            {
                this.weight += w.getWeight();
            }
        }


        public static bool buildEnd(char wHead, char wTail) //判断是否应该停止成环
        {
            if (word2end == wHead && word2end != wTail) return true;
            return false;
        }

        public bool isChain() //返回这条链是否符合条件
        {
            if (word2end == '#') return true;
            if (this.wordChain.Count < 1) return false;
            if (this.wordChain[this.getSize() - 1].Get_tail() == word2end) return true;
            return false;
        }

        public void copyChain(WordChain wchain)
        {
            this.weight = wchain.weight;
            this.wordChain = new List<Word>(wchain.GetWordChain().ToArray());
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
            foreach (Word w in wordChain)
            {
                Console.Write(w + " ");
            }
            Console.WriteLine();
        }
    }

    public class Topo //拓扑类
    {
        private const int size = 26, MINI = -100000;

        public List<char> topoList = new List<char>(); //拓扑排序的结果
        private static int[,] degreeGraph = new int[size, size]; //判断每个顶点的值
        public static int[] degreeArray = new int[size]; //每个点的入度
        private static int[] distance = new int[size]; //每个点到起点的距离， 0-25分别代表a-z
        private static int[] selfCircle = new int[size]; //判断自环的个数
        private static WordChain[] wordChainList = new WordChain[size]; //到达每个点权重最大的路线
        public static List<char> topoListCopy = new List<char>();


        public void run(char wHead, char wTail)
        {
            WordChain.setWord2End(wTail);

            this.initialDegreeGraph();
            this.testHeadTail(wHead, wTail);
            this.setDegreeArray();
            this.topoloSort();
            this.buildChain(wHead);
            //this.getLongesChain();
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
                    throw new Exception("输入文件缺少要求的首字母");
                }
            }

            if (wTail != '#')
            {
                for (int i = 0; i < size; i++)
                {
                    if (degreeGraph[i, wTail - 'a'] != 0)
                    {
                        tail++;
                    }
                }

                if (tail == 0)
                {
                    throw new Exception("输入文件缺少要求的尾字母");
                }

            }
        }


        public void initialDegreeGraph()
        {
            topoListCopy = new List<char>();
            topoList = new List<char>();
            topoList.Clear();
            topoListCopy.Clear();

            for (int i = 0; i < size; i++)
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
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (i != j) //统计入度时不考虑自环
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

                topoList.Add((char)(j + 'a'));//保存
                degreeArray[j] = -1;// 设结点j为入度为-1，以免再次输出j

                for (int k = 0; k < size; k++)//更新其他入度点
                    if (degreeGraph[j, k] > 0)
                        degreeArray[k]--;
            }

            judgeCircle();
            for (int i = 0; i < topoList.Count; i++)
            {
                Console.Write(topoList[i] + " ");
            }
            Console.WriteLine();
            topoListCopy = new List<char>( topoList.ToArray());
        }

        public void judgeCircle() //判断当前topo序列是否存在环，存在则报错
        {
            for (int i = 0; i < size; i++)
            {
                if (degreeArray[i] > 0 || selfCircle[i] > 1) //存在环，报错，自环超过一个的情况下，也是有环的，报错
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
            if (headLetter == '#')
            {
                for(char x = 'a'; x<='z'; x++)
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
                    for (int i = ReadFile.indexOfAllLetter[u - 'a']; i < ReadFile.endOfAllLeter[u - 'a']; i++)
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
            return wchain;
        }

        
    }

    class WordChainProcessor
    {
        private List<Word> rawWordList; //原始单词链
        private List<Word> maxWordList; //最长单词、字母链
        private bool b_wc; //w或c标识符:true-w ; false-c
        private char c_h; //h或#标识符
        private char c_t; //t或#标识符

        public WordChainProcessor(WordChain words, char wc, char h, char t)
        {
            //初始化字段
            rawWordList = words.GetWordChain();
            maxWordList = new List<Word>();
            if (wc == 'w') { b_wc = true; }
            else { b_wc = false; }
            c_h = h;
            c_t = t;
        }

        public WordChain getResultChain()
        {
            //确定每个单词的后继
            associateWords();

            //对首字母的要求在这里进行处理
            //遍历计算每个单词开头的最长单词链，并更新最长链
            foreach (Word word in rawWordList)
            {
                //对首字母无要求
                if (c_h == '#')
                {
                    List<Word> wl = calculate(word);
                    updateMaxList(wl, maxWordList);
                }
                //若对首字母要求为c,则在遍历时，仅对首字母为c的单词进行求解
                else
                {
                    if (word.Get_head() == c_h)
                    {
                        List<Word> wl = calculate(word);
                        updateMaxList(wl, maxWordList);
                    }
                    else continue;
                }
            }

            //返回最长链
            return new WordChain(maxWordList);
        }

        private void updateMaxList(List<Word> list, List<Word> maxlist)
        {
            //如果是 找最多单词 的模式
            if (b_wc)
            {
                //如果对尾字母没有要求, 或有要求并且list也满足，那么直接比较
                //否则List无法成为满足要求的最长链

                if (list.Count == 0)
                {
                    return;
                }
                else if(c_t == '#' || c_t == list.Last().Get_tail())
                {
                    if (list.Count > maxlist.Count)
                    {
                        maxlist.Clear();
                        maxlist.AddRange(list);
                    }
                }
            }
            //如果是 找最多字母 的模式
            else
            {
                int num_l = 0, num_ml = 0;
                foreach (Word w in list)
                {
                    num_l += w.Get_allWord().Length;
                }
                foreach (Word w in maxlist)
                {
                    num_ml += w.Get_allWord().Length;
                }
                //如果对尾字母没有要求, 或有要求并且list也满足，那么直接比较
                //否则List无法成为满足要求的最长链
                if (list.Count == 0)
                {
                    return;
                }
                if (c_t == '#' || c_t == list.Last().Get_tail())
                {
                    if (num_l > num_ml)
                    {
                        maxlist.Clear();
                        maxlist.AddRange(list);
                    }
                }
            }
        }

        private List<Word> calculate(Word word)
        {
            //对当前访问单词设置已访问标志
            word.b_used = true;
            List<Word> maxWordlist = new List<Word>();

            //当前访问单词没有后继单词时，返回
            if (word.getAfterWordlist().Count == 0)
            {
                List<Word> al = new List<Word>();
                al.Add(word);
                updateMaxList(al, maxWordlist);
                word.b_used = false;
                return maxWordlist;
            }
            //当前访问单词有后继单词，继续深入遍历
            else
            {
                List<Word> al = new List<Word>();
                bool flag_getLoopEnd = true;

                //访问每个后继单词
                foreach (Word w in word.getAfterWordlist())
                {
                    //如果该单词还没有访问过
                    if (!w.b_used)
                    {
                        flag_getLoopEnd = false;
                        //递归进入，求该单词的最长链
                        al = calculate(w);
                        al.Insert(0, word);
                        updateMaxList(al, maxWordlist);

                    }
                }
                //如果每个单词都访问过了，那说明有环，停止搜索并返回
                if (flag_getLoopEnd)
                {
                    al.Add(word);
                    updateMaxList(al, maxWordlist);
                }
                //清除当前单词的已访问标志
                word.b_used = false;
                //返回对于当前单词而言的最长链
                return maxWordlist;
            }
        }

        private void associateWords()
        {
            foreach (Word word_i in rawWordList)
            {
                foreach (Word word_j in rawWordList)
                {
                    if (word_i.Get_tail() == word_j.Get_head())
                    {
                        word_i.getAfterWordlist().Add(word_j);
                    }
                }
            }
        }
    }
}