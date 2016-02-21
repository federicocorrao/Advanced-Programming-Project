using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace STF
{
    class Parser
    {
        private void PrintTree(Node node, int level = 0)
        {
            string tabs = "";
            for (int i = 0; i < level; i++) tabs += "--|";

            if (node is NodeTable) {
                Console.WriteLine(tabs + "Table");
                PrintTree((node as NodeTable).RowList, level + 1);
            }
            else if (node is NodeRow) {
                Console.WriteLine(tabs + "Row");
                PrintTree((node as NodeRow).FieldList, level + 1);
            }
            else if (node is NodeRowList) {
                Console.WriteLine(tabs + "RowList");
                PrintTree((node as NodeRowList).Row, level + 1);
                PrintTree((node as NodeRowList).RowList, level + 1);
            }
            else if (node is NodeField) {
                Console.WriteLine(tabs + "Field");
                PrintTree((node as NodeField).Text, level + 1);
            }
            else if (node is NodeFieldList) {
                Console.WriteLine(tabs + "FieldList");
                PrintTree((node as NodeFieldList).Field, level + 1);
                PrintTree((node as NodeFieldList).FieldList, level + 1);
            }
            else if (node is NodeText)
            { 
                if (node is NodeIdentifier)
                    Console.WriteLine(tabs + "Text [Identifier] " + (node as NodeText).Attribute);
                else if (node is NodeNumber)
                    Console.WriteLine(tabs + "Text [Number] " + (node as NodeText).Attribute);
            }
            else if (node == null)
                Console.WriteLine(tabs + "#");
        }

    /* Classi Nodi Parse Tree */

        private abstract class Node { }
        private class NodeTable      : Node { public NodeRowList RowList; }
        private class NodeRowList    : Node { public NodeRowList RowList; public NodeRow Row; }
        private class NodeRow        : Node { public NodeFieldList FieldList; }
        private class NodeFieldList  : Node { public NodeFieldList FieldList; public NodeField Field;}
        private class NodeField      : Node { public NodeText Text; }
        private class NodeText       : Node { public string Attribute; }
        private class NodeIdentifier : NodeText { }
        private class NodeNumber     : NodeText { }

        private List<List<Token>> tempTable;
        private List<Token> tempRow;

        private List<Token> Tokens;
        private int Index;

        public Table Parse(string fixtureHTML)
        {
            this.Index = -1;
            this.Tokens = (new Lexer()).Tokenize(fixtureHTML);
            NodeTable parseTree = ParseTable();
            PrintTree(parseTree);
            return GenerateTable();
        }

        private static Dictionary<string, Type> stringToType = new Dictionary<string, Type>()
            { { "int", typeof(int) }, { "float", typeof(float) } };

        private Table GenerateTable()
        {
            // Controlli del caso omessi per semplicità
            string fixtureName = tempTable[0][0].Attribute;
            List<string> argNames = new List<string>();
            List<string> argTypes = new List<string>();
            foreach (Token t in tempTable[1]) argNames.Add(t.Attribute);
            foreach (Token t in tempTable[2]) argTypes.Add(t.Attribute);
            tempTable.RemoveRange(0, 3);
            Table resultTable = new Table(fixtureName, argNames, argTypes);

            foreach (List<Token> l in tempTable)
            {
                Row r = new Row();
                for (int i = 0; i < argNames.Count; i++)
                    r.Add(Convert.ChangeType(
                        l[i].Attribute,
                        stringToType[argTypes[i]],
                        System.Globalization.CultureInfo.InvariantCulture));
                resultTable.Add(r);
            }
            return resultTable;
        }

    /* Utilities */

        private void Match(TokenType expected)
        {
            Index++;
            if(!expected.HasFlag(Tokens[Index].Type))
                throw new Exception("Parser.Match: Expected " + expected.ToString() + " @ Index " + Index);
        }
        
        private bool Lookahead(TokenType expected)
        {
            return expected.HasFlag(Tokens[Index + 1].Type);
        }

        private Token GetToken() { return Tokens[Index]; }

    /* Non-Terminals Procedures */

        private NodeTable ParseTable()
        {
            tempTable = new List<List<Token>>();
            Match(TokenType.TableBegin);
            NodeTable tbl = new NodeTable { RowList = ParseRowList() };
            Match(TokenType.TableEnd);
            return tbl;
        }
        private NodeRowList ParseRowList()
        {
            NodeRowList rl = new NodeRowList { Row = ParseRow() };
            if (Lookahead(TokenType.TableEnd))
                rl.RowList = null;
            else if (Lookahead(TokenType.RowBegin))
                rl.RowList = ParseRowList();
            else throw new Exception("Parser.ParseRowList(): Expected TableEnd or RowBegin");
            return rl;
        }
        private NodeRow ParseRow()
        {
            tempRow = new List<Token>();
            Match(TokenType.RowBegin);
            NodeRow r = new NodeRow { FieldList = ParseFieldList() };
            Match(TokenType.RowEnd);
            tempTable.Add(tempRow);
            return r;
        }
        private NodeFieldList ParseFieldList()
        {
            NodeFieldList fl = new NodeFieldList { Field = ParseField() };
            if (Lookahead(TokenType.RowEnd))
                fl.FieldList = null;
            else if (Lookahead(TokenType.FieldBegin))
                fl.FieldList = ParseFieldList();
            else throw new Exception("Parser.ParseFieldList(): Expected RowEnd or FieldBegin");
            return fl;
        }
        private NodeField ParseField()
        {
            Match(TokenType.FieldBegin);
            NodeField f = new NodeField { Text = ParseText() };
            Match(TokenType.FieldEnd);
            return f;
        }
        private NodeText ParseText()
        {
            if(Lookahead(TokenType.Identifier | TokenType.Number))
            {
                Match(TokenType.Identifier | TokenType.Number);
                Token t = GetToken(); tempRow.Add(t);
                return (t.Type == TokenType.Identifier) ?
                    (NodeText)new NodeIdentifier { Attribute = t.Attribute }:
                    (NodeText)new NodeNumber     { Attribute = t.Attribute };
            }
            else throw new Exception("Parser.ParseText(): Invalid Identifier or Number");
        }
    }

}
