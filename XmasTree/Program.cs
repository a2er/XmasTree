using System.Text;
using XmasTree.Resources;

namespace XmasTree
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;

            var random = new Random();
            var sb = new StringBuilder();

            // ----------------------------
            // 1. Greeting text
            // ----------------------------
            string[] greetings =
            [
                Strings.MerryXmas,
                Strings.MerryChristmas,
                Strings.HappyChristmas,
                Strings.SeasonGreetings,
                Strings.JoyfulHolidays,
            ];

            string greeting = greetings[random.Next(greetings.Length)];
            int minRequiredWidth = greeting.Length;

            // ----------------------------
            // 2. Smooth tree tip
            // ----------------------------
            var treeWidths = new List<int>();
            int tipLevels = random.Next(3, 5); // 1,3,5,(7)

            for (int i = 0; i < tipLevels; i++)
            {
                treeWidths.Add(1 + i * 2);
            }

            // ----------------------------
            // 3. Randomized body
            // ----------------------------
            int segments = random.Next(1, 4);
            int baseWidth = treeWidths.Last() + random.Next(2, 4);

            for (int s = 0; s < segments; s++)
            {
                int height = random.Next(3, 6);
                int width = baseWidth;

                for (int i = 0; i < height; i++)
                {
                    treeWidths.Add(width);
                    width += random.Next(2, 4);
                }

                baseWidth = width - random.Next(2, 5);
            }

            // ----------------------------
            // 4. Enforce minimum width
            // ----------------------------
            int maxWidth = treeWidths.Max();

            if (maxWidth < minRequiredWidth)
            {
                int delta = minRequiredWidth - maxWidth;

                // Grow lower half of the tree only (keeps tip elegant)
                for (int i = treeWidths.Count / 2; i < treeWidths.Count; i++)
                {
                    treeWidths[i] += delta;
                }

                maxWidth = treeWidths.Max();
            }

            // ----------------------------
            // 5. Render tree
            // ----------------------------
            foreach (int width in treeWidths)
            {
                int padding = (maxWidth - width) / 2;
                sb.Append(' ', padding);
                sb.Append('X', width);
                sb.AppendLine();
            }

            // ----------------------------
            // 6. Trunk
            // ----------------------------
            int trunkWidth = Math.Max(3, maxWidth / 6);
            if (trunkWidth % 2 == 0) trunkWidth++;

            int trunkHeight = random.Next(3, 5);
            int trunkPadding = (maxWidth - trunkWidth) / 2;

            for (int i = 0; i < trunkHeight; i++)
            {
                sb.Append(' ', trunkPadding);
                sb.Append('#', trunkWidth);
                sb.AppendLine();
            }

            // ----------------------------
            // 7. Greeting
            // ----------------------------
            sb.AppendLine();
            sb.Append(' ', (maxWidth - greeting.Length) / 2);
            sb.AppendLine(greeting);

            Console.WriteLine(sb.ToString());
            Console.ResetColor();
            Console.ReadKey();
        }
    }
}
