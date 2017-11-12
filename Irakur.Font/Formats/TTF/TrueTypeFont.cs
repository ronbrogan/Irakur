using Irakur.Core.Attributes;
using Irakur.Core.Extensions;
using Irakur.Font.Formats.TTF.Tables;
using Irakur.Font.Formats.TTF.Tables.CharacterToGlyph;
using Irakur.Font.Formats.TTF.Tables.Glyph;
using Irakur.Font.Formats.TTF.Tables.Header;
using Irakur.Font.Formats.TTF.Tables.HorizontalHeader;
using Irakur.Font.Formats.TTF.Tables.HorizontalMetrics;
using Irakur.Font.Formats.TTF.Tables.IndexToLocation;
using Irakur.Font.Formats.TTF.Tables.Kerning;
using Irakur.Font.Formats.TTF.Tables.MaximumProfile;
using System;
using System.Collections.Generic;
using System.IO;

namespace Irakur.Font.Formats.TTF
{
    public class TrueTypeFont : IFont
    {
        public FontFormat Format => FontFormat.TTF;
        public string Name { get; set; }
        public string Version { get; set; }

        /*
         * TTF Specific Properties 
         * */
        private ushort NumberOfTables { get; set; }
        private ushort SearchRange { get; set; }
        private ushort EntrySelector { get; set; }
        private ushort RangeShift { get; set; }
        private List<FontTableBase> Tables { get; set; }

        /*
         * Tables
         * */
        internal CharacterToGlyphTable cmap { get; set; }
        internal HeaderTable head { get; set; }
        internal HorizontalHeaderTable hhea { get; set; }
        internal HorizontalMetricsTable hmtx { get; set; }
        internal KerningTable kern { get; set; }
        internal MaximumProfileTable maxp { get; set; }
        internal IndexToLocationTable loca { get; set; }
        internal GlyphTable glyf { get; set; }

        public TrueTypeFont(Stream fontStream)
        {
            var reader = new TrueTypeReader(fontStream);

            Version = GetVersion(reader);
            this.NumberOfTables = reader.ReadUShort();
            this.SearchRange = reader.ReadUShort();
            this.EntrySelector = reader.ReadUShort();
            this.RangeShift = reader.ReadUShort();

            var tableEntries = new List<FontTableHeaderEntry>();
            for (var i = 0; i < this.NumberOfTables; i++)
            {
                tableEntries.Add(new FontTableHeaderEntry()
                {
                    Type = (FontTableType)reader.ReadULong(),
                    Checksum = reader.ReadULong(),
                    Offset = reader.ReadULong(),
                    Length = reader.ReadULong()
                });
            }

            Tables = new List<FontTableBase>(this.NumberOfTables);
            foreach (var entry in tableEntries)
            {
                var tableType = ImplementationTypeAttribute.GetTypeFromEnumValue(typeof(FontTableType), entry.Type);

                if (tableType == null)
                    continue;

                var table = Activator.CreateInstance(tableType) as FontTableBase;
                table.HeaderEntry = entry;
                table.Checksum = entry.Checksum;
                table.ReadData(reader, entry.Offset, entry.Length);
                Tables.Add(table);
            }

            reader.Dispose();

            ProcessTables();
        }

        private void ProcessTables()
        {
            // TODO: These may need to be Processed() in a certain order, for example the horizontal metrics table
            // 'needs' to know the number of metrics from the horizontal header table.
            // However, it looks like the tables are listed in the proper order, so we get that for free. But that seems awfully fragile....
            foreach (var table in Tables)
            {
                // Call each table's process method to parse data
                table.Process(this);

                switch(table.Type)
                {
                    case FontTableType.CharacterToGlyphMap:
                        cmap = (CharacterToGlyphTable)table;
                        break;

                    case FontTableType.FontHeader:
                        head = (HeaderTable)table;
                        break;

                    case FontTableType.HorizontalHeader:
                        hhea = (HorizontalHeaderTable)table;
                        break;

                    case FontTableType.HorizontalMetrics:
                        hmtx = (HorizontalMetricsTable)table;
                        break;

                    case FontTableType.Kerning:
                        kern = (KerningTable)table;
                        break;

                    case FontTableType.MaximumProfile:
                        maxp = (MaximumProfileTable)table;
                        break;

                    case FontTableType.IndexToLocation:
                        loca = (IndexToLocationTable)table;
                        break;

                    case FontTableType.GlyphData:
                        glyf = (GlyphTable)table;
                        break;
                }
            }
        }

        private string GetVersion(TrueTypeReader reader)
        {
            reader.Seek(0);

            var versionBuf = reader.ReadFixed();

            return BitConverter.ToString(versionBuf).Replace("-", "");
        }

        public ushort GetUnitWidthOfChar(char c)
        {
            var glyphId = cmap.GetGlyphId((ushort)c);

            var metric = hmtx.Metrics[glyphId];

            return metric.AdvanceWidth;
        }

        public ushort GetUnitWidthOfString(string s)
        {
            var ids = new ushort[s.Length];
            var metrics = new HorizontalMetric[s.Length];
            var rsbs = new ushort[s.Length];

            for(var i = 0; i < s.Length; i++)
            {
                ids[i] = cmap.GetGlyphId(s[i]);
                metrics[i] = hmtx.Metrics[ids[i]];
                var glyphData = glyf.GetGlyphData(ids[i]);

                rsbs[i] = (ushort)(metrics[i].AdvanceWidth - (metrics[i].LeftSideBearing + glyphData.XMax - glyphData.XMin));
            }

            ushort width = 0;

            for (var i = 0; i < s.Length; i++)
            {
                width += metrics[i].AdvanceWidth;
                width = width.Sum(metrics[i].LeftSideBearing);
                width += rsbs[i];
            }
            
            var kerning = GetTotalKernOfString(s);

            width = width.Sum(kerning);

            return width;
        }

        public float GetEmWidthOfString(string s, float em = 1)
        {
            var units = GetUnitWidthOfString(s);

            return UnitsToEm(units) * em;
        }

        public float GetPointWidthOfString(string s, float point)
        {
            var units = GetUnitWidthOfString(s);

            var ems = UnitsToEm(units) * FontUnitConverter.PointsToEm(point);

            return FontUnitConverter.EmToPoints(ems);
        }

        public float GetPixelWidthOfString(string s, float point)
        {
            var units = GetUnitWidthOfString(s);

            var ems = UnitsToEm(units) * FontUnitConverter.PointsToEm(point);

            return FontUnitConverter.EmToPixels(ems);
        }

        public float UnitsToEm(uint units)
        {
            return (units / (float)head.UnitsPerEm);
        }

        private short GetTotalKernOfString(string s)
        {
            short aggregateKern = 0;

            for (var i = 0; i < s.Length - 1; i++)
            {
                aggregateKern += GetKernUnits(s[i], s[i + 1]);
            }

            return aggregateKern;
        }

        private short GetKernUnits(char l, char r)
        {
            var leftId = cmap.GetGlyphId(l);
            var rightId = cmap.GetGlyphId(r);

            return kern.GetKernUnits(leftId, rightId);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    foreach(var table in this.Tables)
                    {
                        table.Dispose();
                    }

                    this.cmap.Dispose();
                    this.hhea.Dispose();
                    this.hmtx.Dispose();
                    this.head.Dispose();
                    this.kern.Dispose();
                }

                // TODO: set large fields to null.

                disposedValue = true;
            }
        }


        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}
