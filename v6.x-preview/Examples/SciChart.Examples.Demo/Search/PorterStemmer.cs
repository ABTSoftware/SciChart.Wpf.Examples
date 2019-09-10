using System;
using System.Runtime.InteropServices;

namespace SciChart.Examples.Demo.Search
{
    /*
       Porter stemmer in CSharp, based on the Java port. The original paper is in

           Porter, 1980, An algorithm for suffix stripping, Program, Vol. 14,
           no. 3, pp 130-137,

       See also http://www.tartarus.org/~martin/PorterStemmer

       History:

       Release 1

       Bug 1 (reported by Gonzalo Parra 16/10/99) fixed as marked below.
       The words 'aed', 'eed', 'oed' leave k at 'a' for step 3, and b[k-1]
       is then out outside the bounds of b.

       Release 2

       Similarly,

       Bug 2 (reported by Steve Dyrdahl 22/2/00) fixed as marked below.
       'ion' by itself leaves j = -1 in the test for 'ion' in step 5, and
       b[j] is then outside the bounds of b.

       Release 3

       Considerably revised 4/9/00 in the light of many helpful suggestions
       from Brian Goetz of Quiotix Corporation (brian@quiotix.com).

       Release 4
	   
       This revision allows the Porter Stemmer Algorithm to be exported via the
       .NET Framework. To facilate its use via .NET, the following commands need to be
       issued to the operating system to register the component so that it can be
       imported into .Net compatible languages, such as Delphi.NET, Visual Basic.NET,
       Visual C++.NET, etc. 
	   
       1. Create a stong name: 		
            sn -k Keyfile.snk  
       2. Compile the C# class, which creates an assembly PorterStemmerAlgorithm.dll
            csc /t:library PorterStemmerAlgorithm.cs
       3. Register the dll with the Windows Registry 
          and so expose the interface to COM Clients via the type library 
          ( PorterStemmerAlgorithm.tlb will be created)
            regasm /tlb PorterStemmerAlgorithm.dll
       4. Load the component in the Global Assembly Cache
            gacutil -i PorterStemmerAlgorithm.dll
		
       Note: You must have the .Net Studio installed.
	   
       Once this process is performed you should be able to import the class 
       via the appropiate mechanism in the language that you are using.
	   
       i.e in Delphi 7 .NET this is simply a matter of selecting: 
            Project | Import Type Libary
       And then selecting Porter stemmer in CSharp Version 1.4"!
	   
       Cheers Leif
	
    */

    /**
      * Stemmer, implementing the Porter Stemming Algorithm
      *
      * The Stemmer class transforms a word into its root form.  The input
      * word can be provided a character at time (by calling add()), or at once
      * by calling one of the various stem(something) methods.
      */

    public interface StemmerInterface
    {
        string StemTerm(string s);
    }

    [ClassInterface(ClassInterfaceType.None)]
    public class PorterStemmer : StemmerInterface
    {
        private char[] b;
        private int i,     /* offset into b */
            i_end, /* offset to end of stemmed word */
            j, k;
        private static int INC = 200;
        /* unit of size whereby b is increased */

        public PorterStemmer()
        {
            b = new char[INC];
            i = 0;
            i_end = 0;
        }

        /* Implementation of the .NET interface - added as part of realease 4 (Leif) */
        public string StemTerm(string s)
        {
            SetTerm(s);
            Stem();
            return GetTerm();
        }

        /*
            SetTerm and GetTerm have been simply added to ease the 
            interface with other lanaguages. They replace the add functions 
            and toString function. This was done because the original functions stored
            all stemmed words (and each time a new woprd was added, the buffer would be
            re-copied each time, making it quite slow). Now, The class interface 
            that is provided simply accepts a term and returns its stem, 
            instead of storing all stemmed words.
            (Leif)
        */

        void SetTerm(string s)
        {
            i = s.Length;
            char[] new_b = new char[i];
            for (int c = 0; c < i; c++)
                new_b[c] = s[c];

            b = new_b;

        }

        public string GetTerm()
        {
            return new String(b, 0, i_end);
        }


        /* Old interface to the class - left for posterity. However, it is not
         * used when accessing the class via .NET (Leif)*/

        /**
         * Add a character to the word being stemmed.  When you are finished
         * adding characters, you can call stem(void) to stem the word.
         */

        public void Add(char ch)
        {
            if (i == b.Length)
            {
                char[] new_b = new char[i + INC];
                for (int c = 0; c < i; c++)
                    new_b[c] = b[c];
                b = new_b;
            }
            b[i++] = ch;
        }


        /** Adds wLen characters to the word being stemmed contained in a portion
         * of a char[] array. This is like repeated calls of add(char ch), but
         * faster.
         */

        public void Add(char[] w, int wLen)
        {
            if (i + wLen >= b.Length)
            {
                char[] new_b = new char[i + wLen + INC];
                for (int c = 0; c < i; c++)
                    new_b[c] = b[c];
                b = new_b;
            }
            for (int c = 0; c < wLen; c++)
                b[i++] = w[c];
        }

