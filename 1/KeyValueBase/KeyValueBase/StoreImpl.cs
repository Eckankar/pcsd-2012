using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.MemoryMappedFiles;
using KeyValueBase.Interfaces;

namespace KeyValueBase {
    public class StoreImpl : IStore {
        MemoryMappedViewAccessor file;

        public StoreImpl(int capacity) {
            file = MemoryMappedFile.CreateNew("data", capacity).CreateViewAccessor();
        }

        public byte[] Read(long position, int length) {
            lock (file) {
                byte[] output = new byte[length];
                file.ReadArray<byte>(0, output, (int)position, length);
                return output;
            }
        }

        public void Write(long position, byte[] value) {
            lock (file) {
                file.WriteArray<byte>(0, value, (int)position, value.Length);
            }
        }
    }
}
