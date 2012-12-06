using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeyValueBaseClient.KeyValueBaseReference;

namespace KeyValueBaseClient {
    internal class Program {
        public static void Main(string[] args) {
            KeyValueBaseServiceClient client = new KeyValueBaseServiceClient();

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
                    else if (cmd == "reset") {
                        client.Reset();
                    }
                    else if (cmd == "read") {
                        KeyImpl key = ParseKey(parts[1]);
                        PrintList(client.Read(key));
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
                    else if (cmd == "scan" || cmd == "atomicscan") {
                        bool atomic = cmd == "atomicscan";
                        KeyImpl begin = ParseKey(parts[1]);
                        KeyImpl end = ParseKey(parts[2]);

                        string predicateString = line.Split(new char[] { ' ', '\t' }, 4, StringSplitOptions.RemoveEmptyEntries)[3];
                        object predicate = TokenizeAndParsePredicate(predicateString);
                        ValueListImpl[] lists;
                        if (atomic)
                            lists = client.AtomicScan(begin, end, predicate);
                        else
                            lists = client.Scan(begin, end, predicate);
                        foreach (ValueListImpl vl in lists)
                            PrintList(vl);
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

        private static object TokenizeAndParsePredicate(string p) {
            List<string> tokens = TokenizePredicate(p);
            return ParsePredicate(tokens, 0, tokens.Count - 1);
        }

        private static object ParsePredicate(List<string> tokens, int start, int end) {
            int predicatePos;
            object predicate = FindOp(tokens, start, end, out predicatePos);
            if (predicate == null && tokens[start] == "(") {
                if (tokens[end] != ")")
                    throw new Exception("Parse error: unmatched ')'");
                return ParsePredicate(tokens, start + 1, end - 1);
            }
            else if (predicate == null)
                throw new Exception("Parse error: missing operator");
            bool unary = !(predicate is OrPredicateOfValueListImpl5eNmv5yi 
                || predicate is AndPredicateOfValueListImpl5eNmv5yi);
            if (unary) {
                if (predicatePos != start)
                    throw new Exception(String.Format("Parse error: unexpected token '{0}'", tokens[start]));
                if (predicate is NotPredicateOfValueListImpl5eNmv5yi) {
                    object rhs = ParsePredicate(tokens, start + 1, end);
                    ((NotPredicateOfValueListImpl5eNmv5yi)predicate).Predicate = rhs;
                }
                else {
                    int value = Int32.Parse(tokens[start + 1]);
                    SetPredicateValue(predicate, value);
                }
            }
            else {
                object lhs = ParsePredicate(tokens, start, predicatePos - 1);
                object rhs = ParsePredicate(tokens, predicatePos + 1, end);
                var pOr = predicate as OrPredicateOfValueListImpl5eNmv5yi;
                var pAnd = predicate as AndPredicateOfValueListImpl5eNmv5yi;
                if (pOr != null) {
                    pOr.Predicate1 = lhs;
                    pOr.Predicate2 = rhs;
                }
                else {
                    pAnd.Predicate1 = lhs;
                    pAnd.Predicate2 = rhs;
                }
            }
            return predicate;
        }

        private static void SetPredicateValue(object predicate, int value) {
            if (predicate is ContainsPredicate)
                ((ContainsPredicate)predicate).Element = new ValueImpl() { Value = value };
            else if (predicate is MaxLengthPredicate)
                ((MaxLengthPredicate)predicate).Length = value;
            else if (predicate is MinLengthPredicate)
                ((MinLengthPredicate)predicate).Length = value;
        }

        private static object FindOp(List<string> tokens, int start, int end, out int tokenPos) {
            int nestDepth = 0;
            object unaryPredicate = null;
            int unaryPredicatePos = -1;
            for (tokenPos = start; tokenPos <= end; tokenPos++) {
                string token = tokens[tokenPos];
                if (token == "(")
                    nestDepth++;
                else if (token == ")") {
                    if (nestDepth == 0)
                        throw new Exception("Parse error: unexpected ')'");
                    nestDepth--;
                }
                else if (token == "or" && nestDepth == 0)
                    return new OrPredicateOfValueListImpl5eNmv5yi();
                else if (token == "and" && nestDepth == 0)
                    return new AndPredicateOfValueListImpl5eNmv5yi();
                else if (token == "not" && nestDepth == 0 && unaryPredicate == null)
                    return new NotPredicateOfValueListImpl5eNmv5yi();
                else if (token == "contains" && nestDepth == 0 && unaryPredicate == null) {
                    unaryPredicate = new ContainsPredicate();
                    unaryPredicatePos = tokenPos;
                }
                else if (token == "maxlen" && nestDepth == 0 && unaryPredicate == null) {
                    unaryPredicate = new MaxLengthPredicate();
                    unaryPredicatePos = tokenPos;
                }
                else if (token == "minlen" && nestDepth == 0 && unaryPredicate == null) {
                    unaryPredicate = new MinLengthPredicate();
                    unaryPredicatePos = tokenPos;
                }
                else if (nestDepth == 0 && unaryPredicate == null)
                    throw new Exception(String.Format("Parse error: unexpected token '{0}'", token));
            }
            if (unaryPredicate != null)
                tokenPos = unaryPredicatePos;
            return unaryPredicate;
        }

        private static List<string> TokenizePredicate(string p) {
            List<string> tokens = new List<string>();
            StringBuilder curToken = new StringBuilder();
            for (int i = 0; i < p.Length; i++) {
                char c = p[i];
                if (c == ' ' || c == '\t' || c == '(' || c == ')') {
                    if (curToken.Length > 0) {
                        tokens.Add(curToken.ToString());
                        curToken.Clear();
                    }
                    if (c != ' ' && c != '\t')
                        tokens.Add(c.ToString());
                }
                else
                    curToken.Append(c);
            }
            if (curToken.Length > 0)
                tokens.Add(curToken.ToString());
            return tokens;
        }

        private static void PrintList(ValueListImpl valueListImpl) {
            Console.WriteLine(String.Join(", ", valueListImpl.List.Select(v => v.Value)));
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
