using System.Collections.Generic;
using System.Diagnostics;

namespace Irakur.Font.Formats.TTF.Tables.Kerning.Subtables
{
    public class OrderedKernPairSubtable : KerningSubtableBase
    {
        public override KerningSubtableType Type => KerningSubtableType.OrderedKernPairs;

        public ushort NumberOfPairs { get; set; }
        private ushort SearchRange { get; set; }
        private ushort EntrySelector { get; set; }
        private ushort RangeShift { get; set; }

        private const int BytesPerKernpair = 6;
        private const int BytesInHeader = 14;

        private Dictionary<uint, short> Kernpairs { get; set; }

        public override void ReadData(TrueTypeReader reader, uint offset, uint length)
        {
            // This weirdness is because some fonts have a malformed subtable header that lists the length incorrectly.
            // The actual length can be calculated by peaking the number of kern pairs, and calculating it back.

            reader.Seek(offset);

            var version = reader.ReadUShort();
            var len = reader.ReadUShort();
            var coverage = reader.ReadUShort();

            var numberOfPairs = reader.ReadUShort();
            var kernpairLength = numberOfPairs * BytesPerKernpair;
            var calculatedLength = kernpairLength + BytesInHeader;

            if (calculatedLength != len)
                Debug.WriteLine("ERROR: Detected kern subtable length does not match specified value. Using detected value.");

            base.ReadData(reader, offset, (uint)calculatedLength);
        }

        public override void Process(TrueTypeFont font)
        {
            var reader = new TrueTypeReader(Data);

            var version = reader.ReadUShort();
            var length = reader.ReadUShort();
            var coverage = reader.ReadUShort();

            NumberOfPairs = reader.ReadUShort();
            SearchRange = reader.ReadUShort();
            EntrySelector = reader.ReadUShort();
            RangeShift = reader.ReadUShort();

            Kernpairs = new Dictionary<uint, short>(NumberOfPairs);

            for (var i = 0; i < NumberOfPairs; i++)
            {
                var glyphPair = reader.ReadULong();
                var kernUnits = reader.ReadFWord();

                Kernpairs[glyphPair] = kernUnits;
            }

            reader.Dispose();
        }

        public override short GetKerning(ushort left, ushort right)
        {
            return Kernpairs[(uint)((left << 16) | right)];
        }
    }
}
