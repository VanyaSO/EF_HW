using HW_8.Interfaces;

public class ItemsHelper
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="canCancel"></param>
    /// <param name="userEnum">Enum перечисление пользователя, по которому строим меню</param>
    /// <param name="spacingPerLine">Количество отступов между столбиками</param>
    /// <param name="optionsPerLine">Количество значений в одном столбике</param>
    /// <param name="startX">Количество отступов с левой стороны консоли</param>
    /// <param name="startY">Количество отступов с верхней стороны консоли</param>
    /// <returns></returns>
    public static int MultipleChoice<T>(bool canCancel, List<T> items, bool IsMenu = false, string message = null, int spacingPerLine = 18, int optionsPerLine = 3,
        int startX = 1, int startY = 1) where T : IShow<int>, new()
    {

        if (IsMenu)
        {
            items.Insert(0, new T() { Id = 0, Value = "...Back" });
        }
        int currentSelection = 0;
        int currentId = 0;
        ConsoleKey key;
        Console.CursorVisible = false;
        int length = items.Count;
        
        do
        {
            Console.Clear();
            if (message is not null)
            {
                Console.WriteLine(message);
            }
            
            if (currentSelection >= length)
            {
                currentSelection--;
            }

            for (int i = 0; i < length; i++)
            {
                Console.SetCursorPosition(startX + (i % optionsPerLine) * spacingPerLine, startY + i / optionsPerLine);

                if (i == currentSelection)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    currentId = items[i].Id;
                }

                Console.Write(items[i].Value);

                Console.ResetColor();
            }

            key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.LeftArrow:
                {
                    if (currentSelection % optionsPerLine > 0)
                        currentSelection--;
                    break;
                }
                case ConsoleKey.RightArrow:
                {
                    if (currentSelection % optionsPerLine < optionsPerLine - 1)
                        currentSelection++;
                    break;
                }
                case ConsoleKey.UpArrow:
                {
                    if (currentSelection >= optionsPerLine)
                        currentSelection -= optionsPerLine;
                    break;
                }
                case ConsoleKey.DownArrow:
                {
                    if (currentSelection + optionsPerLine < length)
                        currentSelection += optionsPerLine;
                    break;
                }
                case ConsoleKey.Escape:
                {
                    if (canCancel)
                        return -1;
                    break;
                }
            }
        } while (key != ConsoleKey.Enter);

        Console.CursorVisible = true;
        Console.WriteLine("\n");
        return currentId;
    }
}