        /**
         * After a word has been stemmed, it can be retrieved by toString(),
         * or a reference to the internal buffer can be retrieved by getResultBuffer
         * and getResultLength (which is generally more efficient.)
         */
        public override string ToString()
        {
            return new String(b, 0, i_end);
        }

        /**
         * Returns the length of the word resulting from the stemming process.
         */
        public int GetResultLength()
        {
            return i_end;
        }

        /**
         * Returns a reference to a character buffer containing the results of
         * the stemming process.  You also need to consult getResultLength()
         * to determine the length of the result.
         */
        public char[] GetResultBuffer()
        {
            return b;
        }

        /* cons(i) is true <=> b[i] is a consonant. */
        private bool Cons(int i)
        {
            switch (b[i])
            {
                case 'a':
                case 'e':
                case 'i':
                case 'o':
                case 'u': return false;
                case 'y': return (i == 0) ? true : !Cons(i - 1);
                default: return true;
            }
        }

        /* m() measures the number of consonant sequences between 0 and j. if c is
           a consonant sequence and v a vowel sequence, and <..> indicates arbitrary
           presence,

              <c><v>       gives 0
              <c>vc<v>     gives 1
              <c>vcvc<v>   gives 2
              <c>vcvcvc<v> gives 3
              ....
        */
        private int M()
        {
            int n = 0;
            int i = 0;
            while (true)
            {
                if (i > j) return n;
                if (!Cons(i)) break; i++;
            }
            i++;
            while (true)
            {
                while (true)
                {
                    if (i > j) return n;
                    if (Cons(i)) break;
                    i++;
                }
                i++;
                n++;
                while (true)
                {
                    if (i > j) return n;
                    if (!Cons(i)) break;
                    i++;
                }
                i++;
            }
        }

        /* vowelinstem() is true <=> 0,...j contains a vowel */
        private bool Vowelinstem()
        {
            int i;
            for (i = 0; i <= j; i++)
                if (!Cons(i))
                    return true;
            return false;
        }

        /* doublec(j) is true <=> j,(j-1) contain a double consonant. */
        private bool Doublec(int j)
        {
            if (j < 1)
                return false;
            if (b[j] != b[j - 1])
                return false;
            return Cons(j);
        }

        /* cvc(i) is true <=> i-2,i-1,i has the form consonant - vowel - consonant
           and also if the second c is not w,x or y. this is used when trying to
           restore an e at the end of a short word. e.g.

              cav(e), lov(e), hop(e), crim(e), but
              snow, box, tray.

        */
        private bool Cvc(int i)
        {
            if (i < 2 || !Cons(i) || Cons(i - 1) || !Cons(i - 2))
                return false;
            int ch = b[i];
            if (ch == 'w' || ch == 'x' || ch == 'y')
                return false;
            return true;
        }

        private bool Ends(String s)
        {
            int l = s.Length;
            int o = k - l + 1;
            if (o < 0)
                return false;
            char[] sc = s.ToCharArray();
            for (int i = 0; i < l; i++)
                if (b[o + i] != sc[i])
                    return false;
            j = k - l;
            return true;
        }

        /* setto(s) sets (j+1),...k to the characters in the string s, readjusting
           k. */
        private void Setto(String s)
        {
            int l = s.Length;
            int o = j + 1;
            char[] sc = s.ToCharArray();
            for (int i = 0; i < l; i++)
                b[o + i] = sc[i];
            k = j + l;
        }

        /* r(s) is used further down. */
        private void R(String s)
        {
            if (M() > 0)
                Setto(s);
        }

        /* step1() gets rid of plurals and -ed or -ing. e.g.
               caresses  ->  caress
               ponies    ->  poni
               ties      ->  ti
               caress    ->  caress
               cats      ->  cat

               feed      ->  feed
               agreed    ->  agree
               disabled  ->  disable

               matting   ->  mat
               mating    ->  mate
               meeting   ->  meet
               milling   ->  mill
               messing   ->  mess

               meetings  ->  meet

        */

        private void Step1()
        {
            if (b[k] == 's')
            {
                if (Ends("sses"))
                    k -= 2;
                else if (Ends("ies"))
                    Setto("i");
                else if (b[k - 1] != 's')
                    k--;
            }
            if (Ends("eed"))
            {
                if (M() > 0)
                    k--;
            }
            else if ((Ends("ed") || Ends("ing")) && Vowelinstem())
            {
                k = j;
                if (Ends("at"))
                    Setto("ate");
                else if (Ends("bl"))
                    Setto("ble");
                else if (Ends("iz"))
                    Setto("ize");
                else if (Doublec(k))
                {
                    k--;
                    int ch = b[k];
                    if (ch == 'l' || ch == 's' || ch == 'z')
                        k++;
                }
                else if (M() == 1 && Cvc(k)) Setto("e");
            }
        }

