namespace Irakur.Font.Formats.TTF.Tables.Kerning.Subtables
{
    public abstract class KerningSubtableBase : FontTableBase<KerningSubtableType>
    {
        public abstract short GetKerning(ushort left, ushort right);
    }
}
