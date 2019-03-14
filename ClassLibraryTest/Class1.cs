namespace ClassLibraryTest
{
    public class CStoCPPTest
    {
        public int Add(int x, int y)
        {
            return x + y;
        }

        
        public unsafe void getStringInList(ref char* list, int index, ref char word)
        {
            word = list[index];
        }
}
}
