using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace space1
{
    class main
    {
        static void Main(String [] args)
        {
            ReadFile rf = new ReadFile();
            rf.Read_file();
            Topo tp = new Topo();
            tp.initialDegreeGraph();
            tp.setDegreeArray();
            tp.topoloSort();
            tp.buildChain('#');
            tp.printDistance();
        }
    }

    class ReadFile
    {
        public static int[] indexOfAllLetter = new int[26]; //记录所有的字母在wordList中的下标
        public static int[] endOfAllLeter = new int[26]; //记录所有的字母在wordList中停止的位置
        public static List<Word> wordList = new List<Word>(); //存放所有读取的单词的列表
        static string readPath = null;
        private const int size = 26;

        public void Read_file()
        {
            string line;
            string word2store = "";
            readPath = Console.ReadLine(); //读入文件
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
                            //判断单词是否重复
                            if (!judgeRepeat(word2store.ToLower()))
                            {
                                wordList.Add(new Word(word2store.ToLower()));
                            }
                            word2store = "";
                        }
                    }
                }

                if (word2store.Length > 0)
                {
                    //判断单词是否重复
                    if (!judgeRepeat(word2store.ToLower()))
                    {
                        wordList.Add(new Word(word2store.ToLower()));
                    }
                    word2store = "";
                }
            }

            wordList.Sort(wordCompare); //对wordlist排序
            setIndexOfAllLetter();
            setEndOfAllLetter();

            foreach(int i in indexOfAllLetter)
            {
                Console.WriteLine(i);
            }

            foreach(Word w in wordList)
            {
                Console.WriteLine(w);
            }
            //Console.ReadKey();
        }

        public void setEndOfAllLetter()
        {
            int a;
            for (int i=0; i<size-1; i++)
            {
                a = i;
                while (indexOfAllLetter[i + 1] == -1)
                {
                    i++;
                    endOfAllLeter[i] = -1;
                }
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

        //生成链的方法
        public void buildChain(int headJudge)
        {
            if (headJudge == 26) //全生成
            {
                for (int i = 0; i < 26; i++)
                    buildChain(i);
            }
            else
            {
                
            }
        }

        public void buildChain_r() //有-r的情况
        {

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
        private int weight;


        public Word(string str) //构造方法
        {
            this.allWord = str;
            this.head = str[0];
            this.tail = str[str.Length - 1];
            this.weight = 1; //-w
        }

        public void setWeight_c() //将权值设为单词长，即-c
        {
            this.weight = this.allWord.Length;
        }

        public int getWeight()
        {
            return this.weight;
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
        public List<Word> wordChain;
        private int size;
        private int weight;

        public WordChain()
        {

        }

        public void addWord(Word w)
        {
            this.wordChain.Add(w);
        }

        public int getWeight()
        {
            return this.weight;
        }

        public int getSize()
        {
            return this.size;
        }
    }

    class Topo //拓扑类
    {
        private const int size = 26, MINI = -100000;
        public List<char> topoList = new List<char>(); //拓扑排序的结果
        private static int[,] degreeGraph = new int[size,size]; //判断每个顶点的值
        public static int[] degreeArray = new int[size]; //每个点的入度
        private static int[] distance = new int[size]; //每个点到起点的距离， 0-25分别代表a-z
        private static int[] selfCircle = new int[size]; //判断自环的个数

        public void initialDegreeGraph()
        {
            for (int i=0; i<size; i++)
            {
                degreeArray[i] = -1;
                selfCircle[i] = 0;
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
            for(int i=0; i<topoList.Count; i++)
            {
                Console.Write(topoList[i]+" ");
            }
            Console.WriteLine();

        }

        public void judgeCircle()
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
            if (headLetter == '#') distance[topoList[0]-'a'] = 0;
            else distance[headLetter - 'a'] = 0;

            while (topoList.Count != 0) //当前拓扑序列非空
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
                            distance[w.Get_tail() - 'a'] = (distance[w.Get_head() - 'a'] + w.getWeight()); //更新距离
                        }

                    }
                }

            }
        
        }

        public void printDistance()
        {
            for(int i=0; i<size; i++)
            {
                Console.Write(distance[i] + " ");

            }
        }

    }


}
