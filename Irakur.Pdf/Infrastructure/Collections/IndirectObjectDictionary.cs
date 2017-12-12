using Irakur.Pdf.Infrastructure.Core;
using Irakur.Pdf.Infrastructure.PdfObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Irakur.Pdf.Infrastructure.Collections
{
    public class IndirectObjectDictionary : IEnumerable<IPdfObject>
    {
        private Dictionary<IndirectReference, IPdfObject> internalDictionary;
        private Dictionary<Guid, IndirectReference> guidToReferenceTable;
        private uint nextObjectNumber;

        public IndirectObjectDictionary()
        {
            this.internalDictionary = new Dictionary<IndirectReference, IPdfObject>();
            this.guidToReferenceTable = new Dictionary<Guid, IndirectReference>();
            this.nextObjectNumber = 1;
        }

        public IPdfObject this[uint objectNumber, uint generationNumber]
        {
            get
            {
                return internalDictionary[new IndirectReference(objectNumber, generationNumber)];
            }
            set
            {
                internalDictionary[new IndirectReference(objectNumber, generationNumber)] = value;
            }
        }

        public IPdfObject this[IndirectReference reference]
        {
            get
            {
                return internalDictionary[reference];
            }
            set
            {
                internalDictionary[reference] = value;
            }
        }

        public ICollection<IndirectReference> Keys => internalDictionary.Keys;

        public ICollection<IPdfObject> Values => internalDictionary.Values;

        public int Count => internalDictionary.Count;

        public IndirectReference Add(IPdfObject value) 
        {
            var indRef = new IndirectReference(nextObjectNumber++, 0);

            this[indRef] = value;
            guidToReferenceTable.Add(value.Id, indRef);

            return indRef;
        }

        public void Clear()
        {
            internalDictionary.Clear();
            guidToReferenceTable.Clear();
        }

        public bool Contains(uint objectNumber, uint generationNumber)
        {
            return internalDictionary.ContainsKey(new IndirectReference(objectNumber, generationNumber));
        }

        public bool Contains(IndirectReference reference)
        {
            return internalDictionary.ContainsKey(reference);
        }

        public bool Contains(IPdfObject obj)
        {
            return guidToReferenceTable.ContainsKey(obj.Id);
        }

        public IndirectReference GetReference(IPdfObject obj)
        {
            return guidToReferenceTable[obj.Id];
        }

        public IEnumerator GetEnumerator()
        {
            return Values.GetEnumerator();
        }

        public void Remove(uint objectNumber, uint generationNumber)
        {
            internalDictionary.Remove(new IndirectReference(objectNumber, generationNumber));
        }

        IEnumerator<IPdfObject> IEnumerable<IPdfObject>.GetEnumerator()
        {
            return Values.GetEnumerator();
        }
    }
}
