using Irakur.Core.Attributes;
using Irakur.Font.Formats.TTF.Tables.Kerning.Subtables;

namespace Irakur.Font.Formats.TTF.Tables.Kerning
{
    public enum KerningSubtableType
    {
        [ImplementationType(typeof(OrderedKernPairSubtable))]
        OrderedKernPairs = 0,
        ContextualKerning = 1,
        TwoDArrayKernValues = 2,
        CompactTwoDKernValues = 3
    }
}
