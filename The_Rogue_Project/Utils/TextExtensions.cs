public static class TextExtensions
{
    // 텍스트 출력 string 확장 매소드 글자색, 배경색 설정
    public static void Print(this string text,
        ConsoleColor forColor = ConsoleColor.Gray,
        ConsoleColor backColor = ConsoleColor.Black)
    {
        if (forColor != ConsoleColor.Gray || backColor != ConsoleColor.Black)
        {
            Console.ForegroundColor = forColor;
            Console.BackgroundColor = backColor;
        }
        Console.Write(text);
        if (forColor != ConsoleColor.Gray || backColor != ConsoleColor.Black)
            Console.ResetColor();
    }

    public static void Print(this char text,
        ConsoleColor forColor = ConsoleColor.Gray,
        ConsoleColor backColor = ConsoleColor.Black)
    {
        if (forColor != ConsoleColor.Gray || backColor != ConsoleColor.Black)
        {
            Console.ForegroundColor = forColor;
            Console.BackgroundColor = backColor;
        }
        Console.Write(text);
        if (forColor != ConsoleColor.Gray || backColor != ConsoleColor.Black)
            Console.ResetColor();
    }

    // 텍스트에 글자가 몇개인지 반환하는 함수
    public static int GetTextWidth(this string text)
    {
        int width = 0;
        foreach (char c in text)
        {
            width += c.GetCharacterWidth();
        }
        return width;
    }

    // 한글 = 2, 영어 = 1 이기 때문에 각자 글자 단위로 따로 계산하여 반환 하는 함수
    public static int GetCharacterWidth(this char character)
    {
        // 한글 음절(가-힣), CJK 호환문자, 전각 기호/문자 범위는 2칸으로 처리
        if ((character >= '\uAC00' && character <= '\uD7A3') || // 한글 완성형
            (character >= '\u1100' && character <= '\u11FF') || // 한글 자모
            (character >= '\u3130' && character <= '\u318F') || // 한글 호환 자모
            (character >= '\uFF01' && character <= '\uFF60') || // 전각 기호/영숫자
            (character >= '\uFFE0' && character <= '\uFFE6'))   // 전각 특수기호
        {
            return 2;
        }
        else
        {
            return 1;
        }
    }
}