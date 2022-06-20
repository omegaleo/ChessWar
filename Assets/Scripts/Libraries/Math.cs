namespace Libraries
{
    /// <summary>
    /// Custom class for math with integers
    /// </summary>
    public static class MathI
    {
        //Gets the difference between 2 numbers
        public static int Difference(int number1, int number2)
        {
            return number1 > number2 ? number1 - number2 : number2 - number1;
        }
    }
}