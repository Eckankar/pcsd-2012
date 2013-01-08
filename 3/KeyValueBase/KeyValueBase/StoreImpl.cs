using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.MemoryMappedFiles;
using KeyValueBase.Interfaces;

namespace KeyValueBase {
    public class StoreImpl : IStore {
        MemoryMappedViewAccessor file;

        public StoreImpl(long capacity) {
            file = MemoryMappedFile.CreateNew("data", capacity).CreateViewAccessor();
        }

        public byte[] Read(long position, int length) {
            lock (file) {
                byte[] output = new byte[length];
                file.ReadArray<byte>(position, output, 0, length);
                return output;
            }
        }

        public void Write(long position, byte[] value) {
            lock (file) {
                file.WriteArray<byte>(position, value, 0, value.Length);
            }
        }
    }
}
