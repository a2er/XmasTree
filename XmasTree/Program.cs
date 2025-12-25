using System.Text;
using XmasTree.Resources;

namespace XmasTree
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var random = new Random();
            var sb = new StringBuilder();

            // ----------------------------
            // 1. Greeting texts
            // ----------------------------
            string[] greetings =
            [
                Strings.MerryXmas,
                Strings.MerryChristmas,
                Strings.HappyChristmas,
                Strings.SeasonGreetings,
                Strings.JoyfulHolidays,
            ];
            int minRequiredWidth = greetings.Max(g => g.Length);

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
            sb.AppendLine();

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

            sb.AppendLine();

            // ----------------------------
            // 7. Blinking Christmas colors animation
            // ----------------------------
            ConsoleColor[] colors =
            [
                ConsoleColor.DarkRed,
                ConsoleColor.DarkGreen,
                ConsoleColor.DarkYellow,
            ];

            Console.CursorVisible = false;

            int lastColorIndex = -1;
            int consoleWidth = Console.WindowWidth;
            int leftMargin = Math.Max(0, (consoleWidth - maxWidth) / 2);

            do
            {
                int colorIndex = random.Next(colors.Length);
                if (colorIndex == lastColorIndex)
                {
                    colorIndex = (colorIndex + 1) % colors.Length;
                }
                ConsoleColor nextColor = colors[colorIndex];

                string greeting = greetings[random.Next(greetings.Length)];
                int greetingPadding = (maxWidth - greeting.Length) / 2;
                
                Console.Clear();
                Console.ForegroundColor = nextColor;

                string[] lines = sb.ToString().Split(Environment.NewLine);
                foreach (string line in lines)
                {
                    Console.SetCursorPosition(leftMargin, Console.CursorTop);
                    Console.WriteLine(line);
                }
                
                Console.SetCursorPosition(leftMargin + greetingPadding, Console.CursorTop);
                Console.WriteLine(greeting);
                
                lastColorIndex = colorIndex;
                Thread.Sleep(600);
            } while (!Console.KeyAvailable);

            Console.ResetColor();
            Console.ReadKey();
        }
    }
}
