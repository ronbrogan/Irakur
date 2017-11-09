using Irakur.Core.Attributes;
using Irakur.Font.Formats.TTF.Tables.Kerning.Subtables;
using System;
using System.Collections.Generic;
using System.Text;

namespace Irakur.Font.Formats.TTF.Tables.Kerning
{
    public class KerningTable : FontTableBase
    {
        public ushort Version { get; set; }
        public ushort NumberOfTables { get; set; }

        private List<KerningSubtableBase> Subtables { get; set; }

        public override void Process(TrueTypeFont font)
        {
            var reader = new TrueTypeReader(Data);

            Version = reader.ReadUShort();
            NumberOfTables = reader.ReadUShort();

            Subtables = new List<KerningSubtableBase>(NumberOfTables);

            for(var t = 0; t < NumberOfTables; t++)
            {
                var subtableOffset = reader.Position;
                var version = reader.ReadUShort();
                var length = reader.ReadUShort();
                var coverage = reader.ReadUShort();

                var subtableType = ImplementationTypeAttribute.GetTypeFromEnumValue(typeof(KerningSubtableType), GetSubtypeFromCoverage(coverage));

                var subtable = Activator.CreateInstance(subtableType) as KerningSubtableBase;
                subtable.Offset = (uint)subtableOffset;
                subtable.Length = length;
                subtable.ReadData(reader);

                Subtables.Add(subtable);
            }

            ProcessSubtables(font);

            reader.Dispose();
        }

        public short GetKernUnits(ushort left, ushort right)
        {
            foreach(var table in Subtables)
            {
                if (table == null)
                    continue;

                return table.GetKerning(left, right);
            }

            return 0;
        }

        private KerningSubtableType GetSubtypeFromCoverage(ushort coverage)
        {
            var subtableType = coverage & 0xFF00;

            return (KerningSubtableType)subtableType;
        }

        private void ProcessSubtables(TrueTypeFont font)
        {
            foreach (var subtable in Subtables)
            {
                subtable.Process(font);

            }
        }
    }
}
