namespace Irakur.Core.Extensions
{
    public static class IntegerExtensions
    {
        public static ushort Sum(this ushort left, short right)
        {
            if(right <= 0)
            {
                return (ushort)(left - (ushort)(-1 * right));
            }
            else
            {
                return (ushort)(left + (ushort)right);
            }
        }

    }
}
