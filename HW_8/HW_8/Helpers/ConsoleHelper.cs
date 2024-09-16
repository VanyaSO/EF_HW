namespace HW_8.Data;

public class ConsoleHelper
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
    public static int MultipleChoice(bool canCancel, Enum userEnum, int spacingPerLine = 18, int optionsPerLine = 3,
        int startX = 1, int startY = 1)
    {
        int currentSelection = 0;
        ConsoleKey key;
        Console.CursorVisible = false;
        int length = Enum.GetValues(userEnum.GetType()).Length;
        do
        {
            Console.Clear();
            if (currentSelection >= length)
            {
                currentSelection--;
            }

            for (int i = 0; i < length; i++)
            {
                Console.SetCursorPosition(startX + (i % optionsPerLine) * spacingPerLine, startY + i / optionsPerLine);

                if (i == currentSelection)
                    Console.ForegroundColor = ConsoleColor.Red;

                Console.Write(Enum.Parse(userEnum.GetType(), i.ToString()));

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

        return currentSelection;
    }
}