        /* step2() turns terminal y to i when there is another vowel in the stem. */
        private void Step2()
        {
            if (Ends("y") && Vowelinstem())
                b[k] = 'i';
        }

        /* step3() maps double suffices to single ones. so -ization ( = -ize plus
           -ation) maps to -ize etc. note that the string before the suffix must give
           m() > 0. */
        private void Step3()
        {
            if (k == 0)
                return;

            /* For Bug 1 */
            switch (b[k - 1])
            {
                case 'a':
                    if (Ends("ational")) { R("ate"); break; }
                    if (Ends("tional")) { R("tion"); break; }
                    break;
                case 'c':
                    if (Ends("enci")) { R("ence"); break; }
                    if (Ends("anci")) { R("ance"); break; }
                    break;
                case 'e':
                    if (Ends("izer")) { R("ize"); break; }
                    break;
                case 'l':
                    if (Ends("bli")) { R("ble"); break; }
                    if (Ends("alli")) { R("al"); break; }
                    if (Ends("entli")) { R("ent"); break; }
                    if (Ends("eli")) { R("e"); break; }
                    if (Ends("ousli")) { R("ous"); break; }
                    break;
                case 'o':
                    if (Ends("ization")) { R("ize"); break; }
                    if (Ends("ation")) { R("ate"); break; }
                    if (Ends("ator")) { R("ate"); break; }
                    break;
                case 's':
                    if (Ends("alism")) { R("al"); break; }
                    if (Ends("iveness")) { R("ive"); break; }
                    if (Ends("fulness")) { R("ful"); break; }
                    if (Ends("ousness")) { R("ous"); break; }
                    break;
                case 't':
                    if (Ends("aliti")) { R("al"); break; }
                    if (Ends("iviti")) { R("ive"); break; }
                    if (Ends("biliti")) { R("ble"); break; }
                    break;
                case 'g':
                    if (Ends("logi")) { R("log"); break; }
                    break;
                default:
                    break;
            }
        }

        /* step4() deals with -ic-, -full, -ness etc. similar strategy to step3. */
        private void Step4()
        {
            switch (b[k])
            {
                case 'e':
                    if (Ends("icate")) { R("ic"); break; }
                    if (Ends("ative")) { R(""); break; }
                    if (Ends("alize")) { R("al"); break; }
                    break;
                case 'i':
                    if (Ends("iciti")) { R("ic"); break; }
                    break;
                case 'l':
                    if (Ends("ical")) { R("ic"); break; }
                    if (Ends("ful")) { R(""); break; }
                    break;
                case 's':
                    if (Ends("ness")) { R(""); break; }
                    break;
            }
        }

        /* step5() takes off -ant, -ence etc., in context <c>vcvc<v>. */
        private void Step5()
        {
            if (k == 0)
                return;

            /* for Bug 1 */
            switch (b[k - 1])
            {
                case 'a':
                    if (Ends("al")) break; return;
                case 'c':
                    if (Ends("ance")) break;
                    if (Ends("ence")) break; return;
                case 'e':
                    if (Ends("er")) break; return;
                case 'i':
                    if (Ends("ic")) break; return;
                case 'l':
                    if (Ends("able")) break;
                    if (Ends("ible")) break; return;
                case 'n':
                    if (Ends("ant")) break;
                    if (Ends("ement")) break;
                    if (Ends("ment")) break;
                    /* element etc. not stripped before the m */
                    if (Ends("ent")) break; return;
                case 'o':
                    if (Ends("ion") && j >= 0 && (b[j] == 's' || b[j] == 't')) break;
                    /* j >= 0 fixes Bug 2 */
                    if (Ends("ou")) break; return;
                /* takes care of -ous */
                case 's':
                    if (Ends("ism")) break; return;
                case 't':
                    if (Ends("ate")) break;
                    if (Ends("iti")) break; return;
                case 'u':
                    if (Ends("ous")) break; return;
                case 'v':
                    if (Ends("ive")) break; return;
                case 'z':
                    if (Ends("ize")) break; return;
                default:
                    return;
            }
            if (M() > 1)
                k = j;
        }

        /* step6() removes a final -e if m() > 1. */
        private void Step6()
        {
            j = k;

            if (b[k] == 'e')
            {
                int a = M();
                if (a > 1 || a == 1 && !Cvc(k - 1))
                    k--;
            }
            if (b[k] == 'l' && Doublec(k) && M() > 1)
                k--;
        }

        /** Stem the word placed into the Stemmer buffer through calls to add().
         * Returns true if the stemming process resulted in a word different
         * from the input.  You can retrieve the result with
         * getResultLength()/getResultBuffer() or toString().
         */
        public void Stem()
        {
            k = i - 1;
            if (k > 1)
            {
                Step1();
                Step2();
                Step3();
                Step4();
                Step5();
                Step6();
            }
            i_end = k + 1;
            i = 0;
        }
    }
}