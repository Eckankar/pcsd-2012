using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KeyValueBase.Interfaces;

namespace KeyValueBase {
    public class ValueListSerializerImpl : IValueSerializer<ValueListImpl> {
        ValueSerializerImpl serializer;

        public ValueListSerializerImpl() {
            serializer = new ValueSerializerImpl();
        }

        public ValueListImpl FromByteArray(byte[] array) {
            ValueListImpl list = new ValueListImpl();

            for (int i = 0; i < array.Length; i += 4) {
                byte[] data = new byte[4];
                Array.Copy(array, i, data, 0, 4);

                list.Add(serializer.FromByteArray(data));
            }

            return list;
        }

        public byte[] ToByteArray(ValueListImpl values) {
            List<ValueImpl> list = new List<ValueImpl>(values);

            byte[] data = new byte[4 * list.Count];
            for (int i = 0; i < list.Count; i++) {
                byte[] elm = serializer.ToByteArray(list[i]);
                Array.Copy(elm, 0, data, i * 4, 4);
            }

            return data;
        }
    }
}