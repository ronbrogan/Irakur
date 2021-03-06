﻿using Irakur.Core.CoordinateSystem;
using Irakur.Core.Reflection;
using Irakur.Pdf.Infrastructure.Collections;
using Irakur.Pdf.Infrastructure.PdfObjects;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Irakur.Pdf.Infrastructure.Serialization.Serdes
{
    internal static class SerdesFactory
    {
        private static ConcurrentDictionary<Type, IPdfSerdes> SerdesCache = new ConcurrentDictionary<Type, IPdfSerdes>();

        private static Dictionary<Type, Type> SerdesTypeLookup = new Dictionary<Type, Type>()
        {
            { typeof(Catalog),                  typeof(CatalogSerdes) },
            { typeof(ContentStream),            typeof(ContentStreamSerdes) },
            { typeof(Font),                     typeof(FontSerdes) },
            { typeof(PageNode),                 typeof(PageNodeSerdes) },
            { typeof(Page),                     typeof(PageSerdes) },
            { typeof(PdfTrailer),               typeof(PdfTrailerSerdes) },
            { typeof(Resources),                typeof(ResourcesSerdes) },
            { typeof(XrefTable),                typeof(XrefTableSerdes) },
            { typeof(Rectangle),                typeof(RectangleSerdes) },
            { typeof(ImageExternalObject),      typeof(ImageExternalObjectSerdes) }
        };

        public static IPdfSerdes GetFor(object item)
        {
            return GetSerdes(item.GetType());
        }

        public static IPdfSerdes Get<T>()
        {
            return GetSerdes(typeof(T));
        }

        private static IPdfSerdes GetSerdes(Type objectType)
        {
            var type = SerdesTypeLookup[objectType];

            if (SerdesCache.ContainsKey(type))
                return SerdesCache[type];

            var serdes = (IPdfSerdes)Instance.Create(type);

            SerdesCache.TryAdd(type, serdes);

            return serdes;
        }

    }
}