using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;

namespace KeyValueBase.Interfaces
{
  public class MemoryMappedFilePinnable : IDisposable
  {
    private readonly MemoryMappedFile file;
    private readonly SortedDictionary<PinnedRegion, byte[]> buffer;
    private readonly long size;
    private MemoryMappedViewAccessor accessor;

    public MemoryMappedFilePinnable(MemoryMappedFile file, long size)
    {
      if (file == null)
        throw new ArgumentNullException("file");
      if (size <= 0)
        throw new ArgumentOutOfRangeException("size");

      this.file = file;
      this.size = size;
      this.buffer = new SortedDictionary<PinnedRegion, byte[]>();
    }

    public void Write(long offset, byte[] data)
    {
      if (offset < 0)
        throw new ArgumentOutOfRangeException("offset");
      if (data == null)
        throw new ArgumentNullException("data");
      if (offset + data.Length > this.size)
        throw new ArgumentException("Exceeding total size");

      EnsureAccessorCreated();
      for (long i = 0; i < data.Length; i++)
      {
        this.accessor.Write(offset + i, data[i]);
      }
    }

    public void WritePinned(long offset, byte[] data)
    {
      if (offset < 0)
        throw new ArgumentOutOfRangeException("offset");
      if (data == null)
        throw new ArgumentNullException("data");
      if (offset + data.Length > this.size)
        throw new ArgumentException("Exceeding total size");

      PinnedRegion region = new PinnedRegion(offset, data.Length);
      foreach (PinnedRegion pinnedRegion in this.buffer.Keys)
      {
        if (pinnedRegion.Overlaps(region))
          throw new InvalidOperationException("Overlapping of pinned regions");
      }

      this.buffer.Add(region, data);
    }

    public byte[] Read(long offset, int length)
    {
      if (offset < 0)
        throw new ArgumentOutOfRangeException("offset");
      if (length < 0)
        throw new ArgumentNullException("length");
      if (offset + length > this.size)
        throw new ArgumentException("Exceeding total size");

      PinnedRegion region = new PinnedRegion(offset, length);

      // Try reading from the buffer first
      if (this.buffer.ContainsKey(region))
      {
        return this.buffer[region];
      }

      EnsureAccessorCreated();
      byte[] result = new byte[length];
      for (long i = 0; i < length; i++)
      {
        result[i] = this.accessor.ReadByte(offset + i);
      }
      return result;
    }

    public void Flush()
    {
      EnsureAccessorCreated();
      foreach (KeyValuePair<PinnedRegion, byte[]> pair in this.buffer)
      {
        Write(pair.Key.Offset, pair.Value);
      }
      this.buffer.Clear();
      this.accessor.Flush();
    }

    public void Unpin(long offset, int length)
    {
      if (offset < 0)
        throw new ArgumentOutOfRangeException("offset");
      if (length < 0)
        throw new ArgumentNullException("length");

      PinnedRegion region = new PinnedRegion(offset, length);
      if (!this.buffer.ContainsKey(region))
        throw new InvalidOperationException("No pinned region found");

      byte[] data = this.buffer[region];
      this.buffer.Remove(region);
      Write(offset, data);
    }

    public void Dispose()
    {
      this.buffer.Clear();
      if (this.accessor != null)
      {
        this.accessor.Dispose();
      }
    }

    private void EnsureAccessorCreated()
    {
      if (this.accessor == null)
      {
        this.accessor = this.file.CreateViewAccessor();
      }
    }

    private class PinnedRegion : IComparable<PinnedRegion>, IEquatable<PinnedRegion>
    {
      private readonly long offset;
      private readonly int length;

      public PinnedRegion(long offset, int length)
      {
        if (offset < 0)
          throw new ArgumentOutOfRangeException();
        if (length <= 0)
          throw new ArgumentOutOfRangeException();

        this.offset = offset;
        this.length = length;
      }

      public long Offset
      {
        get { return this.offset; }
      }

      public int Length
      {
        get { return this.length; }
      }

      public bool Overlaps(PinnedRegion other)
      {
        if (other == null)
          throw new ArgumentNullException();

        long otherB = other.Offset + other.Length - 1;
        long thisB = Offset + Length - 1;
        return (other.Offset >= Offset && other.Offset <= thisB) ||
          (otherB >= Offset && otherB <= thisB);
      }

      public int CompareTo(PinnedRegion other)
      {
        if (other == null)
          throw new ArgumentNullException();
        return other.Offset.CompareTo(Offset);
      }

      public bool Equals(PinnedRegion other)
      {
        return other != null && other.offset == offset && other.length == length;
      }

      public override bool Equals(object obj)
      {
        return Equals(obj as PinnedRegion);
      }

      public override int GetHashCode()
      {
        return (int)offset ^ length;
      }
    }
  }
}
