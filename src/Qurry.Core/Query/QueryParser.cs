using Qurry.Core.Query.Nodes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Qurry.Core.Query
{
    public class QueryParser : IQueryParser
    {
        private delegate IQueryNode NodeFactory(QueryParser parser, IQueryNode left, IQueryNode right);

        private static readonly Regex findWhiteChars = new Regex(@"\s + ");

        private static readonly IEnumerable<(Regex regex, string replace)> rules = new (Regex, string)[]
        {
            (new Regex(@"\!\="), " notEqual "),
            (new Regex(@"\<\="), " lessThanOrEqual "),
            (new Regex(@"\>\="), " greaterThanOrEqual "),
            (new Regex(@"\="), " equal "),
            (new Regex(@"\("), " ( "),
            (new Regex(@"\)"), " ) "),
        };

        private static readonly IEnumerable<(string name, NodeFactory factory)> operators = new (string, NodeFactory)[]
        {
            ("or", (parser, left, right) => new OrNode(left, right)),
            ("and", (parser, left, right) => new AndNode(left, right)),
            ("equal", (parser, left, right) => new EqualNode(left, right)),
            ("notEqual", (parser, left, right) => new NotEqualNode(left, right)),
            ("like", (parser, left, right) => new LikeNode(left, right)),
            (">", (parser, left, right) => new GreaterThanNode(left, right)),
            ("greaterThanOrEqual", (parser, left, right) => new GreaterThanOrEqualNode(left, right)),
            ("<", (parser, left, right) => new LessThanNode(left, right)),
            ("lessThanOrEqual", (parser, left, right) => new LessThanOrEqualNode(left, right)),
            ("-", (parser, left, right) => new SubstractNode(left, right)),
            ("+", (parser, left, right) => new AddNode(left, right)),
            ("/", (parser, left, right) => new DivideNode(left, right)),
            ("*", (parser, left, right) => new MultiplyNode(left, right)),
        };


        public virtual IQuery Parse(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                throw new ArgumentException(nameof(query));
            }

            foreach ((Regex? regex, string? replacement) in rules)
            {
                query = regex.Replace(query, replacement);
            }

            var queryParts = this.GetCleanExpression(query)
                .Aggregate(new SplittingAggrecationAccumulator(), (ac, element) => ac.Next(element)).GetResult()
                .Where(part => !string.IsNullOrWhiteSpace(part))
                .Aggregate(new BracketAggregationAccumulator(), (ac, element) => ac.AddPart(element)).Result
                .ToList();

            return this.Parse(queryParts);
        }

        protected virtual IQuery Parse(List<Node> expressions)
        {
            return new QueryImplementation(this.ParseNode(expressions));
        }

        protected virtual IQueryNode ParseNode(List<Node> expressions)
        {
            if (expressions.Count() == 2)
            {
                // unhandled case. It shouldn't happen
                throw new InvalidQueryException();
            }
            else if (expressions.Count() == 1)
            {
                Node node = expressions.First();

                if (node.Elements != null)
                {
                    return this.ParseNode(node.Elements);
                }

                return new ValueNode(expressions.First().Element!);
            }

            foreach ((string name, NodeFactory factory) in operators)
            {
                int index = expressions.FindIndex((op) => op.Element == name);

                if (index != -1)
                {
                    return factory(
                        this,
                        left: this.ParseNode(expressions.Take(index).ToList()),
                        right: this.ParseNode(expressions.Skip(index + 1).ToList()));
                }
            }

            throw new InvalidQueryException("Given query is not constant and no operators found in query");
        }

        protected virtual string GetCleanExpression(string expression)
        {
            return findWhiteChars.Replace(expression, " ").Trim(' ');
        }

        private class BracketAggregationAccumulator
        {
            public int Brackets { get; set; } = 0;
            public List<Node> Result { get; set; } = new List<Node>();

            public Stack<List<Node>> BracketStack = new Stack<List<Node>>();

            public BracketAggregationAccumulator AddPart(string element)
            {
                if (element == "(" || element == ")")
                {
                    if (element == "(")
                    {
                        this.Brackets++;
                        this.BracketStack.Push(new List<Node>());
                    }
                    else if (element == ")")
                    {
                        this.Brackets--;

                        if (this.Brackets < 0)
                        {
                            throw new InvalidQueryException("Unexpected closing bracket");
                        }
                        if (this.Brackets == 0)
                        {
                            this.Result.Add(new Node(BracketStack.Pop()));
                        } 
                        else
                        {
                            List<Node> last = BracketStack.Pop();
                            this.BracketStack.Peek().Add(new Node(last));
                        }
                    }
                }
                else
                {
                    if (this.Brackets == 0)
                    {
                        this.Result.Add(new Node(element));
                    }
                    else
                    {
                        this.BracketStack.Peek().Add(new Node(element));
                    }
                }

                return this;
            }
        }

        protected struct Node
        {
            public string? Element { get; set; }

            public List<Node>? Elements { get; set; }

            public Node(string element)
            {
                this.Element = element;
                this.Elements = null;
            }

            public Node(List<Node> elements)
            {
                this.Element = null;
                this.Elements = elements;
            }
        }

        private class SplittingAggrecationAccumulator
        {
            private readonly List<string> result = new List<string>();
            private readonly StringBuilder last = new StringBuilder();

            private char str = '0';
            private bool inStr = false;

            public SplittingAggrecationAccumulator Next(char element)
            {
                if (element == ' ')
                {
                    if (!inStr)
                    {

                        this.result.Add(last.ToString());
                        last.Clear();
                    }
                    else
                    {
                        last.Append(element);
                    }
                }
                else
                {
                    if (!inStr && (element == '\'' || element == '"'))
                    {
                        inStr = true;
                        str = element;
                    }
                    else if (inStr && element == str)
                    {
                        inStr = false;
                    }

                    last.Append(element);
                }

                return this;
            }

            public IEnumerable<string> GetResult()
            {
                return this.result.Append(last.ToString());
            }
        }
    }
}