namespace AdventOfCode2024.Days;

public class Day1(IInputAgent agent, IResultPrinter resultPrinter) : IDay
{
    public async Task Part1()
    {
        List<string> fileContent = await agent.GetInputLines(1);
        int[] left = new int[1000];
        int[] right = new int[1000];
        for (var index = 0; index < fileContent.Count; index++)
        {
            string line = fileContent[index];
            left[index] = int.Parse(line[..5]);
            right[index] = int.Parse(line[8..]);
        }

        Quick_Sort(left, 0 ,999);
        Quick_Sort(right, 0 ,999);

        int sum = 0;
        for (var i = 0; i < left.Length; i++)
        {
            int leftItem = left[i];
            int rightItem = right[i];

            sum += Math.Abs(leftItem - rightItem);
        }

        resultPrinter.Print(1, 1, sum);
    }

    public async Task Part2()
    {
        List<string> fileContent = await agent.GetInputLines(1);
        long sum = 0;
        List<int> left = [];
        List<int> right = [];
        foreach (string line in fileContent)
        {
            string[] parts = line.Split("   ");
            left.Add(int.Parse(parts[0]));
            right.Add(int.Parse(parts[1]));
        }

        foreach (int leftItem in left)
        {
            IEnumerable<int> rightItems = right.Where(r => r == leftItem);
            sum += leftItem * rightItems.Count();
        }


        resultPrinter.Print(1, 2, sum);
    }
    private static void Quick_Sort(int[] arr, int left, int right) 
    {
        // Check if there are elements to sort
        if (left < right)
        {
            // Find the pivot index
            int pivot = Partition(arr, left, right);

            // Recursively sort elements on the left and right of the pivot
            if (pivot > 1) {
                Quick_Sort(arr, left, pivot - 1);
            }
            if (pivot + 1 < right) {
                Quick_Sort(arr, pivot + 1, right);
            }
        }
    }

    // Method to partition the array
    private static int Partition(int[] arr, int left, int right)
    {
        // Select the pivot element
        int pivot = arr[left];

        // Continue until left and right pointers meet
        while (true) 
        {
            // Move left pointer until a value greater than or equal to pivot is found
            while (arr[left] < pivot) 
            {
                left++;
            }

            // Move right pointer until a value less than or equal to pivot is found
            while (arr[right] > pivot)
            {
                right--;
            }

            // If left pointer is still smaller than right pointer, swap elements
            if (left < right)
            {
                if (arr[left] == arr[right]) return right;

                int temp = arr[left];
                arr[left] = arr[right];
                arr[right] = temp;
            }
            else 
            {
                // Return the right pointer indicating the partitioning position
                return right;
            }
        }
    }
}