using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeyValueBaseClient.KeyValueBaseReference;

namespace KeyValueBaseClient {
    internal class Program {
        public static void Main(string[] args) {
            KeyValueBaseOf_KeyImpl_ValueListImplClient client = new KeyValueBaseOf_KeyImpl_ValueListImplClient();

            while (true) {
                try {
                    Console.Write("Enter command: ");
                    string line = Console.ReadLine();
                    string[] parts = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length == 0) {
                        continue;
                    }
                    string cmd = parts[0];
                    if (cmd == "init") {
                        client.Init(line.Split(new char[] { ' ', '\t' }, 2, StringSplitOptions.RemoveEmptyEntries)[1]);
                    }
                    else if (cmd == "read") {
                        KeyImpl key = ParseKey(parts[1]);
                        Console.WriteLine(client.Read(key));
                    }
                    else if (cmd == "insert") {
                        KeyImpl key = ParseKey(parts[1]);
                        ValueListImpl values = ParseList(parts, 2);
                        client.Insert(key, values);
                    }
                    else if (cmd == "update") {
                        KeyImpl key = ParseKey(parts[1]);
                        ValueListImpl values = ParseList(parts, 2);
                        client.Update(key, values);
                    }
                    else if (cmd == "delete") {
                        KeyImpl key = ParseKey(parts[1]);
                        client.Delete(key);
                    }
                    else if (cmd == "close" || cmd == "quit" || cmd == "exit") {
                        break;
                    }
                    else
                        Console.WriteLine("?");
                }
                catch (Exception e) {
                    Console.WriteLine(e.ToString());
                }
            }
            
            client.Close();
        }

        private static KeyImpl ParseKey(string val) {
            return new KeyImpl() { Key = Int32.Parse(val) };
        }

        private static ValueListImpl ParseList(string[] parts, int offset) {
            return new ValueListImpl() { 
                List = parts.Skip(offset)
                    .Select(s => new ValueImpl() { Value = Int32.Parse(s) }).ToArray() };
        }
    }
}
