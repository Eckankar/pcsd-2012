using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeyValueBase.Interfaces;
using System.Runtime.InteropServices;

namespace KeyValueBase {
    public class ValueSerializerImpl : IValueSerializer<ValueImpl> {
        public ValueImpl FromByteArray(byte[] array) {
            if (array.Length != 4)
                throw new ArgumentException("Length must be 4", "array");
            unsafe {
                fixed (byte* arrayPtr = array) {
                    return new ValueImpl(*(int*)arrayPtr);
                }
            }
        }

        public byte[] ToByteArray(ValueImpl v) {
            byte[] result = new byte[4];
            unsafe {
                int value = v.Value;
                Marshal.Copy((IntPtr)(&value), result, 0, 4);
            }
            return result;
        }
    }
}
