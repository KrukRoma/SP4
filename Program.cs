namespace SP4
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Task task1 = new Task(() =>
            {
                Console.WriteLine($"Method 1 (Task.Start): {DateTime.Now}");
            });
            task1.Start();

            Task task2 = Task.Factory.StartNew(() =>
            {
                Console.WriteLine($"Method 2 (Task.Factory.StartNew): {DateTime.Now}");
            });

            Task task3 = Task.Run(() =>
            {
                Console.WriteLine($"Method 3 (Task.Run): {DateTime.Now}");
            });

            Task.WaitAll(task1, task2, task3);

            Task primeTask = Task.Run(() =>
            {
                Console.WriteLine("Prime numbers from 0 to 1000:");
                for (int i = 2; i <= 1000; i++)
                {
                    if (IsPrime(i))
                    {
                        Console.Write($"{i} ");
                    }
                }
                Console.WriteLine();
            });
            primeTask.Wait(); 

            Task primeTaskWithBounds = Task.Run(() =>
            {
                int lowerBound = 0;
                int upperBound = 1000;
                int primeCount = 0;

                Console.WriteLine($"Prime numbers between {lowerBound} and {upperBound}:");
                for (int i = lowerBound; i <= upperBound; i++)
                {
                    if (IsPrime(i))
                    {
                        primeCount++;
                        Console.Write($"{i} ");
                    }
                }
                Console.WriteLine($"\nTotal primes: {primeCount}");
            });
            primeTaskWithBounds.Wait();

            int[] numbers = { 10, 15, 22, 7, 18, 30, 5 };

            Task<int> minTask = Task.Run(() => numbers.Min());
            Task<int> maxTask = Task.Run(() => numbers.Max());
            Task<double> averageTask = Task.Run(() => numbers.Average());
            Task<int> sumTask = Task.Run(() => numbers.Sum());

            Task.WaitAll(minTask, maxTask, averageTask, sumTask);

            Console.WriteLine($"Min: {minTask.Result}, Max: {maxTask.Result}, Average: {averageTask.Result}, Sum: {sumTask.Result}");

            int[] array = { 5, 3, 8, 3, 1, 5, 8, 10, 2 };

            Task<int[]> removeDuplicatesTask = Task.Run(() => array.Distinct().ToArray());
            Task<int[]> sortTask = removeDuplicatesTask.ContinueWith((t) =>
            {
                int[] distinctArray = t.Result;
                Array.Sort(distinctArray);
                return distinctArray;
            });

            Task binarySearchTask = sortTask.ContinueWith((t) =>
            {
                int[] sortedArray = t.Result;
                int searchValue = 8;
                int result = Array.BinarySearch(sortedArray, searchValue);
                if (result >= 0)
                    Console.WriteLine($"Value {searchValue} found at index {result}");
                else
                    Console.WriteLine($"Value {searchValue} not found");
            });

            binarySearchTask.Wait(); 
        }

        static bool IsPrime(int number)
        {
            if (number < 2) return false;
            for (int i = 2; i <= Math.Sqrt(number); i++)
            {
                if (number % i == 0) return false;
            }
            return true;
        }
    }
}

