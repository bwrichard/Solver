using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Solver
{
    class CypherKeyList : IEnumerable<CypherKeyList>
    {
        public char cypherLetter { get; set; }
        public char clearLetter { get; set; }
        public List<char> triedLetters { get; set; }

        public CypherKeyList()
        {
            triedLetters = new List<char>();
        }
        

        public IEnumerator<CypherKeyList> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
