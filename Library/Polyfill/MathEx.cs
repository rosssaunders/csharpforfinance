namespace System
{
    public static class MathEx
    {
        public static int DivRem(int a, int b, out int result)
        {

            result = a % b;

            return a / b;

        }

        public static long DivRem(long a, long b, out long result)
        {

            result = a % b;

            return a / b;

        }
    }
}