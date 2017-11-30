using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web;

namespace PortafolioWeb.CustomXML
{
    public class CustomXmlFormatter : BufferedMediaTypeFormatter
    {
        public CustomXmlFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/xml"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/xml"));
        }

        public override bool CanReadType(Type type)
        {
            return false;
        }

        public override bool CanWriteType(Type type)
        {
            return true;
        }

        public override void WriteToStream(Type type, object value, Stream writeStream, HttpContent content)
        {
            using (var writer = new StreamWriter(writeStream))
            {
                WriteAny(writer, type, value);
            }

        }

        private void WriteAny(StreamWriter writer, Type type, object value)
        {
            if (type.IsEnumerable()) WriteCollection(writer, type, value);
            else WriteObject(writer, type, value);
        }

        private void WriteObject(StreamWriter writer, Type type, object value)
        {
            var xml = value.ToXml();
            writer.Write(xml.ToString());
        }

        private void WriteCollection(StreamWriter writer, Type type, object value)
        {
            var collection = value as IEnumerable;
            writer.Write("<collection>");
            foreach (var item in collection)
            {
                if (item != null)
                {
                    WriteAny(writer, item.GetType(), item);
                }
            }
            writer.Write("</collection>");
        }
    }
}