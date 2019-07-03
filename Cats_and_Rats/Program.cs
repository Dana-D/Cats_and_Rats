/*
 * Problem source: https://www.codechef.com/problems/CATSRATS
 */


using System;
using System.Collections;

namespace Cats_and_Rats
{
    class Program
    {
        public static void Main(string[] args)
        {
            string input =
@"2
8 7
2 5 1
1 4 1
9 14 10
20 7 9
102 99 1
199 202 1
302 299 3
399 402 3
6 3 1
10 15 10
100 101 1
201 200 1
300 301 5
401 400 5
1000 1010 1020
8 8
2 8 2
12 18 2
22 28 4
32 38 4
48 42 2
58 52 3
68 62 1
78 72 3
3 6 3
13 19 3
21 25 3
31 39 3
46 43 4
59 53 2
65 61 4
79 71 2";

            string input2 =
@"1
1 1
20 7 9
10 15 10";
            ArrayList tests = populateModels(input);
            foreach(Test t in tests)
            {
                runTest(t);
            }

            foreach (Test t in tests)
            {
                foreach(Animal rat in t.rats)
                {
                    Console.WriteLine(rat.eatenBy);
                }
            }
        }

        public static ArrayList populateModels(string input)
        {
            string[] inputs = input.Split('\n');
            int numOfTests = Int32.Parse(inputs[0]);
            int numOfCats = 0;
            int numOfRats = 0;
            ArrayList tests = new ArrayList();
            Test test = new Test(0, 0);

            for (int t = 1; t < inputs.Length; t++)
            {
                
                string[] line = inputs[t].Split(' ');
                if(line.Length == 2)
                {
                    //Add previous test to the list
                    if(test.cats.Length > 0 || test.rats.Length > 0)
                    {
                        tests.Add(test);
                    }
                    int cats = Int32.Parse(line[0]);
                    int rats = Int32.Parse(line[1]);
                    numOfCats = cats;
                    numOfRats = rats;
                    test = new Test(cats, rats);
                }
                else if (line.Length == 3)
                {
                    int initial = Int32.Parse(line[0]);
                    int final = Int32.Parse(line[1]);
                    int time = Int32.Parse(line[2]);
                    Animal animal = new Animal(initial, final, time);
                    if(numOfCats > 0)
                    {
                        test.cats[test.cats.Length - numOfCats] = animal;
                        numOfCats--;
                    }
                    else
                    {
                        test.rats[test.rats.Length - numOfRats] = animal;
                        numOfRats--;
                    }
                }
                else
                {
                    //Do nothing because what's going on with inputs?!
                }
            }
            //Add the final test to the list
            tests.Add(test);
            return tests;
        }

        public static void runTest(Test t)
        {
            bool done = false;
            double time = 0.0;
            while (!done)
            {
                done = true;
                //Make our steps
                foreach (Animal cat in t.cats)
                {
                    cat.step(time);
                    if (cat.current_location != cat.final_location)
                    {
                        done = false;
                    }
                }
                foreach (Animal rat in t.rats)
                {
                    rat.step(time);
                }

                    //we check by rat
                for (int r = 0; r < t.rats.Length; r++)
                {
                    //Don't check eaten rats cuz they be ate.
                    if (!t.rats[r].eaten)
                    {
                        for (int c = 0; c < t.cats.Length; c++)
                        {
                            if (t.cats[c].isAwake && t.cats[c].current_location == t.rats[r].current_location)
                            {
                                t.rats[r].eatenBy = c + 1;
                                t.rats[r].eaten = true;
                            }
                        }
                    }
                }

                foreach (Animal cat in t.cats)
                {
                    cat.finishCheck();
                }
                foreach (Animal rat in t.rats)
                {
                    rat.finishCheck();
                }

                time = time + 0.5;
            }
            
        }
         
    }

    class Test
    {
        public Animal[] cats;
        public Animal[] rats;

        public Test(int c, int r)
        {
            cats = new Animal[c];
            rats = new Animal[r];
        }

    }

    class Animal
    {
        public double initial_location;
        public double final_location;
        public double current_location;
        public double start_time;
        public double end_time;
        public int eatenBy;
        public bool isAwake;
        public bool eaten;

        public Animal(int i, int f, int start)
        {
            initial_location = i;
            current_location = i;
            final_location = f;
            start_time = start;
            end_time = start + Math.Abs(i - f);
            eaten = false;
            eatenBy = -1;
            isAwake = false;
        }

        //Need to fix arriving the same time, should be eaten.
        //Current problem: rat going 14 to 15 and cat going 14 to 15 should eat the rat but does not.
        public void step(double currentTime)
        {
            if(start_time == currentTime)
            {
                isAwake = true;
            }
            if(isAwake && !eaten && currentTime >= start_time)
            {
                if (current_location > final_location)
                {
                    current_location -= 0.5;
                }
                else if (current_location < final_location)
                {
                    current_location += 0.5;
                }
                
            }
        }

        public void finishCheck()
        {
            if (current_location == final_location)
            {
                isAwake = false;
            }
        }

    }
}
